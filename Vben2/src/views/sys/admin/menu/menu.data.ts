import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';
import { Icon } from '/@/components/Icon';

export const columns: BasicColumn[] = [
  {
    title: '菜单名称',
    dataIndex: 'title',
    align: 'left',
  },
  {
    title: '图标',
    dataIndex: 'icon',
    width: 80,
    customRender: ({ record }) => {
      return h(Icon, { icon: record.icon });
    },
  },
  {
    title: '类型',
    dataIndex: 'type',
    width: 80,
    customRender: ({ record }) => {
      const type = record.type;
      if (~~type === 1) return h(Tag, { color: 'gold' }, () => '目录');
      if (~~type === 2) return h(Tag, { color: 'purple' }, () => '菜单');
      if (~~type === 3) return h(Tag, { color: 'blue' }, () => '按钮');
    },
  },
  {
    title: '路由',
    dataIndex: 'path',
  },
  {
    title: '组件',
    dataIndex: 'component',
  },
  {
    title: '权限标识',
    dataIndex: 'permission',
    width: 180,
  },
  {
    title: '是否隐藏',
    dataIndex: 'hideMenu',
    width: 80,
    customRender: ({ record }) => {
      const hideMenu: boolean = record.hideMenu;
      const color = hideMenu ? 'red' : 'green';
      const text = hideMenu ? '隐藏' : '显示';
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
    dataIndex: 'orderNo',
    width: 50,
  },
];

const isDir = (type: number) => type === 1;
const isMenu = (type: number) => type === 2;
const isButton = (type: number) => type === 3;

export const searchFormSchema: FormSchema[] = [
  {
    field: 'title',
    label: '菜单名称',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'type',
    label: '菜单类型',
    component: 'Select',
    componentProps: {
      options: [
        { label: '目录', value: 1 },
        { label: '菜单', value: 2 },
        { label: '按钮', value: 3 },
      ],
    },
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    field: 'type',
    label: '菜单类型',
    component: 'RadioButtonGroup',
    defaultValue: 1,
    componentProps: {
      options: [
        { label: '目录', value: 1 },
        { label: '菜单', value: 2 },
        { label: '按钮', value: 3 },
      ],
    },
    colProps: { lg: 24, md: 24 },
  },
  {
    field: 'title',
    label: '菜单标题',
    component: 'Input',
    required: true,
  },
  {
    field: 'name',
    label: '名称',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'pid',
    label: '上级菜单',
    component: 'TreeSelect',
    defaultValue: 0,
    componentProps: {
      fieldNames: {
        label: 'title',
        key: 'id',
        value: 'id',
      },
      getPopupContainer: () => document.body,
    },
    required: true,
  },
  {
    field: 'orderNo',
    label: '排序',
    component: 'InputNumber',
    defaultValue: 0,
    required: true,
    componentProps: { style: { width: '100%' } },
  },
  {
    field: 'icon',
    label: '图标',
    component: 'IconPicker',
    //required: ({ values }) => isDir(values.type),
    ifShow: ({ values }) => !isButton(values.type),
  },
  {
    field: 'path',
    label: '路由地址',
    component: 'Input',
    required: true,
    ifShow: ({ values }) => !isButton(values.type),
  },
  {
    field: 'component',
    label: '组件路径',
    component: 'Input',
    ifShow: ({ values }) => !isButton(values.type),
  },
  {
    field: 'redirect',
    label: '跳转地址',
    component: 'Input',
    ifShow: ({ values }) => isDir(values.type),
  },
  {
    field: 'frameSrc',
    label: '内嵌地址',
    component: 'Input',
    ifShow: ({ values }) => !isButton(values.type),
  },
  {
    field: 'permission',
    label: '权限标识',
    component: 'Input',
    required: true,
    ifShow: ({ values }) => isButton(values.type),
  },
  {
    field: 'ignoreKeepAlive',
    label: '忽略缓存',
    component: 'RadioButtonGroup',
    defaultValue: true,
    componentProps: {
      options: [
        { label: '是', value: true },
        { label: '否', value: false },
      ],
    },
    ifShow: ({ values }) => isMenu(values.type),
  },
  {
    field: 'hideMenu',
    label: '是否隐藏',
    component: 'RadioButtonGroup',
    defaultValue: false,
    componentProps: {
      options: [
        { label: '是', value: true },
        { label: '否', value: false },
      ],
    },
    ifShow: ({ values }) => isMenu(values.type),
  },
];
