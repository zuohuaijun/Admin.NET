<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <BasicUpload
          :maxSize="20"
          :maxNumber="10"
          @change="handleChange"
          :api="uploadFile"
          class="my-5"
          :accept="['doc', 'docx', 'xls', 'xlsx', 'image/*']"
          :showPreviewNumber="false"
          :emptyHidePreview="true"
          :hidden="!hasPermission('sysFile:upload')"
        />
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              icon: 'ant-design:cloud-download-outlined',
              label: '下载',
              disabled: !hasPermission('sysFile:download'),
              onClick: handleDownload.bind(null, record),
            },
            {
              icon: 'ant-design:delete-outlined',
              label: '删除',
              color: 'error',
              ifShow: hasPermission('sysFile:delete'),
              popConfirm: {
                title: '是否确认删除',
                confirm: handleDelete.bind(null, record),
              },
            },
          ]"
        />
      </template>
    </BasicTable>
  </div>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { BasicUpload } from '/@/components/Upload';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { downloadByUrl } from '/@/utils/file/download';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useGlobSetting } from '/@/hooks/setting';

  import { columns, searchFormSchema } from './file.data';
  import { getFilePageList, uploadFile, deleteFile } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'FileManagement',
    components: { BasicTable, BasicUpload, TableAction },
    setup() {
      const { hasPermission } = usePermission();
      const { createMessage } = useMessage();
      const { uploadUrl = '' } = useGlobSetting();
      const [registerTable, { reload, deleteTableDataRecord }] = useTable({
        title: '文件列表',
        api: getFilePageList,
        columns,
        formConfig: {
          labelWidth: 120,
          schemas: searchFormSchema,
        },
        rowKey: 'id',
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: true,
        pagination: {
          pageSize: 10,
        },
        actionColumn: {
          width: 150,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
          fixed: undefined,
        },
      });

      async function handleDownload(record: Recordable) {
        const filePath = record.url
          ? record.url
          : uploadUrl + '/' + record.filePath + '/' + record.id + record.suffix;
        downloadByUrl({ url: filePath });
      }

      async function handleDelete(record: Recordable) {
        await deleteFile(record.id);
        deleteTableDataRecord(record.id);
      }

      return {
        registerTable,
        handleDownload,
        handleDelete,
        handleChange: (fileList) => {
          reload();
          createMessage.info(`已上传文件${JSON.stringify(fileList)}`);
        },
        uploadFile,
        hasPermission,
      };
    },
  });
</script>
