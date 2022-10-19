<template>
	<div class="sys-config-container">
		<el-dialog v-model="isShowDialog" width="600px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog', '.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="配置名称" prop="name">
							<el-input v-model="ruleForm.name" placeholder="配置名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="配置编码" prop="code">
							<el-input v-model="ruleForm.code" placeholder="配置编码" clearable :disabled="ruleForm.sysFlag == 1"></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="值" prop="value">
							<el-input v-model="ruleForm.value" placeholder="值" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="系统参数" prop="sysFlag">
							<el-radio-group v-model="ruleForm.sysFlag" :disabled="ruleForm.sysFlag == 1">
								<el-radio :label="1">是</el-radio>
								<el-radio :label="2">否</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="所属分类" prop="groupCode">
							<el-input v-model="ruleForm.groupCode" placeholder="所属分类" clearable :disabled="ruleForm.sysFlag == 1"></el-input>
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
import { SysConfigApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditPos',
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
			isShowDialog: false,
			ruleForm: {
				id: 0, // Id
				name: '', // 配置名称
				code: '', // 配置编码
				value: '', // 值
				sysFlag: 1, // 系统参数
				groupCode: '', // 所属分类
				order: 100, // 排序
				remark: '', // 备注
			},
			ruleRules: {
				name: [{ required: true, message: '配置名称不能为空', trigger: 'blur' }],
				code: [{ required: true, message: '配置编码不能为空', trigger: 'blur' }],
				value: [{ required: true, message: '值不能为空', trigger: 'blur' }],
				sysFlag: [{ required: true, message: '系统参数不能为空', trigger: 'blur' }],
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
					await getAPI(SysConfigApi).sysConfigUpdatePost(state.ruleForm);
				} else {
					await getAPI(SysConfigApi).sysConfigAddPost(state.ruleForm);
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
