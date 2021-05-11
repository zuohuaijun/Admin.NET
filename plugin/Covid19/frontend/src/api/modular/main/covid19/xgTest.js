/**
 * 检测核酸样本
 */
import {
  axios
} from '@/utils/request'

/**
 * 检测核酸样本列表
 *
 */
export function getXgTestPage(parameter) {
  return axios({
    url: '/xgTest/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 批量设置核酸样本为阴性
 *
 */
export function updateNegative(parameter) {
  return axios({
    url: '/xgTest/updateNegative',
    method: 'post',
    data: parameter
  })
}

/**
 * 更新核酸样本检测结果
 *
 */
export function updateTestResult(parameter) {
  return axios({
    url: '/xgTest/updateTestResult',
    method: 'post',
    data: parameter
  })
}

/**
 * 审核核酸样本检测结果
 *
 */
export function checkTestResult(parameter) {
  return axios({
    url: '/xgTest/checkTestResult',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除核酸样本检测结果
 *
 */
export function deleteXgTest(parameter) {
  return axios({
    url: '/xgTest/delete',
    method: 'post',
    data: parameter
  })
}
