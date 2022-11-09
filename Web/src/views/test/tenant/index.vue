<template>
	<div class="tenant-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="queryParams" ref="queryForm" :inline="true">
				<el-form-item label="名称" prop="name">
					<el-input placeholder="名称" clearable @keyup.enter="handleQuery" v-model="queryParams.name" />
				</el-form-item>
				<el-form-item>
					<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					<el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
					<el-button icon="ele-Plus" @click="openAddTenant"> 新增 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="tenantData" style="width: 100%" v-loading="loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="name" label="名称" show-overflow-tooltip />
				<el-table-column prop="createTime" label="修改时间" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditTenant(scope.row)"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delTenant(scope.row)"> 删除 </el-button>
					</template>
				</el-table-column>
			</el-table>
		</el-card>
		<EditTenant ref="editTenantRef" :title="editTenantTitle" />
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, onMounted, ref, defineComponent, onUnmounted, getCurrentInstance } from 'vue';
import { ElMessageBox } from 'element-plus';
import EditTenant from '/@/views/test/tenant/component/editTenant.vue';

import { getAPI } from '/@/utils/axios-utils';
import { TenantBusinessApi } from '/@/api-services/_business/api';
import { TenantBusiness } from '/@/api-services/_business/models';

export default defineComponent({
	name: 'testTenant',
	components: { EditTenant },
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const editTenantRef = ref();
		const state = reactive({
			loading: false,
			tenantData: [] as Array<TenantBusiness>,
			queryParams: {
				name: undefined,
			},
			editTenantTitle: '',
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
			var res = await getAPI(TenantBusinessApi).apiTenantBusinessGetBusinessListGet();
			state.tenantData = res.data.result ?? [];
			state.loading = false;
		};
		// 重置操作
		const resetQuery = () => {
			state.queryParams.name = undefined;
			handleQuery();
		};
		// 打开新增页面
		const openAddTenant = () => {
			state.editTenantTitle = '添加业务';
			editTenantRef.value.openDialog({});
		};
		// 打开编辑页面
		const openEditTenant = (row: any) => {
			state.editTenantTitle = '编辑业务';
			editTenantRef.value.openDialog(row);
		};
		// 删除
		const delTenant = (row: any) => {
			ElMessageBox.confirm(`确定删除业务：【${row.name}】?`, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			});
			// .then(async () => {
			// 	await getAPI(TenantBusinessApi).sysConfigDeletePost({ id: row.id });
			// 	handleQuery();
			// 	ElMessage.success('删除成功');
			// })
			// .catch(() => {});
		};
		return {
			handleQuery,
			resetQuery,
			editTenantRef,
			openAddTenant,
			openEditTenant,
			delTenant,
			...toRefs(state),
		};
	},
});
</script>
