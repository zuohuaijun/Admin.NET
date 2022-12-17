import request from '/@/utils/request';
enum Api {
    DictTypeDataList = '/sysDictData/DictDataDropdown',
}

// 增加配置金蝶云信息
export const getDictDataDropdown = (params?: any) =>
	request({
		url: `${Api.DictTypeDataList}/${params}`,
		method: 'get'
	});
 