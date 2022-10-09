<template>
	<div class="sys-pos-container">
		<el-card shadow="hover">
			<el-form :model="queryParams" ref="queryForm" :inline="true">
				<el-form-item label="职位名称" prop="name">
					<el-input placeholder="职位名称" clearable @keyup.enter="handleQuery" v-model="queryParams.name" />
				</el-form-item>
				<el-form-item label="职位编码" prop="code">
					<el-input placeholder="职位编码" clearable @keyup.enter="handleQuery" v-model="queryParams.code" />
				</el-form-item>
				<el-form-item>
					<el-button @click="resetQuery">
						<el-icon>
							<ele-Refresh />
						</el-icon>
						重置
					</el-button>
					<el-button type="primary" @click="handleQuery">
						<el-icon>
							<ele-Search />
						</el-icon>
						查询
					</el-button>
					<el-button @click="openAddPos">
						<el-icon>
							<ele-Plus />
						</el-icon>
						新增
					</el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 5px;">
			<el-table :data="posData" style="width: 100%">
				<el-table-column type="index" label="序号" width="80" />
				<el-table-column prop="name" label="职位名称" show-overflow-tooltip></el-table-column>
				<el-table-column prop="code" label="职位编码" show-overflow-tooltip></el-table-column>
				<el-table-column prop="order" label="排序" show-overflow-tooltip width="80" align="center">
				</el-table-column>
				<el-table-column prop="status" label="状态" show-overflow-tooltip width="80" align="center">
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
						<el-tag type="danger" v-else>禁用</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="createTime" label="修改时间" show-overflow-tooltip></el-table-column>
				<el-table-column prop="remark" label="备注" show-overflow-tooltip></el-table-column>
				<el-table-column label="操作" show-overflow-tooltip width="80" fixed="right" align="center">
					<template #default="scope">
						<el-button size="small" text type="primary" @click="openEditPos(scope.row)">
							<el-icon>
								<ele-Edit />
							</el-icon>
						</el-button>
						<el-button size="small" text type="primary" @click="delPos(scope.row)">
							<el-icon>
								<ele-Delete />
							</el-icon>
						</el-button>
					</template>
				</el-table-column>
			</el-table>
		</el-card>
		<EditPos ref="editPosRef" :title="editPosTitle" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, getCurrentInstance } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditPos from '/@/views/system/pos/component/editPos.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysPosApi } from '/@/api-services';

export default defineComponent({
	name: 'sysPos',
	components: { EditPos },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const editPosRef = ref();
		const state = reactive({
			loading: true,
			posData: [] as any,
			queryParams: {
				name: undefined,
				code: undefined,
			},
			editPosTitle: "",
		});
		onMounted(async () => {
			handleQuery();

			proxy.mittBus.on("submitRefresh", () => {
				handleQuery();
			});
		});
		onUnmounted(() => {
			proxy.mittBus.off("submitRefresh");
		});

		// 查询操作
		const handleQuery = () => {
			state.loading = true;
			getAPI(SysPosApi).sysPosListGet(state.queryParams.name, state.queryParams.code).then((res) => {
				state.posData = res.data.result;
				state.loading = false;
			});
		};
		// 重置操作
		const resetQuery = () => {
			state.queryParams.name = undefined;
			state.queryParams.code = undefined;
			handleQuery();
		};
		// 打开新增页面
		const openAddPos = () => {
			state.editPosTitle = "添加职位";
			editPosRef.value.openDialog({});
		};
		// 打开编辑页面
		const openEditPos = (row: any) => {
			state.editPosTitle = "编辑职位";
			editPosRef.value.openDialog(row);
		};
		// 删除
		const delPos = (row: any) => {
			ElMessageBox.confirm(`确定删除职位：【${row.name}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			})
				.then(() => {
					getAPI(SysPosApi).sysPosDeletePost({ id: row.id }).then(() => {
						handleQuery();
						ElMessage.success('删除成功');
					})
				})
				.catch(() => { });
		};
		return {
			handleQuery,
			resetQuery,
			editPosRef,
			openAddPos,
			openEditPos,
			delPos,
			...toRefs(state),
		};
	},
});
</script>
