import {
  axios
} from '@/utils/request'

/**
 *
 * 系统属性监控
 *
 */
export function sysMachineUse(parameter) {
  return axios({
    url: '/sysMachine/use',
    method: 'get',
    params: parameter
  })
}

export function sysMachineBase(parameter) {
  return axios({
    url: '/sysMachine/base',
    method: 'get',
    params: parameter
  })
}

export function sysMachineNetwork(parameter) {
  return axios({
    url: '/sysMachine/network',
    method: 'get',
    params: parameter
  })
}
