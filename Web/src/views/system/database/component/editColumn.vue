<template>
	<div class="sys-dbColumn-container">
		<el-dialog v-model="isShowDialog" width="600px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog', '.el-dialog__header']">
					{{ '列编辑' }}
				</div>
			</template>
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="列名称" prop="columnName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.columnName" placeholder="列名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="description" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.description" placeholder="描述" clearable type="textarea"></el-input>
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
import { UpdateDbColumnInput } from '/@/api-services/models';

export default defineComponent({
	name: 'sysEditColumn',
	components: {},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as UpdateDbColumnInput,
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
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
				await getAPI(SysDatabaseApi).sysDatabaseColumnUpdatePost(state.ruleForm);
				closeDialog();
			});
		};
		return {
			ruleFormRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			...toRefs(state),
		};
	},
});
</script>
