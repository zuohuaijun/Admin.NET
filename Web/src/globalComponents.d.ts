import SvgIcon from '/@/components/svgIcon/index.vue';

import GridLayout from 'vue-grid-layout';
import VueSignaturePad from 'vue-signature-pad';
import Vue3TreeOrg from 'vue3-tree-org';

// 用于 volar 插件的全局注册组件声明
declare module 'vue' {
	export interface GlobalComponents {
		SvgIcon: typeof SvgIcon;
		GridLayout: typeof GridLayout;
		Vue3TreeOrg: typeof Vue3TreeOrg.Vue3TreeOrg;
		VueSignaturePad: typeof VueSignaturePad;
	}
}

export {};
