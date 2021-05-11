/**
 * 核酸检测项目
 */
import {
  axios
} from '@/utils/request'

/**
 * 核酸检测项目列表
 *
 */
export function getXgTestItemPage(parameter) {
  return axios({
    url: '/xgTestItem/page',
    method: 'get',
    params: parameter
  })
}

/**
 * 新增核酸检测项目
 *
 */
export function addXgTestItem(parameter) {
  return axios({
    url: '/xgTestItem/add',
    method: 'post',
    data: parameter
  })
}

/**
 * 删除核酸检测项目
 *
 */
export function deleteXgTestItem(parameter) {
  return axios({
    url: '/xgTestItem/delete',
    method: 'post',
    data: parameter
  })
}

/**
 * 编辑核酸检测项目
 *
 */
export function editXgTestItem(parameter) {
  return axios({
    url: '/xgTestItem/edit',
    method: 'post',
    data: parameter
  })
}
