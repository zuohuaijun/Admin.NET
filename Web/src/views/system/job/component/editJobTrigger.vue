<template>
	<div class="sys-jobTrigger-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="700px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="触发器编号" prop="triggerId" :rules="[{ required: true, message: '触发器编号不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.triggerId" placeholder="触发器编号" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="触发器类型">
							<el-select v-model="state.ruleForm.triggerType" style="width: 100%">
								<el-option value="Furion.Schedule.PeriodTrigger" label="间隔"></el-option>
								<el-option value="Furion.Schedule.CronTrigger" label="Cron表达式"></el-option>
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" v-if="state.ruleForm.triggerType == 'Furion.Schedule.PeriodTrigger'">
						<el-form-item label="间隔时间(ms)">
							<el-input-number v-model="periodValue" placeholder="间隔" :min="1000" :step="1000" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" v-else>
						<el-form-item label="Cron表达式">
							<el-input v-model="cronValue" placeholder="Cron表达式">
								<template #append>
									<el-dropdown style="color: inherit" trigger="click" @command="macroDropDownCommand">
										<el-button style="margin: 0px -10px 0px -20px; color: inherit"> Macro </el-button>
										<template #dropdown>
											<el-dropdown-menu>
												<el-dropdown-item v-for="(item, index) in macroData" :key="index" :command="item">
													<el-row style="width: 240px">
														<el-col :span="9">{{ item.key }}</el-col>
														<el-col :span="15">{{ item.description }}</el-col>
													</el-row>
												</el-dropdown-item>
											</el-dropdown-menu>
										</template>
									</el-dropdown>
									<el-button style="margin: 0px -20px 0px -10px" @click="state.showCronDialog = true">Cron表达式</el-button>
								</template>
							</el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="起始时间">
							<el-date-picker v-model="state.ruleForm.startTime" type="datetime" placeholder="起始时间" style="width: 100%" format="YYYY-MM-DD HH:mm:ss" value-format="YYYY-MM-DD HH:mm:ss" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="结束时间">
							<el-date-picker v-model="state.ruleForm.endTime" type="datetime" placeholder="结束时间" style="width: 100%" format="YYYY-MM-DD HH:mm:ss" value-format="YYYY-MM-DD HH:mm:ss" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="最大触发次数">
							<el-input-number v-model="state.ruleForm.maxNumberOfRuns" placeholder="最大触发次数" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="最大出错次数">
							<el-input-number v-model="state.ruleForm.maxNumberOfErrors" placeholder="最大出错次数" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="重试次数">
							<el-input-number v-model="state.ruleForm.numRetries" placeholder="重试次数" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="重试间隔(ms)">
							<el-input-number v-model="state.ruleForm.retryTimeout" placeholder="重试间隔ms" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="立即启动">
							<el-radio-group v-model="state.ruleForm.startNow">
								<el-radio :label="true">是</el-radio>
								<el-radio :label="false">否</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="启动时执行一次">
							<el-radio-group v-model="state.ruleForm.runOnStart">
								<el-radio :label="true">是</el-radio>
								<el-radio :label="false">否</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item>
							<template v-slot:label>
								<div>
									重置触发次数
									<el-tooltip raw-content content="是否在启动时重置最大触发次数等于一次的作业<br/>解决因持久化数据已完成一次触发但启动时不再执行的问题" placement="top">
										<SvgIcon name="fa fa-question-circle-o" :size="16" style="vertical-align: middle" />
									</el-tooltip>
								</div>
							</template>
							<el-radio-group v-model="state.ruleForm.resetOnlyOnce">
								<el-radio :label="true">是</el-radio>
								<el-radio :label="false">否</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述信息" prop="description">
							<el-input v-model="state.ruleForm.description" placeholder="描述信息" clearable type="textarea" />
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

		<el-dialog v-model="state.showCronDialog" draggable :close-on-click-modal="false" class="scrollbar">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> Cron表达式生成器 </span>
				</div>
			</template>
			<cronTab @hide="state.showCronDialog = false" @fill="crontabFill" :expression="cronValue"></cronTab>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditJobTrigger">
import { reactive, ref, computed } from 'vue';
import type { WritableComputedRef } from 'vue';
import { ElMessage } from 'element-plus';

import cronTab from './cronTab/index.vue';
import { getAPI } from '/@/utils/axios-utils';
import { SysJobApi } from '/@/api-services/api';
import { UpdateJobTriggerInput } from '/@/api-services/models';

// Macro 标识符数据结构
interface MacroData {
	key: string;
	description: string;
}

const props = defineProps({
	title: String,
});
const emits = defineEmits(['handleQuery']);
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdateJobTriggerInput,
	showCronDialog: false,
});

const macroData: MacroData[] = reactive([
	{ key: '@secondly', description: '每秒 .0000000' },
	{ key: '@minutely', description: '每分钟 00' },
	{ key: '@hourly', description: '每小时 00:00' },
	{ key: '@daily', description: '每天 00:00:00' },
	{ key: '@monthly', description: '每月 1 号 00:00:00' },
	{ key: '@weekly', description: '每周日 00:00:00' },
	{ key: '@yearly', description: '每年 1 月 1 号 00:00:00' },
	{ key: '@workday', description: '每周一至周五 00:00:00' },
]);

// 间隔周期值
const periodValue: WritableComputedRef<number | undefined> = computed({
	get() {
		const defaultValue: number | undefined = undefined;
		// 触发器周期不是周期，返回默认值
		if (state.ruleForm.triggerType != 'Furion.Schedule.PeriodTrigger') return defaultValue;
		if (!state.ruleForm.args) return defaultValue;

		const value: number | undefined = Number(state.ruleForm.args);
		if (Number.isNaN(value)) return defaultValue;

		return value;
	},
	set(value: number | undefined) {
		state.ruleForm.args = String(value);
	},
});

// cron 表达式值
const cronValue: WritableComputedRef<string> = computed({
	get() {
		const defaultValue = '';
		// 触发器周期不是周期，返回默认值
		if (state.ruleForm.triggerType != 'Furion.Schedule.CronTrigger') return defaultValue;
		if (!state.ruleForm.args) return defaultValue;

		// Furion 的 cron 表达式有2个入参
		const value = String(state.ruleForm.args);
		const parameters = value.split(',');
		if (parameters.length != 2) return defaultValue;

		const cron = parameters[0].replace(new RegExp('"', 'gm'), '').trim();
		return cron;
	},
	set(value: string) {
		if (state.ruleForm.args == value) return;

		const newValue = value.trim();
		// 第二个参数值参阅 https://furion.baiqian.ltd/docs/cron#2624-cronstringformat-%E6%A0%BC%E5%BC%8F%E5%8C%96
		let cronStringFormatValue = -1;

		// 如果是 Macro 标识符，使用默认格式
		if (newValue.startsWith('@')) cronStringFormatValue = 0; // 默认格式，书写顺序：分 时 天 月 周
		else {
			if (newValue.split(' ').length == 6) cronStringFormatValue = 2; // 带秒格式，书写顺序：秒 分 时 天 月 周
			else cronStringFormatValue = 3; // 带秒和年格式，书写顺序：秒 分 时 天 月 周 年
		}

		state.ruleForm.args = `"${newValue}",${cronStringFormatValue}`;
	},
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;
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
		if (state.ruleForm.triggerType == 'Furion.Schedule.PeriodTrigger' && !periodValue.value) {
			ElMessage.warning('间隔时间不能为空');
			return;
		} else if (state.ruleForm.triggerType == 'Furion.Schedule.CronTrigger' && !cronValue.value) {
			ElMessage.warning('Cron表达式不能为空');
			return;
		}

		if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
			await getAPI(SysJobApi).apiSysJobUpdateJobTriggerPost(state.ruleForm);
		} else {
			await getAPI(SysJobApi).apiSysJobAddJobTriggerPost(state.ruleForm);
		}
		closeDialog();
	});
};

// cron窗体确定后值
const crontabFill = (value: string | null | undefined) => {
	cronValue.value = value == null || value == undefined ? '' : value;
};

// macro 下拉选中回调
const macroDropDownCommand = (item: MacroData) => {
	cronValue.value = item.key;
};

// 导出对象
defineExpose({ openDialog });
</script>
