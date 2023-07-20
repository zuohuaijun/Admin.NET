<template>
	<el-dialog v-model="state.dialogVisible" draggable :close-on-click-modal="false" :width="state.width + 8 + 'mm'">
		<div id="preview_content" ref="previewContentRef"></div>
		<template #footer>
			<el-button :loading="state.waitShowPrinter" type="primary" icon="ele-Printer" @click.stop="print">直接打印</el-button>
			<el-button type="primary" icon="ele-Printer" @click.stop="toPdf">导出PDF</el-button>
			<el-button key="close" @click="hideDialog"> 关闭 </el-button>
		</template>
	</el-dialog>
</template>

<script lang="ts" setup>
import { nextTick, reactive, ref } from 'vue';

const state = reactive({
	dialogVisible: false,
	waitShowPrinter: false,
	width: 0, // 纸张宽 mm
	printData: {}, // 打印数据
	hiprintTemplate: {} as any,
});

const previewContentRef = ref();

const showDialog = (hiprintTemplate: any, printData: {}, width = 210) => {
	state.dialogVisible = true;
	state.width = width;
	state.hiprintTemplate = hiprintTemplate;
	state.printData = printData;
	nextTick(() => {
		const newHtml = hiprintTemplate.getHtml(printData);
		previewContentRef.value.appendChild(newHtml[0]);
	});
};

const print = () => {
	state.waitShowPrinter = true;
	state.hiprintTemplate.print(
		state.printData,
		{},
		{
			callback: () => {
				state.waitShowPrinter = false;
			},
		}
	);
};

const toPdf = () => {
	state.hiprintTemplate.toPdf({}, 'PDF文件');
};

const hideDialog = () => {
	state.dialogVisible = false;
};

defineExpose({ showDialog });
</script>

<style lang="less" scoped>
:deep(.ant-modal-body) {
	padding: 0px;
}

:deep(.ant-modal-content) {
	margin-bottom: 24px;
}
</style>
