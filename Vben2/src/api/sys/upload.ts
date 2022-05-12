import { UploadApiResult } from './model/uploadModel';
import { defHttp } from '/@/utils/http/axios';
import { UploadFileParams } from '/#/axios';
import { useGlobSetting } from '/@/hooks/setting';

const { apiUrl = '' } = useGlobSetting();

/**
 * @description: Upload interface
 */
export function uploadApi(
  params: UploadFileParams,
  onUploadProgress: (progressEvent: ProgressEvent) => void,
) {
  return defHttp.uploadFile<UploadApiResult>(
    {
      url: apiUrl,
      onUploadProgress,
    },
    params,
  );
}

// 系统默认文件上传接口
export function uploadFileApi(
  params: UploadFileParams,
  onUploadProgress: (progressEvent: ProgressEvent) => void,
) {
  return defHttp.uploadFile<UploadApiResult>(
    {
      url: apiUrl + '/sysFile/upload',
      onUploadProgress,
    },
    params,
  );
}

// 上传登记数据文件接口
export function uploadFileApi_sign(
  params: UploadFileParams,
  onUploadProgress: (progressEvent: ProgressEvent) => void,
) {
  return defHttp.uploadFile<UploadApiResult>(
    {
      url: apiUrl + '/personSign/import',
      onUploadProgress,
    },
    params,
  );
}
