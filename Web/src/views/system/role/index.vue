<template>
	<div class="sys-role-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="角色名称">
					<el-input v-model="state.queryParams.name" placeholder="角色名称" clearable />
				</el-form-item>
				<el-form-item label="角色编码">
					<el-input v-model="state.queryParams.code" placeholder="角色编码" clearable />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysRole:page'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" icon="ele-Plus" @click="openAddRole" v-auth="'sysRole:add'"> 新增 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.roleData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" fixed />
				<el-table-column prop="name" label="角色名称" align="center" show-overflow-tooltip />
				<el-table-column prop="code" label="角色编码" align="center" show-overflow-tooltip />
				<el-table-column label="数据范围" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag effect="plain" v-if="scope.row.dataScope === 1">全部数据</el-tag>
						<el-tag effect="plain" v-else-if="scope.row.dataScope === 2">本部门及以下数据</el-tag>
						<el-tag effect="plain" v-else-if="scope.row.dataScope === 3">本部门数据</el-tag>
						<el-tag effect="plain" v-else-if="scope.row.dataScope === 4">仅本人数据</el-tag>
						<el-tag effect="plain" v-else-if="scope.row.dataScope === 5">自定义数据</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="orderNo" label="排序" width="70" align="center" show-overflow-tooltip />
				<el-table-column label="状态" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
						<el-tag type="danger" v-else>禁用</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="createTime" label="修改时间" align="center" show-overflow-tooltip />
				<el-table-column prop="remark" label="备注" header-align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="110" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditRole(scope.row)" v-auth="'sysRole:update'"> 编辑 </el-button>
						<el-dropdown>
							<el-button icon="ele-MoreFilled" size="small" text type="primary" style="padding-left: 12px" />
							<template #dropdown>
								<el-dropdown-menu>
									<el-dropdown-item icon="ele-OfficeBuilding" @click="openGrantData(scope.row)" :disabled="!auth('sysRole:grantDataScope')"> 数据范围 </el-dropdown-item>
									<el-dropdown-item icon="ele-Delete" @click="delRole(scope.row)" divided :disabled="!auth('sysRole:delete')"> 删除角色 </el-dropdown-item>
								</el-dropdown-menu>
							</template>
						</el-dropdown>
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

		<EditRole ref="editRoleRef" :title="state.editRoleTitle" @handleQuery="handleQuery" />
		<GrantData ref="grantDataRef" @handleQuery="handleQuery" />
	</div>
</template>

<script lang="ts" setup name="sysRole">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import { auth } from '/@/utils/authFunction';
import EditRole from '/@/views/system/role/component/editRole.vue';
import GrantData from '/@/views/system/role/component/grantData.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysRoleApi } from '/@/api-services/api';
import { SysRole } from '/@/api-services/models';

const editRoleRef = ref<InstanceType<typeof EditRole>>();
const grantDataRef = ref<InstanceType<typeof GrantData>>();
const state = reactive({
	loading: false,
	roleData: [] as Array<SysRole>,
	queryParams: {
		name: undefined,
		code: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editRoleTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysRoleApi).apiSysRolePagePost(params);
	state.roleData = res.data.result?.items ?? [];
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
const openAddRole = () => {
	state.editRoleTitle = '添加角色';
	editRoleRef.value?.openDialog({ id: undefined, status: 1, orderNo: 100 });
};

// 打开编辑页面
const openEditRole = async (row: any) => {
	state.editRoleTitle = '编辑角色';
	editRoleRef.value?.openDialog(row);
};

// 打开授权数据范围页面
const openGrantData = (row: any) => {
	grantDataRef.value?.openDialog(row);
};

// 删除
const delRole = (row: any) => {
	ElMessageBox.confirm(`确定删角色：【${row.name}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysRoleApi).apiSysRoleDeletePost({ id: row.id });
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
