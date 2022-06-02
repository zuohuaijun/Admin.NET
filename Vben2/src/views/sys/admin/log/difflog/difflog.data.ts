import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';

export const columns: BasicColumn[] = [
  {
    title: '差异操作',
    dataIndex: 'diffType',
    width: 100,
    align: 'left',
    slots: { customRender: 'diffType' },
  },
  {
    title: 'SQL语句',
    dataIndex: 'sql',
    width: 200,
    slots: { customRender: 'sql' },
  },
  {
    title: '耗时(毫秒)',
    dataIndex: 'duration',
    width: 200,
    slots: { customRender: 'duration' },
  },
  {
    title: '变更前数据',
    dataIndex: 'beforeData',
    width: 200,
    slots: { customRender: 'beforeData' },
  },
  {
    title: '变更后数据',
    dataIndex: 'afterData',
    width: 200,
    slots: { customRender: 'afterData' },
  },
  {
    title: '业务对象',
    dataIndex: 'businessData',
    width: 200,
    slots: { customRender: 'businessData' },
  },
  {
    title: '操作时间',
    dataIndex: 'createTime',
    width: 180,
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
