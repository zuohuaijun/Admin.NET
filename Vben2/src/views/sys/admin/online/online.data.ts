import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';

export const columns: BasicColumn[] = [
  {
    title: '账号',
    dataIndex: 'account',
    width: 100,
    align: 'left',
  },
  {
    title: '普通用户',
    dataIndex: 'name',
    width: 80,
  },
  {
    title: '登录时间',
    dataIndex: 'lastTime',
    width: 100,
  },
  {
    title: '登录IP',
    dataIndex: 'lastLoginIp',
    width: 100,
  },
  {
    title: '浏览器',
    dataIndex: 'lastLoginBrowser',
    width: 100,
  },
  {
    title: '操作系统',
    dataIndex: 'lastLoginOs',
    width: 100,
  },
];

export const searchFormSchema: FormSchema[] = [
  {
    field: 'startTime',
    label: '开始时间',
    component: 'DatePicker',
    colProps: { span: 8 },
  },
  {
    field: 'endTime',
    label: '结束时间',
    component: 'DatePicker',
    colProps: { span: 8 },
  },
];
