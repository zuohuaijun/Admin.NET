import { axios } from '@/utils/request'

/**
 * 根据枚举名称获取枚举数据，返回格式为：[{code:"M",value:"男"},{code:"F",value:"女"}]
 *
 * @author taoran
 * @date 2021-04-16 21:13/sysEnumData/list/{enumName}
 */
export function sysEnumDataList (parameter) {
  return axios({
    url: '/sysEnumData/list',
    method: 'get',
    params: parameter
  })
}

/**
 * 根据实体名和字段名获取枚举数据
 *
 * @author taoran
 * @date 2021-04-16 21:13
 */
export function sysEnumDataListByField (parameter) {
  return axios({
    url: '/sysEnumData/listByFiled',
    method: 'get',
    params: parameter
  })
}
