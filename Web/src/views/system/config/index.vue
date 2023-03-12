<template>
	<div class="sys-config-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<TableSearch :search="tb.tableData.search" @search="onSearch" />
		</el-card>
		<el-card shadow="hover" style="margin-top: 8px">
			<Table ref="tableRef" v-bind="tb.tableData" :getData="getData" :exportChangeData="exportChangeData" @sortHeader="onSortHeader" @selectionChange="tableSelection">
				<template #command>
					<el-button type="primary" icon="ele-Plus" @click="openAddConfig" v-auth="'sysConfig:add'"> 新增 </el-button>
					<el-button v-if="state.selectlist.length > 0" type="danger" icon="ele-Delete" @click="bacthDelete" v-auth="'sysConfig:delete'"> 批量删除 </el-button>
				</template>
				<template #sysFlag="scope">
					<el-tag v-if="scope.row.sysFlag === 1"> 是 </el-tag>
					<el-tag type="danger" v-else> 否 </el-tag>
				</template>
				<template #action="scope">
					<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditConfig(scope.row)" v-auth="'sysConfig:update'"> 编辑 </el-button>
					<el-button icon="ele-Delete" size="small" text type="danger" @click="delConfig(scope.row)" v-auth="'sysConfig:delete'"> 删除 </el-button>
				</template>
			</Table>
		</el-card>
		<EditConfig ref="editConfigRef" :title="state.editConfigTitle" />
	</div>
</template>

<script lang="ts" setup name="sysConfig">
import { onMounted, onUnmounted, reactive, ref, defineAsyncComponent, nextTick } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import mittBus from '/@/utils/mitt';
import EditConfig from '/@/views/system/config/component/editConfig.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysConfigApi } from '/@/api-services/api';

// 引入组件
const Table = defineAsyncComponent(() => import('/@/components/table/index.vue'));
const TableSearch = defineAsyncComponent(() => import('/@/components/table/search.vue'));
const editConfigRef = ref<InstanceType<typeof EditConfig>>();
const tableRef = ref<RefType>();

const state = reactive({
	editConfigTitle: '',
	selectlist: [] as EmptyObjectType[],
});

const tb = reactive<TableDemoState>({
	tableData: {
		// 表头内容（必传，注意格式）
		header: [
			{ key: 'name', colWidth: '180', title: '配置名称', type: 'text', align: 'center', headerAlign: '', toolTip: false, sortable: true, isCheck: true },
			{ key: 'code', colWidth: '180', title: '配置编码', type: 'text', align: 'center', headerAlign: '', toolTip: true, sortable: true, isCheck: true },
			{ key: 'value', colWidth: '120', title: '属性值', type: 'text', align: 'center', headerAlign: '', toolTip: false, sortable: false, isCheck: true },
			{ key: 'sysFlag', colWidth: '120', title: '内置参数', type: 'text', align: 'center', headerAlign: '', toolTip: false, sortable: false, isCheck: true },
			{ key: 'groupCode', colWidth: '120', title: '分组编码', type: 'text', align: 'center', headerAlign: '', toolTip: false, sortable: false, isCheck: true },
			{ key: 'orderNo', colWidth: '80', title: '排序', type: 'text', align: 'center', headerAlign: '', toolTip: false, sortable: false, isCheck: true },
			{ key: 'remark', colWidth: '', title: '备注', type: 'text', align: '', headerAlign: 'center', toolTip: false, sortable: false, isCheck: true },
			{ key: 'action', colWidth: '150', title: '操作', type: 'action', align: 'center', headerAlign: '', toolTip: false, sortable: false, isCheck: true },
		],
		// 配置项（必传）
		config: {
			isBorder: true, // 是否显示表格边框
			isSerialNo: true, // 是否显示表格序号
			isSelection: false, // 是否勾选表格多选
			showSelection: false, //是否显示表格多选
			pageSize: 20, // 每页条数
			hideExport: false, //是否隐藏导出按钮
			exportFileName: '参数配置', //导出报表的文件名，不填写取应用名称
		},
		// 搜索表单，动态生成（传空数组时，将不显示搜索，type有3种类型：input,date,select）
		search: [
			{ label: '配置名称', prop: 'name', placeholder: '配置名称', required: false, type: 'input' },
			{ label: '配置编码', prop: 'code', placeholder: '配置编码', required: false, type: 'input' },
			// { label: '分组编码', prop: 'groupCode', placeholder: '分组编码', required: false, type: 'input' },
			// { label: '创建时间', prop: 'time', placeholder: '请选择', required: false, type: 'date' },
		],
		param: {},
		defaultSort: {
			prop: 'orderNo',
			order: 'ascending',
		},
	},
});
const getData = async (param: any) => {
	var res = await getAPI(SysConfigApi).apiSysConfigPagePost(param);
	return res.data;
};
const exportChangeData = (data: Array<EmptyObjectType>) => {
	data.forEach((item) => {
		item.sysFlag = item.sysFlag == 1 ? '是' : '否';
	});
	return data;
};
// 拖动显示列排序回调
const onSortHeader = (data: TableHeaderType[]) => {
	tb.tableData.header = data;
};
// 搜索点击时表单回调
const onSearch = (data: EmptyObjectType) => {
	tb.tableData.param = Object.assign({}, tb.tableData.param, { ...data });
	nextTick(() => {
		tableRef.value.pageReset();
	});
};

const getGroupList = async () => {
	const res = await getAPI(SysConfigApi).apiSysConfigGroupListGet();
	const groupSearch = {
		label: '分组编码',
		prop: 'groupCode',
		placeholder: '请选择',
		required: false,
		type: 'select',
		options: [],
	} as TableSearchType;
	res.data.result?.forEach((item) => {
		groupSearch.options?.push({ label: item, value: item });
	});
	tb.tableData.search.push(groupSearch);
};
//表格多选事件
const tableSelection = (data: EmptyObjectType[]) => {
	console.log('表格多选事件', data);
	state.selectlist = data;
};

onMounted(async () => {
	getGroupList();
	mittBus.on('submitRefresh', () => {
		tableRef.value.handleList();
	});
});

onUnmounted(() => {
	mittBus.off('submitRefresh');
});

// 打开新增页面
const openAddConfig = () => {
	state.editConfigTitle = '添加配置';
	editConfigRef.value?.openDialog({});
};

// 打开编辑页面
const openEditConfig = (row: any) => {
	state.editConfigTitle = '编辑配置';
	editConfigRef.value?.openDialog(row);
};

// 删除
const delConfig = (row: any) => {
	ElMessageBox.confirm(`确定删除配置：【${row.name}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysConfigApi).apiSysConfigDeletePost({ id: row.id });
			tableRef.value.handleList();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};
//批量删除
const bacthDelete = () => {
	if (state.selectlist.length == 0) return false;
	ElMessageBox.confirm(`确定批量删除【${state.selectlist[0].name}】等${state.selectlist.length}个配置?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	});
	console.log('批量删除了');
};
</script>
