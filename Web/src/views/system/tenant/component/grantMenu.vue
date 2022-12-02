<template>
	<div class="sys-grantMenu-container">
		<el-dialog v-model="isShowDialog" title="授权租户菜单" draggable width="769px">
			<el-form :model="ruleForm" size="default" v-loading="loading">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl1="24">
						<el-form-item prop="orgIdList" label="">
							<el-tree
								ref="treeRef"
								:data="menuData"
								node-key="id"
								show-checkbox
								:props="{ children: 'children', label: 'title', class: treeNodeClass }"
								:default-checked-keys="ownMenuData"
								icon="ele-Menu"
								highlight-current
								default-expand-all
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
import { reactive, toRefs, defineComponent, ref, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import type { ElTree } from 'element-plus';
import type Node from 'element-plus/es/components/tree/src/model/node';

import { getAPI } from '/@/utils/axios-utils';
import { SysMenuApi, SysTenantApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysGrantMenu',
	components: {},
	setup() {
		const treeRef = ref<InstanceType<typeof ElTree>>();
		const state = reactive({
			loading: false,
			isShowDialog: false,
			ruleForm: {
				id: 0,
				menuIdList: [] as any, // 菜单集合
			},
			menuData: [] as any, // 菜单数据
			ownMenuData: [] as any, // 拥有菜单
		});
		onMounted(async () => {
			state.loading = true;
			var res = await getAPI(SysMenuApi).sysMenuListGet();
			state.menuData = res.data.result;
			state.loading = false;
		});
		// 打开弹窗
		const openDialog = async (row: any) => {
			treeRef.value?.setCheckedKeys([]); // 先清空已选择节点
			state.ruleForm = row;
			var res = await getAPI(SysTenantApi).sysTenantOwnMenuListGet(row.userId);
			setTimeout(() => {
				// 延迟传递数据
				treeRef.value?.setCheckedKeys(res.data.result);
			}, 100);
			state.isShowDialog = true;
		};
		// 取消
		const cancel = () => {
			state.isShowDialog = false;
		};
		// 提交
		const submit = async () => {
			//提交全选和半选的key
			var allCheckedKeys = treeRef.value?.getCheckedKeys() as Array<number>;
			var halfCheckedKeys = treeRef.value?.getHalfCheckedKeys() as Array<number>;
			state.ruleForm.menuIdList = allCheckedKeys.concat(halfCheckedKeys);
			var res = await getAPI(SysTenantApi).sysTenantGrantMenuPost(state.ruleForm);
			state.isShowDialog = false;
			if(res.data && res.data.code == 200) ElMessage.success('操作成功');
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
			treeRef,
			openDialog,
			cancel,
			submit,
			treeNodeClass,
			...toRefs(state),
		};
	},
});
</script>

<style scoped lang="scss">
:deep(.penultimate-node) {
	.el-tree-node__children {
		padding-left: 40px;
		white-space: pre-wrap;
		line-height: 100%;

		.el-tree-node {
			display: inline-block;
		}

		.el-tree-node__content {
			padding-left: 5px !important;
			padding-right: 5px;

			// .el-tree-node__expand-icon {
			// 	display: none;
			// }
		}
	}
}
</style>
