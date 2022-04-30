import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Switch, Tag } from 'ant-design-vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { formatToDate } from '/@/utils/dateUtil';
import { usePermission } from '/@/hooks/web/usePermission';

import { setUserStatus, getOrgList, getPosList } from '/@/api/sys/admin';

const { hasPermission } = usePermission();

export const columns: BasicColumn[] = [
  {
    title: '用户名',
    dataIndex: 'userName',
    width: 120,
    fixed: 'left',
  },
  {
    title: '昵称',
    dataIndex: 'nickName',
    width: 100,
  },
  {
    title: '头像',
    dataIndex: 'avatar',
    width: 100,
    slots: { customRender: 'avatar' },
  },
  {
    title: '出生日期',
    dataIndex: 'birthday',
    width: 100,
    customRender: ({ record }) => {
      return formatToDate(record.birthday);
    },
  },
  {
    title: '性别',
    dataIndex: 'sex',
    width: 50,
    customRender: ({ record }) => {
      const sex = record.sex;
      switch (sex) {
        case 1:
          return h(Tag, { color: 'blue' }, () => '男');
        case 2:
          return h(Tag, { color: 'red' }, () => '女');
        default:
          return h(Tag, { color: 'orange' }, () => '未知');
      }
    },
  },
  {
    title: '邮箱',
    dataIndex: 'email',
    width: 100,
  },
  {
    title: '手机号码',
    dataIndex: 'phone',
    width: 100,
  },
  {
    title: '真实姓名',
    dataIndex: 'realName',
    width: 100,
  },
  {
    title: '证件号码',
    dataIndex: 'idCard',
    width: 100,
  },
  {
    title: '状态',
    dataIndex: 'status',
    width: 100,
    customRender: ({ record }) => {
      if (!Reflect.has(record, 'pendingStatus')) {
        record.pendingStatus = false;
      }
      return h(Switch, {
        checked: record.status === 1,
        checkedChildren: '已启用',
        unCheckedChildren: '已禁用',
        disabled: !hasPermission('sysUser:setStatus'),
        loading: record.pendingStatus,
        onChange(checked: boolean) {
          record.pendingStatus = true;
          const newStatus = checked ? 1 : 2;
          const { createMessage } = useMessage();
          setUserStatus(record.id, newStatus)
            .then(() => {
              record.status = newStatus;
              createMessage.success(`已成功修改账号状态`);
            })
            .catch(() => {
              createMessage.error('修改账号状态失败');
            })
            .finally(() => {
              record.pendingStatus = false;
            });
        },
      });
    },
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
    field: 'userName',
    label: '用户名称',
    component: 'Input',
    colProps: { span: 8 },
  },
  {
    field: 'phone',
    label: '手机号码',
    component: 'Input',
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    field: 'userName',
    label: '用户名',
    component: 'Input',
    required: true,
    colProps: { span: 12 },
  },
  {
    field: 'nickName',
    label: '昵称',
    component: 'Input',
    colProps: { span: 12 },
  },
  {
    field: 'realName',
    label: '真实姓名',
    component: 'Input',
    required: true,
    colProps: { span: 12 },
  },
  {
    field: 'birthday',
    label: '出生日期',
    component: 'DatePicker',
    colProps: { span: 12 },
    componentProps: { style: { width: '100%' } },
  },
  {
    field: 'sex',
    label: '性别',
    component: 'RadioGroup',
    colProps: { span: 12 },
    componentProps: {
      options: [
        {
          label: '男',
          value: 1,
        },
        {
          label: '女',
          value: 2,
        },
      ],
    },
  },
  {
    field: 'phone',
    label: '手机号码',
    component: 'Input',
    colProps: { span: 12 },
    required: true,
  },
  {
    field: 'email',
    label: '邮箱',
    component: 'Input',
    colProps: { span: 12 },
  },
  {
    field: 'idCard',
    label: '证件号码',
    component: 'Input',
    colProps: { span: 12 },
  },
  {
    field: 'signature',
    label: '个性签名',
    component: 'Input',
    colProps: { span: 24 },
  },
  {
    field: 'introduction',
    label: '本人简介',
    component: 'Input',
    colProps: { span: 24 },
  },
  {
    field: 'orgId',
    label: '所属机构',
    component: 'ApiTreeSelect',
    componentProps: {
      api: getOrgList,
      fieldNames: {
        label: 'name',
        key: 'id',
        value: 'id',
      },
    },
    required: true,
    colProps: { span: 12 },
  },
  {
    field: 'posId',
    label: '职位',
    component: 'ApiSelect',
    componentProps: {
      api: getPosList,
      labelField: 'name',
      valueField: 'id',
    },
    colProps: { span: 12 },
  },
  {
    field: 'jobNum',
    label: '工号',
    component: 'Input',
    colProps: { span: 12 },
  },
  {
    field: 'jobStatus',
    label: '岗位状态',
    component: 'Select',
    colProps: { span: 12 },
    componentProps: {
      options: [
        {
          label: '在职',
          value: 1,
          key: 1,
        },
        {
          label: '离职',
          value: 2,
          key: 2,
        },
        {
          label: '请假',
          value: 3,
          key: 3,
        },
      ],
    },
  },
  {
    field: 'remark',
    label: '备注',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
];
