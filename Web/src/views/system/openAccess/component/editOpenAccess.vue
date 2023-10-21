<template>
	<div class="sys-notice-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="600px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="身份标识" prop="accessKey" :rules="[{ required: true, message: '身份标识不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.accessKey" placeholder="身份标识" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="密钥" prop="accessSecret" :rules="[{ required: true, message: '密钥不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.accessSecret" placeholder="密钥" clearable>
								<template #append>
									<el-button @click="createSecret">生成密钥</el-button>
								</template>
							</el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="绑定租户" prop="bindTenantId" :rules="[{ required: true, message: '绑定租户不能为空', trigger: 'blur' }]">
							<el-select v-model="state.ruleForm.bindTenantId" placeholder="绑定租户" filterable default-first-option style="width: 100%" @change="tenantChange">
								<el-option v-for="item in state.tenantData" :key="item.id" :label="item.name" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="绑定用户" prop="bindUserId" :rules="[{ required: true, message: '绑定用户不能为空', trigger: 'blur' }]">
							<el-select v-model="state.ruleForm.bindUserId" placeholder="绑定用户" filterable default-first-option style="width: 100%">
								<el-option v-for="item in state.userData" :key="item.id" :label="`${item.account}(${item.realName})`" :value="item.id" />
							</el-select>
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

<script lang="ts" setup name="sysOpenAccessEdit">
import { onMounted, reactive, ref } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysOpenAccessApi, SysTenantApi } from '/@/api-services/api';
import { SysUser, TenantOutput, UpdateOpenAccessInput } from '/@/api-services/models';

const props = defineProps({
	title: String,
});
const emits = defineEmits(['handleQuery']);
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdateOpenAccessInput,
	tenantData: [] as Array<TenantOutput>, // 租户数据
	userData: [] as Array<SysUser>, // 用户数据
});

onMounted(async () => {
	var res = await getAPI(SysTenantApi).apiSysTenantPagePost({ page: 1, pageSize: 10000 });
	state.tenantData = res.data.result?.items ?? [];
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;
	ruleFormRef.value?.resetFields();

	tenantChange();
};

// 关闭弹窗
const closeDialog = () => {
	emits('handleQuery');
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
			await getAPI(SysOpenAccessApi).apiSysOpenAccessUpdatePost(state.ruleForm);
		} else {
			await getAPI(SysOpenAccessApi).apiSysOpenAccessAddPost(state.ruleForm);
		}
		closeDialog();
	});
};

/** 租户值变更 */
const tenantChange = async () => {
	var res = await getAPI(SysTenantApi).apiSysTenantUserListPost({ tenantId: state.ruleForm.bindTenantId ?? 0 });
	state.userData = res.data.result ?? [];
};

/** 生成密钥 */
const createSecret = async () => {
	var res = await getAPI(SysOpenAccessApi).apiSysOpenAccessSecretPost();
	state.ruleForm.accessSecret = res.data.result!;
};

// 导出对象
defineExpose({ openDialog });
</script>
