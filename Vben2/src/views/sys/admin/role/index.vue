<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate" :disabled="!hasPermission('sysRole:add')">
          新增角色
        </a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              label: '编辑',
              disabled: !hasPermission('sysRole:update'),
              onClick: handleEdit.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              icon: 'ant-design:menu-outlined',
              label: '授权菜单',
              disabled: !hasPermission('sysRole:grantMenu'),
              onClick: handleGrantMenu.bind(null, record),
            },
            {
              icon: 'ant-design:database-outlined',
              label: '授权数据',
              disabled: !hasPermission('sysRole:grantData'),
              onClick: handleGrantData.bind(null, record),
            },
            {
              icon: 'ant-design:delete-outlined',
              color: 'error',
              label: '删除',
              ifShow: hasPermission('sysRole:delete'),
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <RoleDrawer @register="registerDrawer" @success="handleSuccess" />
    <GrantMenuDrawer @register="registerGrantMenuDrawer" />
    <GrantDataDrawer @register="registerGrantDataDrawer" @success="handleSuccess" />
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useDrawer } from '/@/components/Drawer';
  import { usePermission } from '/@/hooks/web/usePermission';

  import RoleDrawer from './RoleDrawer.vue';
  import GrantMenuDrawer from './GrantMenuDrawer.vue';
  import GrantDataDrawer from './GrantDataDrawer.vue';

  import { columns, searchFormSchema } from './role.data';
  import { getRolePageList, deleteRole } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'RoleManagement',
    components: {
      BasicTable,
      RoleDrawer,
      TableAction,
      GrantMenuDrawer,
      GrantDataDrawer,
    },
    setup() {
      const { hasPermission } = usePermission();
      const [registerDrawer, { openDrawer }] = useDrawer();
      const [registerGrantMenuDrawer, { openDrawer: openGrantMenuDrawer }] = useDrawer();
      const [registerGrantDataDrawer, { openDrawer: openGrantDataDrawer }] = useDrawer();
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '角色列表',
        api: getRolePageList,
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
        actionColumn: {
          width: 160,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });

      function handleCreate() {
        openDrawer(true, {
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openDrawer(true, {
          record,
          isUpdate: true,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteRole(record.id);
        reload();
      }

      function handleSuccess({ isUpdate, values }) {
        if (isUpdate) {
          updateTableDataRecord(values.id, values);
        } else {
          reload();
        }
      }

      function handleGrantMenu(record: Recordable) {
        openGrantMenuDrawer(true, { record });
      }

      function handleGrantData(record: Recordable) {
        openGrantDataDrawer(true, { record });
      }

      return {
        registerTable,
        registerDrawer,
        registerGrantMenuDrawer,
        registerGrantDataDrawer,
        handleCreate,
        handleEdit,
        handleDelete,
        handleSuccess,
        handleGrantMenu,
        handleGrantData,
        hasPermission,
      };
    },
  });
</script>
