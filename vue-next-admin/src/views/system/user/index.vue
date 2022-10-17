<template>
	<div class="sys-user-container">
		<el-row :gutter="8" style="width:100%">
			<el-col :span="4" :xs="24">
				<OrgTree ref="orgTreeRef" @node-click='nodeClick' />
			</el-col>

			<el-col :span="20" :xs="24">
				<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
					<el-form :model="queryParams" ref="queryForm" :inline="true">
						<el-form-item label="账号名称" prop="name">
							<el-input placeholder="账号名称" clearable @keyup.enter="handleQuery"
								v-model="queryParams.userName" />
						</el-form-item>
						<el-form-item label="手机号码" prop="code">
							<el-input placeholder="手机号码" clearable @keyup.enter="handleQuery"
								v-model="queryParams.phone" />
						</el-form-item>
						<el-form-item>
							<el-button icon="ele-Refresh" @click="resetQuery">
								重置
							</el-button>
							<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysUser:page'">
								查询
							</el-button>
							<el-button icon="ele-Plus" @click="openAddUser" v-auth="'sysUser:add'">
								新增
							</el-button>
						</el-form-item>
					</el-form>
				</el-card>

				<el-card shadow="hover" style="margin-top: 8px;">
					<el-table :data="userData" style="width: 100%;" v-loading="loading" border>
						<el-table-column type="index" label="序号" width="55" align="center" fixed />
						<el-table-column prop="userName" label="账号名称" width="120" fixed show-overflow-tooltip>
						</el-table-column>
						<el-table-column prop="nickName" label="昵称" width="120" show-overflow-tooltip></el-table-column>
						<el-table-column label="头像" width="80" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-avatar src="" size="small">{{scope.row.userName}} </el-avatar>
							</template>
						</el-table-column>
						<el-table-column label="出生日期" width="100" align="center" show-overflow-tooltip>
							<template #default="scope">
								{{formatDate(new Date(scope.row.birthday), 'YYYY-mm-dd')}}
							</template>
						</el-table-column>
						<el-table-column label="性别" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tag v-if="scope.row.sex === 1">男</el-tag>
								<el-tag type="danger" v-else>女</el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="phone" label="手机号码" width="120" align="center" show-overflow-tooltip>
						</el-table-column>
						<el-table-column prop="realName" label="真实姓名" width="120" show-overflow-tooltip>
						</el-table-column>
						<el-table-column prop="idCard" label="证件号码" width="150" show-overflow-tooltip></el-table-column>
						<el-table-column label="状态" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-switch v-model="scope.row.status" :active-value="1" :inactive-value="2" size="small"
									@change="changeStatus(scope.row)" v-auth="'sysUser:setStatus'">
								</el-switch>
							</template>
						</el-table-column>
						<el-table-column prop="order" label="排序" width="70" align="center" show-overflow-tooltip>
						</el-table-column>
						<el-table-column prop="createTime" label="修改时间" width="160" show-overflow-tooltip>
						</el-table-column>
						<el-table-column prop="remark" label="备注" width="200" show-overflow-tooltip></el-table-column>
						<el-table-column label="操作" width="110" align="center" fixed="right" show-overflow-tooltip>
							<template #default="scope">
								<el-tooltip content="用户编辑">
									<el-button icon="ele-Edit" size="small" text type="primary"
										@click="openEditUser(scope.row)" v-auth="'sysUser:update'">编辑
									</el-button>
								</el-tooltip>
								<el-dropdown>
									<span style="color: var(--el-color-primary);padding-top: 6px;padding-left: 12px;">
										<el-icon>
											<ele-MoreFilled />
										</el-icon>
									</span>
									<template #dropdown>
										<el-dropdown-menu>
											<el-dropdown-item icon="ele-OfficeBuilding" @click="openGrantOrg(scope.row)"
												:v-auth="'sysUser:grantData'">数据范围
											</el-dropdown-item>
											<el-dropdown-item icon="ele-RefreshLeft" @click="resetUserPwd(scope.row)"
												:v-auth="'sysUser:resetPwd'" disabled>重置密码
											</el-dropdown-item>
											<el-dropdown-item icon="ele-Delete" @click="delUser(scope.row)"
												:v-auth="'sysUser:delete'">
												删除账号
											</el-dropdown-item>
										</el-dropdown-menu>
									</template>
								</el-dropdown>
							</template>
						</el-table-column>
					</el-table>
					<el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
						:total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small background
						@size-change="handleSizeChange" @current-change="handleCurrentChange"
						layout="total, sizes, prev, pager, next, jumper" />
				</el-card>
			</el-col>
		</el-row>
		<EditUser ref="editUserRef" :title="editUserTitle" :orgData="orgTreeData" />
		<GrantOrg ref="grantOrgRef" />
	</div>
</template>

<script lang="ts">
import { ref, toRefs, reactive, onMounted, defineComponent, getCurrentInstance, onUnmounted } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import { formatDate } from '/@/utils/formatTime';
import OrgTree from '/@/views/system/org/component/orgTree.vue';
import EditUser from '/@/views/system/user/component/editUser.vue';
import GrantOrg from '/@/views/system/user/component/grantOrg.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysUserApi, SysOrgApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysUser',
	components: { OrgTree, EditUser, GrantOrg },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const orgTreeRef = ref()
		const editUserRef = ref();
		const grantOrgRef = ref();
		const state = reactive({
			loading: true,
			userData: [] as any,
			orgTreeData: [] as any, // 编辑页面上级机构树
			queryParams: {
				orgId: -1,
				userName: undefined,
				phone: undefined,

			},
			tableParams: {
				page: 1,
				pageSize: 10,
				total: 0 as any,
			},
			editUserTitle: "",
		});
		onMounted(async () => {
			loadOrgData();
			handleQuery();

			proxy.mittBus.on("submitRefresh", () => {
				handleQuery();
			});
		});
		onUnmounted(() => {
			proxy.mittBus.off("submitRefresh");
		});
		// 查询机构数据(可以从机构组件里面获取)
		const loadOrgData = async () => {
			state.loading = true;
			var res = await getAPI(SysOrgApi).sysOrgListGet(0);
			state.orgTreeData = res.data.result;
			state.loading = false;
		};
		// 查询操作
		const handleQuery = async () => {
			state.loading = true;
			var res = await getAPI(SysUserApi).sysUserPageGet(state.queryParams.userName, state.queryParams.phone,
				state.queryParams.orgId, state.tableParams.page, state.tableParams.pageSize);
			state.userData = res.data.result?.items;
			state.tableParams.total = res.data.result?.total;
			state.loading = false;
		};
		// 重置操作
		const resetQuery = () => {
			state.queryParams.orgId = -1;
			state.queryParams.userName = undefined;
			state.queryParams.phone = undefined;
			handleQuery();
		};
		// 打开新增页面
		const openAddUser = () => {
			state.editUserTitle = "添加账号";
			editUserRef.value.openDialog({});
		};
		// 打开编辑页面
		const openEditUser = (row: any) => {
			state.editUserTitle = "编辑账号";
			editUserRef.value.openDialog(row);
		};
		// 删除
		const delUser = (row: any) => {
			ElMessageBox.confirm(`确定删除账号：【${row.userName}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysUserApi).sysUserDeletePost({ id: row.id });
					handleQuery();
					ElMessage.success('删除成功');
				})
				.catch(() => { });
		};
		// 分页改变
		const handleSizeChange = (val: number) => {
			state.tableParams.pageSize = val;
			handleQuery();
		};
		// 分页改变
		const handleCurrentChange = (val: number) => {
			state.tableParams.page = val;
			handleQuery();
		};
		// 修改状态
		const changeStatus = async (row: any) => {
			await getAPI(SysUserApi).sysUserSetStatusPost({ id: row.id, status: row.status });
		}
		// 打开授权数据范围页面
		const openGrantOrg = (row: any) => {
			grantOrgRef.value.openDialog(row);
		};
		// 重置密码
		const resetUserPwd = async (row: any) => {
			ElMessageBox.confirm(`确定重置密码：【${row.userName}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysUserApi).sysUserResetPwdPost({ id: row.id });
					ElMessage.success('密码重置成功：123456');
				})
				.catch(() => { });
		}
		// 树组件点击
		const nodeClick = async (node: any) => {
			state.queryParams.orgId = node.id;
			state.queryParams.userName = undefined; // node.name;
			state.queryParams.phone = undefined;
			handleQuery();
		};
		return {
			handleQuery,
			resetQuery,
			orgTreeRef,
			editUserRef,
			grantOrgRef,
			openAddUser,
			openEditUser,
			delUser,
			handleSizeChange,
			handleCurrentChange,
			changeStatus,
			openGrantOrg,
			resetUserPwd,
			nodeClick,
			formatDate,
			...toRefs(state),
		};
	},
});
</script>
