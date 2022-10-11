<template>
	<div class="sys-role-container">
		<el-dialog v-model="isShowDialog" width="500px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog','.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="角色名称" prop="name">
							<el-input v-model="ruleForm.name" placeholder="角色名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="角色编码" prop="code">
							<el-input v-model="ruleForm.code" placeholder="角色编码" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="排序">
							<el-input-number v-model="ruleForm.order" controls-position="right" placeholder="排序"
								class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="是否启用">
							<el-radio-group v-model="ruleForm.status">
								<el-radio :label="1">启用</el-radio>
								<el-radio :label="2">不启用</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea">
							</el-input>
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
import { reactive, toRefs, defineComponent, getCurrentInstance, ref, unref } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysRoleApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditRole',
	components: {},
	props: {
		// 弹窗标题
		title: {
			type: String,
			default: () => "",
		},
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref<HTMLElement | null>(null);
		const state = reactive({
			isShowDialog: false,
			ruleForm: {
				id: 0, // Id
				name: '', // 角色名称
				code: '', // 角色编码
				order: 10, // 排序
				status: 1, // 是否启用
				remark: '', // 备注
			},
			ruleRules: {
				name: [{ required: true, message: "角色名称不能为空", trigger: "blur" }],
				code: [{ required: true, message: "角色编码不能为空", trigger: "blur" }],
			},
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit("submitRefresh");
			state.isShowDialog = false;
		};
		// 取消
		const cancel = () => {
			state.isShowDialog = false;
		};
		// 提交
		const submit = () => {
			const formWrap = unref(ruleFormRef) as any;
			if (!formWrap) return;

			formWrap.validate(async () => {
				if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
					await getAPI(SysRoleApi).sysRoleUpdatePost(state.ruleForm);
				}
				else {
					await getAPI(SysRoleApi).sysRoleAddPost(state.ruleForm);
				}
				closeDialog();
			})
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
