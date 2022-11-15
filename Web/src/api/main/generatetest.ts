import request from '/@/utils/request';
enum Api {
  Addgeneratetest = '/generatetest/add',
  Deletegeneratetest = '/generatetest/delete',
  Updategeneratetest = '/generatetest/edit',
  GetgeneratetestPage = '/generatetest/page',
}

// 增加GenerateTest
export const addgeneratetest = (params?: any) =>
	request({
		url: Api.Addgeneratetest,
		method: 'post',
		data: params,
	});

// 删除GenerateTest
export const deletegeneratetest = (params?: any) => 
	request({
			url: Api.Deletegeneratetest,
			method: 'post',
			data: params,
		});

// 编辑GenerateTest
export const updategeneratetest = (params?: any) => 
	request({
			url: Api.Updategeneratetest,
			method: 'post',
			data: params,
		});

// 分页查询GenerateTest
export const getgeneratetestPageList = (params?: any) => 
	request({
			url: Api.GetgeneratetestPage,
			method: 'get',
			data: params,
		});

