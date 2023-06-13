<template>
	<div class="sys-dict-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="字典名称">
					<el-input v-model="state.queryParams.name" placeholder="字典名称" clearable />
				</el-form-item>
				<el-form-item label="字典编码">
					<el-input v-model="state.queryParams.code" placeholder="字典编码" clearable />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysDictType:page'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" icon="ele-Plus" @click="openAddDictType" v-auth="'sysDictType:add'"> 新增 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.dictTypeData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="name" label="字典名称" header-align="center" show-overflow-tooltip />
				<el-table-column prop="code" label="字典编码" header-align="center" show-overflow-tooltip />
				<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
						<el-tag type="danger" v-else>禁用</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="orderNo" label="排序" width="70" align="center" show-overflow-tooltip />
				<el-table-column prop="createTime" label="修改时间" align="center" show-overflow-tooltip />
				<el-table-column prop="remark" label="备注" header-align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="200" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditDictType(scope.row)" v-auth="'sysDictType:update'"> 编辑 </el-button>
						<el-button icon="ele-Memo" size="small" text type="primary" @click="openDictDataDialog(scope.row)" v-auth="'sysDictType:page'"> 字典 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delDictType(scope.row)" v-auth="'sysDictType:delete'"> 删除 </el-button>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination
				v-model:currentPage="state.tableParams.page"
				v-model:page-size="state.tableParams.pageSize"
				:total="state.tableParams.total"
				:page-sizes="[10, 20, 50, 100]"
				small
				background
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
			/>
		</el-card>

		<EditDictType ref="editDictTypeRef" :title="state.editDictTypeTitle" @handleQuery="handleQuery" />
		<DictDataDialog ref="dictDataDialogRef" />
	</div>
</template>

<script lang="ts" setup name="sysDict">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditDictType from '/@/views/system/dict/component/editDictType.vue';
import DictDataDialog from '/@/views/system/dict/component/dictDataDialog.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDictTypeApi } from '/@/api-services/api';
import { SysDictType } from '/@/api-services/models';

const editDictTypeRef = ref<InstanceType<typeof EditDictType>>();
const dictDataDialogRef = ref<InstanceType<typeof DictDataDialog>>();
const state = reactive({
	loading: false,
	dictTypeData: [] as Array<SysDictType>,
	queryParams: {
		name: undefined,
		code: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editDictTypeTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysDictTypeApi).apiSysDictTypePagePost(params);
	state.dictTypeData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.name = undefined;
	state.queryParams.code = undefined;
	handleQuery();
};

// 打开新增页面
const openAddDictType = () => {
	state.editDictTypeTitle = '添加字典';
	editDictTypeRef.value?.openDialog({ status: 1, orderNo: 100 });
};

// 打开编辑页面
const openEditDictType = (row: any) => {
	state.editDictTypeTitle = '编辑字典';
	editDictTypeRef.value?.openDialog(row);
};

// 打开字典值页面
const openDictDataDialog = (row: any) => {
	dictDataDialogRef.value?.openDialog(row);
};

// 删除
const delDictType = (row: any) => {
	ElMessageBox.confirm(`确定删除字典：【${row.name}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysDictTypeApi).apiSysDictTypeDeletePost({ id: row.id });
			handleQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
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
</script>
