<template>
	<div class="sys-file-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="500px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto">
				<el-row>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="文件名称" prop="fileName" :rules="[{ required: true, message: '文件名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.fileName" placeholder="文件名称" clearable />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel">取 消</el-button>
					<el-button type="primary" @click="submit">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditFile">
import { reactive, ref } from 'vue';

import { ElMessage } from 'element-plus';
import { getAPI } from '/@/utils/axios-utils';
import { SysFileApi } from '/@/api-services/api';
import { FileInput } from '/@/api-services/models';

const props = defineProps({
	title: String,
	sysFileId: Number,
});
const emits = defineEmits(['handleQuery']);
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as FileInput,
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;
	ruleFormRef.value?.resetFields();
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
		if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
			await getAPI(SysFileApi)
				.apiSysFileUpdatePost(state.ruleForm)
				.then((rsp) => {
					if (rsp.data.code == 200) {
						ElMessage.success('修改文件信息成功！');
					} else {
						Elmessage.error('修改文件信息失败：' + rsp.data.message);
					}
				});
		}
		closeDialog();
	});
};

// 导出对象
defineExpose({ openDialog });
</script>
