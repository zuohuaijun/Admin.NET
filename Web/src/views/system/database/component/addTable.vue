<template>
	<div class="sys-dbTable-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="1400px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> 增加表 </span>
				</div>
			</template>
			<el-divider content-position="left">数据表信息</el-divider>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="表名称" prop="tableName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.tableName" placeholder="表名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="description" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.description" placeholder="描述" clearable type="textarea" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<el-divider content-position="left">数据列信息</el-divider>
			<el-table :data="state.tableData" style="width: 100%" max-height="400">
				<el-table-column prop="dbColumnName" label="字段名" width="220" fixed>
					<template #default="scope">
						<el-input v-model="scope.row.dbColumnName" autocomplete="off" />
					</template>
				</el-table-column>
				<el-table-column prop="columnDescription" label="描述" width="220">
					<template #default="scope">
						<el-input v-model="scope.row.columnDescription" autocomplete="off" />
					</template>
				</el-table-column>
				<el-table-column prop="isPrimarykey" label="主键" width="100">
					<template #default="scope">
						<el-select v-model="scope.row.isPrimarykey" class="m-2" placeholder="Select">
							<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="isIdentity" label="自增" width="100">
					<template #default="scope">
						<el-select v-model="scope.row.isIdentity" class="m-2" placeholder="Select">
							<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="dataType" label="类型" width="150">
					<template #default="scope">
						<el-select v-model="scope.row.dataType" class="m-2" placeholder="Select">
							<el-option v-for="item in dataTypeList" :key="item.value" :label="item.value" :value="item.value" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="isNullable" label="可空" width="100">
					<template #default="scope">
						<el-select v-model="scope.row.isNullable" class="m-2" placeholder="Select">
							<el-option v-for="item in yesNoSelect" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="length" label="长度" width="100">
					<template #default="scope">
						<el-input-number v-model="scope.row.length" controls-position="right" class="w100" />
					</template>
				</el-table-column>
				<el-table-column prop="decimalDigits" label="小数位" width="100">
					<template #default="scope">
						<el-input-number v-model="scope.row.decimalDigits" controls-position="right" class="w100" />
					</template>
				</el-table-column>
				<el-table-column label="操作" minWidth="200" align="center" fixed="right">
					<template #default="scope">
						<el-button link type="primary" icon="el-icon-delete" @click.prevent="handleColDelete(scope.$index)">删除</el-button>
						<el-button v-if="state.tableData.length > 1" link type="primary" icon="ele-Top" @click.prevent="handleColUp(scope.row, scope.$index)">上移</el-button>
						<el-button v-if="state.tableData.length > 1" link type="primary" icon="ele-Bottom" @click.prevent="handleColDown(scope.row, scope.$index)">下移</el-button>
					</template>
				</el-table-column>
			</el-table>
			<div style="text-align: left; margin-top: 10px">
				<el-button icon="ele-Plus" @click="addPrimaryColumn">新增主键字段</el-button>
				<el-button icon="ele-Plus" @click="addColumn">新增普通字段</el-button>
				<el-button icon="ele-Plus" @click="addTenantColumn">新增租户字段</el-button>
				<el-button icon="ele-Plus" @click="addBaseColumn">新增基础字段</el-button>
			</div>

			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel">取 消</el-button>
					<el-button type="primary" @click="submit">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysAddTable">
import { reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { UpdateDbTableInput } from '/@/api-services/models';
import { dataTypeList, EditRecordRow, yesNoSelect } from '../database';

var colIndex = 0;
const emits = defineEmits(['addTableSubmitted']);
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as UpdateDbTableInput,
	tableData: [] as any,
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = row;
	state.isShowDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	emits('addTableSubmitted', state.ruleForm.tableName ?? '');
	state.tableData = [];
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
		if (state.tableData.length === 0) {
			ElMessage({
				type: 'error',
				message: `请选择库名!`,
			});
			return;
		}
		const params: any = {
			dbColumnInfoList: state.tableData,
			...state.ruleForm,
		};
		await getAPI(SysDatabaseApi).apiSysDatabaseAddTablePost(params);
		closeDialog();
	});
};

// 增加主键列
function addPrimaryColumn() {
	state.tableData.push({
		columnDescription: '主键Id',
		dataType: 'bigint',
		dbColumnName: 'Id',
		decimalDigits: 0,
		isIdentity: 0,
		isNullable: 0,
		isPrimarykey: 1,
		length: 0,
		key: colIndex,
		editable: true,
		isNew: true,
	});
	colIndex++;
}

// 增加普通列
function addColumn() {
	state.tableData.push({
		columnDescription: '',
		dataType: '',
		dbColumnName: '',
		decimalDigits: 0,
		isIdentity: 0,
		isNullable: 1,
		isPrimarykey: 0,
		length: 0,
		key: colIndex,
		editable: true,
		isNew: true,
	});
	colIndex++;
}

// 增加租户列
function addTenantColumn() {
	state.tableData.push({
		columnDescription: '租户Id',
		dataType: 'bigint',
		dbColumnName: 'TenantId',
		decimalDigits: 0,
		isIdentity: 0,
		isNullable: 1,
		isPrimarykey: 0,
		length: 0,
		key: colIndex,
		editable: true,
		isNew: true,
	});
	colIndex++;
}

// 增加通用基础列
function addBaseColumn() {
	const fileds = [
		{
			dataType: 'datetime',
			name: 'CreateTime',
			desc: '创建时间',
		},
		{
			dataType: 'datetime',
			name: 'UpdateTime',
			desc: '更新时间',
		},
		{
			dataType: 'bigint',
			name: 'CreateUserId',
			desc: '创建者Id',
		},
		{
			dataType: 'bigint',
			name: 'UpdateUserId',
			desc: '修改者Id',
		},
		{
			dataType: 'bit',
			name: 'IsDelete',
			desc: '软删除',
			isNullable: 0,
		},
	];

	fileds.forEach((m: any) => {
		state.tableData.push({
			columnDescription: m.desc,
			dataType: m.dataType,
			dbColumnName: m.name,
			decimalDigits: 0,
			isIdentity: 0,
			isNullable: m.isNullable === 0 ? 0 : 1,
			isPrimarykey: 0,
			length: m.length || 0,
			key: colIndex,
			editable: true,
			isNew: true,
		});
		colIndex++;
	});
}

function handleColDelete(index: number) {
	state.tableData.splice(index, 1);
}

// 上移
function handleColUp(record: EditRecordRow, index: number) {
	if (record.isNew) {
		var data1 = ChangeExForArray(index, index - 1, state.tableData);
		return data1;
	}
}

// 下移
function handleColDown(record: EditRecordRow, index: number) {
	if (record.isNew) {
		return ChangeExForArray(index, index + 1, state.tableData);
	}
}

function ChangeExForArray(index1: number, index2: number, array: Array<EditRecordRow>) {
	let temp = array[index1];
	array[index1] = array[index2];
	array[index2] = temp;
	return array;
}

// 导出对象
defineExpose({ openDialog });
</script>
