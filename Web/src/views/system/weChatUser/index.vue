<template>
	<div class="weChatUser-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
				<el-form-item label="微信昵称">
					<el-input v-model="state.queryParams.nickName" placeholder="微信昵称" clearable />
				</el-form-item>
				<el-form-item label="手机号码">
					<el-input v-model="state.queryParams.mobile" placeholder="手机号码" clearable />
				</el-form-item>
				<el-form-item>
					<el-button-group>
						<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysWechatUser:page'"> 查询 </el-button>
						<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
					</el-button-group>
				</el-form-item>
			</el-form>
		</el-card>

		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="state.weChatUserData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="openId" label="OpenId" align="center" show-overflow-tooltip />
				<el-table-column prop="unionId" label="UnionId" align="center" show-overflow-tooltip />
				<el-table-column prop="platformType" label="平台类型" width="110" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.platformType === 1"> 微信公众号 </el-tag>
						<el-tag v-if="scope.row.platformType === 2"> 微信小程序 </el-tag>
						<el-tag v-if="scope.row.platformType === 3"> 支付宝小程序 </el-tag>
						<el-tag v-if="scope.row.platformType === 4"> 微信APP快捷登陆 </el-tag>
						<el-tag v-if="scope.row.platformType === 5"> QQ在APP中快捷登陆 </el-tag>
						<el-tag v-if="scope.row.platformType === 6"> 头条系小程序 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="nickName" label="昵称" align="center" show-overflow-tooltip />
				<el-table-column prop="avatar" label="头像" width="70" align="center">
					<template #default="scope">
						<el-avatar :src="scope.row.avatar" :size="24" style="vertical-align: middle" />
					</template>
				</el-table-column>
				<el-table-column prop="mobile" label="手机号码" align="center" show-overflow-tooltip />
				<el-table-column prop="sex" label="性别" width="60" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.sex === 0"> 男 </el-tag>
						<el-tag type="danger" v-else> 女 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="city" label="城市" align="center" show-overflow-tooltip />
				<el-table-column prop="province" label="省" align="center" show-overflow-tooltip />
				<el-table-column prop="country" label="国家" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditWeChatUser(scope.row)" v-auth="'sysWechatUser:update'"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delWeChatUser(scope.row)" v-auth="'sysWechatUser:delete'"> 删除 </el-button>
					</template>
				</el-table-column>
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

		<EditWeChatUser ref="editWeChatUserRef" :title="state.editWeChatUserTitle" @handleQuery="handleQuery" />
	</div>
</template>

<script lang="ts" setup name="weChatUser">
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditWeChatUser from '/@/views/system/weChatUser/component/editWeChatUser.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysWechatUserApi } from '/@/api-services/api';
import { SysWechatUser } from '/@/api-services/models';

const editWeChatUserRef = ref<InstanceType<typeof EditWeChatUser>>();
const state = reactive({
	loading: false,
	weChatUserData: [] as Array<SysWechatUser>,
	queryParams: {
		nickName: undefined,
		mobile: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 20,
		total: 0 as any,
	},
	editWeChatUserTitle: '',
});

onMounted(async () => {
	handleQuery();
});

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysWechatUserApi).apiSysWechatUserPagePost(params);
	state.weChatUserData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.nickName = undefined;
	state.queryParams.mobile = undefined;
	handleQuery();
};

// 打开编辑页面
const openEditWeChatUser = (row: any) => {
	state.editWeChatUserTitle = '编辑微信账号';
	editWeChatUserRef.value?.openDialog(row);
};

// 删除
const delWeChatUser = (row: any) => {
	ElMessageBox.confirm(`确定删除微信账号：【${row.nickName}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysWechatUserApi).apiSysWechatUserDeletePost({ id: row.id });
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
</script>
