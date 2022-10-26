<template>
	<div class="sys-database-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="queryParams" ref="queryForm" :inline="true" v-loading="loading">
				<el-form-item label="库名" prop="configId">
					<el-select v-model="configId" placeholder="库名" filterable @change="handleQueryTable">
						<el-option v-for="item in dbData" :key="item" :label="item" :value="item" />
					</el-select>
				</el-form-item>
				<el-form-item label="表名" prop="tableName">
					<el-select v-model="tableName" placeholder="表名" filterable clearable @change="handleQueryColunm">
						<el-option v-for="item in tableData" :key="item.name" :label="item.name + '[' + item.description + ']'" :value="item.name" />
					</el-select>
				</el-form-item>
				<el-form-item>
					<!-- <el-button type="primary" icon="ele-Search" @click="handleQueryTable"> 查看 </el-button> -->
					<el-button icon="ele-Edit" type="primary" @click="openEditTable"> 编辑表 </el-button>
					<el-button icon="ele-Delete" type="danger" @click="delTable"> 删除表 </el-button>
					<el-button icon="ele-Plus" @click="openAddTable"> 增加表 </el-button>
					<el-button icon="ele-Plus" @click="openAddColumn"> 增加列 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="columnData" style="width: 100%" v-loading="loading1" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="dbColumnName" label="字段名" show-overflow-tooltip />
				<el-table-column prop="dataType" label="数据类型" show-overflow-tooltip />
				<el-table-column prop="isPrimarykey" label="主键" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.isPrimarykey === true">是</el-tag>
						<el-tag type="info" v-else>否</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="isIdentity" label="自增" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.isIdentity === true">是</el-tag>
						<el-tag type="info" v-else>否</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="isNullable" label="可空" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.isNullable === true">是</el-tag>
						<el-tag type="info" v-else>否</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="length" label="长度" width="70" align="center" show-overflow-tooltip />
				<el-table-column prop="decimalDigits" label="精度" width="70" align="center" show-overflow-tooltip />
				<el-table-column prop="defaultValue" label="默认值" show-overflow-tooltip />
				<el-table-column prop="columnDescription" label="描述" show-overflow-tooltip />
				<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditColumn(scope.row)"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delColumn(scope.row)"> 删除 </el-button>
					</template>
				</el-table-column>
			</el-table>
		</el-card>
		<EditTable ref="editTableRef" />
		<EditColumn ref="editColumnRef" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, getCurrentInstance } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditTable from '/@/views/system/database/component/editTable.vue';
import EditColumn from '/@/views/system/database/component/editColumn.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { DbColumnOutput, DbTableInfo } from '/@/api-services/models';

export default defineComponent({
	name: 'sysDatabase',
	components: { EditTable, EditColumn },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const editTableRef = ref();
		const editColumnRef = ref();
		const state = reactive({
			loading: false,
			loading1: false,
			dbData: [] as any,
			configId: '',
			tableData: [] as Array<DbTableInfo>,
			tableName: '',
			columnData: [] as Array<DbColumnOutput>,
			queryParams: {
				name: undefined,
				code: undefined,
			},
			editPosTitle: '',
		});
		onMounted(async () => {
			state.loading = true;
			var res = await getAPI(SysDatabaseApi).sysDatabaseListGet();
			state.dbData = res.data.result;
			state.loading = false;

			proxy.mittBus.on('submitRefreshTable', () => {
				handleQueryTable();
			});
			proxy.mittBus.on('submitRefreshColumn', () => {
				handleQueryColunm();
			});
		});
		onUnmounted(() => {
			proxy.mittBus.off('submitRefreshTable');
			proxy.mittBus.off('submitRefreshColumn');
		});
		// 表查询操作
		const handleQueryTable = async () => {
			state.tableName = '';
			state.columnData = [];

			state.loading = true;
			var res = await getAPI(SysDatabaseApi).sysDatabaseTableListGet(state.configId);
			state.tableData = res.data.result ?? [];
			state.loading = false;
		};
		// 列查询操作
		const handleQueryColunm = async () => {
			state.loading1 = true;
			var res = await getAPI(SysDatabaseApi).sysDatabaseColumnListGet(state.tableName, state.configId);
			state.columnData = res.data.result ?? [];
			state.loading1 = false;
		};
		// 打开表编辑页面
		const openEditTable = () => {
			if (state.configId == '' || state.tableName == '') return;

			var res = state.tableData.filter((u: any) => u.name == state.tableName);
			var table: any = {
				configId: state.configId,
				tableName: state.tableName,
				oldTableName: state.tableName,
				description: res[0].description,
			};
			editTableRef.value.openDialog(table);
		};
		// 打开表增加页面
		const openAddTable = () => {
			ElMessage({
				type: 'success',
				message: `期待您的PR！ ${state.configId}`,
			});
		};
		// 打开列编辑页面
		const openEditColumn = (row: any) => {
			var column: any = {
				configId: state.configId,
				tableName: row.tableName,
				columnName: row.dbColumnName,
				oldColumnName: row.dbColumnName,
				description: row.columnDescription,
			};
			editColumnRef.value.openDialog(column);
		};
		// 打开列增加页面
		const openAddColumn = () => {
			ElMessage({
				type: 'success',
				message: `期待您的PR！ ${state.configId}`,
			});
		};
		// 删除表
		const delTable = () => {
			ElMessageBox.confirm(`确定删除表：【${state.tableName}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysDatabaseApi).sysDatabaseTableDeletePost({ configId: state.configId, tableName: state.tableName });
					handleQueryTable();
					ElMessage.success('表删除成功');
				})
				.catch(() => {});
		};
		// 删除列
		const delColumn = (row: any) => {
			ElMessageBox.confirm(`确定删除列：【${row.dbColumnName}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysDatabaseApi).sysDatabaseColumnDeletePost({ configId: state.configId, tableName: state.tableName, dbColumnName: row.dbColumnName });
					handleQueryTable();
					ElMessage.success('列删除成功');
				})
				.catch(() => {});
		};
		return {
			handleQueryTable,
			handleQueryColunm,
			openEditTable,
			openAddTable,
			openEditColumn,
			openAddColumn,
			delTable,
			delColumn,
			editTableRef,
			editColumnRef,
			...toRefs(state),
		};
	},
});
</script>
