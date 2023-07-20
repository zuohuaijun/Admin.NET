<template>
	<el-row :gutter="8" style="margin-bottom: 5px">
		<el-col :span="4">
			<!-- 流程下拉 模板选择 -->
			<el-select v-model="state.mode" showSearch @change="changeMode" :defaultValue="0" option-label-prop="label" class="w100">
				<el-option v-for="(opt, idx) in state.modeList" :key="idx" :label="opt.name" :value="idx">
					{{ opt.name }}
				</el-option>
			</el-select>
		</el-col>

		<el-col :span="20">
			<!-- 纸张设置 -->
			<el-button-group style="margin: 0 2px">
				<el-button v-for="(value, type) in state.paperTypes" :type="curPaperType === type ? 'primary' : ''" @click="setPaper(type, value)" :key="type">
					{{ type }}
				</el-button>
				<el-popover v-model="state.paperPopVisible" placement="bottom" width="300" title="设置纸张宽高(mm)">
					<div style="display: flex; align-items: center; justify-content: space-between; margin-bottom: 10px">
						<el-input type="number" v-model="state.paperWidth" style="width: 100px; text-align: center" place="宽（mm）"></el-input>~
						<el-input type="number" v-model="state.paperHeight" style="width: 100px; text-align: center" place="高（mm）"></el-input>
					</div>
					<div>
						<el-button type="primary" style="width: 100%" @click="otherPaper">确定</el-button>
					</div>
					<template #reference>
						<el-button :type="'other' == curPaperType ? 'primary' : ''" style="margin: 0 10px">自定义宽高</el-button>
					</template>
				</el-popover>
			</el-button-group>
			<el-input-number style="margin-right: 8px" :value="state.scaleValue" :precision="2" :step="0.1" :min="state.scaleMin" :max="state.scaleMax" @change="changeScale"></el-input-number>

			<el-button-group>
				<el-tooltip content="左对齐" placement="bottom">
					<el-button icon="ele-Back" @click="setElsAlign('left')"> </el-button>
				</el-tooltip>
				<el-tooltip content="居中" placement="bottom">
					<el-button icon="ele-FullScreen" @click="setElsAlign('vertical')"> </el-button>
				</el-tooltip>
				<el-tooltip content="右对齐" placement="bottom">
					<el-button icon="ele-Right" @click="setElsAlign('right')"> </el-button>
				</el-tooltip>
				<el-tooltip content="顶对齐" placement="bottom">
					<el-button icon="ele-Top" @click="setElsAlign('top')"> </el-button>
				</el-tooltip>
				<el-tooltip content="垂直居中" placement="bottom">
					<el-button icon="ele-DCaret" @click="setElsAlign('horizontal')"> </el-button>
				</el-tooltip>
				<el-tooltip content="底对齐" placement="bottom">
					<el-button icon="ele-Bottom" @click="setElsAlign('bottom')"> </el-button>
				</el-tooltip>
				<el-tooltip content="横向分散" placement="bottom">
					<el-button icon="ele-Sort" @click="setElsAlign('distributeHor')"> </el-button>
				</el-tooltip>
				<el-tooltip content="纵向分散" placement="bottom">
					<el-button icon="ele-Switch" @click="setElsAlign('distributeVer')"> </el-button>
				</el-tooltip>
			</el-button-group>

			<!-- 预览/打印 -->
			<el-button-group style="margin-left: 8px">
				<el-button type="primary" icon="ele-RefreshRight" @click="rotatePaper"> 旋转 </el-button>
				<el-button type="primary" icon="ele-View" @click="preView"> 预览 </el-button>
				<el-button type="primary" icon="ele-Printer" @click="print"> 直接打印 </el-button>
				<el-button type="primary" icon="ele-CircleCheck" @click="viewJson"> 模板JSON </el-button>
				<el-button type="danger" icon="ele-Delete" @click="clearPaper"> 清空模板 </el-button>
			</el-button-group>
		</el-col>
	</el-row>

	<el-row :gutter="8">
		<el-col :span="4">
			<el-card style="height: 100%" shadow="never">
				<el-row>
					<el-col :span="24" id="hiprintEpContainer" class="rect-printElement-types hiprintEpContainer"> </el-col>
				</el-row>
			</el-card>
		</el-col>
		<el-col :span="14">
			<el-card shadow="never" class="card-design">
				<div id="hiprint-printTemplate" class="hiprint-printTemplate"></div>
			</el-card>
		</el-col>
		<el-col :span="6" class="params_setting_container">
			<el-card shadow="never">
				<el-row class="hinnn-layout-sider">
					<div id="PrintElementOptionSetting"></div>
				</el-row>
			</el-card>
		</el-col>
	</el-row>

	<el-drawer title="打印模板" v-model="state.templateDialogVisible">
		<vue-json-pretty :data="state.templateContent" showLength showIcon showLineNumber showSelectController />
	</el-drawer>

	<!-- 预览 -->
	<PrintPreview ref="preViewRef" />
</template>

<script lang="ts" setup name="hiprintDesign">
import { computed, onMounted, ref, reactive } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import VueJsonPretty from 'vue-json-pretty';
import 'vue-json-pretty/lib/styles.css';

import { hiprint } from 'vue-plugin-hiprint';
import providers from './providers';
import PrintPreview from './preview.vue';
import printData from './print-data';

let hiprintTemplate = ref();

const preViewRef = ref();
const state = reactive({
	// 模板选择
	mode: 0,
	modeList: [] as any,
	// 当前纸张
	curPaper: {
		type: 'A4',
		width: 220,
		height: 296.6,
	},
	// 纸张类型
	paperTypes: {
		A3: {
			width: 420,
			height: 296.6,
		},
		A4: {
			width: 210,
			height: 296.6,
		},
		A5: {
			width: 210,
			height: 147.6,
		},
		B3: {
			width: 500,
			height: 352.6,
		},
		B4: {
			width: 250,
			height: 352.6,
		},
		B5: {
			width: 250,
			height: 175.6,
		},
	},
	scaleValue: 1,
	scaleMax: 5,
	scaleMin: 0.5,
	// 自定义纸张
	paperPopVisible: false,
	paperWidth: 220,
	paperHeight: 80,

	templateDialogVisible: false,
	templateContent: '',
});

// 计算当前纸张类型
const curPaperType = computed(() => {
	let type = 'other';
	let types: any = state.paperTypes;
	for (const key in types) {
		let item = types[key];
		let { width, height } = state.curPaper;
		if (item.width === width && item.height === height) {
			type = key;
		}
	}
	return type;
});

// 选择模板
const changeMode = () => {
	let provider = providers[state.mode];
	hiprint.init({
		providers: [provider.f],
	});

	// 渲染自定义选项
	const hiprintEpContainerEl = document.getElementById('hiprintEpContainer');
	if (hiprintEpContainerEl) {
		hiprintEpContainerEl.innerHTML = '';
	}
	hiprint.PrintElementTypeManager.build('.hiprintEpContainer', provider.value);

	// 渲染绘画模板
	const hiprintPrintTemplate = document.getElementById('hiprint-printTemplate');
	if (hiprintPrintTemplate) {
		hiprintPrintTemplate.innerHTML = '';
	}
	// 初始化打印模板设计器
	let template = {};
	hiprintTemplate.value = new hiprint.PrintTemplate({
		template: template,
		settingContainer: '#PrintElementOptionSetting',
		paginationContainer: '.hiprint-printPagination',
	});
	hiprintTemplate.value.design('#hiprint-printTemplate');
	// 获取当前放大比例, 当zoom时传true才会有
	state.scaleValue = hiprintTemplate.value.editingPanel.scale || 1;
};

/**
 * 设置纸张大小
 * @param type [A3, A4, A5, B3, B4, B5, other]
 * @param value {width,height} mm
 */
const setPaper = (type: string, value: { width: number; height: number }) => {
	try {
		if (Object.keys(state.paperTypes).includes(type)) {
			state.curPaper = { type: type, width: value.width, height: value.height };
			hiprintTemplate.value.setPaper(value.width, value.height);
		} else {
			state.curPaper = { type: 'other', width: value.width, height: value.height };
			hiprintTemplate.value.setPaper(value.width, value.height);
		}
	} catch (error) {
		ElMessage.error(`操作失败: ${error}`);
	}
};

// 改变缩放比例
const changeScale = (currentValue: number, oldValue: number) => {
	let big = false;
	currentValue <= oldValue ? (big = false) : (big = true);

	let scaleVal = state.scaleValue;
	if (big) {
		scaleVal += 0.1;
		if (scaleVal > state.scaleMax) scaleVal = 5;
	} else {
		scaleVal -= 0.1;
		if (scaleVal < state.scaleMin) scaleVal = 0.5;
	}
	if (hiprintTemplate.value) {
		// scaleVal: 放大缩小值, false: 不保存(不传也一样), 如果传 true, 打印时也会放大
		hiprintTemplate.value.zoom(scaleVal);
		state.scaleValue = scaleVal;
	}
};

// 旋转模板
const rotatePaper = () => {
	if (hiprintTemplate.value) {
		hiprintTemplate.value.rotatePaper();
	}
};

// 对齐模板
const setElsAlign = (e: any) => {
	hiprintTemplate.value.setElsAlign(e);
};

// 清空模板
const clearPaper = () => {
	ElMessageBox.confirm('是否确认清空模板信息?', '警告', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(() => {
			try {
				hiprintTemplate.value.clear();
			} catch (error) {
				ElMessage.error(`操作失败: ${error}`);
			}
		})
		.catch((err) => {
			console.log(err);
		});
};

// 自定义纸张
const otherPaper = () => {
	let value = {
		width: 0,
		height: 0,
	};
	value.width = state.paperWidth;
	value.height = state.paperHeight;
	state.paperPopVisible = false;
	setPaper('other', value);
};

// 预览
const preView = () => {
	let { width } = state.curPaper;
	preViewRef.value.showDialog(hiprintTemplate.value, printData, width);
};
// 直接打印
const print = () => {
	console.log('直接打印');
};

// 查看模板JSON
const viewJson = () => {
	if (hiprintTemplate.value) {
		var templateJson = JSON.stringify(hiprintTemplate.value.getJson() || {});
		state.templateContent = JSON.parse(templateJson);
		state.templateDialogVisible = true;
	}
};

onMounted(() => {
	state.modeList = providers.map((e) => {
		return { type: e.type, name: e.name, value: e.value };
	});
	changeMode();
	// otherPaper(); // 默认纸张
});

// 导出对象
defineExpose({ hiprintTemplate });
</script>

<style lang="scss" scoped>
:deep(.rect-printElement-types .hiprint-printElement-type > li > ul > li > a) {
	// padding: 4px 4px;
	//color: #1296db;
	// line-height: 1;
	height: auto;
	text-overflow: ellipsis;
	color: var(--el-color-primary);
	box-shadow: none !important;
	border: 1px dashed var(--el-color-primary);
}

// 默认图片
:deep(.hiprint-printElement-image-content) {
	img {
		content: url('~@/assets/logo.png');
	}
}

// 设计容器
.card-design {
	overflow: hidden;
	overflow-x: auto;
	overflow-y: auto;
}

:deep(.hiprint-option-item-submitBtn) {
	background: var(--el-color-primary);
}
:deep(.hiprint-option-item-deleteBtn) {
	background: var(--el-color-danger);
}
:deep(.prop-tabs .prop-tab-items li.active) {
	color: var(--el-color-primary);
	border-bottom: 2px solid var(--el-color-primary);
}
</style>
