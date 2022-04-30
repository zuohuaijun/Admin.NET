<template>
  <BasicDrawer
    v-bind="$attrs"
    @register="registerDrawer"
    showFooter
    title="授权用户角色"
    width="500px"
    @ok="handleSubmit"
  >
    <BasicTable @register="registerTable" />
  </BasicDrawer>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable } from '/@/components/Table';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';

  import { getRoleList, grantUserRole, userOwnRoleList } from '/@/api/sys/admin';
  import { GrantUserRoleItem } from '/@/api/sys/model/adminModel';

  export default defineComponent({
    name: 'GranRoleDrawer',
    components: { BasicDrawer, BasicTable },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      let rowId: number;
      let orgId: number;

      const [registerTable, { getSelectRowKeys, setSelectedRowKeys }] = useTable({
        title: '',
        api: getRoleList,
        columns: [
          {
            title: '角色名称',
            dataIndex: 'name',
            width: 150,
          },
          {
            title: '编码',
            dataIndex: 'code',
            width: 120,
          },
        ],
        useSearchForm: false,
        showTableSetting: false,
        bordered: true,
        showIndexColumn: false,
        canResize: true,
        rowKey: 'id',
        rowSelection: {
          type: 'checkbox',
        },
        pagination: {
          pageSize: 50,
        },
      });

      const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner(async (data) => {
        setDrawerProps({ confirmLoading: false });

        rowId = data.record.id;
        orgId = data.record.orgId;
        setSelectedRowKeys(await userOwnRoleList(rowId));
      });

      async function handleSubmit() {
        try {
          setDrawerProps({ confirmLoading: true });

          let userRole: GrantUserRoleItem = {
            id: rowId,
            orgId: orgId,
            roleIdList: getSelectRowKeys() as unknown as number[],
          };
          await grantUserRole(userRole);

          closeDrawer();
          emit('success');
        } finally {
          setDrawerProps({ confirmLoading: false });
        }
      }

      return {
        registerDrawer,
        registerTable,
        handleSubmit,
      };
    },
  });
</script>
