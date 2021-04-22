/**
 * 租户
 */
import {
  axios
} from '@/utils/request'

/**
 * 租户列表
 *
 */
export function sysTenantPage(parameter) {
  return axios({
    url: '/sysTenant/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 新增租户
 *
 */
export function sysTenantAdd(parameter) {
  return axios({
    url: '/sysTenant/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除租户
 *
 */
export function sysTenantDelete(parameter) {
  return axios({
    url: '/sysTenant/delete',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑租户
 *
 */
export function sysTenantEdit(parameter) {
  return axios({
    url: '/sysTenant/edit',
    method: 'post',
    data: parameter
  })
}

/**
 * 拥有菜单
 *
 * @author zuohuaijun
 * @date 2021/04/22 10:30
 */
export function sysTenantOwnMenu (parameter) {
  return axios({
    url: '/sysTenant/ownMenu',
    method: 'get',
    params: parameter
  })
}

/**
 * 授权菜单
 *
 * @author zuohuaijun
 * @date 2021/04/22 10:30
 */
export function sysTenantGrantMenu (parameter) {
  return axios({
    url: '/sysTenant/grantMenu',
    method: 'post',
    data: parameter
  })
}

/**
 * 重置密码
 *
 * @author zuohuaijun
 * @date 2021/04/22 11:00
 */
export function sysTenantResetPwd (parameter) {
  return axios({
    url: '/sysTenant/resetPwd',
    method: 'post',
    data: parameter
  })
}
