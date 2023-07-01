<template>
	<div class="sys-dictData-container">
		<el-dialog v-model="state.isShowDialog" draggable :close-on-click-modal="false" width="950px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> 字典值列表 </span>
				</div>
			</template>
			<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
				<el-form :model="state.queryParams" ref="queryForm" :inline="true">
					<el-form-item label="字典值">
						<el-input v-model="state.queryParams.value" placeholder="字典值" />
					</el-form-item>
					<el-form-item label="编码">
						<el-input v-model="state.queryParams.code" placeholder="编码" />
					</el-form-item>
					<el-form-item>
						<el-button-group>
							<el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
							<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
						</el-button-group>
					</el-form-item>
					<el-form-item>
						<el-button type="primary" icon="ele-Plus" @click="openAddDictData"> 新增 </el-button>
					</el-form-item>
				</el-form>
			</el-card>

			<el-card class="full-table" shadow="hover" style="margin-top: 8px">
				<el-table :data="state.dictDataData" style="width: 100%" v-loading="state.loading" border>
					<el-table-column type="index" label="序号" width="55" align="center" />
					<el-table-column prop="value" label="字典值" header-align="center" show-overflow-tooltip />
					<el-table-column prop="code" label="编码" header-align="center" show-overflow-tooltip />
					<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
						<template #default="scope">
							<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
							<el-tag type="danger" v-else>禁用</el-tag>
						</template>
					</el-table-column>
					<el-table-column prop="orderNo" label="排序" width="70" align="center" show-overflow-tooltip />
					<el-table-column prop="createTime" label="修改时间" width="130" align="center" show-overflow-tooltip />
					<el-table-column prop="remark" label="备注" header-align="center" show-overflow-tooltip />
					<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
						<template #default="scope">
							<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditDictData(scope.row)"> 编辑 </el-button>
							<el-button icon="ele-Delete" size="small" text type="danger" @click="delDictData(scope.row)"> 删除 </el-button>
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
		</el-dialog>

		<EditDictData ref="editDictDataRef" :title="state.editDictDataTitle" :dictTypeId="state.queryParams.dictTypeId" @handleQuery="handleQuery" />
	</div>
</template>

<script lang="ts" setup name="sysDictData">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditDictData from '/@/views/system/dict/component/editDictData.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDictDataApi } from '/@/api-services/api';
import { SysDictData } from '/@/api-services/models';

const editDictDataRef = ref();
const state = reactive({
	isShowDialog: false,
	loading: false,
	dictDataData: [] as Array<SysDictData>,
	queryParams: {
		value: undefined,
		code: undefined,
		dictTypeId: 0, // 字典类型Id
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editDictDataTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 打开弹窗
const openDialog = async (row: any) => {
	state.queryParams.dictTypeId = row.id;
	handleQuery();
	state.isShowDialog = true;
};

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysDictDataApi).apiSysDictDataPagePost(params);
	state.dictDataData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.value = undefined;
	state.queryParams.code = undefined;
	handleQuery();
};

// 打开新增页面
const openAddDictData = () => {
	state.editDictDataTitle = '添加字典值';
	editDictDataRef.value.openDialog({ status: 1, orderNo: 100, dictTypeId: state.queryParams.dictTypeId });
};

// 打开编辑页面
const openEditDictData = (row: any) => {
	state.editDictDataTitle = '编辑字典值';
	editDictDataRef.value.openDialog(row);
};

// 删除
const delDictData = (row: any) => {
	ElMessageBox.confirm(`确定删除字典值：【${row.value}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysDictDataApi).apiSysDictDataDeletePost({ id: row.id });
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

// 导出对象
defineExpose({ openDialog });
</script>
