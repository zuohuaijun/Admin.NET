<template>
	<div class="sys-codeGenConfig-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="1400px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> 生成配置 </span>
				</div>
			</template>
			<el-table :data="state.tableData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="columnName" label="字段" width="180" show-overflow-tooltip />
				<el-table-column prop="columnComment" label="描述" width="180" show-overflow-tooltip>
					<template #default="scope">
						<el-input v-model="scope.row.columnComment" autocomplete="off" />
					</template>
				</el-table-column>
				<el-table-column prop="netType" label="数据类型" minWidth="120" show-overflow-tooltip />
				<el-table-column prop="effectType" label="作用类型" width="120" show-overflow-tooltip>
					<template #default="scope">
						<el-select v-model="scope.row.effectType" class="m-2" placeholder="Select" :disabled="judgeColumns(scope.row)" @change="effectTypeChange(scope.row, scope.$index)">
							<el-option v-for="item in state.effectTypeList" :key="item.code" :label="item.value" :value="item.code" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="dictTypeCode" label="字典" width="180" show-overflow-tooltip>
					<template #default="scope">
						<el-select v-model="scope.row.dictTypeCode" class="m-2" :disabled="effectTypeEnable(scope.row)">
							<el-option v-for="item in state.dictTypeCodeList" :key="item.code" :label="item.name" :value="item.code" />
						</el-select>
					</template>
				</el-table-column>

				<el-table-column prop="whetherTable" label="列表显示" width="85" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-checkbox v-model="scope.row.whetherTable" />
					</template>
				</el-table-column>
				<el-table-column prop="whetherAddUpdate" label="增改" width="80" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-checkbox v-model="scope.row.whetherAddUpdate" :disabled="judgeColumns(scope.row)" />
					</template>
				</el-table-column>
				<el-table-column prop="whetherRequired" label="必填" width="80" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-checkbox v-model="scope.row.whetherRequired" :disabled="judgeColumns(scope.row)" />
					</template>
				</el-table-column>
				<el-table-column prop="queryWhether" label="是否是查询" minWidth="80" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-switch v-model="scope.row.queryWhether" :active-value="true" :inactive-value="false" />
					</template>
				</el-table-column>
				<el-table-column prop="queryType" label="查询方式" minWidth="120" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-select v-model="scope.row.queryType" class="m-2" placeholder="Select" :disabled="!scope.row.queryWhether">
							<el-option v-for="item in state.queryTypeList" :key="item.code" :label="item.value" :value="item.code" />
						</el-select>
					</template>
				</el-table-column>
			</el-table>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel">取 消</el-button>
					<el-button type="primary" @click="submit">确 定</el-button>
				</span>
			</template>
		</el-dialog>

		<fkDialog ref="fkDialogRef" @submitRefreshFk="submitRefreshFk" />
		<treeDialog ref="treeDialogRef" @submitRefreshFk="submitRefreshFk" />
	</div>
</template>

<script lang="ts" setup name="sysCodeGenConfig">
import { onMounted, onUnmounted, reactive, ref } from 'vue';
import mittBus from '/@/utils/mitt';
import fkDialog from '/@/views/system/codeGen/component/fkDialog.vue';
import treeDialog from '/@/views/system/codeGen/component/treeDialog.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysCodeGenConfigApi, SysConstApi, SysDictDataApi, SysDictTypeApi, SysEnumApi } from '/@/api-services/api';
import { CodeGenConfig } from '/@/api-services/models/code-gen-config';

const emits = defineEmits(['handleQuery']);
const fkDialogRef = ref();
const treeDialogRef = ref();
const state = reactive({
	isShowDialog: false,
	loading: false,
	tableData: [] as CodeGenConfig[],
	dbData: [] as any,
	effectTypeList: [] as any,
	dictTypeCodeList: [] as any,
	dictDataAll: [] as any,
	queryTypeList: [] as any,
	allConstSelector: [] as any,
	allEnumSelector: [] as any,
});

onMounted(async () => {
	var res = await getAPI(SysDictDataApi).apiSysDictDataDataListCodeGet('code_gen_effect_type');
	state.effectTypeList = res.data.result;

	var res1 = await getAPI(SysDictTypeApi).apiSysDictTypeListGet();
	state.dictTypeCodeList = res1.data.result;
	state.dictDataAll = res1.data.result;

	var res2 = await getAPI(SysDictDataApi).apiSysDictDataDataListCodeGet('code_gen_query_type');
	state.queryTypeList = res2.data.result;

	var res3 = await getAPI(SysConstApi).apiSysConstListGet();
	state.allConstSelector = res3.data.result;

	let resEnum = await getAPI(SysEnumApi).apiSysEnumEnumTypeListGet();
	state.allEnumSelector = resEnum.data.result?.map((item) => ({ ...item, name: item.typeDescribe, code: item.typeName }));

	mittBus.on('submitRefreshFk', (data: any) => {
		state.tableData[data.index] = data;
	});
});

// 更新主键
const submitRefreshFk = (data: any) => {
	state.tableData[data.index] = data;
};

onUnmounted(() => {
	mittBus.off('submitRefresh', () => {});
	mittBus.off('submitRefreshFk', () => {});
});
// 控件类型改变
const effectTypeChange = (data: any, index: number) => {
	let value = data.effectType;
	if (value === 'fk') {
		openFkDialog(data, index);
	} else if (value === 'ApiTreeSelect') {
		openTreeDialog(data, index);
	} else if (value === 'Select') {
		data.dictTypeCode = '';
		state.dictTypeCodeList = state.dictDataAll;
	} else if (value === 'ConstSelector') {
		data.dictTypeCode = '';
		state.dictTypeCodeList = state.allConstSelector;
	} else if (value == 'EnumSelector') {
		data.dictTypeCode = '';
		state.dictTypeCodeList = state.allEnumSelector;
	}
};

// 查询操作
const handleQuery = async (row: any) => {
	state.loading = true;
	var res = await getAPI(SysCodeGenConfigApi).apiSysCodeGenConfigListGet(undefined, row.id);
	var data = res.data.result ?? [];
	data.forEach((item: any) => {
		for (const key in item) {
			if (item[key] === 'Y') {
				item[key] = true;
			}
			if (item[key] === 'N') {
				item[key] = false;
			}
		}
	});
	state.tableData = data;
	state.loading = false;
};

// 判断是否（用于是否能选择或输入等）
function judgeColumns(data: any) {
	var lst = ['createdUserName', 'createdTime', 'updatedUserName', 'updatedTime'];
	return lst.indexOf(data.columnName) > -1 || data.columnKey === 'True';
}

function effectTypeEnable(data: any) {
	var lst = ['Radio', 'Select', 'Checkbox', 'ConstSelector', 'EnumSelector'];
	return lst.indexOf(data.effectType) === -1;
}

// 打开弹窗
const openDialog = (addRow: any) => {
	handleQuery(addRow);
	state.isShowDialog = true;
};

// 打开弹窗
const openFkDialog = (addRow: any, index: number) => {
	addRow.index = index;
	fkDialogRef.value.openDialog(addRow);
};

const openTreeDialog = (addRow: any, index: number) => {
	addRow.index = index;
	treeDialogRef.value.openDialog(addRow);
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
const submit = async () => {
	state.loading = true;
	var lst = state.tableData;
	lst.forEach((item: CodeGenConfig) => {
		// 必填那一项转换
		for (var key in item) {
			if (item[key] === true) {
				item[key] = 'Y';
			}
			if (item[key] === false) {
				item[key] = 'N';
			}
		}
	});
	await getAPI(SysCodeGenConfigApi).apiSysCodeGenConfigUpdatePost(lst);
	state.loading = false;
	closeDialog();
};

const convertDbType = (dbType: number) => {
	let result = '';
	switch (dbType) {
		case 0:
			result = 'MySql';
			break;
		case 1:
			result = 'SqlServer';
			break;
		case 2:
			result = 'Sqlite';
			break;
		case 3:
			result = 'Oracle';
			break;
		case 4:
			result = 'PostgreSql';
			break;
		case 5:
			result = 'Dm';
			break;
		case 6:
			result = 'Kdbndp';
			break;
		case 7:
			result = 'Oscar';
			break;
		case 8:
			result = 'MySqlConnector';
			break;
		case 9:
			result = 'Access';
			break;
		default:
			result = 'Custom';
			break;
	}
	return result;
};

const isOrNotSelect = () => {
	return [
		{
			label: '是',
			value: 1,
		},
		{
			label: '否',
			value: 0,
		},
	];
};

// 导出对象
defineExpose({ openDialog });
</script>
