import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';

export const columns: BasicColumn[] = [
  {
    title: '账号名称',
    dataIndex: 'userName',
    width: 100,
    align: 'left',
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
    title: '异常名称',
    dataIndex: 'exceptionName',
    width: 200,
  },
  {
    title: '异常信息',
    dataIndex: 'exceptionMsg',
    width: 200,
  },
  {
    title: '异常源',
    dataIndex: 'exceptionSource',
    width: 200,
  },
  {
    title: '堆栈信息',
    dataIndex: 'stackTrace',
    width: 200,
  },
  {
    title: '参数对象',
    dataIndex: 'paramsObj',
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
