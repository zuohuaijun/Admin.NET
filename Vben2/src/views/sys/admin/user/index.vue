<template>
  <PageWrapper dense contentFullHeight fixedHeight contentClass="flex">
    <OrgTree class="w-1/4 xl:w-1/5" style="overflow: auto" @select="handleSelect" />
    <BasicTable @register="registerTable" class="w-3/4 xl:w-4/5" :searchInfo="searchInfo">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate" :disabled="!hasPermission('sysUser:add')">
          新增账号
        </a-button>
      </template>
      <template #avatar="{ record }">
        <Avatar :size="60" :src="record.avatar" />
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'clarity:note-edit-line',
              label: '编辑',
              tooltip: '编辑账号',
              disabled: !hasPermission('sysUser:update'),
              onClick: handleEdit.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              icon: 'ant-design:menu-outlined',
              label: '授权角色',
              disabled: !hasPermission('sysUser:grantRole'),
              onClick: handleGrantRole.bind(null, record),
            },
            {
              icon: 'ant-design:database-outlined',
              label: '授权数据',
              disabled: !hasPermission('sysUser:grantData'),
              onClick: handleGrantData.bind(null, record),
            },
            {
              icon: 'ant-design:redo-outlined',
              label: '重置密码',
              ifShow: hasPermission('sysUser:resetPwd'),
              popConfirm: {
                title: '是否确认重置密码',
                confirm: handleResetUserPwd.bind(null, record),
              },
            },
            {
              icon: 'ant-design:delete-outlined',
              color: 'error',
              label: '删除账号',
              ifShow: hasPermission('sysUser:delete'),
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <UserDrawer @register="registerUserDrawer" @success="handleSuccess" />
    <GrantOrgDrawer @register="registerGrantOrgDrawer" />
    <GrantRoleDrawer @register="registerGrantRoleDrawer" />
  </PageWrapper>
</template>
<script lang="ts">
  import { defineComponent, reactive } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { PageWrapper } from '/@/components/Page';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useDrawer } from '/@/components/Drawer';
  import { usePermission } from '/@/hooks/web/usePermission';

  import OrgTree from './OrgTree.vue';
  import UserDrawer from './UserDrawer.vue';
  import GrantOrgDrawer from './GrantOrgDrawer.vue';
  import GrantRoleDrawer from './GrantRoleDrawer.vue';

  import { columns, searchFormSchema } from './user.data';
  import { getUserPageList, deleteUser, resetUserPwd } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'UserManagement',
    components: {
      BasicTable,
      PageWrapper,
      OrgTree,
      TableAction,
      UserDrawer,
      GrantOrgDrawer,
      GrantRoleDrawer,
    },
    setup() {
      const { hasPermission } = usePermission();
      const { createMessage } = useMessage();
      const [registerUserDrawer, { openDrawer: openUserDrawer }] = useDrawer();
      const [registerGrantOrgDrawer, { openDrawer: openGrantOrgDrawer }] = useDrawer();
      const [registerGrantRoleDrawer, { openDrawer: openGrantRoleDrawer }] = useDrawer();
      const searchInfo = reactive<Recordable>({});
      const [registerTable, { reload, updateTableDataRecord }] = useTable({
        title: '账号列表',
        api: getUserPageList,
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
        canResize: true,
        actionColumn: {
          width: 140,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleCreate() {
        openUserDrawer(true, {
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openUserDrawer(true, {
          record,
          isUpdate: true,
        });
      }

      async function handleDelete(record: Recordable) {
        await deleteUser(record.id);
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

      function handleSelect(orgId: number) {
        searchInfo.orgId = orgId;
        reload();
      }

      function handleGrantData(record: Recordable) {
        openGrantOrgDrawer(true, { record });
      }

      function handleGrantRole(record: Recordable) {
        openGrantRoleDrawer(true, { record });
      }

      function handleResetUserPwd(record: Recordable) {
        resetUserPwd(record.id);
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
        registerUserDrawer,
        registerGrantOrgDrawer,
        registerGrantRoleDrawer,
        handleGrantData,
        handleGrantRole,
        handleResetUserPwd,
        hasPermission,
      };
    },
  });
</script>
