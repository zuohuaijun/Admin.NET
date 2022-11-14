import { createApp } from 'vue';
import pinia from '/@/stores/index';
import App from './App.vue';
import router from './router';
import { directive } from '/@/utils/directive';
import { i18n } from '/@/i18n/index';
import other from '/@/utils/other';
import { signalR } from '/@/views/system/onlineUser/signalR';

import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css';
import VForm3 from 'vform3-builds'; // VForm3表单设计
import 'vform3-builds/dist/designer.style.css';
import '/@/theme/index.scss';
import mitt from 'mitt';
import VueGridLayout from 'vue-grid-layout';
import VueSignaturePad from 'vue-signature-pad'; // 电子签名
import vue3TreeOrg from 'vue3-tree-org'; // 组织架构图
import 'vue3-tree-org/lib/vue3-tree-org.css';
import { setupConstSelectorFilter } from './utils/helper/constSelectorHelper';

const app = createApp(App);

directive(app);
other.elSvg(app);
setupConstSelectorFilter(app);

app.use(pinia).use(router).use(ElementPlus, { i18n: i18n.global.t }).use(VForm3).use(i18n).use(VueGridLayout).use(VueSignaturePad).use(vue3TreeOrg).mount('#app');

const globalProperties = {
	mittBus: mitt(),
	i18n,
	signalR,
};

// 必须合并vue默认的变量，否则有问题
app.config.globalProperties = Object.assign(app.config.globalProperties, globalProperties);
