<template>
	<div class="login-container">
		<div class="login-icon-group">
			<!-- <div class="login-icon-group-title">
				<img :src="logoMini" />
				<div class="login-icon-group-title-text font25">{{ getThemeConfig.globalViceTitle }}</div>
			</div> -->
			<el-carousel height="550px" style="width: 100%; padding-right: 38%; top: 50%; transform: translateY(-50%) translate3d(0, 0, 0)">
				<el-carousel-item>
					<img :src="loginIconTwo" class="login-icon-group-icon" />
				</el-carousel-item>
				<el-carousel-item>
					<img :src="loginIconTwo1" class="login-icon-group-icon" />
				</el-carousel-item>
				<el-carousel-item>
					<img :src="loginIconTwo2" class="login-icon-group-icon" />
				</el-carousel-item>
			</el-carousel>
		</div>
		<div class="login-content">
			<div class="login-content-main">
				<h4 class="login-content-title">{{ getThemeConfig.globalTitle }}</h4>
				<div v-if="!isScan">
					<el-tabs v-model="tabsActiveName">
						<el-tab-pane :label="$t('message.label.one1')" name="account">
							<Account />
						</el-tab-pane>
						<el-tab-pane :label="$t('message.label.two2')" name="mobile">
							<Mobile />
						</el-tab-pane>
					</el-tabs>
				</div>
				<Scan v-if="isScan" />
				<div class="login-content-main-sacn" @click="isScan = !isScan">
					<i class="iconfont" :class="isScan ? 'icon-diannao1' : 'icon-barcode-qr'"></i>
					<div class="login-content-main-sacn-delta"></div>
				</div>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, computed, defineComponent, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useThemeConfig } from '/@/stores/themeConfig';
import logoMini from '/@/assets/logo-mini.svg';
import loginIconTwo from '/@/assets/login-icon-two.svg';
import loginIconTwo1 from '/@/assets/login-icon-two1.svg';
import loginIconTwo2 from '/@/assets/login-icon-two2.svg';
import { NextLoading } from '/@/utils/loading';
import Account from '/@/views/login/component/account.vue';
import Mobile from '/@/views/login/component/mobile.vue';
import Scan from '/@/views/login/component/scan.vue';

// 定义接口来定义对象的类型
interface LoginState {
	tabsActiveName: string;
	isScan: boolean;
}

export default defineComponent({
	name: 'loginIndex',
	components: { Account, Mobile, Scan },
	setup() {
		const storesThemeConfig = useThemeConfig();
		const { themeConfig } = storeToRefs(storesThemeConfig);
		const state = reactive<LoginState>({
			tabsActiveName: 'account',
			isScan: false,
		});
		// 获取布局配置信息
		const getThemeConfig = computed(() => {
			return themeConfig.value;
		});
		// 页面加载时
		onMounted(() => {
			NextLoading.done();
		});
		return {
			logoMini,
			loginIconTwo,
			loginIconTwo1,
			loginIconTwo2,
			getThemeConfig,
			...toRefs(state),
		};
	},
});
</script>

<style scoped lang="scss">
.login-container {
	width: 100%;
	height: 100%;
	position: relative;
	background: var(--el-color-white);

	.login-icon-group {
		width: 100%;
		height: 100%;
		position: relative;

		.login-icon-group-title {
			position: absolute;
			top: 50px;
			left: 80px;
			display: flex;
			align-items: center;

			img {
				width: 64px;
				height: 64px;
			}

			// &-text {
			// 	padding-left: 15px;
			// 	color: var(--el-color-primary);
			// }
		}

		&-icon {
			width: 85%;
			height: 85%;
			position: absolute;
			left: 10%;
			top: 50%;
			transform: translateY(-50%) translate3d(0, 0, 0);
		}
	}

	.login-content {
		width: 500px;
		height: 500px;
		padding: 20px;
		position: absolute;
		right: 10%;
		top: 50%;
		transform: translateY(-50%) translate3d(0, 0, 0);
		background-color: var(--el-color-white);
		border: 1px solid var(--el-color-primary-light-8);
		border-radius: 5px;
		overflow: hidden;
		z-index: 1;

		.login-content-main {
			margin: 0 auto;
			width: 80%;

			.login-content-title {
				color: var(--el-text-color-primary);
				font-weight: 800;
				font-size: 32px;
				text-align: center;
				//letter-spacing: 4px;
				margin: 5px 0 20px;
				white-space: nowrap;
				z-index: 5;
				position: relative;
				transition: all 0.3s ease;
			}
		}

		.login-content-main-sacn {
			position: absolute;
			top: 0;
			right: 0;
			width: 50px;
			height: 50px;
			overflow: hidden;
			cursor: pointer;
			transition: all ease 0.3s;
			color: var(--el-text-color-primary);

			&-delta {
				position: absolute;
				width: 35px;
				height: 70px;
				z-index: 2;
				top: 2px;
				right: 21px;
				background: var(--el-color-white);
				transform: rotate(-45deg);
			}

			&:hover {
				opacity: 1;
				transition: all ease 0.3s;
				color: var(--el-color-primary) !important;
			}

			i {
				width: 47px;
				height: 50px;
				display: inline-block;
				font-size: 48px;
				position: absolute;
				right: 2px;
				top: -1px;
			}
		}
	}
}
</style>
