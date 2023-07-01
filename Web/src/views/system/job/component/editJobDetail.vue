<template>
	<div class="sys-jobDetail-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="900px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-tabs v-model="state.selectedTabName">
				<el-tab-pane label="作业信息">
					<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" style="height: 500px">
						<el-row :gutter="35">
							<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
								<el-form-item label="作业编号" prop="jobId" :rules="[{ required: true, message: '作业编号不能为空', trigger: 'blur' }]">
									<el-input v-model="state.ruleForm.jobId" placeholder="作业编号" :disabled="isEdit" clearable />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
								<el-form-item label="组名称" prop="groupName" :rules="[{ required: true, message: '组名称不能为空', trigger: 'blur' }]">
									<el-input v-model="state.ruleForm.groupName" placeholder="组名称" clearable />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
								<el-form-item label="创建类型">
									<el-radio-group v-model="state.ruleForm.createType" :disabled="isEdit">
										<el-radio :label="JobCreateTypeEnum.NUMBER_0" v-show="isEdit">内置</el-radio>
										<el-radio :label="JobCreateTypeEnum.NUMBER_1">脚本</el-radio>
										<el-radio :label="JobCreateTypeEnum.NUMBER_2">Http请求</el-radio>
									</el-radio-group>
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
								<el-form-item label="执行方式">
									<el-radio-group v-model="state.ruleForm.concurrent">
										<el-radio :label="true">并行</el-radio>
										<el-radio :label="false">串行</el-radio>
									</el-radio-group>
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" v-show="!isEdit && !isHttpCreateType">
								<el-form-item prop="includeAnnotations">
									<template v-slot:label>
										<div>
											扫描触发器
											<el-tooltip raw-content content="此参数只在新增作业时生效<br/>扫描定义在作业上的触发器" placement="top">
												<SvgIcon name="fa fa-question-circle-o" :size="16" style="vertical-align: middle" />
											</el-tooltip>
										</div>
									</template>
									<el-radio-group v-model="state.ruleForm.includeAnnotations">
										<el-radio :label="true">是</el-radio>
										<el-radio :label="false">否</el-radio>
									</el-radio-group>
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
								<el-form-item label="描述信息">
									<el-input v-model="state.ruleForm.description" placeholder="描述信息" clearable type="textarea" :autosize="{ minRows: 1, maxRows: 3 }" />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" v-if="!isHttpCreateType">
								<el-form-item label="额外数据">
									<el-input v-model="state.ruleForm.properties" placeholder="额外数据" clearable type="textarea" :autosize="{ minRows: 3, maxRows: 6 }" />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" v-if="isHttpCreateType">
								<el-form-item label="请求地址">
									<el-input v-model="state.httpJobMessage.requestUri" placeholder="请求地址" clearable />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" v-if="isHttpCreateType">
								<el-form-item label="请求方法">
									<el-radio-group v-model="state.httpJobMessage.httpMethod">
										<el-radio :label="httpMethodDef.get">Get</el-radio>
										<el-radio :label="httpMethodDef.post">Post</el-radio>
										<el-radio :label="httpMethodDef.put">Put</el-radio>
										<el-radio :label="httpMethodDef.delete">Delete</el-radio>
									</el-radio-group>
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" v-if="isHttpCreateType">
								<el-form-item label="请求报文体">
									<el-input v-model="state.httpJobMessage.body" placeholder="请求报文体" clearable type="textarea" :autosize="{ minRows: 3, maxRows: 6 }" />
								</el-form-item>
							</el-col>
						</el-row>
					</el-form>
				</el-tab-pane>
				<el-tab-pane label="脚本代码" :disabled="!isScriptCreateType">
					<div ref="monacoEditorRef" style="width: 100%; height: 500px"></div>
				</el-tab-pane>
			</el-tabs>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel">取 消</el-button>
					<el-button type="primary" @click="submit">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditJobDetail">
import { reactive, ref, computed } from 'vue';
import * as monaco from 'monaco-editor';
import { JobScriptCode } from './JobScriptCode';

import { getAPI } from '/@/utils/axios-utils';
import { SysJobApi } from '/@/api-services/api';
import { JobCreateTypeEnum, UpdateJobDetailInput } from '/@/api-services/models';

// HttpMethod 定义，来源后端 HttpMethod 对象的序列化
// 下面定义内容【不要】加空格，否则 getHttpJobMessage 中 JSON.stringify(httpJobMessageNet.HttpMethod) 后无法匹配
const httpMethodDef = {
	get: '{"Method":"GET"}',
	post: '{"Method":"POST"}',
	put: '{"Method":"PUT"}',
	delete: '{"Method":"DELETE"}',
};

const props = defineProps({
	title: String,
});
const emits = defineEmits(['handleQuery']);
const ruleFormRef = ref();
const monacoEditorRef = ref();
const state = reactive({
	isShowDialog: false,
	selectedTabName: '0', // 选中的 tab 页
	ruleForm: {} as UpdateJobDetailInput,
	httpJobMessage: { requestUri: '', httpMethod: httpMethodDef.get, body: '' } as HttpJobMessage,
});

// 是否编辑状态
const isEdit = computed(() => {
	return state.ruleForm.id != undefined && state.ruleForm.id > 0;
});

// 是否脚本创建类型
const isScriptCreateType = computed(() => {
	return state.ruleForm.createType === JobCreateTypeEnum.NUMBER_1;
});

// 是否Http请求创建类型
const isHttpCreateType = computed(() => {
	return state.ruleForm.createType === JobCreateTypeEnum.NUMBER_2;
});

// 初始化monacoEditor对象
var monacoEditor: any = null;
const initMonacoEditor = () => {
	monacoEditor = monaco.editor.create(monacoEditorRef.value, {
		theme: 'vs-dark', // 主题 vs vs-dark hc-black
		value: '', // 默认显示的值
		language: 'csharp',
		formatOnPaste: true,
		wordWrap: 'on', //自动换行，注意大小写
		wrappingIndent: 'indent',
		folding: true, // 是否折叠
		foldingHighlight: true, // 折叠等高线
		foldingStrategy: 'indentation', // 折叠方式  auto | indentation
		showFoldingControls: 'always', // 是否一直显示折叠 always | mouSEOver
		disableLayerHinting: true, // 等宽优化
		emptySelectionClipboard: false, // 空选择剪切板
		selectionClipboard: false, // 选择剪切板
		automaticLayout: true, // 自动布局
		codeLens: false, // 代码镜头
		scrollBeyondLastLine: false, // 滚动完最后一行后再滚动一屏幕
		colorDecorators: true, // 颜色装饰器
		accessibilitySupport: 'auto', // 辅助功能支持  "auto" | "off" | "on"
		lineNumbers: 'on', // 行号 取值： "on" | "off" | "relative" | "interval" | function
		lineNumbersMinChars: 5, // 行号最小字符   number
		//enableSplitViewResizing: false,
		readOnly: false, //是否只读  取值 true | false
	});
};

// 打开弹窗
const openDialog = (row: any) => {
	state.selectedTabName = '0'; // 重置为第一个 tab 页
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;

	// Http请求
	if (row.id && state.ruleForm.createType === JobCreateTypeEnum.NUMBER_2) {
		state.httpJobMessage = getHttpJobMessage(state.ruleForm.properties);
	}

	// 延迟拿值防止取不到
	setTimeout(() => {
		if (monacoEditor == null) initMonacoEditor();
		monacoEditor.setValue(row.id == undefined ? JobScriptCode : state.ruleForm.scriptCode);
	}, 1);
};

// 关闭弹窗
const closeDialog = () => {
	emits('handleQuery');
	state.isShowDialog = false;
};

// 取消
const cancel = () => {
	state.isShowDialog = false;
};

// 提交
const submit = () => {
	ruleFormRef.value.validate(async (valid: boolean) => {
		if (!valid) return;

		// 脚本创建类型
		if (state.ruleForm.createType === JobCreateTypeEnum.NUMBER_1) {
			state.ruleForm.scriptCode = monacoEditor.getValue();
		} else {
			state.ruleForm.scriptCode = '';
		}

		// Http请求创建类型
		if (state.ruleForm.createType === JobCreateTypeEnum.NUMBER_2) {
			// 将 httpJobMessage 重新封装，按后端 HttpJob 序列化要求，字段要大写开头
			// HttpJob 约定读取属性为“HttpJob”的值
			const httpJobPropValue = JSON.stringify({
				RequestUri: state.httpJobMessage.requestUri,
				HttpMethod: JSON.parse(state.httpJobMessage.httpMethod + ''),
				Body: state.httpJobMessage.body,
				ClientName: 'HttpJob',
				EnsureSuccessStatusCode: true,
			});
			const prop = { HttpJob: httpJobPropValue };
			state.ruleForm.properties = JSON.stringify(prop);
		}

		if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
			await getAPI(SysJobApi).apiSysJobUpdateJobDetailPost(state.ruleForm);
		} else {
			await getAPI(SysJobApi).apiSysJobAddJobDetailPost(state.ruleForm);
		}
		closeDialog();
	});
};

// 根据任务属性获取 HttpJobMessage
const getHttpJobMessage = (properties: string | undefined | null): HttpJobMessage => {
	if (properties === undefined || properties === null || properties === '') return {};

	const propData = JSON.parse(properties);
	const httpJobMessageNet = JSON.parse(propData['HttpJob']); // 后端大写开头的 HttpJobMessage

	return {
		requestUri: httpJobMessageNet.RequestUri,
		httpMethod: JSON.stringify(httpJobMessageNet.HttpMethod),
		body: httpJobMessageNet.Body,
	};
};

// 导出对象
defineExpose({ httpMethodDef, openDialog, getHttpJobMessage });
</script>
