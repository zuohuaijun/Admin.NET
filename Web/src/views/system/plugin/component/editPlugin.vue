<template>
	<div class="sys-plugin-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="900px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-tabs v-model="state.selectedTabName">
				<el-tab-pane label="插件信息">
					<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" style="height: 500px">
						<el-row :gutter="35">
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
								<el-form-item label="功能名称" prop="name" :rules="[{ required: true, message: '功能名称不能为空', trigger: 'blur' }]">
									<el-input v-model="state.ruleForm.name" placeholder="功能名称" clearable />
								</el-form-item>
							</el-col>
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
								<el-form-item label="程序集名称">
									<el-input v-model="state.ruleForm.assemblyName" placeholder="程序集名称" clearable />
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
				</el-tab-pane>
				<el-tab-pane label="C# 代码">
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

<script lang="ts" setup name="sysEditPlugin">
import { reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import * as monaco from 'monaco-editor';

import { getAPI } from '/@/utils/axios-utils';
import { SysPluginApi } from '/@/api-services/api';
import { UpdatePluginInput } from '/@/api-services/models';

const props = defineProps({
	title: String,
});
const emits = defineEmits(['handleQuery']);
const ruleFormRef = ref();
const monacoEditorRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdatePluginInput,
	selectedTabName: '0', // 选中的 tab
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
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;

	// 延迟拿值防止取不到
	setTimeout(() => {
		if (monacoEditor == null) initMonacoEditor();
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

		state.ruleForm.csharpCode = monacoEditor.getValue();
		if (state.ruleForm.csharpCode.length < 100) {
			ElMessage.warning('请正确编写 C# 代码');
			return;
		}
		if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
			await getAPI(SysPluginApi).apiSysPluginUpdatePost(state.ruleForm);
		} else {
			await getAPI(SysPluginApi).apiSysPluginAddPost(state.ruleForm);
		}
		closeDialog();
	});
};

// 导出对象
defineExpose({ openDialog });
</script>
