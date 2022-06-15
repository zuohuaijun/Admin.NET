<template>
  <div>
    <PageWrapper dense contentFullHeight fixedHeight contentClass="flex">
      <BasicTable @register="registerTable" class="w-2/6" @row-dbClick="onRowClick">
        <template #tableTitle>
          <Select
            style="width: 150px"
            v-model:value="currentDB"
            :options="dbList"
            :field-names="{ label: 'configId', value: 'configId' }"
            @change="handleDbChange"
          />
        </template>
        <template #toolbar>
          <a-button type="primary" @click="handleCreateTable">新增数据表</a-button>
        </template>
        <template #action="{ record }">
          <TableAction
            :actions="[
              {
                icon: 'clarity:note-edit-line',
                label: '',
                tooltip: '编辑',
                onClick: handleEdit.bind(null, record),
              },
              {
                icon: 'ant-design:delete-outlined',
                label: '',
                tooltip: '删除',
                popConfirm: {
                  confirm: handleDelete.bind(null, record),
                  title: '确认删除？',
                },
              },
              {
                icon: 'ant-design:check-circle-outlined',
                label: '',
                tooltip: '生成实体',
                onClick: handleCreateEntity.bind(null, record),
              },
            ]"
          />
        </template>
      </BasicTable>
      <BasicTable @register="registerColumnTable" class="w-4/6">
        <template #toolbar>
          <a-button type="primary" v-if="currentTable" @click="handleCreateColumn"
            >新增({{ currentTable }})数据列</a-button
          >
        </template>
        <template #action="{ record }">
          <TableAction
            :actions="[
              {
                icon: 'clarity:note-edit-line',
                label: '',
                onClick: handleColumnEdit.bind(null, record),
              },
              {
                icon: 'ant-design:delete-outlined',
                label: '',
                popConfirm: {
                  confirm: handleColumnDelete.bind(null, record),
                  title: '确认删除？',
                },
              },
            ]"
          />
        </template>
      </BasicTable>
    </PageWrapper>
    <TableModal @register="registerTableModal" @success="handleSuccess" width="1400px" />
    <CreateEntityModal @register="registerCreateEntityModal" />
    <ColumnModal @register="registerColumnModal" @success="handleColumnSuccess" />
  </div>
</template>
<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { Select } from 'ant-design-vue';
  import { PageWrapper } from '/@/components/Page';
  import { tableShowColumns, columnShowColumns } from './database.data';
  import { useModal } from '/@/components/Modal';
  import TableModal from './TableModal.vue';
  import ColumnModal from './ColumnModal.vue';
  import CreateEntityModal from './CreateEntityModal.vue';
  import {
    getTableInfoList,
    getColumnInfoList,
    deleteTable,
    deleteColumn,
    getDatabaseList,
  } from '/@/api/sys/admin';
  import { useMessage } from '/@/hooks/web/useMessage';

  export default defineComponent({
    components: {
      BasicTable,
      PageWrapper,
      TableAction,
      TableModal,
      CreateEntityModal,
      ColumnModal,
      Select,
    },
    setup() {
      const { createMessage } = useMessage();
      const [registerTableModal, { openModal }] = useModal();
      const [registerCreateEntityModal, { openModal: openCreateEntityModal }] = useModal();
      const [registerColumnModal, { openModal: openColumnModal }] = useModal();
      const currentDB = ref('');
      const currentTable = ref('');
      const dbList = ref<any[]>([]);
      const columnApi = async () => {
        const result = await getColumnInfoList({
          tableName: currentTable.value,
          configId: currentDB.value,
        });
        return result;
      };
      const [registerTable, { reload }] = useTable({
        api: getTableInfoList,
        beforeFetch: (params: any) => {
          params.configId = currentDB.value;
        },
        immediate: false,
        rowKey: 'name',
        showIndexColumn: false,
        pagination: false,
        columns: tableShowColumns,
        actionColumn: {
          width: 120,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });
      getDatabaseList().then((res) => {
        dbList.value = res;
        currentDB.value = res[0].configId;
        reload();
      });
      const [registerColumnTable, { reload: colReload }] = useTable({
        api: columnApi,
        immediate: false,
        rowKey: 'dbColumnName',
        showIndexColumn: false,
        pagination: false,
        columns: columnShowColumns,
        actionColumn: {
          width: 120,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleCreateTable() {
        openModal(true, {
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openModal(true, {
          record,
          isUpdate: true,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteTable(record);
        createMessage.success('删除成功！');
        reload();
        colReload();
      }

      function handleCreateEntity(record: Recordable) {
        openCreateEntityModal(true, { tableName: record.name, configId: currentDB.value });
      }

      function handleCreateColumn() {
        openColumnModal(true, {
          tableName: currentTable.value,
          isUpdate: false,
        });
      }

      function handleColumnEdit(record: Recordable) {
        openColumnModal(true, {
          record,
          isUpdate: true,
        });
      }

      async function handleColumnDelete(record: Recordable) {
        await deleteColumn(record);
        createMessage.success('删除成功！');
        colReload();
      }

      function handleSuccess() {
        reload();
        colReload();
      }

      function handleColumnSuccess() {
        colReload();
      }

      function onRowClick(record: any) {
        currentTable.value = record.name;
        colReload();
      }
      function handleDbChange(value: string) {
        currentDB.value = value;
        currentTable.value = '';
        reload();
        colReload();
      }

      return {
        registerTable,
        registerColumnTable,
        handleCreateTable,
        handleEdit,
        handleDelete,
        registerTableModal,
        onRowClick,
        currentTable,
        handleSuccess,
        handleColumnSuccess,
        registerCreateEntityModal,
        handleCreateEntity,
        registerColumnModal,
        handleColumnEdit,
        handleCreateColumn,
        handleColumnDelete,
        dbList,
        currentDB,
        handleDbChange,
      };
    },
  });
</script>
