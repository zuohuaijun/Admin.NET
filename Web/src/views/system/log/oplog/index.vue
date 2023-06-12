<template>
	<div class="sys-oplog-container" v-loading="state.loading">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="开始时间">
					<el-date-picker v-model="state.queryParams.startTime" type="datetime" placeholder="开始时间" value-format="YYYY-MM-DD HH:mm:ss" :shortcuts="shortcuts" />
				</el-form-item>
				<el-form-item label="结束时间">
					<el-date-picker v-model="state.queryParams.endTime" type="datetime" placeholder="结束时间" value-format="YYYY-MM-DD HH:mm:ss" :shortcuts="shortcuts" />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysOplog:page'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button icon="ele-DeleteFilled" type="danger" @click="clearLog" v-auth="'sysOplog:clear'"> 清空 </el-button>
					<el-button icon="ele-FolderOpened" @click="exportLog" v-auth="'sysOplog:export'"> 导出 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.logData" @sort-change="sortChange" style="width: 100%" border :row-class-name="tableRowClassName">
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="controllerName" label="模块名称" width="100" header-align="center" show-overflow-tooltip />
				<el-table-column prop="displayTitle" label="显示名称" width="170" header-align="center" show-overflow-tooltip />
				<el-table-column prop="actionName" label="方法名称" width="150" header-align="center" show-overflow-tooltip />
				<el-table-column prop="httpMethod" label="请求方式" width="90" align="center" show-overflow-tooltip />
				<el-table-column prop="requestUrl" label="请求地址" width="300" header-align="center" show-overflow-tooltip />
				<!-- <el-table-column prop="requestParam" label="请求参数" show-overflow-tooltip />
				<el-table-column prop="returnResult" label="返回结果" show-overflow-tooltip /> -->
				<el-table-column prop="logLevel" label="级别" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.logLevel === 1">调试</el-tag>
						<el-tag v-else-if="scope.row.logLevel === 2">消息</el-tag>
						<el-tag v-else-if="scope.row.logLevel === 3">警告</el-tag>
						<el-tag v-else-if="scope.row.logLevel === 4">错误</el-tag>
						<el-tag v-else>其他</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="eventId" label="事件Id" width="70" align="center" show-overflow-tooltip />
				<el-table-column prop="threadId" label="线程Id" sortable="custom" width="90" align="center" show-overflow-tooltip />
				<el-table-column prop="traceId" label="请求跟踪Id" width="150" header-align="center" sortable="custom" show-overflow-tooltip />
				<el-table-column prop="account" label="账号名称" width="100" align="center" show-overflow-tooltip />
				<el-table-column prop="realName" label="真实姓名" width="100" align="center" show-overflow-tooltip />
				<el-table-column prop="remoteIp" label="IP地址" width="120" align="center" show-overflow-tooltip />
				<el-table-column prop="location" label="登录地点" width="120" align="center" show-overflow-tooltip />
				<el-table-column prop="browser" label="浏览器" width="160" align="center" show-overflow-tooltip />
				<el-table-column prop="os" label="操作系统" width="120" align="center" show-overflow-tooltip />
				<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status === '200'">成功</el-tag>
						<el-tag type="danger" v-else>失败</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="elapsed" label="耗时(ms)" width="90" align="center" show-overflow-tooltip />
				<!-- <el-table-column prop="exception" label="异常对象" width="150" show-overflow-tooltip /> -->
				<!-- <el-table-column prop="message" label="日志消息" width="160" fixed="right" show-overflow-tooltip /> -->
				<el-table-column prop="logDateTime" label="日志时间" width="160" align="center" fixed="right" show-overflow-tooltip />
				<el-table-column label="操作" width="80" align="center" fixed="right" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-InfoFilled" size="small" text type="primary" @click="viewDetail(scope.row)" v-auth="'sysOplog:page'">详情 </el-button>
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
		<el-dialog v-model="state.dialogVisible" draggable width="1000px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Document /> </el-icon>
					<span> 日志详情 </span>
				</div>
			</template>
			<pre>{{ state.content }}</pre>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysOpLog">
import { onMounted, reactive } from 'vue';
import { ElMessage } from 'element-plus';
import { downloadByData, getFileName } from '/@/utils/download';

import { getAPI } from '/@/utils/axios-utils';
import { SysLogOpApi } from '/@/api-services/api';
import { SysLogOp } from '/@/api-services/models';

const state = reactive({
	loading: false,
	queryParams: {
		startTime: undefined,
		endTime: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		field: 'createTime', // 默认的排序字段
		order: 'descending', // 排序方向
		descStr: 'descending', // 降序排序的关键字符
		total: 0 as any,
	},
	logData: [] as Array<SysLogOp>,
	dialogVisible: false,
	content: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	if (state.queryParams.startTime == null) state.queryParams.startTime = undefined;
	if (state.queryParams.endTime == null) state.queryParams.endTime = undefined;

	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysLogOpApi).apiSysLogOpPagePost(params);
	state.logData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.startTime = undefined;
	state.queryParams.endTime = undefined;
	handleQuery();
};

// 清空日志
const clearLog = async () => {
	state.loading = true;
	await getAPI(SysLogOpApi).apiSysLogOpClearPost();
	state.loading = false;

	ElMessage.success('清空成功');
	handleQuery();
};

// 导出日志
const exportLog = async () => {
	state.loading = true;
	var res = await getAPI(SysLogOpApi).apiSysLogOpExportPost(state.queryParams, { responseType: 'blob' });
	state.loading = false;

	var fileName = getFileName(res.headers);
	downloadByData(res.data as any, fileName);
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

// 查看详情
const viewDetail = (row: any) => {
	state.content = row.message;
	state.dialogVisible = true;
};

// 设置行颜色
const tableRowClassName = (row: any) => {
	return row.row.exception != null ? 'warning-row' : '';
};

const shortcuts = [
	{
		text: '今天',
		value: new Date(),
	},
	{
		text: '昨天',
		value: () => {
			const date = new Date();
			date.setTime(date.getTime() - 3600 * 1000 * 24);
			return date;
		},
	},
	{
		text: '上周',
		value: () => {
			const date = new Date();
			date.setTime(date.getTime() - 3600 * 1000 * 24 * 7);
			return date;
		},
	},
];

// 列排序
const sortChange = (column: any) => {
	state.tableParams.field = column.prop;
	state.tableParams.order = column.order;
	handleQuery();
};
</script>

<style lang="scss" scoped>
.el-popper {
	max-width: 60%;
}
pre {
	white-space: break-spaces;
	line-height: 20px;
}
.el-table .warning-row {
	--el-table-tr-bg-color: var(--el-color-warning-light-9);
}
.el-table .success-row {
	--el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>
