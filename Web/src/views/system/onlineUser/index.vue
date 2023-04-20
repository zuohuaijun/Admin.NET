<template>
	<div class="sys-onlineUser-container">
		<el-drawer v-model="state.isVisible" title="在线用户列表" size="40%">
			<el-card shadow="hover" :body-style="{ paddingBottom: '0' }" style="margin: 8px">
				<el-form :model="state.queryParams" ref="queryForm" :inline="true">
					<el-form-item label="账号名称" prop="userName">
						<el-input placeholder="账号名称" clearable @keyup.enter="handleQuery" v-model="state.queryParams.userName" />
					</el-form-item>
					<!-- <el-form-item label="真实姓名" prop="realName">
						<el-input placeholder="账号名称" clearable @keyup.enter="handleQuery" v-model="state.queryParams.realName" />
					</el-form-item> -->
					<el-form-item>
						<el-button-group>
							<el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
							<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
						</el-button-group>
					</el-form-item>
				</el-form>
			</el-card>

			<el-card shadow="hover" style="margin: 8px; padding-bottom: 15px">
				<el-table :data="state.onlineUserList" style="width: 100%" v-loading="state.loading" border>
					<el-table-column type="index" label="序号" width="55" align="center" />
					<el-table-column prop="userName" label="账号" show-overflow-tooltip />
					<el-table-column prop="realName" label="姓名" show-overflow-tooltip />
					<el-table-column prop="ip" label="IP地址" show-overflow-tooltip />
					<el-table-column prop="browser" label="浏览器" show-overflow-tooltip />
					<!-- <el-table-column prop="connectionId" label="连接Id" show-overflow-tooltip></el-table-column> -->
					<el-table-column prop="time" label="登录时间" show-overflow-tooltip />
					<el-table-column label="操作" width="81" fixed="right" align="center" show-overflow-tooltip>
						<template #default="scope">
							<el-button icon="ele-CircleClose" size="small" text type="danger" v-auth="'sysOnlineUser:forceOffline'" @click="forceOffline(scope.row)"> 下线 </el-button>
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
		</el-drawer>
	</div>
</template>

<script lang="ts" setup>
import { onMounted, reactive } from 'vue';
import { ElMessageBox, ElNotification } from 'element-plus';
import { throttle } from 'lodash-es';

import { getAPI, clearAccessTokens } from '/@/utils/axios-utils';
import { SysOnlineUserApi, SysAuthApi } from '/@/api-services/api';
import { SysOnlineUser } from '/@/api-services/models';

import { signalR } from './signalR';

const state = reactive({
	loading: false,
	isVisible: false,
	queryParams: {
		userName: undefined,
		realName: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 10,
		total: 1 as any,
	},
	onlineUserList: [] as Array<SysOnlineUser>, // 在线用户列表
	lastUserState: {
		online: false,
		realName: '',
	}, // 最后接收的用户变更状态信息
});

onMounted(async () => {
	// 在线用户列表
	signalR.off('OnlineUserList');
	signalR.on('OnlineUserList', (data: any) => {
		state.onlineUserList = data.userList;
		state.lastUserState = {
			online: data.online,
			realName: data.realName,
		};
		notificationThrottle();
	});
	// 强制下线
	signalR.off('ForceOffline');
	signalR.on('ForceOffline', async (data: any) => {
		console.log('强制下线', data);
		await signalR.stop();

		await getAPI(SysAuthApi).apiSysAuthLogoutPost();
		clearAccessTokens();
	});
});

// 通知提示节流
const notificationThrottle = throttle(
	function () {
		ElNotification({
			title: '提示',
			message: `${state.lastUserState.online ? `【${state.lastUserState.realName}】上线了` : `【${state.lastUserState.realName}】离开了`}`,
			type: `${state.lastUserState.online ? 'info' : 'error'}`,
			position: 'bottom-right',
		});
	},
	3000,
	{
		leading: true,
		trailing: false,
	}
);

// 打开页面
const openDrawer = () => {
	state.isVisible = true;
};

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysOnlineUserApi).apiSysOnlineUserPagePost(params);
	state.onlineUserList = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.userName = undefined;
	state.queryParams.realName = undefined;
	handleQuery();
};

// 强制下线
const forceOffline = async (row: any) => {
	ElMessageBox.confirm(`确定踢掉账号：【${row.realName}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await signalR.send('ForceOffline', { connectionId: row.connectionId }).catch(function (err: any) {
				console.log(err);
			});
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
defineExpose({ openDrawer });
</script>
