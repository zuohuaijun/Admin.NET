import request from '/@/utils/request';
enum Api {
    DictTypeDataList = '/api/sysDictData/DataList',
}

// 根据字典类型编码获取字典值集合
export const getDictDataList = (params?: any) =>
	request({
		url: `${Api.DictTypeDataList}/${params}`,
		method: 'get'
	});
 