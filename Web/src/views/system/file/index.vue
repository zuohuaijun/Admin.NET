<template>
	<div class="sys-file-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<el-form :model="state.queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
            <el-form-item label="文件名称" prop="fileName">
              <el-input placeholder="文件名称" clearable @keyup.enter="handleQuery" v-model="state.queryParams.fileName" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">  
            <el-form-item label="时间范围" prop="timeRange">
              <el-date-picker
                v-model="state.queryParams.timeRange"
                type="datetimerange"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                format="YYYY-MM-DD HH:mm:ss"
                value-format="YYYY-MM-DD HH:mm:ss"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20 search-actions">
            <div>
              <el-button type="primary"  icon="ele-Plus" @click="openUploadDialog" v-auth="'sysFile:uploadFile'"> 上传 </el-button>
            </div>
            <div>
              <el-form-item>
                <el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysFile:page'" plain> 查询 </el-button> 
              </el-form-item>
            </div>
          </el-col>
        </el-row>
			</el-form>
		</el-card>

		<el-card shadow="hover" style="margin-top: 8px">
			<el-table :data="state.fileData" style="width: 100%" v-loading="state.loading" border>
				<el-table-column type="index" label="序号" width="55" align="center" />
				<el-table-column prop="fileName" label="名称" show-overflow-tooltip />
				<el-table-column prop="suffix" label="后缀" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag round>{{ scope.row.suffix }}</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="sizeKb" label="大小kb" align="center" show-overflow-tooltip />
				<el-table-column prop="url" label="预览" align="center">
					<template #default="scope">
						<el-image
							style="width: 60px; height: 60px"
							:src="scope.row.url"
							:lazy="true"
							:hide-on-click-modal="true"
							:preview-src-list="[scope.row.url]"
							:initial-index="0"
							fit="scale-down"
							preview-teleported
						/>
					</template>
				</el-table-column>
				<el-table-column prop="bucketName" label="存储位置" align="center" show-overflow-tooltip />
				<el-table-column prop="id" label="存储标识" align="center" show-overflow-tooltip />
				<!-- <el-table-column prop="userName" label="上传者" align="center" show-overflow-tooltip/> -->
				<el-table-column prop="createTime" label="创建时间" align="center" show-overflow-tooltip />
				<el-table-column label="操作" width="140" fixed="right" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-button icon="ele-Download" size="small" text type="primary" @click="downloadFile(scope.row)" v-auth="'sysFile:downloadFile'"> 下载 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delFile(scope.row)" v-auth="'sysFile:delete'"> 删除 </el-button>
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

		<el-dialog v-model="state.dialogVisible" :lock-scroll="false" draggable width="400px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-UploadFilled /> </el-icon>
					<span> 上传文件 </span>
				</div>
			</template>
			<div>
				<el-upload ref="uploadRef" drag :auto-upload="false" :limit="1" :file-list="state.fileList" action="" :on-change="handleChange" accept=".jpg,.png,.bmp,.gif,.txt,.pdf,.xlsx,.docx">
					<el-icon class="el-icon--upload">
						<ele-UploadFilled />
					</el-icon>
					<div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
					<template #tip>
						<div class="el-upload__tip">请上传大小不超过 10MB 的文件</div>
					</template>
				</el-upload>
			</div>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="state.dialogVisible = false">取消</el-button>
					<el-button type="primary" @click="uploadFile">确定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysFile">
import { onMounted, onUnmounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage, UploadInstance } from 'element-plus';
import mittBus from '/@/utils/mitt';

import { downloadByUrl } from '/@/utils/download';
import { getAPI } from '/@/utils/axios-utils';
import { SysFileApi } from '/@/api-services/api';
import { SysFile } from '/@/api-services/models';

const uploadRef = ref<UploadInstance>();
const state = reactive({
	loading: false,
	fileData: [] as Array<SysFile>,
	queryParams: {
		fileName: undefined,
		timeRange: [] as any,
	},
	tableParams: {
		page: 1,
		pageSize: 10,
		total: 0 as any,
	},
	dialogVisible: false,
	fileList: [] as any,
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
	let startTime = undefined;
	let endTime = undefined;
	if (state.queryParams.timeRange != undefined) {
		startTime = state.queryParams.timeRange[0];
		endTime = state.queryParams.timeRange[1];
	}

	state.loading = true;
	var res = await getAPI(SysFileApi).apiSysFilePageGet(state.queryParams.fileName, startTime, endTime, state.tableParams.page, state.tableParams.pageSize);
	state.fileData = res.data.result?.items ?? [];
	state.tableParams.total = res.data.result?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.fileName = undefined;
	state.queryParams.timeRange = undefined;
	handleQuery();
};

// 打开上传页面
const openUploadDialog = () => {
	state.fileList = [];
	state.dialogVisible = true;
};

// 通过onChanne方法获得文件列表
const handleChange = (file: any, fileList: []) => {
	state.fileList = fileList;
};

// 上传
const uploadFile = async () => {
	if (state.fileList.length < 1) return;
	await getAPI(SysFileApi).apiSysFileUploadFilePostForm(state.fileList[0].raw);
	handleQuery();
	ElMessage.success('上传成功');
	state.dialogVisible = false;
};

// 下载
const downloadFile = async (row: any) => {
	// var res = await getAPI(SysFileApi).sysFileDownloadPost({ id: row.id });
	downloadByUrl({ url: row.url });
};

// 删除
const delFile = (row: any) => {
	ElMessageBox.confirm(`确定删除文件：【${row.fileName}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysFileApi).apiSysFileDeletePost({ id: row.id });
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
