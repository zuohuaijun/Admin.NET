<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate"> 新增类型 </a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              onClick: handleEdit.bind(null, record),
              label: '编辑',
            },
            {
              icon: 'arcticons:colordict',
              label: '字典',
              onClick: handleDictData.bind(null, record),
            },
            {
              icon: 'ant-design:delete-outlined',
              color: 'error',
              label: '删除',
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <DcitModal @register="registerModal" @success="handleSuccess" />
    <DcitDataModal @register="registerDictDataModal" />
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { columns, searchFormSchema } from './dict.data';
  import { getDictTypeList, deleteDictType } from '/@/api/sys/admin';
  import { useModal } from '/@/components/Modal';
  import DcitModal from './DictModal.vue';
  import DcitDataModal from './dictdata/index.vue';
  export default defineComponent({
    components: { BasicTable, TableAction, DcitModal, DcitDataModal },
    setup() {
      const [registerModal, { openModal: openDictModal }] = useModal();
      const [registerDictDataModal, { openModal: openDictDataModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        title: '字典列表',
        api: getDictTypeList,
        columns: columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
          autoSubmitOnEnter: true,
        },
        rowKey: 'id',
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        actionColumn: {
          width: 220,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });
      function handleCreate() {
        openDictModal(true, {
          isUpdate: false,
        });
      }
      function handleEdit(record: Recordable) {
        openDictModal(true, {
          record,
          isUpdate: true,
        });
      }
      function handleDictData(record: Recordable) {
        openDictDataModal(true, {
          typeId: record.id,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteDictType(record.id);
        reload();
      }
      function handleSuccess() {
        reload();
      }
      return {
        registerTable,
        registerModal,
        registerDictDataModal,
        handleCreate,
        handleEdit,
        handleDelete,
        handleSuccess,
        handleDictData,
      };
    },
  });
</script>
