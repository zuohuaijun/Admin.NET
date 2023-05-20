<template>
	<div class="table-container">
		<div class="table-header mb15">
			<div>
				<slot name="command"></slot>
			</div>
			<div v-loading="state.importLoading" class="table-footer-tool">
				<SvgIcon name="iconfont icon-shuaxin" :size="22" title="刷新" @click="onRefreshTable" />
				<el-dropdown v-if="!config.hideExport" trigger="click">
					<SvgIcon name="iconfont icon-yunxiazai_o" :size="22" title="导出" />
					<template #dropdown>
						<el-dropdown-menu>
							<el-dropdown-item @click="onImportTable">导出本页数据</el-dropdown-item>
							<el-dropdown-item @click="onImportTableAll">导出全部数据</el-dropdown-item>
						</el-dropdown-menu>
					</template>
					<SvgIcon name="iconfont icon-dayin" :size="19" title="打印" @click="onPrintTable" />
				</el-dropdown>
				<el-popover placement="top-end" trigger="click" transition="el-zoom-in-top" popper-class="table-tool-popper" :width="300" :persistent="false" @show="onSetTable">
					<template #reference>
						<SvgIcon name="iconfont icon-quanjushezhi_o" :size="22" title="设置" />
					</template>
					<template #default>
						<div class="tool-box">
							<el-tooltip content="拖动进行排序" placement="top-start">
								<SvgIcon name="fa fa-question-circle-o" :size="17" class="ml11" color="#909399" />
							</el-tooltip>
							<el-checkbox v-model="state.checkListAll" :indeterminate="state.checkListIndeterminate" class="ml10 mr1" label="列显示" @change="onCheckAllChange" />
							<el-checkbox v-model="getConfig.isSerialNo" class="ml12 mr1" label="序号" />
							<el-checkbox v-if="getConfig.showSelection" v-model="getConfig.isSelection" class="ml12 mr1" label="多选" />
						</div>
						<el-scrollbar>
							<div ref="toolSetRef" class="tool-sortable">
								<div class="tool-sortable-item" v-for="v in columns" :key="v.prop" v-show="!v.hideCheck && !v.fixed" :data-key="v.prop">
									<i class="fa fa-arrows-alt handle cursor-pointer"></i>
									<el-checkbox v-model="v.isCheck" size="default" class="ml12 mr8" :label="v.label" @change="onCheckChange" />
								</div>
							</div>
						</el-scrollbar>
					</template>
				</el-popover>
			</div>
		</div>
		<el-table
			ref="tableRef"
			:data="state.data"
			:border="setBorder"
			:stripe="setStripe"
			v-bind="$attrs"
			row-key="id"
			default-expand-all
			style="width: 100%"
			v-loading="state.loading"
			:default-sort="defaultSort"
			@selection-change="onSelectionChange"
			@sort-change="sortChange"
		>
			<el-table-column type="selection" :reserve-selection="true" :width="30" v-if="config.isSelection && config.showSelection" />
			<el-table-column type="index" label="序号" align="center" :width="60" v-if="config.isSerialNo" />
			<el-table-column v-for="(item, index) in setHeader" :key="index" v-bind="item">
				<!-- 自定义列插槽，插槽名为columns属性的prop -->
				<template #default="scope" v-if="$slots[item.prop]">
					<slot :name="item.prop" v-bind="scope"></slot>
				</template>
				<template v-else v-slot="scope">
					<template v-if="item.type === 'image'">
						<el-image
							:style="{ width: `${item.width}px`, height: `${item.height}px` }"
							:src="scope.row[item.prop]"
							:zoom-rate="1.2"
							:preview-src-list="[scope.row[item.prop]]"
							preview-teleported
							fit="cover"
						/>
					</template>
					<template v-else>
						{{ scope.row[item.prop] }}
					</template>
				</template>
			</el-table-column>
			<template #empty>
				<el-empty description="暂无数据" />
			</template>
		</el-table>
		<div v-if="state.showPagination" class="table-footer mt15">
			<el-pagination
				v-model:current-page="state.page.page"
				v-model:page-size="state.page.pageSize"
				small
				:pager-count="5"
				:page-sizes="[10, 30, 50, 100]"
				:total="state.total"
				layout="total, sizes, prev, pager, next, jumper"
				background
				@size-change="onHandleSizeChange"
				@current-change="onHandleCurrentChange"
			>
			</el-pagination>
		</div>
	</div>
</template>

<script setup lang="ts" name="netxTable">
import { reactive, computed, nextTick, ref, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import Sortable from 'sortablejs';
import { storeToRefs } from 'pinia';
import { useThemeConfig } from '/@/stores/themeConfig';
import { exportExcel } from '/@/utils/exportExcel';
// import '/@/theme/tableTool.scss';
import printJs from 'print-js';

// 定义父组件传过来的值
const props = defineProps({
	//获取数据的方法，由父组件传递
	getData: {
		type: Function,
		required: true,
	},
	//列属性，和elementUI的Table-column 属性相同，附加属性：isCheck-是否默认勾选展示，hideCheck-是否隐藏该列的可勾选和拖拽
	columns: {
		type: Array<any>,
		default: () => [],
	},
	// 配置项：isBorder-是否显示表格边框，isSerialNo-是否显示表格序号，showSelection-是否显示表格可多选，isSelection-是否默认选中表格多选，pageSize-每页条数，hideExport-是否隐藏导出按钮，exportFileName-导出表格的文件名，空值默认用应用名称作为文件名
	config: {
		type: Object,
		default: () => {},
	},
	// 筛选参数
	param: {
		type: Object,
		default: () => {},
	},
	// 默认排序方式，{prop:"排序字段",order:"ascending or descending"}
	defaultSort: {
		type: Object,
		default: () => {},
	},
	// 导出报表自定义数据转换方法，不传按字段值导出
	exportChangeData: {
		type: Function,
	},
	// 打印标题
	printName: {
		type: String,
		default: () => '',
	},
});

// 定义子组件向父组件传值/事件，pageChange-翻页事件，selectionChange-表格多选事件，可以在父组件处理批量删除/修改等功能，sortHeader-拖拽列顺序事件
const emit = defineEmits(['pageChange', 'selectionChange', 'sortHeader']);

// 定义变量内容
const toolSetRef = ref();
const tableRef = ref();
const storesThemeConfig = useThemeConfig();
const { themeConfig } = storeToRefs(storesThemeConfig);
const state = reactive({
	data: [] as Array<EmptyObjectType>,
	loading: false,
	importLoading: false,
	total: 0,
	page: {
		page: 1,
		pageSize: 10,
		field: '',
		order: '',
	},
	showPagination: true,
	selectlist: [] as EmptyObjectType[],
	checkListAll: true,
	checkListIndeterminate: false,
});

// 设置边框显示/隐藏
const setBorder = computed(() => {
	return props.config.isBorder ? true : false;
});
// 设置斑马纹显示/隐藏
const setStripe = computed(() => {
	return props.config.isStripe ? true : false;
});
// 获取父组件 配置项（必传）
const getConfig = computed(() => {
	return props.config;
});
// 设置 tool header 数据
const setHeader = computed(() => {
	return props.columns.filter((v) => v.isCheck);
});
// tool 列显示全选改变时
const onCheckAllChange = <T>(val: T) => {
	if (val) props.columns.forEach((v) => (v.isCheck = true));
	else props.columns.forEach((v) => (v.isCheck = false));
	state.checkListIndeterminate = false;
};
// tool 列显示当前项改变时
const onCheckChange = () => {
	const headers = props.columns.filter((v) => v.isCheck).length;
	state.checkListAll = headers === props.columns.length;
	state.checkListIndeterminate = headers > 0 && headers < props.columns.length;
};
// 表格多选改变时
const onSelectionChange = (val: EmptyObjectType[]) => {
	state.selectlist = val;
	emit('selectionChange', state.selectlist);
};
// 分页改变
const onHandleSizeChange = (val: number) => {
	state.page.pageSize = val;
	handleList();
	emit('pageChange', state.page);
};
// 改变当前页
const onHandleCurrentChange = (val: number) => {
	state.page.page = val;
	handleList();
	emit('pageChange', state.page);
};
// 列排序
const sortChange = (column: any) => {
	state.page.field = column.prop;
	state.page.order = column.order;
	handleList();
};
// 重置列表
const pageReset = () => {
	tableRef.value.clearSelection();
	state.page.page = 1;
	handleList();
};
// 导出当前页
const onImportTable = () => {
	if (setHeader.value.length <= 0) return ElMessage.error('没有勾选要导出的列');
	importData(state.data);
};
// 全部导出
const onImportTableAll = async () => {
	if (setHeader.value.length <= 0) return ElMessage.error('没有勾选要导出的列');
	state.importLoading = true;
	const param = Object.assign({}, props.param, { page: 1, pageSize: 99999 });
	const res = await props.getData(param);
	state.importLoading = false;
	const data = res.result?.items ?? [];
	importData(data);
};
// 导出方法
const importData = (data: Array<EmptyObjectType>) => {
	if (data.length <= 0) return ElMessage.error('没有数据可以导出');
	state.importLoading = true;
	let exportData = JSON.parse(JSON.stringify(data));
	if (props.exportChangeData) {
		exportData = props.exportChangeData(exportData);
	}
	exportExcel(
		exportData,
		`${props.config.exportFileName ? props.config.exportFileName : themeConfig.value.globalTitle}_${new Date().toLocaleString()}.xlsx`,
		setHeader.value.filter((item) => {
			return item.type != 'action';
		}),
		'导出数据'
	);
	state.importLoading = false;
};
// 打印
const onPrintTable = () => {
	// https://printjs.crabbly.com/#documentation
	// 自定义打印
	let tableTh = '';
	let tableTrTd = '';
	let tableTd: any = {};
	// 表头
	props.header.forEach((v: any) => {
		tableTh += `<th class="table-th">${v.title}</th>`;
	});
	// 表格内容
	props.data.forEach((val: any, key: any) => {
		if (!tableTd[key]) tableTd[key] = [];
		props.header.forEach((v: any) => {
			if (v.type === 'text') {
				tableTd[key].push(`<td class="table-th table-center">${val[v.key]}</td>`);
			} else if (v.type === 'image') {
				tableTd[key].push(`<td class="table-th table-center"><img src="${val[v.key]}" style="width:${v.width}px;height:${v.height}px;"/></td>`);
			}
		});
		tableTrTd += `<tr>${tableTd[key].join('')}</tr>`;
	});
	// 打印
	printJs({
		printable: `<div style=display:flex;flex-direction:column;text-align:center><h3>${props.printName}</h3></div><table border=1 cellspacing=0><tr>${tableTh}${tableTrTd}</table>`,
		type: 'raw-html',
		css: ['//at.alicdn.com/t/c/font_2298093_rnp72ifj3ba.css', '//unpkg.com/element-plus/dist/index.css'],
		style: `@media print{.mb15{margin-bottom:15px;}.el-button--small i.iconfont{font-size: 12px !important;margin-right: 5px;}}; .table-th{word-break: break-all;white-space: pre-wrap;}.table-center{text-align: center;}`,
	});
};
// 刷新
const onRefreshTable = () => {
	handleList();
	// emit('pageChange', state.page);
};
// 拖拽设置
const onSetTable = () => {
	nextTick(() => {
		const sortable = Sortable.create(toolSetRef.value, {
			handle: '.handle',
			dataIdAttr: 'data-key',
			animation: 150,
			onEnd: () => {
				const headerList: EmptyObjectType[] = [];
				sortable.toArray().forEach((val: any) => {
					props.columns.forEach((v) => {
						if (v.prop === val) headerList.push({ ...v });
					});
				});
				console.log(headerList);
				emit('sortHeader', headerList);
			},
		});
	});
};

const handleList = async () => {
	state.loading = true;
	let param = Object.assign({}, props.param, { ...state.page });
	Object.keys(param).forEach((key) => !param[key] && delete param[key]);
	const res = await props.getData(param);
	state.loading = false;
	if (res.result.items) {
		state.showPagination = true;
		state.data = res.result?.items ?? [];
		state.total = res.result?.total ?? 0;
	} else {
		state.showPagination = false;
		state.data = res.result ?? [];
	}
};

const toggleSelection = (row: any, statu?: boolean) => {
	tableRef.value!.toggleRowSelection(row, statu);
};

onMounted(() => {
	if (props.defaultSort) {
		state.page.field = props.defaultSort.prop;
		state.page.order = props.defaultSort.order;
	}
	state.page.pageSize = props.config.pageSize;
	handleList();
});

// 暴露变量
defineExpose({
	pageReset,
	handleList,
	toggleSelection,
});
</script>

<style scoped lang="scss">
.table-container {
	display: flex;
	flex-direction: column;
	height: 100%;

	.el-table {
		flex: 1;
	}

	.table-footer {
		display: flex;
		justify-content: flex-end;
	}

	.table-header {
		display: flex;

		.table-footer-tool {
			flex: 1;
			display: flex;
			align-items: center;
			justify-content: flex-end;

			i {
				margin-right: 10px;
				cursor: pointer;
				color: var(--el-text-color-regular);

				&:last-of-type {
					margin-right: 0;
				}
			}

			.el-dropdown {
				i {
					margin-right: 10px;
					color: var(--el-text-color-regular);
				}
			}
		}
	}
}
</style>
