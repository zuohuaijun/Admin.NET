<template>
	<div class="tree-container">
		<el-dialog v-model="isShowDialog" title="树选择配置" draggable width="600px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="库定位器" prop="configId">
							<el-select clearable v-model="ruleForm.configId" placeholder="库名" filterable @change="DbChanged()">
								<el-option v-for="item in dbData" :key="item.configId" :label="item.configId" :value="item.configId" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="数据库表" prop="tableName">
							<el-select v-model="ruleForm.tableName" class="m-2" filterable clearable @change="TableChanged()">
								<el-option v-for="item in tableData" :key="item.entityName" :label="item.tableName" :value="item.tableName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="显示字段" prop="displayColumn">
							<el-select v-model="ruleForm.displayColumn">
								<el-option v-for="item in columnData" :key="item.columnName" :label="item.columnName" :value="item.columnName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="选择值字段" prop="valueColumn">
							<el-select v-model="ruleForm.valueColumn">
								<el-option v-for="item in columnData" :key="item.columnName" :label="item.columnName" :value="item.columnName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="父级字段" prop="pidColumn">
							<el-select v-model="ruleForm.pidColumn">
								<el-option v-for="item in columnData" :key="item.columnName" :label="item.columnName" :value="item.columnName" />
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

<script lang="ts">
import { reactive, toRefs, defineComponent, getCurrentInstance, ref, onMounted } from 'vue';

import { getDatabaseList, getTableList, getColumnList } from '/@/api/system/admin';
import { AddDbColumnInput } from '/@/api/system/interface';

export default defineComponent({
	name: 'tree',
	components: {},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		var rowdata = {} as any;
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as any,
			dbData: [] as any,
			tableData: [] as any,
			columnData: [] as any,
		});

		onMounted(async () => {
			var res = await getDatabaseList();
			state.dbData = res.data.result;
		});

		const DbChanged = async () => {
			state.tableData = [];
			state.columnData = [];
			await getTableInfoList();
		};

		const TableChanged = async () => {
			state.columnData = [];
			await getColumnInfoList();
		};

		const getDbList = async () => {
			var res = await getDatabaseList();
			state.dbData = res.data.result;
		};

		const getTableInfoList = async () => {
			if (state.ruleForm.configId == '') return;
			var res = await getTableList(state.ruleForm.configId);
			state.tableData = res.data.result;
		};

		const getColumnInfoList = async () => {
			if (state.ruleForm.configId == '' || state.ruleForm.tableName == '') return;
			console.log(state.ruleForm.configId, state.ruleForm.tableName);
			var res = await getColumnList(state.ruleForm.configId, state.ruleForm.tableName);
			state.columnData = res.data.result;
		};

		// 打开弹窗
		const openDialog = (row: any) => {
			rowdata = row;
			state.isShowDialog = true;
		};

		// 关闭弹窗
		const closeDialog = () => {
			rowdata.fkTableName = state.ruleForm.tableName;
			// rowdata.fkEntityName = state.ruleForm.entityName;
			// rowdata.fkColumnName = state.ruleForm.columnName;
			// rowdata.fkColumnNetType = state.ruleForm.columnNetType;
			rowdata.displayColumn = state.ruleForm.displayColumn;
			rowdata.valueColumn = state.ruleForm.valueColumn;
			rowdata.pidColumn = state.ruleForm.pidColumn;
			proxy.mittBus.emit('submitRefreshFk', rowdata);
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
			DbChanged,
			TableChanged,
			getDbList,
			getTableInfoList,
			getColumnInfoList,
		};
	},
});
</script>
