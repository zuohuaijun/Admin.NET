<template>
	<div class="sys-password-container">
		<NoticeBar text="账号密码修改，请慎重操作！" leftIcon="iconfont icon-tongzhi2" background="var(--el-color-primary-light-9)" color="var(--el-color-primary)" />
		<el-card shadow="hover" header="修改当前账号密码" class="mt8">
			<el-form ref="ruleFormRef" :model="ruleForm" status-icon :rules="ruleRules" label-width="80px">
				<el-form-item label="当前密码" prop="passwordOld">
					<el-input v-model="ruleForm.passwordOld" type="password" autocomplete="off" />
				</el-form-item>
				<el-form-item label="新密码" prop="passwordNew">
					<el-input v-model="ruleForm.passwordNew" type="password" autocomplete="off" />
				</el-form-item>
				<el-form-item label="确认密码" prop="passwordNew2">
					<el-input v-model.number="ruleForm.passwordNew2" type="password" />
				</el-form-item>
				<el-form-item>
					<el-button icon="ele-Refresh" @click="reset" size="default">重 置</el-button>
					<el-button icon="ele-Select" type="primary" @click="submit" size="default" v-auth="'sysPassword:update'">确 定</el-button>
				</el-form-item>
			</el-form>
		</el-card>
	</div>
</template>

<script lang="ts">
import { reactive, toRefs, defineComponent, ref } from 'vue';
import NoticeBar from '/@/components/noticeBar/index.vue';
import { ElMessageBox } from 'element-plus';
import { Session } from '/@/utils/storage';

import { getAPI } from '/@/utils/axios-utils';
import { SysUserApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysPassword',
	components: { NoticeBar },
	setup() {
		const ruleFormRef = ref();

		const validatePassword = (rule: any, value: any, callback: any) => {
			if (value != state.ruleForm.passwordNew) {
				callback(new Error('两次密码不一致！'));
			} else {
				callback();
			}
		};
		const state = reactive({
			ruleForm: {
				id: 0,
				passwordOld: '',
				passwordNew: '',
				passwordNew2: '',
			},
			ruleRules: {
				passwordOld: [{ required: true, message: '当前密码不能为空', trigger: 'blur' }],
				passwordNew: [{ required: true, message: '新密码不能为空', trigger: 'blur' }],
				passwordNew2: [{ validator: validatePassword, required: true, trigger: 'blur' }],
			},
		});
		// 重置
		const reset = () => {
			state.ruleForm.passwordOld = '';
			state.ruleForm.passwordNew = '';
			state.ruleForm.passwordNew2 = '';
		};
		// 提交
		const submit = () => {
			ruleFormRef.value.validate(async (valid: boolean) => {
				if (!valid) return;
				await getAPI(SysUserApi).sysUserChangeUserPwdPost(state.ruleForm);
				// 退出系统
				ElMessageBox.confirm('密码已修改，是否重新登录系统？', '提示', {
					confirmButtonText: '确定',
					cancelButtonText: '取消',
					type: 'warning',
				}).then(async () => {
					// 清除缓存
					Session.clear();
					window.location.reload();
				});
			});
		};
		return {
			ruleFormRef,
			reset,
			submit,
			...toRefs(state),
		};
	},
});
</script>
