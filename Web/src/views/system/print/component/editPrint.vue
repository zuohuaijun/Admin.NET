<template>
	<div class="sys-print-container">
		<div class="printDialog">
			<el-dialog v-model="state.isShowDialog" :show-close="false" :close-on-click-modal="false" fullscreen>
				<Designer @onDesigned="onDesigned" :autoConnect="false" theme="bumblebee" style="margin: -20px -12px 0 -20px" />
				<template #footer>
					<span class="dialog-footer">
						<el-button @click="cancel">取 消</el-button>
						<el-button type="primary" @click="submit">保存模板</el-button>
					</span>
				</template>
			</el-dialog>
		</div>

		<el-dialog v-model="state.showDialog2" draggable :close-on-click-modal="false" width="600px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span>{{ props.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="模板名称" prop="name" :rules="[{ required: true, message: '模板名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.name" placeholder="模板名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序">
							<el-input-number v-model="state.ruleForm.orderNo" placeholder="排序" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态">
							<el-radio-group v-model="state.ruleForm.status">
								<el-radio :label="1">启用</el-radio>
								<el-radio :label="2">禁用</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel2">取 消</el-button>
					<el-button type="primary" @click="submit2">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditPrint">
import { onMounted, reactive, ref } from 'vue';
import 'sv-print/dist/style.css';
import { Designer } from '@sv-print/vue3';
// import { disAutoConnect } from '@sv-print/hiprint';

import { getAPI } from '/@/utils/axios-utils';
import { SysPrintApi } from '/@/api-services/api';
import { UpdatePrintInput } from '/@/api-services/models';

const props = defineProps({
	title: String
});
const emits = defineEmits(['handleQuery']);
let svPrint = ref();
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdatePrintInput,
	showDialog2: false,
});

onMounted(async () => {
	// disAutoConnect();
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;

	if (svPrint.value != undefined) loadTemplate();
};

// 取消
const cancel = () => {
	state.isShowDialog = false;
};

// 提交
const submit = async () => {
	state.showDialog2 = true;
	if (state.ruleForm.orderNo == undefined) state.ruleForm.orderNo = 100;
	if (state.ruleForm.status == undefined) state.ruleForm.status = 1;
};

// 打印模板页面初始化
const onDesigned = (e: any) => {
	svPrint.value = e.detail;
	console.log(e);

	loadTemplate();
};

// 加载已有模板
const loadTemplate = () => {
	svPrint.value.printTemplate.clear();
	if (JSON.stringify(state.ruleForm) !== '{}') {
		svPrint.value.printTemplate.update(JSON.parse(state.ruleForm.template));
	}
};

// 取消
const cancel2 = () => {
	state.showDialog2 = false;
};

// 提交
const submit2 = async () => {
	state.ruleForm.template = JSON.stringify(svPrint.value.printTemplate.getJson());
	if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
		await getAPI(SysPrintApi).apiSysPrintUpdatePost(state.ruleForm);
	} else {
		await getAPI(SysPrintApi).apiSysPrintAddPost(state.ruleForm);
	}
	cancel();
	cancel2();
	emits('handleQuery');
};

// 导出对象
defineExpose({ openDialog });
</script>

<style lang="scss" scoped>
.printDialog {
	:deep(.el-dialog) {
		.el-dialog__header {
			display: none !important;
		}
		.el-dialog__body {
			max-height: calc(100vh - 50px) !important;
		}
	}
}
</style>
