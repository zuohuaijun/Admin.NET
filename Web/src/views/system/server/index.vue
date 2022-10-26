<template>
	<div class="sys-server-container">
		<el-row :gutter="8">
			<el-col :md="12" :sm="24">
				<el-card shadow="hover" header="系统信息">
					<table class="sysInfo_table">
						<tr>
							<td class="sysInfo_td">主机名称：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.hostName }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">操作系统：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.systemOs }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">系统架构：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.osArchitecture }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">CPU核数：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.processorCount }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">运行时长：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.sysRunTime }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">外网地址：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.remoteIp }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">内网地址：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.localIp }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">运行框架：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.frameworkDescription }}</td>
						</tr>
					</table>
				</el-card>
			</el-col>
			<el-col :md="12" :sm="24">
				<el-card shadow="hover" header="使用信息" v-loading="loading">
					<table class="sysInfo_table">
						<tr>
							<td class="sysInfo_td">启动时间：</td>
							<td class="sysInfo_td">{{ machineUseInfo.startTime }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">运行时长：</td>
							<td class="sysInfo_td">{{ machineUseInfo.runTime }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">内存总量：</td>
							<td class="sysInfo_td">{{ machineUseInfo.totalRam }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">使用内存：</td>
							<td class="sysInfo_td">{{ machineUseInfo.usedRam }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">空闲内存：</td>
							<td class="sysInfo_td">{{ machineUseInfo.freeRam }}</td>
						</tr>
						<tr>
							<td class="sysInfo_td">内存使用率：</td>
							<td class="sysInfo_td">
								<el-tag round>{{ machineUseInfo.ramRate }}</el-tag>
							</td>
						</tr>
						<tr>
							<td class="sysInfo_td">CPU使用率：</td>
							<td class="sysInfo_td">
								<el-tag round>{{ machineUseInfo.cpuRate }}</el-tag>
							</td>
						</tr>
						<tr>
							<td class="sysInfo_td">其他信息：</td>
							<td class="sysInfo_td">{{ machineBaseInfo.environment }}</td>
						</tr>
					</table>
				</el-card>
			</el-col>
		</el-row>
		<el-card shadow="hover" header="程序集信息" style="margin-top: 8px">
			<div v-for="d in assemblyInfo" :key="d.name" style="display: inline-flex; margin: 4px">
				<el-tag round>
					<div style="display: inline-flex">
						<div style="">{{ d.name }}</div>
						<div style="font-size: 9px; color: black; margin-left: 3px">{{ d.version }}</div>
					</div>
				</el-tag>
			</div>
		</el-card>
		<el-card shadow="hover" header="磁盘信息" style="margin-top: 8px">
			<el-table :data="machineDiskInfo" style="width: 100%">
				<el-table-column prop="diskName" label="盘符">
					<template #default="scope">
						<el-tag round>{{ scope.row.diskName }}</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="typeName" label="类型" />
				<el-table-column prop="totalSize" label="磁盘总量">
					<template #default="scope">{{ scope.row.totalSize }} GB</template>
				</el-table-column>
				<el-table-column prop="used" label="已使用">
					<template #default="scope">{{ scope.row.used }} GB</template>
				</el-table-column>
				<el-table-column prop="availableFreeSpace" label="剩余量">
					<template #default="scope">{{ scope.row.availableFreeSpace }} GB</template>
				</el-table-column>
				<el-table-column prop="availablePercent" label="使用率">
					<template #default="scope">
						<el-tag>{{ scope.row.availablePercent }}%</el-tag>
					</template>
				</el-table-column>
			</el-table>
		</el-card>
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, defineComponent, onActivated, onDeactivated, onMounted } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysServerApi } from '/@/api-services';

export default defineComponent({
	name: 'sysServer',
	components: {},
	setup() {
		const state = reactive({
			loading: false,
			machineBaseInfo: [] as any,
			machineUseInfo: [] as any,
			machineDiskInfo: [] as any,
			assemblyInfo: [] as any,
			timer: null as any,
		});
		onMounted(async () => {
			loadMachineBaseInfo();
			loadMachineUseInfo();
			loadMachineDiskInfo();
			loadAssemblyInfo();
		});
		// 服务器配置信息
		const loadMachineBaseInfo = async () => {
			var res = await getAPI(SysServerApi).serverBaseGet();
			state.machineBaseInfo = res.data.result;
		};
		// 服务器内存信息
		const loadMachineUseInfo = async () => {
			state.loading = true;
			var res = await getAPI(SysServerApi).serverUseGet();
			state.machineUseInfo = res.data.result;
			state.loading = false;
		};
		// 服务器磁盘信息
		const loadMachineDiskInfo = async () => {
			var res = await getAPI(SysServerApi).serverDiskGet();
			state.machineDiskInfo = res.data.result;
		};
		// 框架程序集信息
		const loadAssemblyInfo = async () => {
			var res = await getAPI(SysServerApi).serverAssemblyGet();
			state.assemblyInfo = res.data.result;
		};
		// 实时刷新内存
		const refreshData = () => {
			loadMachineUseInfo();
		};
		onActivated(() => {
			state.timer = setInterval(() => {
				refreshData();
			}, 10000);
		});
		onDeactivated(() => {
			clearInterval(state.timer);
		});
		return {
			loadMachineBaseInfo,
			loadMachineUseInfo,
			loadMachineDiskInfo,
			loadAssemblyInfo,
			refreshData,
			...toRefs(state),
		};
	},
});
</script>

<style lang="scss">
.sysInfo_table {
	width: 100%;
	min-height: 40px;
	line-height: 40px;
	text-align: center;
}

.sysInfo_td {
	border-bottom: 1px solid #e8e8e8;
}
</style>
