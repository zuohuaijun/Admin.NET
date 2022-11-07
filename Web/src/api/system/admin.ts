import request from '/@/utils/request';

enum Api {
	// 代码生成接口
	GetGeneratePage = '/codeGenerate/page',
	AddGenerate = '/codeGenerate/add',
	UpdateGenerate = '/codeGenerate/edit',
	DeleGenerate = '/codeGenerate/delete',
	GetTableList = '/codeGenerate/InformationList',
	GetColumnList = '/codeGenerate/ColumnList',
	GenerateRunLocal = '/codeGenerate/runLocal',
	GenerateRunDown = '/codeGenerate/runDown',
	GetGenerateConfigList = '/sysCodeGenerateConfig/list',
	UpdateGenerateConfig = '/sysCodeGenerateConfig/edit',

	// 数据库接口
	GetColumnInfoList = '/sysDatabase/columnList',
	GetTableInfoList = '/sysDatabase/tableList',
	AddTable = '/sysDatabase/table/add',
	UpdateTable = '/sysDatabase/table/update',
	DeleTetable = '/sysDatabase/table/delete',
	AddColumn = '/sysDatabase/column/add',
	UpdateColumn = '/sysDatabase/column/update',
	DeleteColumn = '/sysDatabase/column/delete',
	CreateEntity = '/sysDatabase/entity/createEntity',
}

//////////数据库管理接口 //////////
// 获取表字段
export const getColumnInfoList = (params: any) =>
	request({
		url: Api.GetColumnInfoList,
		method: 'get',
		data: params,
	});

// 获取所有表
export const getTableInfoList = (params?: any) =>
	request({
		url: Api.GetTableInfoList,
		method: 'get',
		data: params,
	});

// 添加表
export const addTable = (params: any) =>
request({
    url: Api.AddTable,
    method: 'post',
    data: params,
});

// 修改表
export const updateTable = (params: any) =>
request({
    url: Api.UpdateTable,
    method: 'post',
    data: params,
});

// 删除表
export const deleteTable = (params: any) =>
request({
    url: Api.DeleTetable,
    method: 'post',
    data: params,
});

// 添加字段
export const addColumn = (params: any) =>
request({
    url: Api.AddColumn,
    method: 'post',
    data: params,
});

// 修改字段
export const updateColumn = (params: any) =>
request({
    url: Api.UpdateColumn,
    method: 'post',
    data: params,
});

// 删除字段
export const deleteColumn = (params: any) =>
request({
    url: Api.DeleteColumn,
    method: 'post',
    data: params,
});

// 生成实体
export const createEntity = (params: any) =>
request({
    url: Api.CreateEntity,
    method: 'post',
    data: params,
});