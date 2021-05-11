/**
 * 核酸样本采集人员
 */
import {
  axios
} from '@/utils/request'

/**
 * 核酸样本采集人员列表
 *
 */
export function getXgCollectorPage(parameter) {
  return axios({
    url: '/xgCollector/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 新增核酸样本采集人员
 *
 */
export function addXgCollector(parameter) {
  return axios({
    url: '/xgCollector/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除核酸样本采集人员
 *
 */
export function deleteXgCollector(parameter) {
  return axios({
    url: '/xgCollector/delete',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑核酸样本采集人员
 *
 */
export function editXgCollector(parameter) {
  return axios({
    url: '/xgCollector/edit',
    method: 'post',
    data: parameter
  })
}
