<template>
	<div class="generatetest-container">
		<el-dialog
		v-model="isShowDialog"
		:title="title"
		:width="700"
		draggable>
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false"><el-input v-model="ruleForm.Id" /></el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="编码">
							<el-input v-model="ruleForm.Code" placeholder="ruleForm.Code" clearable/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="名称">
							<el-input v-model="ruleForm.Name" placeholder="ruleForm.Name" clearable/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="价格">
							<el-input v-model="ruleForm.Price" placeholder="ruleForm.Price" clearable/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="过期日期">
							<el-date-picker v-model="ruleForm.ExpireDate" type="datetime" placeholder="过期日期"  />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态">
							<el-switch v-model="ruleForm.Status" active-text="是" inactive-text="否" />
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
	import { ref, toRefs, reactive, onMounted, defineComponent, getCurrentInstance, onUnmounted } from 'vue';
	import { ElMessageBox, ElMessage } from 'element-plus';
	import { auth } from '/@/utils/authFunction';
	import { formatDate } from '/@/utils/formatTime';
	import type { FormInstance, FormRules } from 'element-plus'


	export default defineComponent({
	name: 'editgeneratetest',
	props: {
	title: {
	type: String,
	default: '',
	},
	},
	setup() {
	const { proxy } = getCurrentInstance() as any;
	const ruleFormRef = ref();
	const state = reactive({
	isShowDialog: false,
	ruleForm: {} as any,
	});

	const rules = reactive<FormRules>({
			Code: [{required: true, message: '请输入编码！', trigger: 'blur',},],
			Name: [{required: true, message: '请输入名称！', trigger: 'blur',},],
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
	await updategeneratetest(values);
	} else {
	await addgeneratetest(state.ruleForm);
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
	rules,
	};
	}
	})

	}
</script>
