import request from '/@/utils/request';

enum Api {
	// 获取菜单列表
	GetMenuList = '/sysMenu/list',
	// 字典接口
	GetDictTypePageList = '/sysDictType/pageList',
	GetDictTypeList = '/sysDictType/list',
	AddDictType = '/sysDictType/add',
	UpdateDictType = '/sysDictType/update',
	DeleteDictType = '/sysDictType/delete',

	GetDictDataPageList = '/sysDictData/pageList',
	AddDictData = '/sysDictData/add',
	UpdateDictData = '/sysDictData/update',
	DeleteDictData = '/sysDictData/delete',
	GetDictDataDropdown = '/sysDictData/DictDataDropdown',
	QueryDictDataDropdown = '/sysDictData/queryDictDataDropdown',
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
	GetDatabaseList = '/codeGenerate/DatabaseList',

	// 数据库接口
	GetColumnInfoList = '/sysDatabase/columnList',
	GetTableInfoList = '/sysDatabase/tableList',
	AddTable = '/sysDatabase/table/add',
	UpdateTable = '/sysDatabase/table/update',
	DeleTetable = '/sysDatabase/table/delete',
	AddColumn = '/sysDatabase/column/add',
	UpdateColumn = '/sysDatabase/column/update',
	DeleteColumn = '/sysDatabase/column/delete',
	CreateEntity = '/sysDatabase/entity/create',

	// 常量下拉框接口
	AllConstSelector = '/constSelector/allConstSelector',
	ConstSelector = '/constSelector/constSelector',
	AllConstSelectorWithOptions = '/constSelector/allConstSelectorWithOptions',
}

////////// 常量下拉框管理接口 //////////
// 获取所有常量下拉框列表
export const getAllConstSelector = () =>
	request({
		url: Api.AllConstSelector,
		method: 'get',
	});

// 根据类名获取下拉框数据
export const getConstSelector = (typeName?: string) =>
	request({
		url: Api.ConstSelector,
		method: 'get',
		data: { typeName },
	});

// 获取所有下拉框及选项
export const getAllConstSelectorWithOptions = () =>
	request({
		url: Api.AllConstSelectorWithOptions,
		method: 'get',
	});

export const getDictTypeList = (params?: any) =>
	request({
		url: Api.GetDictTypeList,
		method: 'get',
		data: params,
	});

export const getMenuList = (params?: any) =>
	request({
		url: Api.GetMenuList,
		method: 'get',
		data: params,
	});

// 从字典中值，下拉框控件使用
export const getDictDataDropdown = (params: any) =>
	request({
		url: Api.GetDictDataDropdown + '/' + params,
		method: 'get',
	});

////////// 代码生成接口 //////////
// 分页查询代码生成
export const getGeneratePage = (params?: any) =>
	request({
		url: Api.GetGeneratePage,
		method: 'get',
		data: params,
	});

// 增加代码生成
export const addGenerate = (params: any) =>
	request({
		url: Api.AddGenerate,
		method: 'post',
		data: params,
	});

// 修改代码生成
export const updateGenerate = (params: any) =>
	request({
		url: Api.UpdateGenerate,
		method: 'post',
		data: params,
	});

// 删除代码生成
export const deleGenerate = (params: any) =>
	request({
		url: Api.DeleGenerate,
		method: 'post',
		data: params,
	});

// 获取数据库(上下文定位器)集合
export const getDatabaseList = (params?: any) =>
	request({
		url: Api.GetDatabaseList,
		method: 'get',
		data: params,
	});

// 获取数据库表(实体)集合
export const getTableList = (configId: string) =>
	request({
		url: Api.GetTableList + '/' + configId,
		method: 'get',
	});

// 根据表名获取列
export const getColumnList = (configId: string, tableName: string) =>
	request({
		url: Api.GetColumnList + '/' + configId + '/' + tableName,
		method: 'get',
	});

// 本地生成
export const generateRunLocal = (params: any) =>
	request({
		url: Api.GenerateRunLocal,
		method: 'post',
		data: params,
	});

// 代码生成详细配置列表
export const getGenerateConfigList = (params?: any) =>
	request({
		url: Api.GetGenerateConfigList,
		method: 'get',
		data: params,
	});

// 编辑代码生成详细配置
export const updateGenerateConfig = (params: any) =>
	request({
		url: Api.UpdateGenerateConfig,
		method: 'post',
		data: params,
	});

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
