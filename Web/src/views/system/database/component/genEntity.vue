<template>
	<div class="sys-dbEntity-container">
		<el-dialog v-model="state.isShowDialog" draggable width="600px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Cpu /> </el-icon>
					<span> 生成实体 </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="表名" prop="tableName" :rules="[{ required: true, message: '表名不能为空', trigger: 'blur' }]">
							<el-input disabled v-model="state.ruleForm.tableName" placeholder="表名" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="实体名称" prop="entityName" :rules="[{ required: true, message: '实体名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.entityName" placeholder="实体名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="基类" prop="baseClassName">
							<el-select v-model="state.ruleForm.baseClassName" clearable class="w100">
								<el-option v-for="item in state.codeGenBaseClassName" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="存放位置" prop="position">
							<el-select v-model="state.ruleForm.position" clearable class="w100">
								<el-option label="Admin.NET.Application" value="Admin.NET.Application" />
								<el-option label="Admin.NET.Core" value="Admin.NET.Core" />
							</el-select>
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

<script lang="ts" setup name="sysGenEntity">
import { onMounted, reactive, ref } from 'vue';
import mittBus from '/@/utils/mitt';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi, SysDictDataApi } from '/@/api-services/api';

const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as any,
	codeGenBaseClassName: [] as any,
});

onMounted(async () => {
	let resDicData = await getAPI(SysDictDataApi).apiSysDictDataDataListCodeGet('code_gen_base_class');
	state.codeGenBaseClassName = resDicData.data.result;
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm.configId = row.configId;
	state.ruleForm.tableName = row.tableName;
	state.ruleForm.baseClassName = 'EntityBase';
	state.ruleForm.position = 'Admin.NET.Application';
	state.isShowDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	mittBus.emit('submitRefreshColumn');
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
		await getAPI(SysDatabaseApi).apiSysDatabaseCreateEntityPost(state.ruleForm);
		closeDialog();
	});
};

// 导出对象
defineExpose({ openDialog });
</script>
