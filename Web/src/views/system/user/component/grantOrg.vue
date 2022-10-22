<template>
	<div class="sys-grantOrg-container">
		<el-dialog v-model="isShowDialog" width="450px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog', '.el-dialog__header']">授权数据范围</div>
			</template>
			<el-form :model="ruleForm" size="default" label-width="0">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl1="24">
						<el-form-item prop="orgIdList">
							<OrgTree ref="orgTreeRef" style="width: 100%" />
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
import { reactive, toRefs, defineComponent, ref } from 'vue';
import OrgTree from '/@/views/system/org/component/orgTree.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysUserApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysGrantOrg',
	components: { OrgTree },
	setup() {
		const orgTreeRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {
				id: 0,
				org: 0, // 用户所属机构
				orgIdList: [] as any, // 机构集合
			},
		});
		// 打开弹窗
		const openDialog = async (row: any) => {
			state.ruleForm = row;
			var res = await getAPI(SysUserApi).sysUserOwnOrgGet(row.id);
			setTimeout(() => {
				orgTreeRef.value?.setCheckedKeys(res.data.result);
			}, 100);
			state.isShowDialog = true;
		};
		// 取消
		const cancel = () => {
			state.isShowDialog = false;
		};
		// 提交
		const submit = async () => {
			state.ruleForm.orgIdList = orgTreeRef.value?.getCheckedKeys();
			await getAPI(SysUserApi).sysUserGrantOrgPost(state.ruleForm);
			state.isShowDialog = false;
		};
		return {
			orgTreeRef,
			openDialog,
			cancel,
			submit,
			...toRefs(state),
		};
	},
});
</script>
