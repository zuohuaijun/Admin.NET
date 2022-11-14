<template>
  <div class="generatetest-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
		          
     <el-form :model="queryParams" ref="queryForm" :inline="true">
		 <a-row>
       <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
       <el-form-item label="编码">
       <el-input v-model="queryParam.Code" clearable placeholder="请输入编码"/>
       </el-form-item>
       </el-col>
			<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" >
              <span class="table-page-search-submitButtons">
                 <el-button icon="ele-Refresh" @click="() => queryParam = {}"> 重置 </el-button>
               <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'generatetest:page'"> 查询 </el-button>
				  <el-button icon="ele-Plus" @click="openAddgeneratetest" v-auth="'generatetest:add'"> 新增 </el-button>  
              </span>
            </el-col>
          </a-row>
        </a-form>
       <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
       <el-form-item label="名称">
       <el-input v-model="queryParam.Name" clearable placeholder="请输入名称"/>
       </el-form-item>
       </el-col>
			<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" >
              <span class="table-page-search-submitButtons">
                 <el-button icon="ele-Refresh" @click="() => queryParam = {}"> 重置 </el-button>
               <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'generatetest:page'"> 查询 </el-button>
				  <el-button icon="ele-Plus" @click="openAddgeneratetest" v-auth="'generatetest:add'"> 新增 </el-button>  
              </span>
            </el-col>
          </a-row>
        </a-form>
      
    </a-card>
    <a-card :bordered="false">
	<el-table 
		:data="tableData" 
		style="width: 100%" 
		v-loading="loading" 
		tooltip-effect="light"
		row-key="id"
		border>
		<el-table-column type="index" label="序号" width="55" align="center" fixed />
				     <el-table-column prop="Code" label="编码" fixed show-overflow-tooltip />
				     <el-table-column prop="Name" label="名称" fixed show-overflow-tooltip />
				     <el-table-column prop="Price" label="价格" fixed show-overflow-tooltip />
				     <el-table-column prop="ExpireDate" label="过期日期" fixed show-overflow-tooltip />
				  <el-table-column prop="Status" label="状态" fixed show-overflow-tooltip>
					    <template #default="scope">
								<el-tag v-if="scope.row.status"> 是 </el-tag>
								<el-tag type="danger" v-else> 否 </el-tag>
					    </template>
					</el-table-column>
			           <el-table-column label="操作" width="110" align="center" fixed="right" show-overflow-tooltip v-if="auth('generatetest:edit') || auth('generatetest:delete')">
							<template #default="scope">
								<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditgeneratetest(scope.row)" v-auth="'generatetest:update'"> 编辑 </el-button>
								<el-button icon="ele-Edit" size="small" text type="primary" @click="delgeneratetest(scope.row)" v-auth="'generatetest:delete'"> 删除 </el-button>								
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
<editDialog ref="editDialogRef" :title="editgeneratetestTitle"/>
    </a-card>
  </div>
</template>

<script lang="ts">
	import { ref, toRefs, reactive, onMounted, defineComponent, getCurrentInstance, onUnmounted } from 'vue';
    import { ElMessageBox, ElMessage } from 'element-plus';
	import { auth } from '/@/utils/authFunction';
	import { formatDate } from '/@/utils/formatTime';
	
	import editDialog from '/@/views/main/generatetest/component/editDialog.vue'
	import { getgeneratetestPage, deletegeneratetest } from '/@/api/main/generatetest';
	export default defineComponent({
	    name: 'generatetest',
		components: { editDialog },
		setup() {
		    const { proxy } = getCurrentInstance() as any;
			const editDialogRef = ref();
			const state = reactive({
			    loading: false,
			    tableData: [] as any,
			    queryParams: {} as any,
			    tableParams: {
				    page: 1,
				    pageSize: 10,
				    total: 0 as any,
			    },
			    editgeneratetestTitle: '',
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
			    var res = await getgeneratetestPage(Object.assign(state.queryParams, state.tableParams));
			    state.tableData = res.data.result?.items ?? [];
			    state.tableParams.total = res.data.result?.total;
			    state.loading = false;
		    };
			
			// 打开新增页面
		    const openAddgeneratetest = () => {
			    state.editgeneratetestTitle = '添加GenerateTest';
			    editDialogRef.value.openDialog({});
		    };
			
		    // 打开编辑页面
		    const openEditgeneratetest = (row: any) => {
			    state.editgeneratetestTitle = '编辑GenerateTest';
			    editDialogRef.value.openDialog(row);
		    };
			
		    // 删除
		    const delgeneratetest = (row: any) => {
			    ElMessageBox.confirm(`确定要删除吗?`, '提示', {
				    confirmButtonText: '确定',
				    cancelButtonText: '取消',
				    type: 'warning',
			    })
				    .then(async () => {
					    await deletegeneratetest(row);
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
			

            return {
			    handleQuery,
			    editDialogRef,
			    openAddgeneratetest,
			    openEditgeneratetest,
			    delgeneratetest,
			    handleSizeChange,
			    handleCurrentChange,
			    formatDate,
			    auth,
			    ...toRefs(state),
		   };
		}
	})
</script>