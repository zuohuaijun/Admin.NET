<template>
	<div class="sys-codeGen-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="queryParams" ref="queryForm" :inline="true" v-loading="loading">
				<el-form-item>
					<el-button type="primary" icon="ele-Plus" @click="openAddDialog">增加</el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="tableData" style="width: 100%" v-loading="loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="configId" label="库定位器" show-overflow-tooltip />
				<el-table-column prop="tableName" label="表名称" show-overflow-tooltip />
				<el-table-column prop="busName" label="业务名" show-overflow-tooltip />
				<el-table-column prop="nameSpace" label="命名空间" show-overflow-tooltip />
				<el-table-column prop="authorName" label="作者姓名" show-overflow-tooltip />
				<el-table-column prop="generateType" label="生成方式" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.generateType === 1"> 下载压缩包 </el-tag>
						<el-tag type="danger" v-else> 生成到本项目 </el-tag>
					</template>
				</el-table-column>
				<el-table-column label="操作" width="200" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button size="small" text type="primary" @click="handleGenerate(scope.row)">开始生成</el-button>
						<el-button size="small" text type="primary" @click="openConfigDialog(scope.row)">配置</el-button>
						<el-button size="small" text type="primary" @click="openEditDialog(scope.row)">编辑</el-button>
						<el-button size="small" text type="primary" @click="deleConfig(scope.row)">删除</el-button>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination
				v-model:currentPage="tableParams.page"
				v-model:page-size="tableParams.pageSize"
				:total="tableParams.total"
				:page-sizes="[10, 20, 50, 100]"
				small
				background
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
			/>
		</el-card>
		<EditCodeGenDialog :title="editMenuTitle" ref="EditCodeGenRef" />
		<CodeConfigDialog ref="CodeConfigRef" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import mittBus from '/@/utils/mitt';
import EditCodeGenDialog from './component/editCodeGenDialog.vue';
import CodeConfigDialog from './component/genConfigDialog.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysCodeGenApi } from '/@/api-services/api';
import { SysCodeGen } from '/@/api-services/models';

export default defineComponent({
	name: 'sysCodeGen',
	components: { EditCodeGenDialog, CodeConfigDialog },
	setup() {
		const EditCodeGenRef = ref();
		const CodeConfigRef = ref();
		const state = reactive({
			loading: false,
			loading1: false,
			dbData: [] as any,
			configId: '',
			tableData: [] as Array<SysCodeGen>,
			tableName: '',
			queryParams: {
				name: undefined,
				code: undefined,
				tableName: undefined,
			},
			tableParams: {
				page: 1,
				pageSize: 10,
				total: 0 as any,
			},
			editMenuTitle: '',
		});

		onMounted(async () => {
			handleQuery();

			mittBus.on('submitRefresh', () => {
				handleQuery();
			});
			mittBus.on('submitRefreshFk', () => {
				state.tableData;
			});
		});

		onUnmounted(() => {
			mittBus.off('submitRefresh', () => {});
			mittBus.off('submitRefreshFk', () => {});
		});

		const openConfigDialog = (row: any) => {
			CodeConfigRef.value.openDialog(row);
		};
		// 表查询操作
		const handleQuery = async () => {
			state.loading = true;
			// let params = Object.assign(state.queryParams, state.tableParams);
			let res = await getAPI(SysCodeGenApi).sysCodeGenPageGet(
				undefined,
				undefined,
				undefined,
				undefined,
				undefined,
				undefined,
				undefined,
				undefined,
				state.queryParams.tableName,
				undefined,
				undefined,
				undefined,
				undefined,
				undefined,
				state.tableParams.page,
				state.tableParams.pageSize
			);
			state.tableData = res.data.result?.items ?? [];
			state.tableParams.total = res.data.result?.total;
			state.loading = false;
		};

		// 改变页面容量
		const handleSizeChange = (val: number) => {
			state.tableParams.pageSize = val;
			handleQuery();
		};

		// 改变页码序号
		const handleCurrentChange = (val: number) => {
			state.tableParams.page = val;
			handleQuery();
		};

		// 打开表增加页面
		const openAddDialog = () => {
			state.editMenuTitle = '增加';
			EditCodeGenRef.value.openDialog({ nameSpace: 'Admin.NET.Application', authorName: 'Admin.NET', generateType: '2' });
		};
		// 打开表编辑页面
		const openEditDialog = (row: any) => {
			state.editMenuTitle = '编辑';
			EditCodeGenRef.value.openDialog(row);
		};

		// 删除表
		const deleConfig = (row: any) => {
			ElMessageBox.confirm(`确定删除吗?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysCodeGenApi).sysCodeGenDeletePost([{ id: row.id }]);
					handleQuery();
					ElMessage.success('操作成功');
				})
				.catch(() => {});
		};

		// 开始生成代码
		const handleGenerate = (row: any) => {
			ElMessageBox.confirm(`确定要生成吗?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysCodeGenApi).sysCodeGenRunLocalPost(row);
					handleQuery();
					ElMessage.success('操作成功');
				})
				.catch(() => {});
		};

		return {
			EditCodeGenRef,
			CodeConfigRef,
			handleQuery,
			openAddDialog,
			openEditDialog,
			deleConfig,
			handleSizeChange,
			handleCurrentChange,
			openConfigDialog,
			handleGenerate,
			...toRefs(state),
		};
	},
});
</script>
