<template>
	<div class="sys-role-container">
		<el-dialog v-model="isShowDialog" :title="title" draggable :close-on-click-modal="false" width="769px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="角色名称" prop="name" :rules="[{ required: true, message: '角色名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.name" placeholder="角色名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="角色编码" prop="code">
							<el-input v-model="ruleForm.code" placeholder="角色编码" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序">
							<el-input-number v-model="ruleForm.order" placeholder="排序" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态">
							<el-radio-group v-model="ruleForm.status">
								<el-radio :label="1">启用</el-radio>
								<el-radio :label="2">禁用</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="菜单权限" v-loading="loading">
							<el-tree
								ref="treeRef"
								:data="menuData"
								node-key="id"
								show-checkbox
								:props="{ children: 'children', label: 'title', class: treeNodeClass }"
								highlight-current
								class="menu-data-tree"
								icon="ele-Menu"
								check-strictly
							/>
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
import { reactive, toRefs, defineComponent, getCurrentInstance, ref, onMounted } from 'vue';
import type { ElTree } from 'element-plus';
import type Node from 'element-plus/es/components/tree/src/model/node';

import { getAPI } from '/@/utils/axios-utils';
import { SysMenuApi, SysRoleApi } from '/@/api-services/api';
import { SysMenu, UpdateRoleInput } from '/@/api-services/models';

export default defineComponent({
	name: 'sysEditRole',
	components: {},
	props: {
		title: {
			type: String,
			default: '',
		},
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const treeRef = ref<InstanceType<typeof ElTree>>();
		const state = reactive({
			loading: false,
			isShowDialog: false,
			ruleForm: {} as UpdateRoleInput,
			menuData: [] as Array<SysMenu>, // 菜单数据
		});
		onMounted(async () => {
			state.loading = true;
			var res = await getAPI(SysMenuApi).sysMenuListGet();
			state.menuData = res.data.result ?? [];
			state.loading = false;
		});
		// 打开弹窗
		const openDialog = async (row: any) => {
			state.ruleForm = row;
			if (JSON.stringify(row) !== '{}') {
				var res = await getAPI(SysRoleApi).sysRoleOwnMenuListGet(row.id);
				setTimeout(() => {
					treeRef.value?.setCheckedKeys(res.data.result);
				}, 100);
			}
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
				state.ruleForm.menuIdList = treeRef.value?.getCheckedKeys() as Array<number>; //.concat(treeRef.value?.getHalfCheckedKeys());
				if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
					await getAPI(SysRoleApi).sysRoleUpdatePost(state.ruleForm);
				} else {
					await getAPI(SysRoleApi).sysRoleAddPost(state.ruleForm);
				}
				closeDialog();
			});
		};
		// 叶子节点同行显示样式
		const treeNodeClass = (node: Node) => {
			if (node.isLeaf) return '';
			let addClass = true;
			for (const key in node.childNodes) {
				if (!node.childNodes[key].isLeaf) addClass = false;
			}
			return addClass ? 'penultimate-node' : '';
		};
		return {
			ruleFormRef,
			treeRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			treeNodeClass,
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
