import {
  HubConnectionBuilder,
  LogLevel
} from '@microsoft/signalr'

const EventEmitter = require('events')

class Socket extends EventEmitter {
  constructor() {
    super()

    this.socket = null
    this.manuallyClosed = false
    this.startedPromise = null
  }

  init(token) {
    this.socket = new HubConnectionBuilder()
      .withUrl(
        `hubs/chathub`,
        token ? {
          accessTokenFactory: () => token
        } : null
      )
      .configureLogging(LogLevel.Information)
      .build()

    this.socket.onclose(() => {
      if (!this.manuallyClosed) this.start()
    })

    this.manuallyClosed = false
    this.start()
  }

  // 启动方法, 失败了 5s 后重试
  start() {
    this.startedPromise = this.socket.start()
      .catch(err => {
        console.error('连接 hub 失败', err)
        return new Promise((resolve, reject) => setTimeout(() => this.start().then(resolve).catch(reject), 5000))
      })
  }

  stop() {
    if (!this.startedPromise) return

    this.manuallyClosed = true
    return this.startedPromise
      .then(() => this.socket.stop())
      .then(() => {
        this.startedPromise = null
      })
  }

  listen(method) {
    if (!this.startedPromise) return

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
  }

  send(method, ...args) {
    if (!this.startedPromise) return

    this.socket.send(method, ...args)
  }

  invoke(method, ...args) {
    if (!this.startedPromise) return

    this.socket.invoke(method, ...args)
  }
}

function install(Vue) {
  const socket = new Socket()

  Vue.prototype.$socket = socket
  Vue.$socket = socket
}

export default install
