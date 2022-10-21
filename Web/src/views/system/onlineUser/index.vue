<template>
	<div class="sys-onlineUser-container">
		<el-drawer v-model="state.isVisible" title="在线用户列表" size="45%">
			<el-card shadow="hover" :body-style="{ paddingBottom: '0' }" style="margin: 8px">
				<el-form :model="state.queryParams" ref="queryForm" :inline="true">
					<el-form-item label="账号名称" prop="userName">
						<el-input placeholder="账号名称" clearable @keyup.enter="handleQuery" v-model="state.queryParams.userName" />
					</el-form-item>
					<el-form-item label="真实姓名" prop="realName">
						<el-input placeholder="账号名称" clearable @keyup.enter="handleQuery" v-model="state.queryParams.realName" />
					</el-form-item>
					<el-form-item>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
						<el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
					</el-form-item>
				</el-form>
			</el-card>

			<el-card shadow="hover" style="margin: 8px">
				<el-table :data="state.onlineUserList" style="width: 100%" v-loading="state.loading" border>
					<el-table-column type="index" label="序号" width="55" align="center" />
					<el-table-column prop="userName" label="账号" show-overflow-tooltip></el-table-column>
					<el-table-column prop="realName" label="姓名" show-overflow-tooltip></el-table-column>
					<el-table-column prop="time" label="登录时间" show-overflow-tooltip></el-table-column>
					<el-table-column prop="ip" label="IP地址" show-overflow-tooltip> </el-table-column>
					<el-table-column prop="browser" label="浏览器" show-overflow-tooltip></el-table-column>
					<el-table-column prop="connectionId" label="连接Id" show-overflow-tooltip></el-table-column>
					<el-table-column label="操作" width="70" fixed="right" align="center" show-overflow-tooltip>
						<template #default="scope">
							<el-button icon="ele-CircleClose" size="small" text type="danger" v-auth="'sysUser:forceOffline'" @click="forceOffline(scope.row.connectionId)"> 下线 </el-button>
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

<script setup lang="ts">
import { reactive } from 'vue';
import { ElNotification } from 'element-plus';
import * as SignalR from '@microsoft/signalr';

import { getAPI, getToken, clearAccessTokens } from '/@/utils/axios-utils';
import { SysOnlineUserApi, SysAuthApi } from '/@/api-services/api';

const reciveMessage = (msg: any) => {
	console.log('接收消息：', msg);
};

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
		total: 0 as any,
	},
	onlineUserList: [] as any, // 在线用户列表
});

const signalrUrl = `${import.meta.env.VITE_API_URL}/hubs/onlineUser`;
// 初始化SignalR对象
const connection = new SignalR.HubConnectionBuilder()
	.configureLogging(SignalR.LogLevel.Information)
	.withUrl(`${signalrUrl}?access_token=${getToken()}`)
	.withAutomaticReconnect({
		nextRetryDelayInMilliseconds: () => {
			return 5000; // 每5秒重连一次
		},
	})
	.build();

connection.keepAliveIntervalInMilliseconds = 15000; // 心跳检测15s

// 注册web端方法供后端调用
connection.on('ReceiveMessage', reciveMessage);
// 强制用户下线
connection.on('ForceOffline', async (data: any) => {
	console.log('强制下线', data);
	await connection.stop();

	await getAPI(SysAuthApi).logoutPost();
	clearAccessTokens();
});
// 在线用户改变
connection.on('OnlineUserChange', (data: any) => {
	state.onlineUserList = data.userList;
	ElNotification({
		title: '提示',
		message: `${data.online ? `【${data.realName}】上线了` : `【${data.realName}】离开了`}`,
		type: 'info',
		position: 'bottom-right',
	});
});

// 第一次连接成功
connection.start().then(() => {});

// 连接断开
connection.onclose(async () => {});

// 掉线重连中
connection.onreconnecting(() => {});

// 重新连接成功
connection.onreconnected(() => {});

// 打开页面
const openDrawer = () => {
	state.isVisible = true;
};
// 查询操作
const handleQuery = async () => {
	state.loading = true;
	var res = await getAPI(SysOnlineUserApi).sysOnlineUserPageGet(state.queryParams.userName, state.queryParams.realName, state.tableParams.page, state.tableParams.pageSize);
	state.onlineUserList = res.data.result?.items;
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
const forceOffline = async (connectionId: any) => {
	await connection.send('ForceOffline', { connectionId }).catch(function (err) {
		console.log(err);
	});
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

// 导出
defineExpose({ openDrawer });
</script>

<style lang="scss"></style>
