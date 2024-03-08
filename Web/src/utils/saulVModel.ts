import { computed } from 'vue'
export const saulVModel = <T extends Readonly<{ [k: string]: any }>, K extends keyof T>(
  props: T,
  propName: K,
  emit: (...args: any[]) => void
) => {
  return computed({
    get() {
      if (typeof props[propName] === 'object') {
        return new Proxy(props[propName], {
          set(obj, name, val) {
            emit(`update:${String(propName)}`, { ...obj, [name]: val })
            return true
          }
        })
      }
      return props[propName]
    },
    set(val) {
      emit(`update:${String(propName)}`, val)
    }
  })
}