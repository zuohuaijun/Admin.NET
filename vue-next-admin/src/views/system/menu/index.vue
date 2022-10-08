<template>
  <div class="page-container">
    <el-card shadow="hover">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="菜单名称" prop="title">
          <el-input placeholder="菜单名称" clearable @keyup.enter="handleQuery" v-model="queryParams.title" />
        </el-form-item>
        <el-form-item label="类型" prop="type">
          <el-select v-model="queryParams.type" placeholder="类型" clearable>
            <el-option v-for="dict in menuType" :key="dict.value" :label="dict.label" :value="dict.value" />
          </el-select>
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
          <el-button @click="openAddMenu">
            <el-icon>
              <ele-Plus />
            </el-icon>
            新增
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card shadow="hover" style="margin-top: 5px;">
      <el-table :data="menuData" v-loading="loading" row-key="id"
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }">
        <el-table-column label="菜单名称" show-overflow-tooltip>
          <template #default="scope">
            <SvgIcon :name="scope.row.icon" />
            <span class="ml10">{{ $t(scope.row.title) }}</span>
          </template>
        </el-table-column>
        <el-table-column label="类型" show-overflow-tooltip width="80" align="center">
          <template #default="scope">
            <el-tag type="warning" v-if="scope.row.type === 1">目录</el-tag>
            <el-tag v-else-if="scope.row.type === 2">菜单</el-tag>
            <el-tag type="info" v-else>按钮</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="path" label="路由路径" show-overflow-tooltip></el-table-column>
        <el-table-column label="组件路径" show-overflow-tooltip>
          <template #default="scope">
            <span>{{ scope.row.component }}</span>
          </template>
        </el-table-column>
        <el-table-column label="权限标识" show-overflow-tooltip>
          <template #default="scope">
            <span>{{ scope.row.permission }}</span>
          </template>
        </el-table-column>
        <el-table-column label="排序" show-overflow-tooltip width="80" align="center">
          <template #default="scope">
            {{ scope.row.orderNo }}
          </template>
        </el-table-column>
        <el-table-column label="状态" show-overflow-tooltip width="80" align="center">
          <template #default="scope">
            <el-tag type="success" v-if="scope.row.status === 1">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="修改时间" show-overflow-tooltip align="center">
          <template #default="scope">
            {{ scope.row.createTime }}
          </template>
        </el-table-column>
        <el-table-column label="操作" show-overflow-tooltip width="80" fixed="right" align="center">
          <template #default="scope">
            <el-button size="small" text type="primary" @click="openEditMenu(scope.row)">
              <el-icon>
                <ele-Edit />
              </el-icon>
            </el-button>
            <el-button size="small" text type="primary" @click="delTabelRow(scope.row)">
              <el-icon>
                <ele-Delete />
              </el-icon>
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <EditMenu ref="editMenuRef" :title="editMenuTitle" :menuData="menuData" />
  </div>
</template>

<script lang="ts">
import { ref, toRefs, reactive, defineComponent, onMounted, getCurrentInstance, onUnmounted } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import EditMenu from '/@/views/system/menu/component/editMenu.vue';

import { SysMenuApi } from '/@/api-services';
import { getAPI } from '/@/utils/axios-utils';

export default defineComponent({
  name: 'sysMenu',
  components: { EditMenu },
  setup() {
    const { proxy } = getCurrentInstance() as any;
    const editMenuRef = ref();
    const state: any = reactive({
      loading: true,
      menuData: [],
      queryParams: {
        title: undefined,
        type: undefined,
      },
      menuType: [{ value: 1, label: "目录" }, { value: 2, label: "菜单" }, { value: 3, label: "按钮" }],
      editMenuTitle: "",
    });
    onMounted(async () => {
      handleQuery();

      proxy.mittBus.on("onSubmitRefresh", () => {
        handleQuery();
      });
    });
    onUnmounted(() => {
      proxy.mittBus.off("onSubmitRefresh");
    });

    // 查询操作
    const handleQuery = async () => {
      state.loading = true;
      var res = await getAPI(SysMenuApi).sysMenuListGet(state.queryParams.title, state.queryParams.type);
      state.menuData = res.data.result;
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
      state.editMenuTitle = "添加菜单";
      editMenuRef.value.openDialog({});
    };
    // 打开编辑页面
    const openEditMenu = (row: any) => {
      state.editMenuTitle = "编辑菜单";
      editMenuRef.value.openDialog(row);
    };
    // 删除当前行
    const delTabelRow = (row: any) => {
      ElMessageBox.confirm(`确定删除菜单：【${row.title}】?`, '提示', {
        confirmButtonText: '删除',
        cancelButtonText: '取消',
        type: 'warning',
      })
        .then(async () => {
          getAPI(SysMenuApi).sysMenuDeletePost({ id: row.id }).then(() => {
            handleQuery();
            ElMessage.success('删除成功');
          })
        })
        .catch(() => { });
    };
    return {
      handleQuery,
      resetQuery,
      editMenuRef,
      openAddMenu,
      openEditMenu,
      delTabelRow,
      ...toRefs(state),
    };
  },
});
</script>
