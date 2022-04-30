import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { getMenuList, getDictDataDropdown, getTableList, getColumnList } from '/@/api/sys/admin';
const apiTableList = async (param: any) => {
  const result = await getTableList(param);
  return result;
};
let currentTable = '';
let columnList: any[] = [];
const apiColumnList = async (param: any) => {
  if (typeof param !== 'string') return [];
  if (columnList.length === 0 || currentTable !== param) {
    const result = await getColumnList(param);
    columnList = result;
  } else {
  }
  currentTable = param;
  return columnList;
};
const apiDictTypeDropDown = async () => {
  const result = await getDictDataDropdown('code_gen_create_type');
  return result;
};
export const codeShowColumns: BasicColumn[] = [
  {
    title: '表名称',
    dataIndex: 'tableName',
  },
  {
    title: '业务名',
    dataIndex: 'busName',
  },
  {
    title: '命名空间',
    dataIndex: 'nameSpace',
  },
  {
    title: '作者姓名',
    dataIndex: 'authorName',
  },
  {
    title: '生成方式',
    dataIndex: 'generateType',
    slots: { customRender: 'generateType' },
  },
];
export const codeFormSchema: FormSchema[] = [
  {
    field: 'id',
    label: 'Id',
    component: 'Input',
    show: false,
  },
  {
    field: 'tableName',
    label: '生成表',
    component: 'ApiSelect',
    componentProps: {
      api: apiTableList,
      fieldNames: {
        label: 'tableName',
        value: 'entityName',
      },
    },
  },
  {
    field: 'busName',
    label: '业务名',
    component: 'Input',
    required: true,
  },
  {
    field: 'menuPid',
    label: '父级菜单',
    component: 'ApiTreeSelect',
    componentProps: {
      api: getMenuList,
      fieldNames: {
        title: 'title',
        key: 'id',
        value: 'id',
      },
      getPopupContainer: () => document.body,
    },
  },
  {
    field: 'nameSpace',
    label: '命名空间',
    component: 'Input',
    required: true,
    defaultValue: 'Admin.NET.Application',
  },
  {
    field: 'authorName',
    label: '作者姓名',
    component: 'Input',
    required: true,
    defaultValue: 'one',
  },
  {
    field: 'generateType',
    label: '生成方式',
    component: 'ApiSelect',
    componentProps: {
      api: apiDictTypeDropDown,
      fieldNames: {
        label: 'label',
        value: 'value',
      },
    },
    defaultValue: '2',
    required: true,
  },
];
// 表头
export const columns = [
  {
    title: '字段',
    dataIndex: 'columnName',
    align: 'center',
  },
  {
    title: '描述',
    dataIndex: 'columnComment',
    align: 'center',
    width: 150,
    slots: {
      customRender: 'columnComment',
    },
  },
  {
    title: '类型',
    dataIndex: 'netType',
    align: 'center',
  },
  // {
  //   title: 'java类型',
  //   dataIndex: 'javaType',
  //   slots: { customRender: 'javaType' }
  // },
  {
    title: '作用类型',
    dataIndex: 'effectType',
    align: 'center',
    slots: {
      customRender: 'effectType',
    },
  },
  {
    title: '字典',
    dataIndex: 'dictTypeCode',
    align: 'center',
    slots: {
      customRender: 'dictTypeCode',
    },
  },
  {
    title: '列表显示',
    align: 'center',
    dataIndex: 'whetherTable',
    slots: {
      customRender: 'whetherTable',
    },
  },
  {
    title: '增改',
    align: 'center',
    dataIndex: 'whetherAddUpdate',
    slots: {
      customRender: 'whetherAddUpdate',
    },
  },
  {
    title: '必填',
    align: 'center',
    dataIndex: 'whetherRequired',
    slots: {
      customRender: 'whetherRequired',
    },
  },
  {
    title: '是否是查询',
    align: 'center',
    dataIndex: 'queryWhether',
    slots: {
      customRender: 'queryWhether',
    },
  },
  {
    title: '查询方式',
    dataIndex: 'queryType',
    align: 'center',
    slots: {
      customRender: 'queryType',
    },
  },
];
export const fkFormSchema: FormSchema[] = [
  {
    field: 'tableName',
    label: '数据库表',
    component: 'ApiSelect',
    componentProps: ({ formModel, formActionType }) => {
      return {
        api: apiTableList,
        fieldNames: {
          label: 'tableName',
          value: 'tableName',
        },
        onChange: (e: any, option: any) => {
          formModel.columnName = undefined;
          formModel.entityName = option.entityName;
          const { updateSchema } = formActionType;
          updateSchema({
            field: 'columnName',
            componentProps: {
              api: apiColumnList,
              immediate: false,
              fieldNames: {
                label: 'columnName',
                value: 'columnName',
              },
              params: e,
              onChange: (e: any, option: any) => {
                formModel.columnNetType = option.netType;
              },
            },
          });
        },
      };
    },
  },
  {
    field: 'columnName',
    label: '显示字段',
    component: 'ApiSelect',
  },
  {
    field: 'columnNetType',
    label: '字段类型',
    component: 'Input',
    show: false,
  },
  {
    field: 'entityName',
    label: '实体名称',
    component: 'Input',
    show: false,
  },
];

export const treeFormSchema: FormSchema[] = [
  {
    field: 'tableName',
    label: '数据库表',
    component: 'ApiSelect',
    componentProps: ({ formModel, formActionType }) => {
      return {
        api: apiTableList,
        fieldNames: {
          label: 'tableName',
          value: 'tableName',
        },
        onChange: (e: any, option: any) => {
          formModel.columnName = undefined;
          formModel.entityName = option.entityName;
          const { updateSchema } = formActionType;
          updateSchema([
            {
              field: 'displayColumn',
              componentProps: {
                api: apiColumnList,
                immediate: false,
                params: e,
                fieldNames: {
                  label: 'columnName',
                  value: 'columnName',
                },
              },
            },
            {
              field: 'valueColumn',
              componentProps: {
                api: apiColumnList,
                immediate: false,
                params: e,
                fieldNames: {
                  label: 'columnName',
                  value: 'columnName',
                },
              },
            },
            {
              field: 'pidColumn',
              componentProps: {
                api: apiColumnList,
                immediate: false,
                params: e,
                fieldNames: {
                  label: 'columnName',
                  value: 'columnName',
                },
              },
            },
          ]);
        },
      };
    },
  },
  {
    field: 'displayColumn',
    label: '显示文本字段',
    component: 'ApiSelect',
  },
  {
    field: 'valueColumn',
    label: '选择值字段',
    component: 'ApiSelect',
  },
  {
    field: 'pidColumn',
    label: '父级字段',
    component: 'ApiSelect',
  },
  {
    field: 'entityName',
    label: '实体名称',
    component: 'Input',
    show: false,
  },
];
