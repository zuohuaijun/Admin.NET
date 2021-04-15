/**
 * 租户开通
 *
 * @author baimch
 * @date 2020/5/26 19:06
 */
 import { axios } from '@/utils/request'

 /**
  * 登录
  *
  * @author baimch
  * @date 2020/5/26 19:06
  */
 export function regist (parameter) {
   return axios({
     url: '/regist',
     method: 'post',
     data: parameter
   })
 }
