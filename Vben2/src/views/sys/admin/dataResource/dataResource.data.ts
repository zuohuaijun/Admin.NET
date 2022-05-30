import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';

export const columns: BasicColumn[] = [
  {
    title: '资源名称',
    dataIndex: 'name',
    width: 200,
    align: 'left',
  },
  {
    title: '资源值',
    dataIndex: 'value',
    width: 200,
    align: 'left',
  },
  {
    title: '编码',
    dataIndex: 'code',
    width: 200,
    align: 'left',
  },
  {
    title: '状态',
    dataIndex: 'status',
    width: 80,
    customRender: ({ record }) => {
      const status = record.status;
      const enable = ~~status === 1;
      const color = enable ? 'green' : 'red';
      const text = enable ? '启用' : '停用';
      return h(Tag, { color: color }, () => text);
    },
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    width: 180,
  },
  {
    title: '排序',
    dataIndex: 'order',
    width: 50,
  },
  {
    title: '备注',
    dataIndex: 'remark',
  },
];

export const searchFormSchema: FormSchema[] = [
  {
    field: 'name',
    label: '资源名称',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'value',
    label: '资源值',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'code',
    label: '资源编码',
    component: 'Input',
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    field: 'pid',
    label: '上级资源',
    component: 'TreeSelect',
    defaultValue: 0,
    componentProps: {
      fieldNames: {
        label: 'name',
        key: 'id',
        value: 'id',
      },
      getPopupContainer: () => document.body,
    },
    required: true,
    colProps: { span: 24 },
  },
  {
    field: 'name',
    label: '资源名称',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    field: 'value',
    label: '资源值',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  // {
  //   field: 'code',
  //   label: '编码',
  //   component: 'Input',
  //   colProps: { span: 24 },
  // },
  {
    field: 'order',
    label: '排序',
    component: 'InputNumber',
    defaultValue: 0,
    required: true,
    componentProps: { style: { width: '100%' } },
    colProps: { span: 24 },
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
    required: true,
    colProps: { span: 24 },
  },
  {
    label: '备注',
    field: 'remark',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
];
