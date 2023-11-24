<template>
	<div class="sys-open-access-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="身份标识">
					<el-input v-model="state.queryParams.accessKey" placeholder="身份标识" clearable />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysOpenAccess:page'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" icon="ele-Plus" @click="openAddOpenAccess" v-auth="'sysOpenAccess:add'"> 新增 </el-button>
					<el-button icon="ele-QuestionFilled" @click="openHelp"> 说明 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.openAccessData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="accessKey" label="身份标识" header-align="center" show-overflow-tooltip />
				<el-table-column prop="accessSecret" label="密钥" header-align="center" show-overflow-tooltip />
				<el-table-column prop="bindUserAccount" label="绑定用户账号" header-align="center" show-overflow-tooltip />
				<el-table-column prop="bindTenantName" label="绑定租户名称" header-align="center" show-overflow-tooltip />
				<el-table-column prop="createTime" label="创建时间" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="200" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditOpenAccess(scope.row)" v-auth="'sysOpenAccess:update'" :disabled="scope.row.status === 1"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delOpenAccess(scope.row)" v-auth="'sysOpenAccess:delete'" :disabled="scope.row.status === 1"> 删除 </el-button>
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

		<EditOpenAccess ref="editOpenAccessRef" :title="state.editOpenAccessTitle" @handleQuery="handleQuery" />
		<HelpView ref="helpViewRef" />
	</div>
</template>

<script lang="ts" setup name="sysOpenAccess">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditOpenAccess from '/@/views/system/openAccess/component/editOpenAccess.vue';
import HelpView from '/@/views/system/openAccess/component/helpView.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysOpenAccessApi } from '/@/api-services/api';
import { OpenAccessOutput } from '/@/api-services/models';

const editOpenAccessRef = ref<InstanceType<typeof EditOpenAccess>>();
const helpViewRef = ref<InstanceType<typeof HelpView>>();
const state = reactive({
	loading: false,
	openAccessData: [] as Array<OpenAccessOutput>,
	queryParams: {
		accessKey: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editOpenAccessTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysOpenAccessApi).apiSysOpenAccessPagePost(params);
	state.openAccessData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.accessKey = undefined;
	handleQuery();
};

// 打开新增页面
const openAddOpenAccess = () => {
	state.editOpenAccessTitle = '添加开放接口身份';
	editOpenAccessRef.value?.openDialog({ type: 1 });
};

// 打开编辑页面
const openEditOpenAccess = (row: any) => {
	state.editOpenAccessTitle = '编辑开放接口身份';
	editOpenAccessRef.value?.openDialog(row);
};

// 删除
const delOpenAccess = (row: any) => {
	ElMessageBox.confirm(`确定删除开放接口身份：【${row.accessKey}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysOpenAccessApi).apiSysOpenAccessDeletePost({ id: row.id });
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

// 打开说明页面
const openHelp = () => {
	helpViewRef.value?.openDialog();
};
</script>
