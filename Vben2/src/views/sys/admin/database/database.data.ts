import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Switch } from 'ant-design-vue';

const apiTypeSelect = async () => {
  return [
    {
      value: 'text',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'varchar',
      hasLength: true,
      hasDecimalDigits: false,
    },
    {
      value: 'nvarchar',
      hasLength: true,
      hasDecimalDigits: false,
    },
    {
      value: 'char',
      hasLength: true,
      hasDecimalDigits: false,
    },
    {
      value: 'nchar',
      hasLength: true,
      hasDecimalDigits: false,
    },
    {
      value: 'timestamp',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'int',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'smallint',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'tinyint',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'bigint',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'bit',
      hasLength: false,
      hasDecimalDigits: false,
    },
    {
      value: 'decimal',
      hasLength: true,
      hasDecimalDigits: true,
    },
    {
      value: 'datetime',
      hasLength: false,
      hasDecimalDigits: false,
    },
  ];
};

export const tableShowColumns: BasicColumn[] = [
  {
    title: '表名',
    dataIndex: 'name',
  },
  {
    title: '描述',
    dataIndex: 'description',
  },
];

export const tableFormSchema: FormSchema[] = [
  {
    field: 'dataTableInfo',
    label: '数据表信息',
    component: 'Divider',
  },
  {
    field: 'oldName',
    label: '表名',
    component: 'Input',
    colProps: { span: 18 },
    ifShow: false,
    show: false,
  },
  {
    field: 'name',
    label: '表名',
    component: 'Input',
    required: true,
    colProps: { span: 18 },
  },
  {
    field: 'description',
    label: '描述',
    component: 'InputTextArea',
    required: true,
    colProps: { span: 18 },
  },
];
export const columnFormSchema: FormSchema[] = [
  {
    field: 'tableName',
    label: '表名',
    component: 'Input',
    required: true,
    show: false,
  },
  {
    field: 'oldName',
    label: '字段名',
    component: 'Input',
    colProps: { span: 18 },
    ifShow: false,
    show: false,
  },
  {
    field: 'dbColumnName',
    label: '字段名',
    component: 'Input',
    required: true,
  },
  {
    field: 'columnDescription',
    label: '描述',
    component: 'Input',
    required: false,
  },
  {
    field: 'isPrimarykey',
    label: '主键',
    component: 'Select',
    componentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
    defaultValue: 0,
  },
  {
    field: 'isIdentity',
    label: '自增',
    component: 'Select',
    componentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
    defaultValue: 0,
  },
  {
    field: 'dataType',
    label: '类型',
    component: 'ApiSelect',
    componentProps: {
      api: apiTypeSelect,
      labelField: 'value',
      valueField: 'value',
    },
    required: false,
  },
  {
    field: 'isNullable',
    label: '可空',
    component: 'Select',
    componentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
    defaultValue: 1,
  },
  {
    field: 'length',
    label: '长度',
    component: 'InputNumber',
    required: false,
  },
  {
    field: 'decimalDigits',
    label: '保留几位小数',
    component: 'InputNumber',
    required: false,
  },
];
export const createEntityFormSchema: FormSchema[] = [
  {
    field: 'tableName',
    label: '表名',
    component: 'Input',
    required: false,
    dynamicDisabled: true,
  },
  {
    field: 'entityName',
    label: '实体名称',
    component: 'Input',
    required: true,
  },
  {
    field: 'baseClassName',
    label: '基类',
    component: 'Select',
    componentProps: {
      options: [
        { label: 'EntityBaseId', value: 'EntityBaseId' },
        //{ label: 'AutoIncrementEntity', value: 'AutoIncrementEntity' },
        { label: 'EntityBase', value: 'EntityBase' },
        //{ label: 'DBEntityTenant', value: 'DBEntityTenant' },
      ],
    },
    defaultValue: 'EntityBase',
    required: true,
  },
  {
    field: 'position',
    label: '存放位置',
    component: 'Select',
    componentProps: {
      options: [
        { label: 'Admin.NET.Application', value: 'Admin.NET.Application' },
        { label: 'Admin.NET.Core', value: 'Admin.NET.Core' },
      ],
    },
    defaultValue: 'Admin.NET.Application',
    required: true,
  },
];

export const columnShowColumns: BasicColumn[] = [
  {
    title: '字段名',
    dataIndex: 'dbColumnName',
  },
  {
    title: '描述',
    dataIndex: 'columnDescription',
  },
  {
    title: '主键',
    dataIndex: 'isPrimarykey',
    customRender: ({ record }) => {
      return h(Switch, {
        checkedChildren: '是',
        unCheckedChildren: '否',
        checked: record.isPrimarykey,
      });
    },
  },
  {
    title: '自增',
    dataIndex: 'isIdentity',
    customRender: ({ record }) => {
      return h(Switch, {
        checkedChildren: '是',
        unCheckedChildren: '否',
        checked: record.isIdentity,
      });
    },
  },
  {
    title: '可空',
    dataIndex: 'isNullable',
    customRender: ({ record }) => {
      return h(Switch, {
        checkedChildren: '是',
        unCheckedChildren: '否',
        checked: record.isNullable,
      });
    },
  },
  {
    title: '类型',
    dataIndex: 'dataType',
  },
];
export const editColumn: BasicColumn[] = [
  {
    dataIndex: 'dbColumnName',
    title: '字段名',
    editComponent: 'Input',
    edit: true,
    editRule: false,
  },
  {
    dataIndex: 'columnDescription',
    title: '描述',
    editComponent: 'Input',
    edit: true,
    editRule: false,
  },
  {
    dataIndex: 'isPrimarykey',
    title: '主键',
    editComponent: 'Select',
    width: 100,
    edit: true,
    editRule: false,
    editComponentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
  },
  {
    dataIndex: 'isIdentity',
    title: '自增',
    editComponent: 'Select',
    width: 100,
    edit: true,
    editRule: false,
    editComponentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
  },
  {
    dataIndex: 'dataType',
    title: '类型',
    editComponent: 'ApiSelect',
    edit: true,
    editRule: false,
    editComponentProps: {
      api: apiTypeSelect,
      labelField: 'value',
      valueField: 'value',
    },
  },
  {
    dataIndex: 'isNullable',
    title: '可空',
    editComponent: 'Select',
    width: 100,
    edit: true,
    editRule: false,
    editComponentProps: {
      options: [
        { label: '是', value: 1 },
        { label: '否', value: 0 },
      ],
    },
  },
  {
    dataIndex: 'length',
    title: '长度',
    editComponent: 'InputNumber',
    edit: true,
    editRule: false,
  },
  {
    dataIndex: 'decimalDigits',
    title: '保留几位小数',
    editComponent: 'InputNumber',
    edit: true,
    editRule: false,
  },
];
