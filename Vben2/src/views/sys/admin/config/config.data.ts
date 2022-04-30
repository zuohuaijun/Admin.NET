import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';

export const columns: BasicColumn[] = [
  {
    title: '名称',
    dataIndex: 'name',
    width: 200,
  },
  {
    title: '编码',
    dataIndex: 'code',
    width: 150,
  },
  {
    title: '属性值',
    dataIndex: 'value',
    width: 100,
  },
  {
    title: '平台配置',
    dataIndex: 'sysFlag',
    width: 120,
    customRender: ({ record }) => {
      const sysFlag = record.sysFlag;
      if (~~sysFlag === 1) return h(Tag, { color: 'red' }, () => '是');
      if (~~sysFlag === 2) return h(Tag, { color: 'blue' }, () => '否');
    },
  },
  {
    title: '所属分类',
    dataIndex: 'groupCode',
    width: 100,
  },
  {
    title: '排序',
    dataIndex: 'order',
    width: 80,
  },
  {
    title: '备注',
    dataIndex: 'remark',
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    width: 180,
  },
];

export const searchFormSchema: FormSchema[] = [
  {
    label: '名称',
    field: 'name',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    label: '编码',
    field: 'code',
    component: 'Input',
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    label: '名称',
    field: 'name',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    label: '编码',
    field: 'code',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    label: '值',
    field: 'value',
    component: 'Input',
    required: true,
    colProps: { span: 12 },
  },
  {
    label: '平台配置',
    field: 'sysFlag',
    component: 'RadioButtonGroup',
    defaultValue: 2,
    componentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 2 },
      ],
    },
    required: true,
    colProps: { span: 12 },
  },
  {
    label: '所属分类',
    field: 'groupCode',
    component: 'Input',
    colProps: { span: 12 },
  },
  {
    label: '排序',
    field: 'order',
    component: 'InputNumber',
    defaultValue: 1,
    required: true,
    colProps: { span: 12 },
    componentProps: { style: { width: '100%' } },
  },
  {
    label: '备注',
    field: 'remark',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
];
