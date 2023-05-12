<template>
	<div class="sys-dbColumn-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="500px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> 增加列 </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="60px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="列名" prop="dbColumnName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.dbColumnName" placeholder="列名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="columnDescription" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.columnDescription" placeholder="描述" clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="主键" prop="isPrimarykey">
							<el-select v-model="state.ruleForm.isPrimarykey">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="自增" prop="isIdentity">
							<el-select v-model="state.ruleForm.isIdentity" class="m-2">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="类型" prop="dataType">
							<el-select v-model="state.ruleForm.dataType">
								<el-option v-for="item in dataTypeList" :key="item.value" :label="item.value" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="可空" prop="isNullable">
							<el-select v-model="state.ruleForm.isNullable">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="长度" prop="length">
							<el-input-number v-model="state.ruleForm.length" class="w100" controls-position="right" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="小数位" prop="decimalDigits">
							<el-input-number v-model="state.ruleForm.decimalDigits" class="w100" controls-position="right" />
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

<script lang="ts" setup name="sysAddColumn">
import { reactive, ref } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { DbColumnInput } from '/@/api-services/models';
import { dataTypeList, yesNoSelect } from '../database';

const emits = defineEmits(['handleQueryColumn']);
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as DbColumnInput,
});

// 打开弹窗
const openDialog = (addRow: DbColumnInput) => {
	state.ruleForm = addRow;
	state.isShowDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	emits('handleQueryColumn');
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
		await getAPI(SysDatabaseApi).apiSysDatabaseAddColumnPost(state.ruleForm);
		closeDialog();
	});
};

// 导出对象
defineExpose({ openDialog });
</script>
