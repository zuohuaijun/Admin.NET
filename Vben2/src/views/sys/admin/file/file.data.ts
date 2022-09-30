import { BasicColumn, TableImg } from '/@/components/Table';
import { FormSchema } from '/@/components/Table';
import { h } from 'vue';
import { Tag } from 'ant-design-vue';
import { useGlobSetting } from '/@/hooks/setting';

const { uploadUrl = '' } = useGlobSetting();
export const columns: BasicColumn[] = [
  {
    title: '提供者',
    dataIndex: 'provider',
    width: 100,
    align: 'left',
  },
  {
    title: '仓储名称',
    dataIndex: 'bucketName',
    width: 100,
    align: 'left',
  },
  {
    title: '文件名称',
    dataIndex: 'fileName',
    width: 200,
  },
  {
    title: '文件后缀',
    dataIndex: 'suffix',
    width: 80,
    customRender: ({ record }) => {
      return h(Tag, { color: 'blue' }, () => record.suffix);
    },
  },
  {
    title: '预览',
    width: 120,
    customRender: ({ record }) => {
      if (
        record.suffix.indexOf('png') > 0 ||
        record.suffix.indexOf('jpg') > 0 ||
        record.suffix.indexOf('bmp') > 0
      ) {
        const filePath = record.url
          ? record.url
          : uploadUrl + '/' + record.filePath + '/' + record.id + record.suffix;
        return h(TableImg, {
          size: 60,
          simpleShow: true,
          imgList: [filePath],
        });
      } else return h(Tag, { color: 'orange' }, () => '无法预览');
    },
  },
  {
    title: '大小KB',
    dataIndex: 'sizeKb',
    width: 100,
  },
  {
    title: '存储标识',
    dataIndex: 'id',
    width: 120,
  },
  {
    title: '创建时间',
    dataIndex: 'createTime',
    width: 180,
  },
];

export const searchFormSchema: FormSchema[] = [
  // {
  //   field: 'fileName',
  //   label: '文件名称',
  //   component: 'Input',
  //   colProps: { span: 8 },
  // },
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
