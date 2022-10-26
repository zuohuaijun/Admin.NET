<template>
	<div class="sys-timer-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="queryParams" ref="queryForm" :inline="true">
				<el-form-item label="任务名称" prop="timerName">
					<el-input placeholder="任务名称" clearable @keyup.enter="handleQuery" v-model="queryParams.timerName" />
				</el-form-item>
				<el-form-item>
					<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysTimer:page'"> 查询 </el-button>
					<el-button icon="ele-Plus" @click="openAddTimer" v-auth="'sysTimer:add'"> 新增 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="timerData" style="width: 100%" v-loading="loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="timerName" label="任务名称" show-overflow-tooltip></el-table-column>
				<el-table-column prop="requestUrl" label="请求地址" show-overflow-tooltip></el-table-column>
				<el-table-column prop="requestType" label="请求类型" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="info" v-if="scope.row.requestType == 0"> {{ 'RUN' }} </el-tag>
						<el-tag type="info" v-else-if="scope.row.requestType == 1"> {{ 'GET' }} </el-tag>
						<el-tag type="info" v-else-if="scope.row.requestType == 2"> {{ 'POST' }} </el-tag>
						<el-tag type="info" v-else-if="scope.row.requestType == 3"> {{ 'PUT' }} </el-tag>
						<el-tag type="info" v-else-if="scope.row.requestType == 4"> {{ 'DELETE' }} </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="timerType" label="任务类型" align="center" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.timerType == 0 ? scope.row.interval : scope.row.cron }}
					</template>
				</el-table-column>
				<el-table-column prop="executeType" label="执行类型" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.executeType == 0 ? '并行' : '串行' }}
					</template>
				</el-table-column>
				<el-table-column prop="doOnce" label="执行一次" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.doOnce ? '是' : '否' }}
					</template>
				</el-table-column>
				<el-table-column prop="startNow" label="立即执行" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.startNow ? '是' : '否' }}
					</template>
				</el-table-column>
				<el-table-column prop="status" label="任务状态" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status == 0"> {{ '运行中' }} </el-tag>
						<el-tag type="danger" v-else-if="scope.row.status == 1"> {{ '已停止' }} </el-tag>
						<el-tag type="danger" v-else-if="scope.row.status == 2"> {{ '已失败' }} </el-tag>
						<el-tag type="danger" v-else-if="scope.row.status == 3"> {{ '已删除' }} </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="status" label="启动停止" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-switch v-model="scope.row.status" :active-value="0" :inactive-value="1" size="small" @change="changeStatus(scope.row)" v-auth="'sysTimer:setStatus'" />
					</template>
				</el-table-column>
				<el-table-column prop="tally" label="执行次数" width="100" align="center" show-overflow-tooltip></el-table-column>
				<el-table-column prop="createTime" label="修改时间" width="100" align="center" show-overflow-tooltip></el-table-column>
				<el-table-column prop="remark" label="备注" show-overflow-tooltip></el-table-column>
				<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditTimer(scope.row)" v-auth="'sysTimer:update'"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delTimer(scope.row)" v-auth="'sysTimer:delete'"> 删除 </el-button>
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
		<EditTimer ref="editTimerRef" :title="editTimerTitle" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, getCurrentInstance, onActivated, onDeactivated } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditTimer from '/@/views/system/timer/component/editTimer.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysTimerApi, TimerOutput } from '/@/api-services';

export default defineComponent({
	name: 'sysTimer',
	components: { EditTimer },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const editTimerRef = ref();
		const state = reactive({
			loading: false,
			timerData: [] as Array<TimerOutput>,
			queryParams: {
				timerName: undefined,
			},
			tableParams: {
				page: 1,
				pageSize: 10,
				total: 0 as any,
			},
			editTimerTitle: '',
			timer: null as any,
		});
		onMounted(async () => {
			handleQuery();

			proxy.mittBus.on('submitRefresh', () => {
				handleQuery();
			});
		});
		onUnmounted(() => {
			proxy.mittBus.off('submitRefresh');
		});
		// 查询操作
		const handleQuery = async () => {
			state.loading = true;
			var res = await getAPI(SysTimerApi).sysTimerPageGet(state.queryParams.timerName, state.tableParams.page, state.tableParams.pageSize);
			state.timerData = res.data.result?.items ?? [];
			state.tableParams.total = res.data.result?.total;
			state.loading = false;
		};
		// 重置操作
		const resetQuery = () => {
			state.queryParams.timerName = undefined;
			handleQuery();
		};
		// 打开新增页面
		const openAddTimer = () => {
			state.editTimerTitle = '添加任务';
			editTimerRef.value.openDialog({});
		};
		// 打开编辑页面
		const openEditTimer = (row: any) => {
			state.editTimerTitle = '编辑任务';
			editTimerRef.value.openDialog(row);
		};
		// 删除
		const delTimer = (row: any) => {
			ElMessageBox.confirm(`确定删除任务：【${row.timerName}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(async () => {
					await getAPI(SysTimerApi).sysTimerDeletePost({ id: row.id });
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
			await getAPI(SysTimerApi).sysTimerSetStatusPost({ timerName: row.timerName, status: row.status });
		};
		onActivated(() => {
			state.timer = setInterval(() => {
				handleQuery();
			}, 10000);
		});
		onDeactivated(() => {
			clearInterval(state.timer);
		});
		return {
			handleQuery,
			resetQuery,
			editTimerRef,
			openAddTimer,
			openEditTimer,
			delTimer,
			handleSizeChange,
			handleCurrentChange,
			changeStatus,
			...toRefs(state),
		};
	},
});
</script>
