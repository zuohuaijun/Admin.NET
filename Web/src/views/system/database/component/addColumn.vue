<template>
	<div class="sys-dbColumn-container">
		<el-dialog v-model="isShowDialog" title="增加列" draggable :close-on-click-modal="false" width="600px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="60px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="列名" prop="dbColumnName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.dbColumnName" placeholder="列名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="columnDescription" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.columnDescription" placeholder="描述" clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="主键" prop="isPrimarykey">
							<el-select v-model="ruleForm.isPrimarykey">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="自增" prop="isIdentity">
							<el-select v-model="ruleForm.isIdentity" class="m-2">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="类型" prop="dataType">
							<el-select v-model="ruleForm.dataType">
								<el-option v-for="item in dataTypeList" :key="item.value" :label="item.value" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="可空" prop="isNullable">
							<el-select v-model="ruleForm.isNullable">
								<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="长度" prop="length">
							<el-input-number v-model="ruleForm.length" size="default" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="小数位" prop="decimalDigits">
							<el-input-number v-model="ruleForm.decimalDigits" size="default" />
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

<script lang="ts">
import { reactive, toRefs, defineComponent, getCurrentInstance, ref } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { DbColumnInput } from '/@/api-services/models';
import { dataTypeList, yesNoSelect } from '../database';

export default defineComponent({
	name: 'sysAddColumn',
	components: {},
	setup() {
		const { proxy } = getCurrentInstance() as any;
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
			proxy.mittBus.emit('submitRefreshColumn');
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
				await getAPI(SysDatabaseApi).sysDatabaseAddColumnPost(state.ruleForm);
				closeDialog();
			});
		};

		return {
			ruleFormRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			dataTypeList,
			yesNoSelect,
			...toRefs(state),
		};
	},
});
</script>
