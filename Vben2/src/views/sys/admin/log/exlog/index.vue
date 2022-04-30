<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleClear" :disabled="!hasPermission('sysExlog:clear')">
          清空日志
        </a-button>
      </template>
    </BasicTable>
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable } from '/@/components/Table';
  import { usePermission } from '/@/hooks/web/usePermission';

  import { columns, searchFormSchema } from './exlog.data';
  import { getExLogPageList, clearExLog } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'ExlogManagement',
    components: { BasicTable },
    setup() {
      const { hasPermission } = usePermission();
      const [registerTable, { reload }] = useTable({
        title: '日志列表',
        api: getExLogPageList,
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
        await clearExLog();
        reload();
      }

      return {
        registerTable,
        handleClear,
        hasPermission,
      };
    },
  });
</script>
