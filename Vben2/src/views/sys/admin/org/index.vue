<template>
  <PageWrapper dense contentFullHeight fixedHeight contentClass="flex">
    <OrgTree
      class="w-1/4 xl:w-1/5"
      style="overflow: auto"
      @select="handleSelect"
      ref="OrgTreeChild"
    />
    <BasicTable @register="registerTable" class="w-3/4 xl:w-4/5" :searchInfo="searchInfo">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate" :disabled="!hasPermission('sysOrg:add')">
          新增机构
        </a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              label: '编辑',
              disabled: !hasPermission('sysOrg:update'),
              onClick: handleEdit.bind(null, record),
            },
            {
              icon: 'ant-design:delete-outlined',
              label: '删除',
              color: 'error',
              ifShow: hasPermission('sysOrg:delete'),
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <OrgModal @register="registerModal" @success="handleSuccess" />
  </PageWrapper>
</template>
<script lang="ts">
  import { defineComponent, reactive, ref, unref } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { usePermission } from '/@/hooks/web/usePermission';

  import { PageWrapper } from '/@/components/Page';
  import OrgTree from '/@/views/sys/admin/user/OrgTree.vue';

  import OrgModal from './OrgModal.vue';

  import { columns, searchFormSchema } from './org.data';
  import { getOrgList, deleteOrg } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'OrgManagement',
    components: { BasicTable, OrgModal, TableAction, PageWrapper, OrgTree },
    setup() {
      const { hasPermission } = usePermission();
      const OrgTreeChild = ref(null);
      const [registerModal, { openModal }] = useModal();
      const searchInfo = reactive<Recordable>({});
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '机构列表',
        api: getOrgList,
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
        await deleteOrg(record.id);
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
