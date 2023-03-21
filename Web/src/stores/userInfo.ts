import { defineStore } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import Watermark from '/@/utils/watermark';
import { useThemeConfig } from '/@/stores/themeConfig';

import { getAPI } from '/@/utils/axios-utils';
import { SysAuthApi, SysConstApi } from '/@/api-services/api';

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
	state: (): UserInfosState => ({
		userInfos: {} as any,
		constList: [] as any,
	}),
	getters: {
		// 获取系统常量列表
		async getSysConstList(): Promise<any[]> {
			var res = await getAPI(SysConstApi).apiSysConstListGet();
			this.constList = res.data.result ?? [];
			return this.constList;
		},
	},
	actions: {
		async setUserInfos() {
			// 缓存用户信息
			if (Session.get('userInfo')) {
				this.userInfos = Session.get('userInfo');
			} else {
				const userInfos: any = await this.getApiUserInfo();
				this.userInfos = userInfos;
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
							account: d.account,
							realName: d.realName,
							avatar: d.avatar ? d.avatar : '/favicon.ico',
							address: d.address,
							signature: d.signature,
							orgId: d.orgId,
							orgName: d.orgName,
							posName: d.posName,
							roles: [],
							authBtnList: d.buttons,
							time: new Date().getTime(),
						};

						// 读取用户配置
						const configRes: any = await getAPI(SysAuthApi).apiSysAuthUserConfigGet();
						if (configRes.data.result == null) return;

						const configData = configRes.data.result;
						const storesThemeConfig = useThemeConfig();

						// storesThemeConfig.themeConfig.watermarkText = d.account;
						storesThemeConfig.themeConfig.isWatermark = configData.watermarkEnabled;
						Watermark.set(storesThemeConfig.themeConfig.watermarkText);

						Local.remove('themeConfig');
						Local.set('themeConfig', storesThemeConfig.themeConfig);

						resolve(userInfos);
					});
			});
		},
	},
});
