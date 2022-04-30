<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate" :disabled="!hasPermission('sysTenant:add')"
          >新增租户</a-button
        >
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              label: '编辑',
              disabled: !hasPermission('sysTenant:update'),
              onClick: handleEdit.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              icon: 'ant-design:menu-outlined',
              label: '授权菜单',
              disabled: !hasPermission('sysTenant:grantMenu'),
              onClick: handleGrantMenu.bind(null, record),
            },
            {
              icon: 'ant-design:database-outlined',
              label: '重置密码',
              disabled: !hasPermission('sysTenant:resetPwd'),
              popConfirm: {
                title: '是否确认重置密码？',
                confirm: handleResetTenantPwd.bind(null, record),
              },
            },
            {
              icon: 'ant-design:delete-outlined',
              color: 'error',
              label: '删除',
              ifShow: hasPermission('sysTenant:delete'),
              popConfirm: {
                title: '是否确认删除？',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <TenantDrawer @register="registerTenantDrawer" @success="handleSuccess" />
    <GrantMenuDrawer @register="registerGrantMenuDrawer" />
  </div>
</template>
<script lang="ts">
  import { defineComponent, reactive } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useDrawer } from '/@/components/Drawer';
  import { usePermission } from '/@/hooks/web/usePermission';

  import TenantDrawer from './TenantDrawer.vue';
  import GrantMenuDrawer from './GrantMenuDrawer.vue';

  import { columns, searchFormSchema } from './tenant.data';
  import { getTenantPageList, deleteTenant, resetTenantPwd } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'UserManagement',
    components: {
      BasicTable,
      TableAction,
      TenantDrawer,
      GrantMenuDrawer,
    },
    setup() {
      const { hasPermission } = usePermission();
      const { createMessage } = useMessage();
      const [registerTenantDrawer, { openDrawer: openTenantDrawer }] = useDrawer();
      const [registerGrantMenuDrawer, { openDrawer: openGrantMenuDrawer }] = useDrawer();
      const searchInfo = reactive<Recordable>({});
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '租户列表',
        api: getTenantPageList,
        rowKey: 'id',
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
          autoSubmitOnEnter: true,
        },
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        actionColumn: {
          width: 160,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleCreate() {
        openTenantDrawer(true, {
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openTenantDrawer(true, {
          record,
          isUpdate: true,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteTenant(record.id);
        reload();
        createMessage.success('删除成功！');
      }

      function handleSuccess({ isUpdate, values }) {
        if (isUpdate) {
          const result = updateTableDataRecord(values.id, values);
          console.log(result);
        } else {
          reload();
        }
      }

      function handleSelect(orgId: number) {
        searchInfo.orgId = orgId;
        reload();
      }

      function handleGrantMenu(record: Recordable) {
        openGrantMenuDrawer(true, { record });
      }

      function handleResetTenantPwd(record: Recordable) {
        resetTenantPwd(record.id);
        createMessage.success(`已成功重置密码`);
      }

      return {
        registerTable,
        handleCreate,
        handleEdit,
        handleDelete,
        handleSuccess,
        handleSelect,
        searchInfo,
        registerTenantDrawer,
        registerGrantMenuDrawer,
        handleGrantMenu,
        handleResetTenantPwd,
        hasPermission,
      };
    },
  });
</script>
