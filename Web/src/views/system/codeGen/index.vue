<template>
	<div class="sys-codegenerate-container">
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
						<span>{{ getGenerateTypeString(scope.row.generateType) }}</span>
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
		<CodeGenerateDialog :code-generate-type-list="codeGenerateTypeList" :menu-data="menudata" :title="editMenuTitle" ref="CodeGenerateRef" />
		<CodeConfigDialog ref="CodeConfigRef" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, getCurrentInstance } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import CodeGenerateDialog from '/@/views/system/codeGen/component/codeGenerateDialog.vue';
import CodeConfigDialog from '/@/views/system/codeGen/component/generateConfigDialog.vue';

import { SysCodeGen, SysMenu } from '/@/api/system/interface';
import { getGeneratePage, getMenuList, getDictDataDropdown, deleGenerate, generateRunLocal } from '/@/api/system/admin';

export default defineComponent({
	name: 'codeGen',
	components: { CodeGenerateDialog, CodeConfigDialog },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const CodeGenerateRef = ref();
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
			},
			tableParams: {
				page: 1,
				pageSize: 10,
				total: 0 as any,
			},
			editMenuTitle: '',
			menudata: [] as Array<SysMenu>,
			codeGenerateTypeList: [] as any,
		});

		onMounted(async () => {
			handleQuery();
			let res = await getMenuList();
			state.menudata = res.data.result;

			let list = await getDictDataDropdown('code_gen_create_type');
			state.codeGenerateTypeList = list.data.result;
			proxy.mittBus.on('submitRefresh', () => {
				handleQuery();
			});
			proxy.mittBus.on('submitRefreshFk', () => {
				state.tableData;
			});
		});

		onUnmounted(() => {
			proxy.mittBus.off('submitRefresh');
			proxy.mittBus.off('submitRefreshFk');
		});

		const openConfigDialog = (row: any) => {
			CodeConfigRef.value.openDialog(row);
		};
		// 表查询操作
		const handleQuery = async () => {
			state.loading = true;
			let params = Object.assign(state.queryParams, state.tableParams);
			let res = await getGeneratePage(params);
			state.tableData = res.data.result?.items ?? [];
			state.tableParams.total = res.data.result?.total;
			state.loading = false;
		};

		// 获取生成方式
		const getGenerateTypeString = (val: string) => {
			let lst = state.codeGenerateTypeList.filter((u: any) => u.value == val);
			return lst[0]?.label;
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
			state.editMenuTitle = '新增';
			CodeGenerateRef.value.openDialog({ type: '1' });
		};
		// 打开列编辑页面
		const openEditDialog = (row: any) => {
			state.editMenuTitle = '编辑';
			row.type = '2';
			CodeGenerateRef.value.openDialog(row);
		};

		// 删除表
		const deleConfig = (row: any) => {
			ElMessageBox.confirm(`确定删除吗?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await deleGenerate([{ id: row.id }]);
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
					await generateRunLocal(row);
					handleQuery();
					ElMessage.success('操作成功');
				})
				.catch(() => {});
		};

		return {
			handleQuery,
			openAddDialog,
			openEditDialog,
			deleConfig,
			CodeGenerateRef,
			handleSizeChange,
			handleCurrentChange,
			getGenerateTypeString,
			...toRefs(state),
			CodeConfigRef,
			openConfigDialog,
			handleGenerate,
		};
	},
});
</script>
