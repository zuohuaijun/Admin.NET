<template>
	<div class="sys-dbEntity-container">
		<el-dialog v-model="isShowDialog" title="配置实体" draggable width="600px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="表名" prop="tableName" :rules="[{ required: true, message: '表名不能为空', trigger: 'blur' }]">
							<el-input disabled v-model="ruleForm.tableName" placeholder="表名" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="实体名称" prop="entityName" :rules="[{ required: true, message: '实体名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.entityName" placeholder="实体名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="基类" prop="baseClassName">
							<el-select v-model="ruleForm.baseClassName" clearable>
								<el-option label="EntityBaseId" value="EntityBaseId" />
								<el-option label="EntityBase" value="EntityBase" />
								<el-option label="EntityTenant" value="EntityTenant" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="存放位置" prop="position" clearable>
							<el-select v-model="ruleForm.position">
								<el-option label="Admin.NET.Application" value="Admin.NET.Application" />
								<el-option label="Admin.NET.Core" value="Admin.NET.Core" />
							</el-select>
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

import { createEntity } from '/@/api/system/admin';

export default defineComponent({
	name: 'sysGenEntity',
	components: {},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as any,
		});

		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm.configId = row.configId;
			state.ruleForm.tableName = row.tableName;
			state.ruleForm.baseClassName = 'EntityBase';
			state.ruleForm.position = 'Admin.NET.Application';
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

				await createEntity(state.ruleForm);
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
