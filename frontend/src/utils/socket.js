import * as SignalR from '@microsoft/signalr'

const EventEmitter = require('events')

const defaultOptions = {
  log: false
}

class SocketConnection extends EventEmitter {
  constructor(connection, options = {}) {
    super()
    this.connection = connection
    this.options = Object.assign(defaultOptions, options)
    this.listened = []
    this.toSend = []
    this.offline = false
    this.socket = undefined
  }

  /**
   * 同一种消息只定义一次
   *
   * @param {string| symbol} event
   * @param {(...args: any[]) => void} listener
   * @memberof SocketConnection
   */
  one(event, listener) {
    if (this.listeners(event).length === 0) {
      this.on(event, listener)
    }
  }

  async _initialize() {
    try {
      await this.socket.start()
      this.emit('onstart')
      if (this.offline) {
        this.emit('onrestart')
      }
      this.offline = false
    } catch (error) {
      setTimeout(async () => {
        await this._initialize()
      }, 5000)
    }
  }

  async start(token) {
    // 组件重新加载时, 如果 socket 存在, 不需要新建
    if (!this.socket) {
      this.socket = new SignalR.HubConnectionBuilder()
        .configureLogging(SignalR.LogLevel.Information)
        .withUrl(
          `/hubs/chathub`, {
            accessTokenFactory: () => token,
            skipNegotiation: true,
            transport: SignalR.HttpTransportType.WebSockets
          }
        )
        .build()

      this.socket.onclose(async () => {
        this.offline = true
        this.emit('onclose')
        await this._initialize()
      })

      await this._initialize()
    }
  }

  async authenticate(token) {
    await this.start(token)
  }

  listen(method) {
    if (this.offline) return

    if (this.listened.some((v) => v === method)) return
    this.listened.push(method)

    this.one('onstart', () => {
      this.listened.forEach((method) => {
        this.socket.on(method, (data) => {
          if (this.options.log) {
            console.log({
              type: 'receive',
              method,
              data
            })
          }

          this.emit(method, data)
        })
      })
    })
  }

  send(methodName, ...args) {
    if (this.options.log) {
      console.log({
        type: 'send',
        methodName,
        args
      })
    }
    if (this.offline) return

    if (this.socket) {
      this.socket.send(methodName, ...args)
      return
    }

    this.one('onstart', () => this.socket.send(methodName, ...args))
  }

  async invoke(methodName, ...args) {
    if (this.options.log) {
      console.log({
        type: 'invoke',
        methodName,
        args
      })
    }
    if (this.offline) return false

    if (this.socket) {
      return this.socket.invoke(methodName, ...args)
    }

    // eslint-disable-next-line no-async-promise-executor
    return new Promise(async (resolve) => this.one('onstart', () => resolve(this.socket.invoke(methodName, ...args))))
  }
}

if (!SignalR) {
  throw new Error('[Vue-SignalR] Cannot locate signalr-client')
}

function install(Vue, connection) {
  if (!connection) {
    throw new Error('[Vue-SignalR] Cannot locate connection')
  }

  const Socket = new SocketConnection(connection)

  Vue.socket = Socket

  Object.defineProperties(Vue.prototype, {
    $socket: {
      get() {
        return Socket
      }
    }
  })

  Vue.mixin({
    created() {
      if (this.$options.sockets) {
        const methods = Object.getOwnPropertyNames(this.$options.sockets)

        methods.forEach((method) => {
          Socket.listen(method)

          Socket.one(method, (data) => this.$options.sockets[method].call(this, data))
        })
      }

      if (this.$options.subscribe) {
        Socket.one('authenticated', () => {
          this.$options.subscribe.forEach((channel) => {
            Socket.invoke('join', channel)
          })
        })
      }
    }
  })
}

export default install
