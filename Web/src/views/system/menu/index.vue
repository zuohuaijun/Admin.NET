<template>
	<div class="sys-menu-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="菜单名称">
					<el-input v-model="state.queryParams.title" placeholder="菜单名称" clearable />
				</el-form-item>
				<el-form-item label="类型">
					<el-select v-model="state.queryParams.type" placeholder="类型" clearable>
						<el-option label="目录" :value="1" />
						<el-option label="菜单" :value="2" />
						<el-option label="按钮" :value="3" />
					</el-select>
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysMenu:list'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" icon="ele-Plus" @click="openAddMenu" v-auth="'sysMenu:add'"> 新增 </el-button>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-auto-resizer>
				<template #default="{ height, width }">
					<el-table-v2
						:data="state.menuData"
						:width="width"
						v-model:expanded-row-keys="expandedRowKeys"
						:height="height"
						:expand-column-key="expandColumnKey"
						:columns="state.columns"
						v-loading="state.loading"
						row-key="id"
						border
					>
					</el-table-v2>
				</template>
			</el-auto-resizer>
		</el-card>

		<EditMenu ref="editMenuRef" :title="state.editMenuTitle" :menuData="state.menuData" @handleQuery="handleQuery" />
	</div>
</template>

<script lang="ts" setup name="sysMenu">
import { onMounted, reactive, ref, h, resolveComponent, resolveDirective, withDirectives } from 'vue';
import { ElMessageBox, ElMessage, ElPopover, ElTag, ElButton, ElText, ElIcon, ElDescriptions, ElDescriptionsItem } from 'element-plus';
import EditMenu from '/@/views/system/menu/component/editMenu.vue';
const SvgIcon = resolveComponent('SvgIcon');

import { getAPI } from '/@/utils/axios-utils';
import { SysMenuApi } from '/@/api-services/api';
import { SysMenu } from '/@/api-services/models';

const authBtn = resolveDirective('auth');
const editMenuRef = ref<InstanceType<typeof EditMenu>>();
const expandedRowKeys = ref<string[]>([]);
const expandColumnKey = 'title';
const generateColumns = () => {
	return [
		{
			key: 'title',
			title: '菜单名称',
			dataKey: 'title',
			align: 'left',
			width: 200,
			cellRenderer: ({ rowData }) => {
				return h('div', {}, [h(SvgIcon, { name: rowData.icon }), h('span', { class: 'ml10' }, { default: () => rowData.title })]);
			},
		},
		{
			key: 'type',
			title: '类型',
			dataKey: 'type',
			align: 'center',
			width: 80,
			cellRenderer: ({ rowData }) => {
				let t = '';
				let c = '';
				if (rowData.type === 1) {
					t = 'warning';
					c = '目录';
				} else if (rowData.type === 2) {
					c = '菜单';
				} else {
					t = 'info';
					c = '按钮';
				}
				return h(
					ElTag,
					{
						type: t,
					},
					{ default: () => c }
				);
			},
		},
		{
			key: 'path',
			title: '路由路径',
			dataKey: 'path',
			align: 'left',
			width: 200,
		},
		{
			key: 'component',
			title: '组件路径',
			dataKey: 'component',
			align: 'center',
			width: 220,
		},
		{
			key: 'permission',
			title: '权限标识',
			dataKey: 'permission',
			align: 'center',
			width: 180,
		},
		{
			key: 'orderNo',
			title: '排序',
			dataKey: 'orderNo',
			align: 'center',
			width: 70,
		},
		{
			key: 'status',
			title: '状态',
			dataKey: 'status',
			align: 'center',
			width: 80,
			cellRenderer: ({ rowData }) => {
				let t = '';
				let c = '';
				if (rowData.type === 1) {
					t = 'success';
					c = '启用';
				} else {
					t = 'danger';
					c = '禁用';
				}
				return h(
					ElTag,
					{
						type: t,
					},
					{ default: () => c }
				);
			},
		},
		{
			key: 'status',
			title: '修改记录',
			dataKey: 'status',
			align: 'center',
			width: 100,
			cellRenderer: ({ rowData }) => {
				return h(
					ElPopover,
					{
						placement: 'bottom',
						width: 280,
						trigger: 'hover',
					},
					{
						reference: () =>
							h(
								ElText,
								{
									type: 'primary',
								},
								{
									default: () => [
										h(
											ElIcon,
											{},
											{
												default: () => h(SvgIcon, { name: 'ele-InfoFilled' }),
											}
										),
										'详情',
									],
								}
							),
						default: () =>
							h(
								ElDescriptions,
								{
									direction: 'vertical',
									column: 2,
									size: 'small',
									border: true,
								},
								{
									default: () => [
										h(
											ElDescriptionsItem,
											{
												width: 140,
											},
											{
												label: () =>
													h(
														ElText,
														{},
														{
															default: () => [
																h(
																	ElIcon,
																	{},
																	{
																		default: () => h(SvgIcon, { name: 'ele-UserFilled' }),
																	}
																),
																'创建者',
															],
														}
													),
												default: () => h(ElTag, {}, { default: () => rowData.createUserName ?? '无' }),
											}
										),
										h(
											ElDescriptionsItem,
											{},
											{
												label: () =>
													h(
														ElText,
														{},
														{
															default: () => [
																h(
																	ElIcon,
																	{},
																	{
																		default: () => h(SvgIcon, { name: 'ele-Calendar' }),
																	}
																),
																'创建时间',
															],
														}
													),
												default: () => h(ElTag, {}, { default: () => rowData.createTime ?? '无' }),
											}
										),
										h(
											ElDescriptionsItem,
											{},
											{
												label: () =>
													h(
														ElText,
														{},
														{
															default: () => [
																h(
																	ElIcon,
																	{},
																	{
																		default: () => h(SvgIcon, { name: 'ele-UserFilled' }),
																	}
																),
																'修改者',
															],
														}
													),
												default: () => h(ElTag, {}, { default: () => rowData.updateUserName ?? '无' }),
											}
										),
										h(
											ElDescriptionsItem,
											{},
											{
												label: () =>
													h(
														ElText,
														{},
														{
															default: () => [
																h(
																	ElIcon,
																	{},
																	{
																		default: () => h(SvgIcon, { name: 'ele-Calendar' }),
																	}
																),
																'修改时间',
															],
														}
													),
												default: () => h(ElTag, {}, { default: () => rowData.updateTime ?? '无' }),
											}
										),
										h(
											ElDescriptionsItem,
											{},
											{
												label: () =>
													h(
														ElText,
														{},
														{
															default: () => [
																h(
																	ElIcon,
																	{},
																	{
																		default: () => h(SvgIcon, { name: 'ele-Tickets' }),
																	}
																),
																'备注',
															],
														}
													),
												default: () => h(ElTag, {}, { default: () => rowData.remark ?? '无' }),
											}
										),
									],
								}
							),
					}
				);
			},
		},
		{
			key: 'status',
			title: '操作',
			dataKey: 'status',
			align: 'center',
			width: 240,
			cellRenderer: ({ rowData }) => {
				return h('div', {}, [
					withDirectives(
						h(
							ElButton,
							{
								icon: 'ele-Edit',
								size: 'small',
								type: 'primary',
								onClick: () => openEditMenu(rowData),
							},
							{ default: () => '编辑' }
						),
						[[authBtn, 'sysMenu:update']]
					),
					withDirectives(
						h(
							ElButton,
							{
								icon: 'ele-Delete',
								size: 'small',
								type: 'danger',
								onClick: () => delMenu(rowData),
							},
							{ default: () => '删除' }
						),
						[[authBtn, 'sysMenu:delete']]
					),
				]);
			},
		},
	];
};
const state = reactive({
	loading: false,
	menuData: [] as Array<SysMenu>,
	columns: generateColumns(),
	queryParams: {
		title: undefined,
		type: undefined,
	},
	editMenuTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	var res = await getAPI(SysMenuApi).apiSysMenuListGet(state.queryParams.title, state.queryParams.type);
	state.menuData = res.data.result ?? [];
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.title = undefined;
	state.queryParams.type = undefined;
	handleQuery();
};

// 打开新增页面
const openAddMenu = () => {
	state.editMenuTitle = '添加菜单';
	editMenuRef.value?.openDialog({ type: 2, isHide: false, isKeepAlive: true, isAffix: false, isIframe: false, status: 1, orderNo: 100 });
};

// 打开编辑页面
const openEditMenu = (row: any) => {
	state.editMenuTitle = '编辑菜单';
	editMenuRef.value?.openDialog(row);
};

// 删除当前行
const delMenu = (row: any) => {
	ElMessageBox.confirm(`确定删除菜单：【${row.title}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysMenuApi).apiSysMenuDeletePost({ id: row.id });
			handleQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};
</script>
