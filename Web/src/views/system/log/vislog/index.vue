<template>
	<div class="sys-vislog-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
            <el-form-item label="开始时间" prop="name">
              <el-date-picker v-model="state.queryParams.startTime" type="datetime" placeholder="开始时间" :shortcuts="shortcuts" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">  
            <el-form-item label="结束时间" prop="code">
              <el-date-picker v-model="state.queryParams.endTime" type="datetime" placeholder="结束时间" :shortcuts="shortcuts" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20 search-actions">
            <div>
              <el-button icon="ele-DeleteFilled" type="danger" @click="clearLog" v-auth="'sysVislog:clear'"> 清空 </el-button>
            </div>
            <div>
              <el-form-item>
                <el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysVislog:page'" plain> 查询 </el-button> 
              </el-form-item>
            </div>
          </el-col>
        </el-row>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="state.logData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="displayTitle" label="显示名称" width="150" show-overflow-tooltip />
				<el-table-column prop="actionName" label="方法名称" width="100" show-overflow-tooltip />
				<el-table-column prop="account" label="账号名称" width="100" show-overflow-tooltip />
				<el-table-column prop="realName" label="真实姓名" width="100" show-overflow-tooltip />
				<el-table-column prop="remoteIp" label="IP地址" width="120" show-overflow-tooltip />
				<el-table-column prop="location" label="登录地点" width="150" show-overflow-tooltip />
				<el-table-column prop="browser" label="浏览器" show-overflow-tooltip />
				<el-table-column prop="os" label="操作系统" width="150" show-overflow-tooltip />
				<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status === '200'">成功</el-tag>
						<el-tag type="danger" v-else>失败</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="elapsed" label="耗时(ms)" width="100" align="center" show-overflow-tooltip />
				<el-table-column prop="logDateTime" label="日志时间" width="160" align="center" fixed="right" show-overflow-tooltip />
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
	</div>
</template>

<script lang="ts" setup name="sysVisLog">
import { onMounted, reactive } from 'vue';
import { ElMessage } from 'element-plus';

import { getAPI } from '/@/utils/axios-utils';
import { SysLogVisApi } from '/@/api-services/api';
import { SysLogVis } from '/@/api-services/models';

const state = reactive({
	loading: false,
	queryParams: {
		startTime: undefined,
		endTime: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 10,
		total: 0 as any,
	},
	logData: [] as Array<SysLogVis>,
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	if (state.queryParams.startTime == null) state.queryParams.startTime = undefined;
	if (state.queryParams.endTime == null) state.queryParams.endTime = undefined;
	state.loading = true;
	var res = await getAPI(SysLogVisApi).apiSysLogVisPageGet(state.queryParams.startTime, state.queryParams.endTime, state.tableParams.page, state.tableParams.pageSize);
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
	await getAPI(SysLogVisApi).apiSysLogVisClearPost();
	state.loading = false;

	ElMessage.success('清空成功');
	handleQuery();
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
</script>
