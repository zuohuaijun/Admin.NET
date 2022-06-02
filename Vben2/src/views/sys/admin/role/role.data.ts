import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Switch, Tag } from 'ant-design-vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { usePermission } from '/@/hooks/web/usePermission';

import { setRoleStatus } from '/@/api/sys/admin';

const { hasPermission } = usePermission();

export const columns: BasicColumn[] = [
  {
    title: '角色名称',
    dataIndex: 'name',
    width: 200,
  },
  {
    title: '编码',
    dataIndex: 'code',
    width: 180,
  },
  {
    title: '状态',
    dataIndex: 'status',
    width: 120,
    customRender: ({ record }) => {
      if (!Reflect.has(record, 'pendingStatus')) {
        record.pendingStatus = false;
      }
      return h(Switch, {
        checked: record.status === 1,
        checkedChildren: '已启用',
        unCheckedChildren: '已禁用',
        disabled: !hasPermission('sysRole:setStatus'),
        loading: record.pendingStatus,
        onChange(checked: boolean) {
          record.pendingStatus = true;
          const newStatus = checked ? 1 : 2;
          const { createMessage } = useMessage();
          setRoleStatus(record.id, newStatus)
            .then(() => {
              record.status = newStatus;
              createMessage.success(`已成功修改角色状态`);
            })
            .catch(() => {
              createMessage.error('修改角色状态失败');
            })
            .finally(() => {
              record.pendingStatus = false;
            });
        },
      });
    },
  },
  {
    title: '数据范围',
    dataIndex: 'dataScope',
    width: 200,
    customRender: ({ record }) => {
      const dataScope = record.dataScope;
      switch (dataScope) {
        case 1:
          return h(Tag, { color: 'orange' }, () => '全部数据');
        case 2:
          return h(Tag, { color: 'orange' }, () => '本部门及以下');
        case 3:
          return h(Tag, { color: 'orange' }, () => '本部门数据');
        case 4:
          return h(Tag, { color: 'orange' }, () => '仅本人数据');
        case 5:
          return h(Tag, { color: 'orange' }, () => '自定义数据');
      }
      return dataScope;
    },
  },
  {
    title: '排序',
    dataIndex: 'order',
    width: 50,
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    width: 180,
  },
  {
    title: '备注',
    dataIndex: 'remark',
  },
];

export const searchFormSchema: FormSchema[] = [
  {
    field: 'name',
    label: '角色名称',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'code',
    label: '角色编码',
    component: 'Input',
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    field: 'name',
    label: '角色名称',
    required: true,
    component: 'Input',
    colProps: { span: 24 },
  },
  {
    field: 'code',
    label: '角色编码',
    component: 'Input',
    colProps: { span: 24 },
    //ifShow: ({ values }) => values.code != 'sys_admin_role',
    dynamicDisabled: ({ values }) => values.code == 'sys_admin_role',
  },
  {
    field: 'status',
    label: '状态',
    component: 'RadioButtonGroup',
    defaultValue: 1,
    componentProps: {
      options: [
        { label: '启用', value: 1 },
        { label: '停用', value: 2 },
      ],
    },
  },
  {
    label: '备注',
    field: 'remark',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
];
