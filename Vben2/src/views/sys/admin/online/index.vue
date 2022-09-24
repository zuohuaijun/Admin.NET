<template>
  <div>
    <BasicTable @register="registerTable">
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              label: '强制下线',
              onClick: handleForceExist.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable } from '/@/components/Table';
  import { usePermission } from '/@/hooks/web/usePermission';

  import { columns, searchFormSchema } from './online.data';
  import { getOnlinePageList, forceExist } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'OnlineManagement',
    components: { BasicTable },
    setup() {
      const { hasPermission } = usePermission();
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '在线用户',
        api: getOnlinePageList,
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
        },
        rowKey: 'id',
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        actionColumn: {
          width: 170,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });

      async function handleForceExist(record: Recordable) {
        console.log(record);
        await forceExist(record.id);
        reload();
      }

      function handleSuccess({ isUpdate, values }) {
        if (isUpdate) {
          const result = updateTableDataRecord(values.id, values);
          console.log(result);
        } else {
          reload();
        }
      }

      return {
        registerTable,
        handleForceExist,
        handleSuccess,
        hasPermission,
      };
    },
  });
</script>
