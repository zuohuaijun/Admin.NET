<template>
	<div class="sys-tenant-container">
		<el-dialog v-model="isShowDialog" width="769px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog', '.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="租户名称" prop="name">
							<el-input v-model="ruleForm.name" placeholder="租户名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="管理员" prop="adminName">
							<el-input v-model="ruleForm.adminName" placeholder="管理员" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="电话" prop="phone">
							<el-input v-model="ruleForm.phone" placeholder="电话" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="主机">
							<el-input v-model="ruleForm.host" placeholder="主机" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="邮箱">
							<el-input v-model="ruleForm.email" placeholder="邮箱" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="架构">
							<el-input v-model="ruleForm.schema" placeholder="架构" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="4" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="数据库连接">
							<el-input v-model="ruleForm.connection" placeholder="数据库连接" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序">
							<el-input-number v-model="ruleForm.order" placeholder="排序" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea"> </el-input>
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
import { SysTenantApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditTenant',
	components: {},
	props: {
		// 弹窗标题
		title: {
			type: String,
			default: '',
		},
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			loading: true,
			isShowDialog: false,
			ruleForm: {
				id: 0, // Id
				name: '', // 租户名称
				adminName: '', // 租户编码
				phone: '', // 电话
				host: '', // 主机
				email: '', // 邮箱
				connection: '', // 数据库连接
				schema: '', // 架构
				order: 100, // 排序
				remark: '', // 备注
				menuIdList: [] as any, // 菜单权限
			},
			ruleRules: {
				name: [{ required: true, message: '租户名称不能为空', trigger: 'blur' }],
				adminName: [{ required: true, message: '管理员不能为空', trigger: 'blur' }],
			},
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit('submitRefresh');
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
					await getAPI(SysTenantApi).sysTenantUpdatePost(state.ruleForm);
				} else {
					await getAPI(SysTenantApi).sysTenantAddPost(state.ruleForm);
				}
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

<style scoped lang="scss">
.menu-data-tree {
	width: 100%;
	border: 1px solid var(--el-border-color);
	border-radius: var(--el-input-border-radius, var(--el-border-radius-base));
	padding: 5px;
}

:deep(.penultimate-node) {
	.el-tree-node__children {
		padding-left: 60px;
		white-space: pre-wrap;
		line-height: 12px;

		.el-tree-node {
			display: inline-block;
		}

		.el-tree-node__content {
			padding-left: 5px !important;
			padding-right: 5px;

			.el-tree-node__expand-icon {
				display: none;
			}
		}
	}
}
</style>
