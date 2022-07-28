import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';

export const columns: BasicColumn[] = [
  {
    title: '日志名称',
    dataIndex: 'logName',
    width: 100,
    align: 'left',
  },
  {
    title: '日志级别',
    dataIndex: 'logLevel',
    width: 200,
  },
  {
    title: '事件Id',
    dataIndex: 'eventId',
    width: 200,
  },
  {
    title: '日志消息',
    dataIndex: 'message',
    width: 200,
  },
  {
    title: '异常对象',
    dataIndex: 'exception',
    width: 200,
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

export const formSchema: FormSchema[] = [
  {
    label: '',
    field: 'message',
    component: 'InputTextArea',
    // dynamicDisabled: true,
    colProps: { span: 24 },
    componentProps: {
      placeholder: '',
      rows: 200,
      allowClear: false,
    },
  },
];
