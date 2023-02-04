<template>
	<div class="sys-dbColumn-container">
		<el-dialog v-model="state.isShowDialog" draggable width="600px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> 列编辑 </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="列名称" prop="columnName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.columnName" placeholder="列名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="description" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.description" placeholder="描述" clearable type="textarea" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditColumn">
import { reactive, ref } from 'vue';
import mittBus from '/@/utils/mitt';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { UpdateDbColumnInput } from '/@/api-services/models';

const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdateDbColumnInput,
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	mittBus.emit('submitRefreshColumn');
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
		await getAPI(SysDatabaseApi).apiSysDatabaseUpdateColumnPut(state.ruleForm);
		closeDialog();
	});
};
</script>
