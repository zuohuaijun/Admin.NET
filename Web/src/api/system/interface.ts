import { DbObjectType, MenuTypeEnum, StatusEnum } from './enum';

/**
 *
 * @export
 * @interface DbTableInfo
 */
export interface DbTableInfo {
	/**
	 *
	 * @type {string}
	 * @memberof DbTableInfo
	 */
	name?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof DbTableInfo
	 */
	description?: string | null;
	/**
	 *
	 * @type {DbObjectType}
	 * @memberof DbTableInfo
	 */
	// dbObjectType?: DbObjectType;
}

/**
 *
 * @export
 * @interface EditRecordRow
 */
export interface EditRecordRow {
	columnDescription?: string | null;
	dataType?: string | null;
	dbColumnName?: string | null;
	decimalDigits: number;
	isIdentity: number;
	isNullable: number;
	isPrimarykey: number;
	length: number;
	key?: number;
	editable?: boolean;
	isNew: boolean;
}

/**
 *
 * @export
 * @interface UpdateDbTableInput
 */
export interface UpdateDbTableInput {
	/**
	 *
	 * @type {string}
	 * @memberof UpdateDbTableInput
	 */
	configId?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof UpdateDbTableInput
	 */
	tableName?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof UpdateDbTableInput
	 */
	oldTableName?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof UpdateDbTableInput
	 */
	description?: string | null;
}

/**
 *
 * @export
 * @interface AddDbColumnInput
 */
export interface AddDbColumnInput {
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	configId?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	tableName?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	columnDescription?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	dataType?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	dbColumnName?: string | null;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	decimalDigits: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	isIdentity: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	isNullable: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	isPrimarykey: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	length: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	key: number;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	editable: boolean;
	/**
	 *
	 * @type {string}
	 * @memberof AddDbColumnInput
	 */
	isNew: boolean;
}

/**
 *
 * @export
 * @interface AddCodeGenInput
 */
export interface AddCodeGenInput {
	/**
	 * 当前页码
	 * @type {number}
	 * @memberof AddCodeGenInput
	 */
	page?: number;
	/**
	 * 页码容量
	 * @type {number}
	 * @memberof AddCodeGenInput
	 */
	pageSize?: number;
	/**
	 * 排序字段
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	field?: string | null;
	/**
	 * 排序方向
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	order?: string | null;
	/**
	 * 降序排序
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	descStr?: string | null;
	/**
	 * 类名
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	className?: string | null;
	/**
	 * 是否移除表前缀
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	tablePrefix?: string | null;
	/**
	 * 库定位器名
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	configId?: string | null;
	/**
	 * 数据库名(保留字段)
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	dbName?: string | null;
	/**
	 * 数据库类型
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	dbType: string;
	/**
	 * 数据库类型
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	dbTypeString?: string | null;
	/**
	 * 数据库链接
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	connectionString?: string | null;
	/**
	 * 功能名（数据库表名称）
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	tableComment?: string | null;
	/**
	 * 菜单应用分类（应用编码）
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	menuApplication?: string | null;
	/**
	 * 数据库表名
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	tableName: string;
	/**
	 * 业务名（业务代码包名称）
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	busName: string;
	/**
	 * 命名空间
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	nameSpace: string;
	/**
	 * 作者姓名
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	authorName: string;
	/**
	 * 生成方式
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	generateType: string;
	/**
	 * 菜单父级
	 * @type {number}
	 * @memberof AddCodeGenInput
	 */
	menuPid: number;
	/**
	 * 菜单父级
	 * @type {number}
	 * @memberof AddCodeGenInput
	 */
	id: number;
	/**
	 * 生成方式
	 * @type {string}
	 * @memberof AddCodeGenInput
	 */
	type: string;
}
/**
 * 代码生成表
 * @export
 * @interface SysCodeGen
 */
export interface SysCodeGen {
	/**
	 * 雪花Id
	 * @type {number}
	 * @memberof SysCodeGen
	 */
	id?: number;
	/**
	 * 创建时间
	 * @type {Date}
	 * @memberof SysCodeGen
	 */
	createTime?: Date | null;
	/**
	 * 更新时间
	 * @type {Date}
	 * @memberof SysCodeGen
	 */
	updateTime?: Date | null;
	/**
	 * 创建者Id
	 * @type {number}
	 * @memberof SysCodeGen
	 */
	createUserId?: number | null;
	/**
	 * 修改者Id
	 * @type {number}
	 * @memberof SysCodeGen
	 */
	updateUserId?: number | null;
	/**
	 * 软删除
	 * @type {boolean}
	 * @memberof SysCodeGen
	 */
	isDelete?: boolean;
	/**
	 * 作者姓名
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	authorName?: string | null;
	/**
	 * 是否移除表前缀
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	tablePrefix?: string | null;
	/**
	 * 生成方式
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	generateType?: string | null;
	/**
	 * 库定位器名
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	configId?: string | null;
	/**
	 * 数据库名(保留字段)
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	dbName?: string | null;
	/**
	 * 数据库类型
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	dbType?: string | null;
	/**
	 * 数据库链接
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	connectionString?: string | null;
	/**
	 * 数据库表名
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	tableName?: string | null;
	/**
	 * 命名空间
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	nameSpace?: string | null;
	/**
	 * 业务名
	 * @type {string}
	 * @memberof SysCodeGen
	 */
	busName?: string | null;
	/**
	 * 菜单编码
	 * @type {number}
	 * @memberof SysCodeGen
	 */
	menuPid?: number;
}

/**
 * 系统菜单表
 * @export
 * @interface SysMenu
 */
export interface SysMenu {
	/**
	 * 雪花Id
	 * @type {number}
	 * @memberof SysMenu
	 */
	id?: number;
	/**
	 * 创建时间
	 * @type {Date}
	 * @memberof SysMenu
	 */
	createTime?: Date | null;
	/**
	 * 更新时间
	 * @type {Date}
	 * @memberof SysMenu
	 */
	updateTime?: Date | null;
	/**
	 * 创建者Id
	 * @type {number}
	 * @memberof SysMenu
	 */
	createUserId?: number | null;
	/**
	 * 修改者Id
	 * @type {number}
	 * @memberof SysMenu
	 */
	updateUserId?: number | null;
	/**
	 * 软删除
	 * @type {boolean}
	 * @memberof SysMenu
	 */
	isDelete?: boolean;
	/**
	 * 父Id
	 * @type {number}
	 * @memberof SysMenu
	 */
	pid?: number;
	/**
	 *
	 * @type {MenuTypeEnum}
	 * @memberof SysMenu
	 */
	type?: MenuTypeEnum;
	/**
	 * 名称
	 * @type {string}
	 * @memberof SysMenu
	 */
	name?: string | null;
	/**
	 * 路由地址
	 * @type {string}
	 * @memberof SysMenu
	 */
	path?: string | null;
	/**
	 * 组件路径
	 * @type {string}
	 * @memberof SysMenu
	 */
	component?: string | null;
	/**
	 * 重定向
	 * @type {string}
	 * @memberof SysMenu
	 */
	redirect?: string | null;
	/**
	 * 权限标识
	 * @type {string}
	 * @memberof SysMenu
	 */
	permission?: string | null;
	/**
	 * 标题
	 * @type {string}
	 * @memberof SysMenu
	 */
	title: string;
	/**
	 * 图标
	 * @type {string}
	 * @memberof SysMenu
	 */
	icon?: string | null;
	/**
	 * 是否内嵌
	 * @type {boolean}
	 * @memberof SysMenu
	 */
	isIframe?: boolean;
	/**
	 * 外链链接
	 * @type {string}
	 * @memberof SysMenu
	 */
	outLink?: string | null;
	/**
	 * 是否隐藏
	 * @type {boolean}
	 * @memberof SysMenu
	 */
	isHide?: boolean;
	/**
	 * 是否缓存
	 * @type {boolean}
	 * @memberof SysMenu
	 */
	isKeepAlive?: boolean;
	/**
	 * 是否固定
	 * @type {boolean}
	 * @memberof SysMenu
	 */
	isAffix?: boolean;
	/**
	 * 排序
	 * @type {number}
	 * @memberof SysMenu
	 */
	order?: number;
	/**
	 *
	 * @type {StatusEnum}
	 * @memberof SysMenu
	 */
	status?: StatusEnum;
	/**
	 * 备注
	 * @type {string}
	 * @memberof SysMenu
	 */
	remark?: string | null;
	/**
	 * 菜单子项
	 * @type {Array<SysMenu>}
	 * @memberof SysMenu
	 */
	children?: Array<SysMenu> | null;
}

/* tslint:disable */
/* eslint-disable */
/**
 * Admin.NET
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/Admin.NET/'>https://gitee.com/zuohuaijun/Admin.NET</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
/**
 * 代码生成字段配置表
 * @export
 * @interface SysCodeGenConfig
 */
 export interface SysCodeGenConfig {
    /**
     * 雪花Id
     * @type {number}
     * @memberof SysCodeGenConfig
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof SysCodeGenConfig
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof SysCodeGenConfig
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof SysCodeGenConfig
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof SysCodeGenConfig
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof SysCodeGenConfig
     */
    isDelete?: boolean;
    /**
     * 代码生成主表Id
     * @type {number}
     * @memberof SysCodeGenConfig
     */
    codeGenId?: number;
    /**
     * 数据库字段名
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    columnName: string;
    /**
     * 字段描述
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    columnComment?: string | null;
    /**
     * .NET数据类型
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    netType?: string | null;
    /**
     * 作用类型（字典）
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    effectType?: string | null;
    /**
     * 外键实体名称
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    fkEntityName?: string | null;
    /**
     * 外键表名称
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    fkTableName?: string | null;
    /**
     * 外键显示字段
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    fkColumnName?: string | null;
    /**
     * 外键显示字段.NET类型
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    fkColumnNetType?: string | null;
    /**
     * 字典编码
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    dictTypeCode?: string | null;
    /**
     * 列表是否缩进（字典）
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    whetherRetract?: string | null;
    /**
     * 是否必填（字典）
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    whetherRequired?: string | null;
    /**
     * 是否是查询条件
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    queryWhether?: string | null;
    /**
     * 查询方式
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    queryType?: string | null;
    /**
     * 列表显示
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    whetherTable?: string | null;
    /**
     * 增改
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    whetherAddUpdate?: string | null;
    /**
     * 主键
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    columnKey?: string | null;
    /**
     * 数据库中类型（物理类型）
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    dataType?: string | null;
    /**
     * 是否通用字段
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    whetherCommon?: string | null;
    /**
     * 显示文本字段
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    displayColumn?: string | null;
    /**
     * 选中值字段
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    valueColumn?: string | null;
    /**
     * 父级字段
     * @type {string}
     * @memberof SysCodeGenConfig
     */
    pidColumn?: string | null;
}
