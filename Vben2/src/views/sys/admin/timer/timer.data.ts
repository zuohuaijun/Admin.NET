import { BasicColumn } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Switch, Tag } from 'ant-design-vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { usePermission } from '/@/hooks/web/usePermission';

import { setTimerStatus } from '/@/api/sys/admin';

const { hasPermission } = usePermission();

export const columns: BasicColumn[] = [
  {
    title: '任务名称',
    dataIndex: 'timerName',
    width: 150,
    fixed: 'left',
  },
  {
    title: '请求地址',
    dataIndex: 'requestUrl',
    width: 200,
  },
  {
    title: '请求类型',
    dataIndex: 'requestType',
    width: 100,
    customRender: ({ record }) => {
      const requestType = record.requestType;
      switch (requestType) {
        case 0:
          return h(Tag, { color: 'orange' }, () => 'RUN');
        case 1:
          return h(Tag, { color: 'orange' }, () => 'GET');
        case 2:
          return h(Tag, { color: 'orange' }, () => 'POST');
        case 3:
          return h(Tag, { color: 'orange' }, () => 'PUT');
        case 4:
          return h(Tag, { color: 'orange' }, () => 'DELETE');
      }
      return requestType;
    },
  },
  {
    title: '任务类型',
    dataIndex: 'timerType',
    width: 100,
    customRender: ({ record }) => {
      return record.timerType == 0 ? record.interval + 's' : record.cron;
    },
  },
  {
    title: '执行类型',
    dataIndex: 'executeType',
    width: 100,
    customRender: ({ record }) => {
      return record.executeType == 0 ? '并行' : '串行';
    },
  },
  {
    title: '执行一次',
    dataIndex: 'doOnce',
    width: 100,
    customRender: ({ record }) => {
      return record.doOnce ? '是' : '否';
    },
  },
  {
    title: '立即执行',
    dataIndex: 'startNow',
    width: 100,
    customRender: ({ record }) => {
      return record.startNow ? '是' : '否';
    },
  },
  {
    title: '状态',
    dataIndex: 'status',
    width: 120,
    customRender: ({ record }) => {
      if (!Reflect.has(record, 'pendingStatus')) {
        record.pendingStatus = false;
      }
      return h(Switch, {
        checked: record.status === 0,
        checkedChildren: '运行中',
        unCheckedChildren: '已停止',
        disabled: !hasPermission('sysTimer:setStatus'),
        loading: record.pendingStatus,
        onChange(checked: boolean) {
          record.pendingStatus = true;
          const newStatus = checked ? 0 : 1;
          const { createMessage } = useMessage();
          setTimerStatus(record.timerName, newStatus)
            .then(() => {
              record.status = newStatus;
              createMessage.success(`已成功修改任务状态`);
            })
            .catch(() => {
              createMessage.error('修改任务状态失败');
            })
            .finally(() => {
              record.pendingStatus = false;
            });
        },
      });
    },
  },
  {
    title: '执行次数',
    dataIndex: 'tally',
    width: 100,
    customRender: ({ record }) => {
      return h(Tag, { color: 'red' }, () => record.tally);
    },
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    width: 180,
  },
  {
    title: '备注',
    dataIndex: 'remark',
  },
];

export const searchFormSchema: FormSchema[] = [
  {
    field: 'timerName',
    label: '任务名称',
    component: 'Input',
    colProps: { span: 8 },
  },
];

export const formSchema: FormSchema[] = [
  {
    field: 'timerName',
    label: '任务名称',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    field: 'requestUrl',
    label: '请求地址',
    component: 'Input',
    required: true,
    colProps: { span: 24 },
  },
  {
    field: 'requestType',
    label: '请求类型',
    component: 'RadioButtonGroup',
    defaultValue: 2,
    componentProps: {
      options: [
        { label: 'RUN', value: 0 },
        { label: 'GET', value: 1 },
        { label: 'POST', value: 2 },
        { label: 'PUT', value: 3 },
        { label: 'DELETE', value: 4 },
      ],
    },
    required: true,
  },
  {
    label: '请求参数',
    field: 'requestPara',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
  {
    field: 'timerType',
    label: '任务类型',
    component: 'Select',
    defaultValue: 0,
    componentProps: {
      options: [
        { label: '间隔模式', value: 0 },
        { label: 'Cron模式', value: 1 },
      ],
      style: { width: '100%' },
    },
    required: true,
    colProps: { span: 12 },
  },
  {
    field: 'interval',
    label: '执行间隔',
    component: 'InputNumber',
    defaultValue: 5,
    required: true,
    colProps: { span: 12 },
    componentProps: { style: { width: '100%' } },
    ifShow: ({ values }) => values.timerType == 0,
  },
  {
    field: 'cron',
    label: 'Cron表达式',
    component: 'Input',
    required: true,
    colProps: { span: 12 },
    componentProps: { style: { width: '100%' } },
    ifShow: ({ values }) => values.timerType == 1,
  },
  {
    label: '备注',
    field: 'remark',
    component: 'InputTextArea',
    colProps: { span: 24 },
  },
];
