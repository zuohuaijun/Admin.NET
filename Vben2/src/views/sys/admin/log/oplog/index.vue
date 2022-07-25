<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleClear" :disabled="!hasPermission('sysOplog:clear')">
          清空日志
        </a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'ant-design:eye-outlined',
              label: '查看',
              onClick: handleView.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <OplogDrawer @register="registerDrawer" />
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useDrawer } from '/@/components/Drawer';
  import { usePermission } from '/@/hooks/web/usePermission';

  import OplogDrawer from './OplogDrawer.vue';

  import { columns, searchFormSchema } from './oplog.data';
  import { getOpLogPageList, clearOpLog } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'OplogManagement',
    components: { BasicTable, OplogDrawer, TableAction },
    setup() {
      const { hasPermission } = usePermission();
      const [registerDrawer, { openDrawer }] = useDrawer();
      const [registerTable, { reload }] = useTable({
        title: '日志列表',
        api: getOpLogPageList,
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
        },
        rowKey: 'id',
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        pagination: {
          pageSize: 10,
        },
        actionColumn: {
          width: 100,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });

      async function handleClear() {
        await clearOpLog();
        reload();
      }

      function handleView(record: Recordable) {
        openDrawer(true, {
          record,
          isUpdate: true,
        });
      }

      return {
        registerTable,
        registerDrawer,
        handleClear,
        hasPermission,
        handleView,
      };
    },
  });
</script>
