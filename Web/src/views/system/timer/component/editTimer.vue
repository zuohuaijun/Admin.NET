<template>
	<div class="sys-timer-container">
		<el-dialog v-model="isShowDialog" :title="title" draggable width="769px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="任务名称" prop="timerName" :rules="[{ required: true, message: '任务名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.timerName" placeholder="任务名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="请求地址" prop="requestUrl" :rules="[{ required: true, message: '请求地址不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.requestUrl" placeholder="请求地址" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="请求类型" prop="requestType" :rules="[{ required: true, message: '请求类型不能为空', trigger: 'blur' }]">
							<el-radio-group v-model="ruleForm.requestType">
								<el-radio :label="0">RUN</el-radio>
								<el-radio :label="1">GET</el-radio>
								<el-radio :label="2">POST</el-radio>
								<el-radio :label="3">PUT</el-radio>
								<el-radio :label="4">DELETE</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="请求参数" prop="requestPara">
							<el-input v-model="ruleForm.requestPara" placeholder="所属分类" clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="任务类型" prop="timerType" :rules="[{ required: true, message: '任务类型不能为空', trigger: 'blur' }]">
							<el-select v-model="ruleForm.timerType" placeholder="岗位状态" style="width: 100%">
								<el-option label="间隔模式" :value="0" />
								<el-option label="Cron模式" :value="1" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="执行间隔" prop="interval" v-if="ruleForm.timerType == 0">
							<el-input v-model="ruleForm.interval" placeholder="执行间隔" clearable />
						</el-form-item>
						<el-form-item label="Cron表达式" prop="cron" v-else>
							<el-input v-model="ruleForm.cron" placeholder="Cron表达式" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea" />
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
import mittBus from '/@/utils/mitt';

import { getAPI } from '/@/utils/axios-utils';
import { SysTimerApi } from '/@/api-services/api';
import { UpdateTimerInput } from '/@/api-services/models';

export default defineComponent({
	name: 'sysEditTimer',
	components: {},
	props: {
		title: {
			type: String,
			default: '',
		},
	},
	setup() {
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as UpdateTimerInput,
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = JSON.parse(JSON.stringify(row));
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			mittBus.emit('submitRefresh');
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
					await getAPI(SysTimerApi).sysTimerUpdatePost(state.ruleForm);
				} else {
					await getAPI(SysTimerApi).sysTimerAddPost(state.ruleForm);
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
