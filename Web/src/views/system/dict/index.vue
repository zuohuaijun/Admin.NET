<template>
	<div class="sys-dict-container">
		<el-row :gutter="8" style="width: 100%">
			<el-col :span="12" :xs="24">
				<el-card class="full-table" shadow="hover" :body-style="{ paddingBottom: '20' }">
					<template #header>
						<el-icon><ele-Collection /></el-icon>字典
					</template>
					<el-form :model="state.queryDictTypeParams" ref="queryForm" :inline="true">
						<el-form-item label="名称">
							<el-input v-model="state.queryDictTypeParams.name" placeholder="字典名称" clearable />
						</el-form-item>
						<!-- <el-form-item label="字典编码">
							<el-input v-model="state.queryDictTypeParams.code" placeholder="字典编码" clearable />
						</el-form-item> -->
						<el-form-item>
							<el-button-group>
								<el-button type="primary" icon="ele-Search" @click="handleDictTypeQuery" v-auth="'sysDictType:page'"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="resetDictTypeQuery"> 重置 </el-button>
							</el-button-group>
						</el-form-item>
						<el-form-item>
							<el-button type="primary" icon="ele-Plus" @click="openAddDictType" v-auth="'sysDictType:add'"> 新增 </el-button>
						</el-form-item>
					</el-form>

					<el-table :data="state.dictTypeData" style="width: 100%" v-loading="state.loading" @row-click="handleDictType" highlight-current-row border>
						<el-table-column type="index" label="序号" width="55" align="center" />
						<el-table-column prop="name" label="字典名称" header-align="center" show-overflow-tooltip />
						<el-table-column prop="code" label="字典编码" header-align="center" show-overflow-tooltip />
						<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
								<el-tag type="danger" v-else>禁用</el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="orderNo" label="排序" width="70" align="center" show-overflow-tooltip />
						<el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-popover placement="bottom" width="280" trigger="hover">
									<template #reference>
										<el-text type="primary">
											<el-icon><ele-InfoFilled /></el-icon>详情
										</el-text>
									</template>
									<el-descriptions direction="vertical" :column="2" border>
										<el-descriptions-item width="140">
											<template #label>
												<el-text>
													<el-icon><ele-UserFilled /></el-icon>创建者
												</el-text>
											</template>
											<el-tag>{{ scope.row.createUserName ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-Calendar /></el-icon>创建时间
												</el-text>
											</template>
											<el-tag>{{ scope.row.createTime ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-UserFilled /></el-icon>修改者
												</el-text>
											</template>
											<el-tag>{{ scope.row.updateUserName ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-Calendar /></el-icon>修改时间
												</el-text>
											</template>
											<el-tag>{{ scope.row.updateTime ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-Tickets /></el-icon>备注
												</el-text>
											</template>
											{{ scope.row.remark ?? '无' }}
										</el-descriptions-item>
									</el-descriptions>
								</el-popover>
							</template>
						</el-table-column>
						<el-table-column label="操作" width="100" fixed="right" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tooltip content="字典值">
									<el-button icon="ele-Memo" size="small" text type="primary" @click="openDictDataDialog(scope.row)" v-auth="'sysDictType:page'"> </el-button>
								</el-tooltip>
								<el-tooltip content="编辑">
									<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditDictType(scope.row)" v-auth="'sysDictType:update'"> </el-button>
								</el-tooltip>
								<el-tooltip content="删除">
									<el-button icon="ele-Delete" size="small" text type="danger" @click="delDictType(scope.row)" v-auth="'sysDictType:delete'"> </el-button>
								</el-tooltip>
							</template>
						</el-table-column>
					</el-table>
					<el-pagination
						v-model:currentPage="state.tableDictTypeParams.page"
						v-model:page-size="state.tableDictTypeParams.pageSize"
						:total="state.tableDictTypeParams.total"
						:page-sizes="[10, 20, 50, 100]"
						small
						background
						@size-change="handleDictTypeSizeChange"
						@current-change="handleDictTypeCurrentChange"
						layout="total, sizes, prev, pager, next, jumper"
					/>
				</el-card>
			</el-col>

			<el-col :span="12" :xs="24">
				<el-card class="full-table" shadow="hover" :body-style="{ paddingBottom: '20' }">
					<template #header>
						<el-icon><ele-Collection /></el-icon>字典值【{{ state.editDictTypeName }}】
					</template>
					<el-form :model="state.queryDictDataParams" ref="queryForm" :inline="true">
						<!-- <el-form-item label="字典值">
							<el-input v-model="state.queryDictDataParams.value" placeholder="字典值" />
						</el-form-item> -->
						<el-form-item label="编码">
							<el-input v-model="state.queryDictDataParams.code" placeholder="编码" />
						</el-form-item>
						<el-form-item>
							<el-button-group>
								<el-button type="primary" icon="ele-Search" @click="handleDictDataQuery"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="resetDictDataQuery"> 重置 </el-button>
							</el-button-group>
						</el-form-item>
						<el-form-item>
							<el-button type="primary" icon="ele-Plus" @click="openAddDictData"> 新增 </el-button>
						</el-form-item>
					</el-form>

					<el-table :data="state.dictDataData" style="width: 100%" v-loading="state.loading" border>
						<el-table-column type="index" label="序号" width="55" align="center" />
						<el-table-column prop="value" label="字典值" header-align="center" min-width="100" show-overflow-tooltip>
							<template #default="scope">
								<el-tag :type="scope.row.tagType" :style="scope.row.styleSetting" :class="scope.row.classSetting">{{ scope.row.value }}</el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="code" label="编码" header-align="center" min-width="100" show-overflow-tooltip />
						<el-table-column prop="extData" label="拓展数据" width="90" align="center">
							<template #default="scope">
								<el-tag type="warning" v-if="scope.row.extData == null || scope.row.extData == ''">空</el-tag>
								<el-tag type="success" v-else>有值</el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="status" label="状态" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
								<el-tag type="danger" v-else>禁用</el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="orderNo" label="排序" width="70" align="center" show-overflow-tooltip />
						<el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-popover placement="bottom" width="280" trigger="hover">
									<template #reference>
										<el-text type="primary">
											<el-icon><ele-InfoFilled /></el-icon>详情
										</el-text>
									</template>
									<el-descriptions direction="vertical" :column="2" border>
										<el-descriptions-item width="140">
											<template #label>
												<el-text>
													<el-icon><ele-UserFilled /></el-icon>创建者
												</el-text>
											</template>
											<el-tag>{{ scope.row.createUserName ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-Calendar /></el-icon>创建时间
												</el-text>
											</template>
											<el-tag>{{ scope.row.createTime ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-UserFilled /></el-icon>修改者
												</el-text>
											</template>
											<el-tag>{{ scope.row.updateUserName ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><Calendar /></el-icon>修改时间
												</el-text>
											</template>
											<el-tag>{{ scope.row.updateTime ?? '无' }}</el-tag>
										</el-descriptions-item>
										<el-descriptions-item>
											<template #label>
												<el-text>
													<el-icon><ele-Tickets /></el-icon>备注
												</el-text>
											</template>
											{{ scope.row.remark ?? '无' }}
										</el-descriptions-item>
									</el-descriptions>
								</el-popover>
							</template>
						</el-table-column>
						<el-table-column label="操作" width="80" fixed="right" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tooltip content="编辑">
									<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditDictData(scope.row)"> </el-button>
								</el-tooltip>
								<el-tooltip content="删除">
									<el-button icon="ele-Delete" size="small" text type="danger" @click="delDictData(scope.row)"> </el-button>
								</el-tooltip>
							</template>
						</el-table-column>
					</el-table>
					<el-pagination
						v-model:currentPage="state.tableDictDataParams.page"
						v-model:page-size="state.tableDictDataParams.pageSize"
						:total="state.tableDictDataParams.total"
						:page-sizes="[10, 20, 50, 100]"
						small
						background
						@size-change="handleDictDataSizeChange"
						@current-change="handleDictDataCurrentChange"
						layout="total, sizes, prev, pager, next, jumper"
					/>
				</el-card>
			</el-col>
		</el-row>

		<EditDictType ref="editDictTypeRef" :title="state.editDictTypeTitle" @handleQuery="handleDictTypeQuery" />
		<EditDictData ref="editDictDataRef" :title="state.editDictDataTitle" @handleQuery="handleDictDataQuery" />
	</div>
</template>

<script lang="ts" setup name="sysDict">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditDictType from '/@/views/system/dict/component/editDictType.vue';
import EditDictData from '/@/views/system/dict/component/editDictData.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysDictTypeApi, SysDictDataApi } from '/@/api-services/api';
import { SysDictType, SysDictData } from '/@/api-services/models';

const editDictTypeRef = ref<InstanceType<typeof EditDictType>>();
const editDictDataRef = ref<InstanceType<typeof EditDictData>>();
const state = reactive({
	loading: false,
	dictTypeData: [] as Array<SysDictType>,
	dictDataData: [] as Array<SysDictData>,
	queryDictTypeParams: {
		name: undefined,
		code: undefined,
	},
	tableDictTypeParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	queryDictDataParams: {
		value: undefined,
		code: undefined,
		dictTypeId: 0, // 字典类型Id
	},
	tableDictDataParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editDictTypeTitle: '',
	editDictDataTitle: '',
	editDictTypeName: '',
});

onMounted(async () => {
	handleDictTypeQuery();
});

// 查询字典操作
const handleDictTypeQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryDictTypeParams, state.tableDictTypeParams);
	var res = await getAPI(SysDictTypeApi).apiSysDictTypePagePost(params);
	state.dictTypeData = res.data.result?.items ?? [];
	state.tableDictTypeParams.total = res.data.result?.total;
	state.loading = false;
};

// 查询字典值操作
const handleDictDataQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryDictDataParams, state.tableDictDataParams);
	var res = await getAPI(SysDictDataApi).apiSysDictDataPagePost(params);
	state.dictDataData = res.data.result?.items ?? [];
	state.tableDictDataParams.total = res.data.result?.total;
	state.loading = false;
};

// 点击表格
const handleDictType = (row: any, event: any, column: any) => {
	console.log(row, event, column);
	openDictDataDialog(row);
};

// 重置字典操作
const resetDictTypeQuery = () => {
	state.queryDictTypeParams.name = undefined;
	state.queryDictTypeParams.code = undefined;
	handleDictTypeQuery();
};

// 重置字典值操作
const resetDictDataQuery = () => {
	state.queryDictDataParams.value = undefined;
	state.queryDictDataParams.code = undefined;
	handleDictDataQuery();
};

// 打开新增字典页面
const openAddDictType = () => {
	state.editDictTypeTitle = '添加字典';
	editDictTypeRef.value?.openDialog({ status: 1, orderNo: 100 });
};

// 打开新增字典值页面
const openAddDictData = () => {
	state.editDictDataTitle = '添加字典值';
	editDictDataRef.value?.openDialog({ status: 1, orderNo: 100, dictTypeId: state.queryDictDataParams.dictTypeId });
};

// 打开编辑字典页面
const openEditDictType = (row: any) => {
	state.editDictTypeTitle = '编辑字典';
	editDictTypeRef.value?.openDialog(row);
};

// 打开编辑字典值页面
const openEditDictData = (row: any) => {
	state.editDictDataTitle = '编辑字典值';
	editDictDataRef.value?.openDialog(row);
};

// 打开字典值页面
const openDictDataDialog = (row: any) => {
	state.editDictTypeName = row.name;
	state.queryDictDataParams.dictTypeId = row.id;
	handleDictDataQuery();
};

// 删除字典
const delDictType = (row: any) => {
	ElMessageBox.confirm(`确定删除字典：【${row.name}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysDictTypeApi).apiSysDictTypeDeletePost({ id: row.id });
			handleDictTypeQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};

// 删除字典值
const delDictData = (row: any) => {
	ElMessageBox.confirm(`确定删除字典值：【${row.value}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysDictDataApi).apiSysDictDataDeletePost({ id: row.id });
			handleDictDataQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};

// 改变字典页面容量
const handleDictTypeSizeChange = (val: number) => {
	state.tableDictTypeParams.pageSize = val;
	handleDictTypeQuery();
};

// 改变字典页码序号
const handleDictTypeCurrentChange = (val: number) => {
	state.tableDictTypeParams.page = val;
	handleDictTypeQuery();
};

// 改变字典值页面容量
const handleDictDataSizeChange = (val: number) => {
	state.tableDictDataParams.pageSize = val;
	handleDictDataQuery();
};

// 改变字典值页码序号
const handleDictDataCurrentChange = (val: number) => {
	state.tableDictDataParams.page = val;
	handleDictDataQuery();
};
</script>
