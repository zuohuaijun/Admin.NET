import { defineStore } from 'pinia';
import { UserInfosStates } from './interface';
import { Session } from '/@/utils/storage';
import { getAPI } from '../utils/axios-utils';
import { SysAuthApi } from '../api-services/api';

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
	state: (): UserInfosStates => ({
		userInfos: {
			userName: '',
			photo: '',
			time: 0,
			roles: [],
			authBtnList: [],
		},
	}),
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
		async getApiUserInfo() {
			return new Promise((resolve) => {
				getAPI(SysAuthApi)
					.getUserInfoGet()
					.then((res: any) => {
						if (res.data.result == null) return;
						var d = res.data.result;
						const userInfos = {
							userName: d.username,
							photo: d.avatar ? d.avatar : '/favicon.ico',
							time: new Date().getTime(),
							roles: d.roles,
							authBtnList: d.buttons,
						};
						resolve(userInfos);
					});
			});
		},
	},
});
