<template>
  <PageWrapper dense contentFullHeight fixedHeight contentClass="flex">
    <DistrictTree
      class="w-1/4 xl:w-1/5"
      style="overflow: auto"
      @select="handleSelect"
      ref="DistrictTreeChild"
    />
    <BasicTable @register="registerTable" class="w-3/4 xl:w-4/5" :searchInfo="searchInfo">
      <template #toolbar>
        <a-button
          type="primary"
          @click="handleCreate"
          :disabled="!hasPermission('sysDistrict:add')"
        >
          新增区域
        </a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              label: '编辑',
              disabled: !hasPermission('sysDistrict:update'),
              onClick: handleEdit.bind(null, record),
            },
            {
              icon: 'ant-design:delete-outlined',
              label: '删除',
              color: 'error',
              ifShow: hasPermission('sysDistrict:delete'),
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <DistrictModal @register="registerModal" @success="handleSuccess" />
  </PageWrapper>
</template>
<script lang="ts">
  import { defineComponent, reactive, ref, unref } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { usePermission } from '/@/hooks/web/usePermission';

  import { PageWrapper } from '/@/components/Page';
  import DistrictTree from './DistrictTree.vue';

  import DistrictModal from './DistrictModal.vue';

  import { columns, searchFormSchema } from './district.data';
  import { getDistrictList, deleteDistrict } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'DistrictManagement',
    components: { BasicTable, DistrictModal, TableAction, PageWrapper, DistrictTree },
    setup() {
      const { hasPermission } = usePermission();
      const OrgTreeChild = ref(null);
      const [registerModal, { openModal }] = useModal();
      const searchInfo = reactive<Recordable>({});
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '区域列表',
        api: getDistrictList,
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
        },
        rowKey: 'id',
        pagination: false,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: true,
        actionColumn: {
          width: 170,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function getTree() {
        const tree = unref(OrgTreeChild);
        if (!tree) {
          throw new Error('Tree is null!');
        }
        return tree;
      }

      function appendNodeByKey(parentKey, values) {
        getTree().appendNodeByKey(parentKey, values);
      }

      function updateNodeByKey(key, values) {
        getTree().updateNodeByKey(key, values);
      }

      function deleteNodeByKey(key) {
        getTree().deleteNodeByKey(key);
      }

      function handleCreate() {
        openModal(true, {
          searchInfo,
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
        await deleteDistrict(record.id);
        deleteNodeByKey(record.id);
        searchInfo.Id = record.pid;
        reload();
      }

      function handleSelect(orgId: number, obj) {
        if (obj == undefined) {
          searchInfo.Id = 0;
          searchInfo.pId = 0;
        } else {
          searchInfo.Id = orgId;
          searchInfo.pId = obj.pid ? obj.pid : 0;
        }
        reload();
      }

      function handleSuccess({ isUpdate, values }) {
        if (isUpdate) {
          updateTableDataRecord(values.id, values);
          updateNodeByKey(values.id, values);
        } else {
          reload();
          appendNodeByKey(values.pid, values);
        }
        // getTree().fetch();
      }

      return {
        registerTable,
        registerModal,
        searchInfo,
        OrgTreeChild,
        handleSelect,
        updateNodeByKey,
        appendNodeByKey,
        deleteNodeByKey,
        handleCreate,
        handleEdit,
        handleDelete,
        handleSuccess,
        hasPermission,
      };
    },
  });
</script>
