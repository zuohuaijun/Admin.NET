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
import { LoginRole } from './login-role';
/**
 * 用户登录结果
 * @export
 * @interface LoginOutput
 */
export interface LoginOutput {
    /**
     * 用户Id
     * @type {number}
     * @memberof LoginOutput
     */
    userId?: number;
    /**
     * 
     * @type {LoginRole}
     * @memberof LoginOutput
     */
    roleInfo?: LoginRole;
    /**
     * 令牌Token
     * @type {string}
     * @memberof LoginOutput
     */
    token?: string | null;
    /**
     * 刷新Token
     * @type {string}
     * @memberof LoginOutput
     */
    refreshToken?: string | null;
}