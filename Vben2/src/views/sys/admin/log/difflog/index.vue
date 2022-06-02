<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          type="primary"
          @click="handleClear"
          :disabled="!hasPermission('sysDifflog:clear')"
        >
          清空日志
        </a-button>
      </template>
      <template #diffType="{ text }">
        {{ text.toUpperCase() }}
      </template>
      <template #sql="{ text, record }">
        <a @click="showInfo(record)" :title="text">点击查看</a>
      </template>
      <template #duration="{ text }">
        {{ text }}
      </template>
      <template #beforeData="{ text }">
        <!-- {{ text }} -->
      </template>
      <template #afterData="{ text }">
        <!-- {{ text }} -->
      </template>
      <template #businessData="{ text }">
        <!-- {{ text }} -->
      </template>
    </BasicTable>
    <Drawer @register="register" />
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable } from '/@/components/Table';
  import { useDrawer } from '/@/components/Drawer';
  import { usePermission } from '/@/hooks/web/usePermission';

  import { columns, searchFormSchema } from './difflog.data';
  import { getDiffLogPageList, clearDiffLog } from '/@/api/sys/admin';
  import Drawer from './info.vue';

  export default defineComponent({
    name: 'ExlogManagement',
    components: { BasicTable, Drawer },
    setup() {
      const { hasPermission } = usePermission();
      const [registerTable, { reload }] = useTable({
        title: '日志列表',
        api: getDiffLogPageList,
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
        },
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: true,
        pagination: {
          pageSize: 10,
        },
      });

      async function handleClear() {
        await clearDiffLog();
        reload();
      }
      const [register, { openDrawer }] = useDrawer();

      function showInfo(record) {
        openDrawer(true, { ...record });
      }

      return {
        registerTable,
        handleClear,
        hasPermission,
        register,
        openDrawer,
        showInfo,
      };
    },
  });
</script>
