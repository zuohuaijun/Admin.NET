<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    title="字典值管理"
    :showOkBtn="false"
    width="1000px"
  >
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate"> 新增值 </a-button>
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
  </BasicModal>
  <DictDataModal @register="registerDictModal" @success="handleSuccess" />
</template>
<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { columns, searchFormSchema } from './dictData.data';
  import DictDataModal from './DictDataModal.vue';
  import { BasicModal, useModalInner, useModal } from '/@/components/Modal';
  import { getDictDataList, deleteDictData } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'DcitDataModal',
    components: { BasicModal, BasicTable, TableAction, DictDataModal },
    setup() {
      const typeId = ref(0);
      const [registerDictModal, { openModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        api: getDictDataList,
        columns: columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
          autoSubmitOnEnter: true,
        },
        beforeFetch(params) {
          params.DictTypeId = typeId.value;
          return params;
        },
        rowKey: 'id',
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,

        actionColumn: {
          width: 150,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
        size: 'small',
      });
      const [registerModal, { setModalProps }] = useModalInner(async (data) => {
        setModalProps({ confirmLoading: false });
        typeId.value = data.typeId;
        reload();
      });

      function handleCreate() {
        openModal(true, {
          typeId: typeId.value,
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openModal(true, {
          record,
          typeId: typeId.value,
          isUpdate: true,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteDictData(record.id);
        reload();
      }

      function handleSuccess() {
        reload();
      }

      return {
        registerTable,
        registerDictModal,
        handleCreate,
        handleEdit,
        handleDelete,
        registerModal,
        handleSuccess,
      };
    },
  });
</script>
