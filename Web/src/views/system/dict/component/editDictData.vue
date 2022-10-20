<template>
	<div class="sys-dictData-container">
		<el-dialog v-model="isShowDialog" width="600px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog', '.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="字典值" prop="value">
							<el-input v-model="ruleForm.value" placeholder="字典值" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="编码" prop="code">
							<el-input v-model="ruleForm.code" placeholder="编码" clearable></el-input>
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
import { SysDictDataApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditDictData',
	components: {},
	props: {
		// 弹窗标题
		title: {
			type: String,
			default: '',
		},
		// 字典类型Id
		dictTypeId: {
			type: Number,
			default: 0,
		},
	},
	setup(props) {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {
				dictTypeId: 0, // 字典类型Id
				id: 0, // Id
				value: '', // 字典值
				code: '', // 编码
				status: 1, // 状态
				order: 100, // 排序
				remark: '', // 备注
			},
			ruleRules: {
				value: [{ required: true, message: '字典值不能为空', trigger: 'blur' }],
				code: [{ required: true, message: '编码不能为空', trigger: 'blur' }],
			},
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
			if (JSON.stringify(row) == '{}') {
				state.ruleForm.dictTypeId = props.dictTypeId;
			}
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit('submitRefreshDictData');
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
					await getAPI(SysDictDataApi).sysDictDataUpdatePost(state.ruleForm);
				} else {
					await getAPI(SysDictDataApi).sysDictDataAddPost(state.ruleForm);
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
