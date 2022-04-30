import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';

export const columns: BasicColumn[] = [
  {
    title: '账号名称',
    dataIndex: 'userName',
    width: 100,
    align: 'left',
  },
  {
    title: '是否成功',
    dataIndex: 'success',
    width: 80,
    customRender: ({ record }) => {
      const success = record.success;
      const enable = ~~success === 1;
      const color = enable ? 'green' : 'red';
      const text = enable ? '成功' : '失败';
      return h(Tag, { color: color }, () => text);
    },
  },
  {
    title: 'IP地址',
    dataIndex: 'ip',
    width: 100,
  },
  {
    title: '地址',
    dataIndex: 'location',
    width: 100,
  },
  {
    title: '浏览器',
    dataIndex: 'browser',
    width: 100,
  },
  {
    title: '操作系统',
    dataIndex: 'os',
    width: 100,
  },
  {
    title: '请求地址',
    dataIndex: 'url',
    width: 200,
  },
  {
    title: '类名',
    dataIndex: 'className',
    width: 200,
  },
  {
    title: '方法名',
    dataIndex: 'methodName',
    width: 200,
  },
  {
    title: '请求方式',
    dataIndex: 'reqMethod',
    width: 80,
  },
  {
    title: '请求参数',
    dataIndex: 'param',
    width: 200,
  },
  {
    title: '返回结果',
    dataIndex: 'result',
    width: 300,
  },
  {
    title: '耗时',
    dataIndex: 'elapsedTime',
    width: 50,
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
