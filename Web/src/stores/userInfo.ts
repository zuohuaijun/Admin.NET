import { defineStore } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import Watermark from '/@/utils/watermark';
import { useThemeConfig } from '/@/stores/themeConfig';

import { getAPI } from '/@/utils/axios-utils';
import { SysAuthApi, SysConstApi, SysDictTypeApi } from '/@/api-services/api';

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
	state: (): UserInfosState => ({
		userInfos: {} as any,
		constList: [] as any,
		dictList: [] as any,
	}),
	getters: {
		// // 获取系统常量列表
		// async getSysConstList(): Promise<any[]> {
		// 	var res = await getAPI(SysConstApi).apiSysConstListGet();
		// 	this.constList = res.data.result ?? [];
		// 	return this.constList;
		// },
	},
	actions: {
		async setUserInfos() {
			// 存储用户信息到浏览器缓存
			if (Session.get('userInfo')) {
				this.userInfos = Session.get('userInfo');
			} else {
				const userInfos = <UserInfos>await this.getApiUserInfo();
				this.userInfos = userInfos;
			}
		},
		async setConstList() {
			// 存储常量信息到浏览器缓存
			if (Session.get('constList')) {
				this.constList = Session.get('constList');
			} else {
				const constList = <any[]>await this.getSysConstList();
				Session.set('constList', constList);
				this.constList = constList;
			}
		},
		async setDictList() {
			// 存储字典信息到浏览器缓存
			if (Session.get('dictList')) {
				this.dictList = Session.get('dictList');
			} else {
				const dictList = <any[]>await this.getAllDictList();
				Session.set('dictList', dictList);
				this.dictList = dictList;
			}
		},
		// 获取当前用户信息
		getApiUserInfo() {
			return new Promise((resolve) => {
				getAPI(SysAuthApi)
					.apiSysAuthUserInfoGet()
					.then(async (res: any) => {
						if (res.data.result == null) return;
						var d = res.data.result;
						const userInfos = {
							id: d.id,
							account: d.account,
							realName: d.realName,
							accountType: d.accountType,
							avatar: d.avatar ? '/' + d.avatar : '/favicon.ico',
							address: d.address,
							signature: d.signature,
							orgId: d.orgId,
							orgName: d.orgName,
							posName: d.posName,
							roles: [],
							authBtnList: d.buttons,
							time: new Date().getTime(),
						};
						// vue-next-admin 提交Id：225bce7 提交消息：admin-23.03.26:发布v2.4.32版本
						// 增加了下面代码，引起当前会话的用户信息不会刷新，如：重新提交的头像不更新，需要新开一个页面才能正确显示
						// Session.set('userInfo', userInfos);

						// 水印配置
						const configRes: any = await getAPI(SysAuthApi).apiSysAuthWatermarkConfigGet();
						if (configRes.data.result == null) return;

						const configData = configRes.data.result;
						const storesThemeConfig = useThemeConfig();
						storesThemeConfig.themeConfig.isWatermark = configData.watermarkEnabled;
						storesThemeConfig.themeConfig.watermarkText = userInfos.realName;
						if (storesThemeConfig.themeConfig.isWatermark) Watermark.set(storesThemeConfig.themeConfig.watermarkText);
						else Watermark.del();

						Local.remove('themeConfig');
						Local.set('themeConfig', storesThemeConfig.themeConfig);

						resolve(userInfos);
					});
			});
		},
		// 获取常量集合
		getSysConstList() {
			return new Promise((resolve) => {
				getAPI(SysConstApi)
					.apiSysConstListGet()
					.then(async (res: any) => {
						resolve(res.data.result ?? []);
					});
			});
		},
		// 获取字典集合
		getAllDictList() {
			return new Promise((resolve) => {
				getAPI(SysDictTypeApi)
					.apiSysDictTypeAllDictListGet()
					.then(async (res: any) => {
						resolve(res.data.result ?? []);
					});
			});
		},
	},
});
