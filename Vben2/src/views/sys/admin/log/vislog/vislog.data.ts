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
  // {
  //   title: '消息内容',
  //   dataIndex: 'message',
  //   width: 100,
  // },
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
    title: '访问类型',
    dataIndex: 'visType',
    width: 100,
    customRender: ({ record }) => {
      switch (record.visType) {
        case 0:
          return '登录';
        case 1:
          return '退出';
        case 2:
          return '注册';
        case 3:
          return '改密';
      }
    },
  },
  {
    title: '访问时间',
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
