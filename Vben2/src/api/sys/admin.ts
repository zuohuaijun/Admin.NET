import { defHttp } from '/@/utils/http/axios';

enum Api {
  AccountList = '/system/getAccountList',
  IsAccountExist = '/system/accountExist',

  // 用户接口
  UserPageList = '/sysUser/pageList',
  AddUser = '/sysUser/add',
  DeleteUser = '/sysUser/delete',
  GetDatabaseList = '/codeGenerate/DatabaseList',
  UpdateUser = '/sysUser/update',
  SetUserStatus = '/sysUser/setStatus',
  GrantUserRole = '/sysUser/grantRole',
  GrantUserOrg = '/sysUser/grantOrg',
  ChangeUserPwd = '/sysUser/changeUserPwd',
  ResetUserPwd = '/sysUser/resetPwd',
  UserOwnRoleList = '/sysUser/ownRole',
  UserOwnOrgList = '/sysUser/ownOrg',

  // 角色接口
  RolePageList = '/sysRole/pageList',
  RoleList = '/sysRole/list',
  AddRole = '/sysRole/add',
  DeleteRole = '/sysRole/delete',
  UpdateRole = '/sysRole/update',
  SetRoleStatus = '/sysRole/setStatus',
  RoleOwnMenuList = '/sysRole/ownMenu',
  RoleOwnOrgList = '/sysRole/ownOrg',
  GrantRoleMenu = '/sysRole/grantMenu',
  GrantRoleData = '/sysRole/grantData',

  // 菜单接口
  MenuList = '/sysMenu/list',
  AddMenu = '/sysMenu/add',
  DeleteMenu = '/sysMenu/delete',
  UpdateMenu = '/sysMenu/update',

  // 机构接口
  OrgList = '/sysOrg/list',
  AddOrg = '/sysOrg/add',
  DeleteOrg = '/sysOrg/delete',
  UpdateOrg = '/sysOrg/update',

  // 职位接口
  PosList = '/sysPos/list',
  AddPos = '/sysPos/add',
  DeletePos = '/sysPos/delete',
  UpdatePos = '/sysPos/update',

  // 日志接口
  VislogPageList = '/sysVisLog/pageList',
  ClearVisLog = '/sysVisLog/clear',
  OplogPageList = '/sysOpLog/pageList',
  ClearOpLog = '/sysOpLog/clear',
  ExlogPageList = '/sysExLog/pageList',
  ClearExLog = '/sysExLog/clear',
  DifflogPageList = '/sysDiffLog/pageList',
  ClearDiffLog = '/sysDiffLog/clear',

  // 文件接口
  FilePageList = '/sysFile/pageList',
  UploadFile = '/sysFile/upload',
  DownloadFile = '/sysFile/download',
  DeleteFile = '/sysFile/delete',

  // 参数配置接口
  ConfigPageList = '/sysConfig/pageList',
  AddConfig = '/sysConfig/add',
  DeleteConfig = '/sysConfig/delete',
  UpdateConfig = '/sysConfig/update',

  // 定时器接口
  TimerPageList = '/sysTimer/pageList',
  AddTimer = '/sysTimer/add',
  DeleteTimer = '/sysTimer/delete',
  UpdateTimer = '/sysTimer/update',
  SetTimerStatus = '/sysTimer/setStatus',

  // 服务器监控接口
  ServerBase = '/server/base',
  ServerUse = '/server/use',
  ServerNetWork = '/server/network',

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

  // 数据库接口
  GetColumnInfoList = '/dataBase/columnInfoList',
  GetTableInfoList = '/dataBase/tableInfoList',
  AddTable = '/table/add',
  UpdateTable = '/table/edit',
  DeleTetable = '/table/delete',
  AddColumn = '/column/add',
  UpdateColumn = '/column/edit',
  DeleteColumn = '/column/delete',
  CreateEntity = '/table/createEntity',

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

  // 租户接口
  GetTenantPage = '/sysTenant/page',
  AddTenant = '/sysTenant/add',
  DeleteTenant = '/sysTenant/delete',
  UpdateTenant = '/sysTenant/edit',
  GrantTenantMenu = '/sysTenant/GrantMenu',
  TenantOwnMenuList = '/sysTenant/ownMenu',
  ResetTenantPwd = '/sysTenant/resetPwd',

  // 数据资源接口
  DataResourceList = '/sysDataResource/list',
  AddDataResource = '/sysDataResource/add',
  DeleteDataResource = '/sysDataResource/delete',
  UpdateDataResource = '/sysDataResource/update',

  //常量下拉框接口
  AllConstSelector = '/constSelector/allConstSelector',
  ConstSelector = '/constSelector/constSelector',
}

////////// 账号管理接口 //////////
// 获取账号分页列表
export const getUserPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.UserPageList, params });
// 获取账号列表
export const getUserList = (params?: any) => defHttp.get<any>({ url: Api.RoleList, params });
// 增加账号
export const addUser = (params: any) => defHttp.post({ url: Api.AddUser, params });
// 删除账号
export const deleteUser = (id: number) => defHttp.post({ url: Api.DeleteUser, params: { id } });
// 更新账号
export const updateUser = (params: any) => defHttp.post({ url: Api.UpdateUser, params });
// 设置账号状态
export const setUserStatus = (id: number, status: number) =>
  defHttp.post({ url: Api.SetUserStatus, params: { id, status } });
// 获取账号拥有角色列表
export const userOwnRoleList = (id: number) =>
  defHttp.get<any>({ url: Api.UserOwnRoleList, params: { id } });
// 获取账号拥有机构列表(数据范围)
export const userOwnOrgList = (id: number) =>
  defHttp.get<any>({ url: Api.UserOwnOrgList, params: { id } });
// 授权账号角色
export const grantUserRole = (params: any) => defHttp.post({ url: Api.GrantUserRole, params });
// 授权账号机构
export const grantUserOrg = (params: any) => defHttp.post({ url: Api.GrantUserOrg, params });
// 重置账号密码
export const resetUserPwd = (id: number) => defHttp.post({ url: Api.ResetUserPwd, params: { id } });
// 修改账号密码
export const changeUserPwd = (params: any) => defHttp.post({ url: Api.ChangeUserPwd, params });

////////// 角色管理接口 //////////
// 获取角色分页列表
export const getRolePageList = (params?: any) =>
  defHttp.get<any>({ url: Api.RolePageList, params });
// 获取角色列表
export const getRoleList = (params?: any) => defHttp.get<any>({ url: Api.RoleList, params });
// 增加角色
export const addRole = (params: any) => defHttp.post({ url: Api.AddRole, params });
// 删除角色
export const deleteRole = (id: number) => defHttp.post({ url: Api.DeleteRole, params: { id } });
// 更新角色
export const updateRole = (params: any) => defHttp.post({ url: Api.UpdateRole, params });
// 设置角色状态
export const setRoleStatus = (id: number, status: number) =>
  defHttp.post({ url: Api.SetRoleStatus, params: { id, status } });
// 获取角色拥有菜单列表
export const ownMenuList = (id: number) =>
  defHttp.get<any>({ url: Api.RoleOwnMenuList, params: { id } });
// 获取角色拥有机构列表(数据范围)
export const ownOrgList = (id: number) =>
  defHttp.get<any>({ url: Api.RoleOwnOrgList, params: { id } });
// 授权角色菜单
export const grantRoleMenu = (params: any) => defHttp.post({ url: Api.GrantRoleMenu, params });
// 授权角色数据
export const grantRoleData = (params: any) => defHttp.post({ url: Api.GrantRoleData, params });

////////// 菜单管理接口 //////////
// 获取菜单列表
export const getMenuList = (params?: any) => defHttp.get<any>({ url: Api.MenuList, params });
// 增加菜单
export const addMenu = (params: any) => defHttp.post({ url: Api.AddMenu, params });
// 删除菜单
export const deleteMenu = (id: number) => defHttp.post({ url: Api.DeleteMenu, params: { id } });
// 更新菜单
export const updateMenu = (params: any) => defHttp.post({ url: Api.UpdateMenu, params });

////////// 机构管理接口 //////////
// 获取机构列表
export const getOrgList = (params?: any) => defHttp.get<any>({ url: Api.OrgList, params });
// 增加机构
export const addOrg = (params: any) => defHttp.post({ url: Api.AddOrg, params });
// 删除机构
export const deleteOrg = (id: number) => defHttp.post({ url: Api.DeleteOrg, params: { id } });
// 更新机构
export const updateOrg = (params: any) => defHttp.post({ url: Api.UpdateOrg, params });

////////// 职位管理接口 //////////
// 获取职位列表
export const getPosList = (params?: any) => defHttp.get<any>({ url: Api.PosList, params });
// 增加职位
export const addPos = (params: any) => defHttp.post({ url: Api.AddPos, params });
// 删除职位
export const deletePos = (id: number) => defHttp.post({ url: Api.DeletePos, params: { id } });
// 更新职位
export const updatePos = (params: any) => defHttp.post({ url: Api.UpdatePos, params });

////////// 日志管理接口 //////////
// 获取访问日志分页列表
export const getVisLogPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.VislogPageList, params });
// 清空访问日志
export const clearVisLog = () => defHttp.post({ url: Api.ClearVisLog });

// 获取操作日志分页列表
export const getOpLogPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.OplogPageList, params });
// 清空操作日志
export const clearOpLog = () => defHttp.post({ url: Api.ClearOpLog });

// 获取异常日志分页列表
export const getExLogPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.ExlogPageList, params });
// 清空异常日志
export const clearExLog = () => defHttp.post({ url: Api.ClearExLog });

// 获取差异日志分页列表
export const getDiffLogPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.DifflogPageList, params });
// 清空差异日志
export const clearDiffLog = () => defHttp.post({ url: Api.ClearDiffLog });

////////// 文件管理接口 //////////
// 获取文件分页列表
export const getFilePageList = (params?: any) =>
  defHttp.get<any>({ url: Api.FilePageList, params });
// 上传文件
import { uploadFileApi } from '/@/api/sys/upload';
export const uploadFile = uploadFileApi;
// 下载文件
export const downloadFile = (id: number) => defHttp.post({ url: Api.DownloadFile, params: { id } });
// 删除文件
export const deleteFile = (id: number) => defHttp.post({ url: Api.DeleteFile, params: { id } });

////////// 系统配置管理接口 //////////
// 获取配置分页列表
export const getConfigPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.ConfigPageList, params });
// 增加配置
export const addConfig = (params: any) => defHttp.post({ url: Api.AddConfig, params });
// 删除配置
export const deleteConfig = (id: number) => defHttp.post({ url: Api.DeleteConfig, params: { id } });
// 更新配置
export const updateConfig = (params: any) => defHttp.post({ url: Api.UpdateConfig, params });

////////// 定时任务管理接口 //////////
// 获取定时任务分页列表
export const getTimerPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.TimerPageList, params });
// 增加定时任务
export const addTimer = (params: any) => defHttp.post({ url: Api.AddTimer, params });
// 删除定时任务
export const deleteTimer = (id: number) => defHttp.post({ url: Api.DeleteTimer, params: { id } });
// 更新定时任务
export const updateTimer = (params: any) => defHttp.post({ url: Api.UpdateTimer, params });
// 设置定时任务状态
export const setTimerStatus = (timerName: string, status: number) =>
  defHttp.post({ url: Api.SetTimerStatus, params: { timerName, status } });

////////// 服务器监控管理接口 //////////
// 获取服务器基本配置
export const serverBase = () => defHttp.get<any>({ url: Api.ServerBase });
// 获取服务器资源使用
export const serverUse = () => defHttp.get<any>({ url: Api.ServerUse });
// 获取服务器网络信息
export const serverNetWork = () => defHttp.get<any>({ url: Api.ServerNetWork });

////////// 字典类型管理接口 //////////
// 获取字典类型列表
export const getDictTypePageList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetDictTypePageList, params });
export const getDictTypeList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetDictTypeList, params });
// 增加典类型
export const addDictType = (params: any) => defHttp.post({ url: Api.AddDictType, params });
// 删除字典类型
export const deleteDictType = (id: number) =>
  defHttp.post({ url: Api.DeleteDictType, params: { id } });
// 更新字典类型
export const updateDictType = (params: any) => defHttp.post({ url: Api.UpdateDictType, params });

////////// 字典值管理接口 //////////
// 获取字典类型分页列表
export const getDictDataList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetDictDataPageList, params });
// 从字典中值，下拉框控件使用
export const getDictDataDropdown = (params: any) =>
  defHttp.get<any>({ url: Api.GetDictDataDropdown + '/' + params });
// 增加典类型
export const addDictData = (params: any) => defHttp.post({ url: Api.AddDictData, params });
// 删除字典类型
export const deleteDictData = (id: number) =>
  defHttp.post({ url: Api.DeleteDictData, params: { id } });
// 更新字典类型
export const updateDictData = (params: any) => defHttp.post({ url: Api.UpdateDictData, params });

//////////数据库管理接口 //////////
// 获取表字段
export const getColumnInfoList = (params?) =>
  defHttp.get<any>({ url: Api.GetColumnInfoList, params });
// 获取所有表
export const getTableInfoList = (params?) =>
  defHttp.get<any>({ url: Api.GetTableInfoList, params });
// 添加表
export const addTable = (params: any) => defHttp.post<any>({ url: Api.AddTable, params });
// 修改表
export const updateTable = (params: any) => defHttp.post<any>({ url: Api.UpdateTable, params });
// 删除表
export const deleteTable = (params: any) => defHttp.post<any>({ url: Api.DeleTetable, params });
// 添加字段
export const addColumn = (params: any) => defHttp.post<any>({ url: Api.AddColumn, params });
// 修改字段
export const updateColumn = (params: any) => defHttp.post<any>({ url: Api.UpdateColumn, params });
// 删除字段
export const deleteColumn = (params: any) => defHttp.post<any>({ url: Api.DeleteColumn, params });
// 生成实体
export const createEntity = (params: any) => defHttp.post<any>({ url: Api.CreateEntity, params });

////////// 代码生成接口 //////////
// 分页查询代码生成
export const getGeneratePage = (params?: any) =>
  defHttp.get<any>({ url: Api.GetGeneratePage, params });
// 增加代码生成
export const addGenerate = (params: any) =>
  defHttp.post<any>({
    url: Api.AddGenerate,
    params,
  });
// 修改代码生成
export const updateGenerate = (params: any) =>
  defHttp.post<any>({
    url: Api.UpdateGenerate,
    params,
  });
// 删除代码生成
export const deleGenerate = (params: any) =>
  defHttp.post<any>({
    url: Api.DeleGenerate,
    params,
  });
// 获取数据库(上下文定位器)集合
export const getDatabaseList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetDatabaseList, params });
// 获取数据库表(实体)集合
export const getTableList = (dbConfigId: string) =>
  defHttp.get<any>({ url: Api.GetTableList + '/' + dbConfigId });
// 根据表名获取列
export const getColumnList = (dbConfigId: string, tableName: string) =>
  defHttp.get<any>({ url: Api.GetColumnList + '/' + dbConfigId + '/' + tableName });
// 本地生成
export const generateRunLocal = (params: any) =>
  defHttp.post<any>({
    url: Api.GenerateRunLocal,
    params,
  });

// 代码生成详细配置列表
export const getGenerateConfigList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetGenerateConfigList, params });

// 编辑代码生成详细配置
export const updateGenerateConfig = (params: any) =>
  defHttp.post<any>({
    url: Api.UpdateGenerateConfig,
    params,
  });

////////// 租户管理接口 //////////
// 分页查询租户
export const getTenantPageList = (params?: any) =>
  defHttp.get<any>({ url: Api.GetTenantPage, params });
// 增加租户
export const addTenant = (params: any) => defHttp.post<any>({ url: Api.AddTenant, params });
// 删除租户
export const deleteTenant = (id: number) => defHttp.post({ url: Api.DeleteTenant, params: { id } });
// 编辑租户
export const updateTenant = (params: any) => defHttp.post<any>({ url: Api.UpdateTenant, params });
// 授权租户菜单
export const grantTenantMenu = (params?: any) =>
  defHttp.post<any>({ url: Api.GrantTenantMenu, params });
// 获取租户菜单
export const tenantOwnMenuList = (id: number) =>
  defHttp.get<any>({ url: Api.TenantOwnMenuList, params: { id } });
// 重置租户密码
export const resetTenantPwd = (id: number) =>
  defHttp.post<any>({ url: Api.ResetTenantPwd, params: { id } });

////////// 数据资源管理接口 //////////
// 获取数据资源列表
export const getDataResourceList = (params?: any) =>
  defHttp.get<any>({ url: Api.DataResourceList, params });
// 增加数据资源
export const addDataResource = (params: any) => defHttp.post({ url: Api.AddDataResource, params });
// 删除数据资源
export const deleteDataResource = (id: number) =>
  defHttp.post({ url: Api.DeleteDataResource, params: { id } });
// 更新数据资源
export const updateDataResource = (params: any) =>
  defHttp.post({ url: Api.UpdateDataResource, params });

////////// 常量下拉框管理接口 //////////
// 获取所有常量下拉框列表
export const getAllConstSelector = () => defHttp.get<any>({ url: Api.AllConstSelector });
// 根据类名获取下拉框数据
export const getConstSelector = (typeName?: string) =>
  defHttp.get<any>({ url: Api.ConstSelector, params: { typeName } });
