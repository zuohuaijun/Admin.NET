<template>
	<div class="sys-job-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="queryParams" ref="queryForm" :inline="true">
				<el-form-item label="作业编号" prop="jobId">
					<el-input placeholder="作业编号" clearable @keyup.enter="handleQuery" v-model="queryParams.jobId" />
				</el-form-item>
				<el-form-item label="描述信息" prop="description">
					<el-input placeholder="描述信息" clearable @keyup.enter="handleQuery" v-model="queryParams.description" />
				</el-form-item>
				<el-form-item>
					<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysJob:page'"> 查询 </el-button>
					<el-button icon="ele-Plus" @click="openAddJobDetail" v-auth="'sysJob:add'"> 新增作业 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="jobData" style="width: 100%" v-loading="loading" border>
				<el-table-column type="expand" fixed>
					<template #default="scope">
						<el-table :data="scope.row.jobTriggers" border size="small">
							<el-table-column type="index" label="序号" width="55" align="center" fixed />
							<el-table-column prop="triggerId" label="触发器编号" width="120" fixed show-overflow-tooltip />
							<el-table-column prop="triggerType" label="类型" show-overflow-tooltip />
							<el-table-column prop="assemblyName" label="程序集" show-overflow-tooltip />
							<el-table-column prop="args" label="参数" show-overflow-tooltip />
							<el-table-column prop="description" label="描述" width="120" show-overflow-tooltip />
							<el-table-column prop="status" label="状态" width="100" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="scope.row.status == 0"> 积压 </el-tag>
									<el-tag v-if="scope.row.status == 1"> 就绪 </el-tag>
									<el-tag v-if="scope.row.status == 2"> 正在运行 </el-tag>
									<el-tag v-if="scope.row.status == 3"> 暂停 </el-tag>
									<el-tag v-if="scope.row.status == 4"> 阻塞 </el-tag>
									<el-tag v-if="scope.row.status == 5"> 由失败进入就绪 </el-tag>
									<el-tag v-if="scope.row.status == 6"> 归档 </el-tag>
									<el-tag v-if="scope.row.status == 7"> 崩溃 </el-tag>
									<el-tag v-if="scope.row.status == 8"> 超限 </el-tag>
									<el-tag v-if="scope.row.status == 9"> 无触发时间 </el-tag>
									<el-tag v-if="scope.row.status == 10"> 未启动 </el-tag>
									<el-tag v-if="scope.row.status == 11"> 未知作业触发器 </el-tag>
									<el-tag v-if="scope.row.status == 12"> 未知作业处理程序 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="startTime" label="起始时间" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="endTime" label="结束时间" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="lastRunTime" label="最近运行时间" width="160" align="center" show-overflow-tooltip />
							<el-table-column prop="nextRunTime" label="下一次运行时间" width="160" align="center" show-overflow-tooltip />
							<el-table-column prop="numberOfRuns" label="触发次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="maxNumberOfRuns" label="最大触发次数" width="150" align="center" show-overflow-tooltip />
							<el-table-column prop="numberOfErrors" label="出错次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="maxNumberOfErrors" label="最大出错次数" width="150" align="center" show-overflow-tooltip />
							<el-table-column prop="numRetries" label="重试次数" width="100" align="center" show-overflow-tooltip />
							<el-table-column prop="retryTimeout" label="重试间隔ms" width="120" align="center" show-overflow-tooltip />
							<el-table-column prop="startNow" label="是否立即启动" width="120" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="scope.row.startNow == true"> 是 </el-tag>
									<el-tag v-else> 否 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="runOnStart" label="是否启动时执行一次" width="160" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-tag v-if="scope.row.runOnStart == true"> 是 </el-tag>
									<el-tag v-else> 否 </el-tag>
								</template>
							</el-table-column>
							<el-table-column prop="updatedTime" label="更新时间" width="160" align="center" show-overflow-tooltip />
							<el-table-column label="操作" width="140" align="center" show-overflow-tooltip>
								<template #default="scope">
									<el-button icon="ele-Edit" size="small" text type="primary"> 编辑 </el-button>
									<el-button icon="ele-Delete" size="small" text type="danger"> 删除 </el-button>
								</template>
							</el-table-column>
						</el-table>
					</template>
				</el-table-column>
				<el-table-column type="index" label="序号" width="55" align="center" fixed />
				<el-table-column prop="jobDetail.jobId" label="作业编号" width="150" fixed>
					<template #default="scope">
						<div style="display: flex; align-items: center">
							<el-icon><timer /></el-icon>
							<span style="margin-left: 5px">{{ scope.row.jobDetail.jobId }}</span>
						</div>
					</template>
				</el-table-column>
				<el-table-column prop="jobDetail.groupName" label="组名称" show-overflow-tooltip />
				<el-table-column prop="jobDetail.jobType" label="类型" show-overflow-tooltip />
				<el-table-column prop="jobDetail.assemblyName" label="程序集" show-overflow-tooltip />
				<el-table-column prop="jobDetail.description" label="描述" show-overflow-tooltip />
				<el-table-column prop="jobDetail.concurrent" label="执行方式" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.jobDetail.concurrent == true"> 并行 </el-tag>
						<el-tag v-else> 串行 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="jobDetail.includeAnnotations" label="扫描特性触发器" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.includeAnnotations == true"> 是 </el-tag>
						<el-tag v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="jobDetail.properties" label="额外数据" show-overflow-tooltip />
				<el-table-column prop="jobDetail.updatedTime" label="更新时间" width="160" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="230" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Clock" size="small" text type="warning"> 新增触发器 </el-button>
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditJobDetail(scope.row)" v-auth="'sysJob:update'"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delJobDetail(scope.row)" v-auth="'sysJob:delete'"> 删除 </el-button>
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
		<EditJobDetail ref="editJobDetailRef" :title="editJobDetailTitle" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, onActivated, onDeactivated } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import mittBus from '/@/utils/mitt';
import { Timer } from '@element-plus/icons-vue';
import EditJobDetail from '/@/views/system/job/component/editJobDetail.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysJobApi } from '/@/api-services/api';
import { JobOutput } from '/@/api-services/models';

export default defineComponent({
	name: 'sysJob',
	components: { Timer, EditJobDetail },
	setup() {
		const editJobDetailRef = ref();
		const state = reactive({
			loading: false,
			jobData: [] as Array<JobOutput>,
			queryParams: {
				jobId: undefined,
				description: undefined,
			},
			tableParams: {
				page: 1,
				pageSize: 10,
				total: 0 as any,
			},
			editJobDetailTitle: '',
			timer: null as any,
		});
		onMounted(async () => {
			handleQuery();

			mittBus.on('submitRefresh', () => {
				handleQuery();
			});
		});
		onUnmounted(() => {
			mittBus.off('submitRefresh');
		});
		// 查询操作
		const handleQuery = async () => {
			state.loading = true;
			var res = await getAPI(SysJobApi).sysJobPageGet(state.queryParams.jobId, state.queryParams.description, state.tableParams.page, state.tableParams.pageSize);
			state.jobData = res.data.result?.items ?? [];
			state.tableParams.total = res.data.result?.total;
			state.loading = false;
		};
		// 重置操作
		const resetQuery = () => {
			state.queryParams.jobId = undefined;
			state.queryParams.description = undefined;
			handleQuery();
		};
		// 打开新增作业页面
		const openAddJobDetail = () => {
			state.editJobDetailTitle = '添加作业';
			editJobDetailRef.value.openDialog({});
		};
		// 打开编辑作业页面
		const openEditJobDetail = (row: any) => {
			console.log(row)
			state.editJobDetailTitle = '编辑作业';
			editJobDetailRef.value.openDialog(row.jobDetail);
		};
		// 删除作业
		const delJobDetail = (row: any) => {
			ElMessageBox.confirm(`确定删除作业：【${row.jobDetail.jobId}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysJobApi).sysJobDetailDeletePost({ jobId: row.jobDetail.jobId });
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
		// 修改状态
		const changeStatus = async (row: any) => {
			//await getAPI(SysJobApi).sysTimerSetStatusPost({ timerName: row.timerName, status: row.status });
		};
		onActivated(() => {
			// state.timer = setInterval(() => {
			// 	handleQuery();
			// }, 10000);
		});
		onDeactivated(() => {
			clearInterval(state.timer);
		});
		return {
			editJobDetailRef,
			handleQuery,
			resetQuery,
			openAddJobDetail,
			openEditJobDetail,
			delJobDetail,
			handleSizeChange,
			handleCurrentChange,
			changeStatus,
			...toRefs(state),
		};
	},
});
</script>
