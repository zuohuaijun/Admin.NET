<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleClear" :disabled="!hasPermission('sysExlog:clear')">
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
    <ExlogDrawer @register="registerDrawer" />
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useDrawer } from '/@/components/Drawer';
  import ExlogDrawer from './ExlogDrawer.vue';
  import { columns, searchFormSchema } from './exlog.data';
  import { getExLogPageList, clearExLog } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'ExlogManagement',
    components: { BasicTable, ExlogDrawer, TableAction },
    setup() {
      const { hasPermission } = usePermission();
      const [registerDrawer, { openDrawer }] = useDrawer();
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
        actionColumn: {
          width: 100,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });

      async function handleClear() {
        await clearExLog();
        reload();
      }
      function handleView(record: Recordable) {
        console.log("handleView")
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
