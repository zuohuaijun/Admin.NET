<template>
  <BasicDrawer
    v-bind="$attrs"
    @register="registerDrawer"
    showFooter
    title="授权角色数据"
    width="500px"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm">
      <template #org>
        <BasicTree
          :treeData="treeData"
          :fieldNames="{ title: 'name', key: 'id' }"
          :checkedKeys="ownOrgData"
          checkable
          toolbar
          search
          title="机构列表"
          ref="treeAction"
        />
      </template>
    </BasicForm>
  </BasicDrawer>
</template>
<script lang="ts">
  import { defineComponent, ref, unref, nextTick } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';
  import { BasicTree, TreeItem, TreeActionType } from '/@/components/Tree';

  import { getOrgList, ownOrgList, grantRoleData } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'GranDataDrawer',
    components: { BasicDrawer, BasicForm, BasicTree },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const treeData = ref<TreeItem[]>([]);
      let rowId: number;
      const ownOrgData = ref<number[]>([]);
      const treeAction = ref<Nullable<TreeActionType>>(null);

      const [registerForm, { resetFields, setFieldsValue, validate }] = useForm({
        labelWidth: 90,
        schemas: [
          {
            label: '数据范围',
            field: 'dataScope',
            component: 'Select',
            defaultValue: 1,
            componentProps: {
              options: [
                { label: '全部数据', value: 1 },
                { label: '本部门及以下', value: 2 },
                { label: '本部门数据', value: 3 },
                { label: '仅本人数据', value: 4 },
                { label: '自定义数据', value: 5 },
              ],
              onChange: async (e: any) => {
                InitOwnOrgData(e);
              },
            },
            //colProps: { span: 8 },
            required: true,
          },
          {
            label: ' ',
            field: 'org',
            slot: 'org',
            component: 'Input',
            ifShow: ({ values }) => values.dataScope === 5,
          },
        ],
        showActionButtonGroup: false,
      });

      const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner(async (data) => {
        resetFields();
        setDrawerProps({ confirmLoading: false });

        if (unref(treeData).length === 0) {
          treeData.value = (await getOrgList()) as any as TreeItem[];
          nextTick(() => {
            unref(treeAction)?.filterByLevel(1);
          });
        }

        rowId = data.record.id;
        setFieldsValue({
          ...data.record,
        });

        InitOwnOrgData(data.record.dataScope);
      });

      async function InitOwnOrgData(dataScope) {
        if (dataScope === 5) {
          ownOrgData.value = await ownOrgList(rowId);
          unref(treeAction)?.filterByLevel(1);
        } else {
          ownOrgData.value = [];
        }
      }

      async function handleSubmit() {
        try {
          const values = await validate();
          setDrawerProps({ confirmLoading: true });

          values.orgIdList = unref(treeAction)?.getCheckedKeys();
          values.id = rowId;
          await grantRoleData(values);

          closeDrawer();
          emit('success', {
            isUpdate: true,
            values: { ...values, id: rowId },
          });
        } finally {
          setDrawerProps({ confirmLoading: false });
        }
      }

      return {
        registerDrawer,
        registerForm,
        handleSubmit,
        treeData,
        ownOrgData,
        treeAction,
      };
    },
  });
</script>
