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
import { GenderEnum } from './gender-enum';
import { JobStatusEnum } from './job-status-enum';
import { StatusEnum } from './status-enum';
import { SysOrg } from './sys-org';
import { SysPos } from './sys-pos';
import { UserTypeEnum } from './user-type-enum';
/**
 * 
 * @export
 * @interface UpdateUserInput
 */
export interface UpdateUserInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof UpdateUserInput
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof UpdateUserInput
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof UpdateUserInput
     */
    isDelete?: boolean;
    /**
     * 租户Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    tenantId?: number | null;
    /**
     * 昵称
     * @type {string}
     * @memberof UpdateUserInput
     */
    nickName?: string | null;
    /**
     * 头像
     * @type {string}
     * @memberof UpdateUserInput
     */
    avatar?: string | null;
    /**
     * 出生日期
     * @type {Date}
     * @memberof UpdateUserInput
     */
    birthday?: Date | null;
    /**
     * 
     * @type {GenderEnum}
     * @memberof UpdateUserInput
     */
    sex?: GenderEnum;
    /**
     * 邮箱
     * @type {string}
     * @memberof UpdateUserInput
     */
    email?: string | null;
    /**
     * 手机号码
     * @type {string}
     * @memberof UpdateUserInput
     */
    phone?: string | null;
    /**
     * 身份证号
     * @type {string}
     * @memberof UpdateUserInput
     */
    idCard?: string | null;
    /**
     * 个性签名
     * @type {string}
     * @memberof UpdateUserInput
     */
    signature?: string | null;
    /**
     * 个人简介
     * @type {string}
     * @memberof UpdateUserInput
     */
    introduction?: string | null;
    /**
     * 
     * @type {UserTypeEnum}
     * @memberof UpdateUserInput
     */
    userType?: UserTypeEnum;
    /**
     * 机构Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    orgId?: number;
    /**
     * 
     * @type {SysOrg}
     * @memberof UpdateUserInput
     */
    sysOrg?: SysOrg;
    /**
     * 职位Id
     * @type {number}
     * @memberof UpdateUserInput
     */
    posId?: number;
    /**
     * 
     * @type {SysPos}
     * @memberof UpdateUserInput
     */
    sysPos?: SysPos;
    /**
     * 工号
     * @type {string}
     * @memberof UpdateUserInput
     */
    jobNum?: string | null;
    /**
     * 
     * @type {JobStatusEnum}
     * @memberof UpdateUserInput
     */
    jobStatus?: JobStatusEnum;
    /**
     * 排序
     * @type {number}
     * @memberof UpdateUserInput
     */
    order?: number;
    /**
     * 
     * @type {StatusEnum}
     * @memberof UpdateUserInput
     */
    status?: StatusEnum;
    /**
     * 备注
     * @type {string}
     * @memberof UpdateUserInput
     */
    remark?: string | null;
    /**
     * 账号名称
     * @type {string}
     * @memberof UpdateUserInput
     */
    userName: string;
    /**
     * 真实姓名
     * @type {string}
     * @memberof UpdateUserInput
     */
    realName: string;
    /**
     * 角色Id集合
     * @type {Array<number>}
     * @memberof UpdateUserInput
     */
    roleIdList?: Array<number> | null;
}
