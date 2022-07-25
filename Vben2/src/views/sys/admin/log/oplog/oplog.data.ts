import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';

export const columns: BasicColumn[] = [
  {
    title: '日志名称',
    dataIndex: 'logName',
    width: 200,
  },
  {
    title: '日志级别',
    dataIndex: 'logLevel',
    width: 100,
    customRender: ({ record }) => {
      const logLevel = record.logLevel;
      switch (logLevel) {
        case 'Information':
          return h(Tag, { color: 'green' }, () => 'Information');
        case 'Warning':
          return h(Tag, { color: 'orange' }, () => 'Warning');
        case 'Error':
          return h(Tag, { color: 'red' }, () => 'Error');
        default:
          return h(Tag, { color: 'blue' }, () => '未知');
      }
    },
  },
  {
    title: '事件Id',
    dataIndex: 'eventId',
    width: 150,
  },
  {
    title: '日志消息',
    dataIndex: 'message',
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
    //dynamicDisabled: true,
    colProps: { span: 24 },
    componentProps: {
      placeholder: '',
      rows: 200,
    },
  },
];
