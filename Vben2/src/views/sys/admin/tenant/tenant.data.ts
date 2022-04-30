import { h } from 'vue';
import { Tag } from 'ant-design-vue';
import { BasicColumn, FormSchema } from '/@/components/Table';

export const columns: BasicColumn[] = [
  {
    title: '公司名称',
    dataIndex: 'name',
    customRender: ({ record }) => {
      return h(Tag, { color: 'orange' }, () => record.name);
    },
  },
  {
    title: '管理员名称',
    dataIndex: 'adminName',
  },
  {
    title: '主机',
    dataIndex: 'host',
  },
  {
    title: '电子邮箱',
    dataIndex: 'email',
  },
  {
    title: '电话',
    dataIndex: 'phone',
  },
  {
    title: '数据库连接',
    dataIndex: 'connection',
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
    field: 'name',
    label: '公司名称',
    colProps: { span: 8 },
    component: 'Input',
  },
  {
    field: 'phone',
    label: '电话',
    colProps: { span: 8 },
    component: 'Input',
  },
];

export const formSchema: FormSchema[] = [
  {
    label: '公司名称',
    field: 'name',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    label: '管理员名称',
    field: 'adminName',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    label: '主机',
    field: 'host',
    component: 'Input',
    required: false,
    colProps: { span: 24 },
  },
  {
    label: '电子邮箱',
    field: 'email',
    component: 'Input',
    required: false,
    colProps: { span: 24 },
  },
  {
    label: '电话',
    field: 'phone',
    component: 'Input',
    required: false,
    colProps: { span: 24 },
  },
  {
    label: '数据库连接',
    field: 'connection',
    component: 'Input',
    required: false,
    colProps: { span: 24 },
  },
  {
    label: '备注',
    field: 'remark',
    component: 'InputTextArea',
    required: false,
    colProps: { span: 24 },
  },
];
