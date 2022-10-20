import { createApp } from 'vue';
import pinia from '/@/stores/index';
import App from './App.vue';
import router from './router';
import { directive } from '/@/utils/directive';
import { i18n } from '/@/i18n/index';
import other from '/@/utils/other';

import ElementPlus from 'element-plus';
import VForm3 from 'vform3-builds'; // 引入VForm 3库
import 'element-plus/dist/index.css';
import 'vform3-builds/dist/designer.style.css'; // 引入VForm3样式
import '/@/theme/index.scss';
import mitt from 'mitt';
import VueGridLayout from 'vue-grid-layout';

const app = createApp(App);

directive(app);
other.elSvg(app);

app.use(pinia).use(router).use(ElementPlus, { i18n: i18n.global.t }).use(VForm3).use(i18n).use(VueGridLayout).mount('#app');

const globalProperties = {
	mittBus: mitt(),
	i18n,
};

// 必须合并vue默认的变量，否则有问题
app.config.globalProperties = Object.assign(app.config.globalProperties, globalProperties);
