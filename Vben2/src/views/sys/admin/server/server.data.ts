import { h } from 'vue';
import { Tag } from 'ant-design-vue';
import { DescItem } from '/@/components/Description/index';

const commonTagRender = (color: string) => (curVal) => h(Tag, { color }, () => curVal);

export const BaseSchema: DescItem[] = [
  {
    field: 'hostName',
    label: '主机名称',
  },
  {
    field: 'systemOs',
    label: '操作系统',
  },
  {
    field: 'osArchitecture',
    label: '系统架构',
  },
  {
    field: 'processorCount',
    label: 'CPU核心',
  },
  {
    field: 'frameworkDescription',
    label: '技术框架',
    render: commonTagRender('blue'),
  },
];

export const NetWorkSchema: DescItem[] = [
  {
    field: 'wanIp',
    label: '外网信息',
    contentMinWidth: 200,
  },
  {
    field: 'localIp',
    label: '内网地址',
  },
  {
    field: '',
    label: '网卡MAC',
  },
  {
    field: '',
    label: '流量统计',
  },
  {
    field: '',
    label: '网络速度',
  },
];

export const UseSchema: DescItem[] = [
  {
    field: 'runTime',
    label: '运行时间',
  },
  {
    field: 'cpuRate',
    label: 'CPU使用率',
    render: commonTagRender('orange'),
  },
  {
    field: 'totalRam',
    label: '内存合计',
  },
  {
    field: 'ramRate',
    label: '内存使用率',
    render: commonTagRender('orange'),
  },
];
