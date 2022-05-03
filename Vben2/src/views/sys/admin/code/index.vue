<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleCreate">新增</a-button>
      </template>
      <template #generateType="{ text }">
        {{ convertCodeGenType(text) }}
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              label: '开始生成',
              onClick: handleGenerate.bind(null, record),
            },
            {
              label: '配置',
              onClick: handleConfig.bind(null, record),
            },
            {
              label: '编辑',
              onClick: handleEdit.bind(null, record),
            },
            {
              label: '删除',
              popConfirm: {
                confirm: handleDelete.bind(null, record),
                title: '确认删除？',
              },
            },
          ]"
        />
      </template>
    </BasicTable>
    <CodeGenerateModal @register="registerCodeGenModal" @success="handleSuccess" />
    <GenerateConfigModal
      @register="registerCodeGenConfigModal"
      width="80%"
      @success="handleSuccess"
    />
  </div>
</template>
<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { codeShowColumns } from './codeGenerate.data';
  import { useModal } from '/@/components/Modal';
  import {
    getGeneratePage,
    getDictDataDropdown,
    deleGenerate,
    generateRunLocal,
  } from '/@/api/sys/admin';
  import CodeGenerateModal from './CodeGenerateModal.vue';
  import GenerateConfigModal from './GenerateConfigModal.vue';
  import { useMessage } from '/@/hooks/web/useMessage';

  export default defineComponent({
    components: {
      BasicTable,
      TableAction,
      CodeGenerateModal,
      GenerateConfigModal,
    },
    setup() {
      const { createMessage } = useMessage();
      var codeGenType = ref<any>([]);
      onMounted(async () => {
        codeGenType.value = await getDictDataDropdown('code_gen_create_type');
        reload();
      });
      function convertCodeGenType(code: number) {
        let data = codeGenType.value.filter((c) => c.value == code)[0].label;
        return data;
      }
      const [registerCodeGenModal, { openModal: openCodeGenModal }] = useModal();
      const [registerCodeGenConfigModal, { openModal: openCodeGenConfigModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        api: getGeneratePage,
        rowKey: 'id',
        showIndexColumn: false,
        pagination: true,
        columns: codeShowColumns,
        immediate: false,
        actionColumn: {
          width: 250,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleCreate() {
        openCodeGenModal(true, {
          isUpdate: false,
        });
      }

      function handleEdit(record: Recordable) {
        openCodeGenModal(true, {
          record,
          isUpdate: true,
        });
      }

      function handleConfig(record: Recordable) {
        openCodeGenConfigModal(true, { id: record.id });
      }

      async function handleGenerate(record: Recordable) {
        await generateRunLocal(record);
        createMessage.success('生成成功！');
        reload();
      }

      async function handleDelete(record: Recordable) {
        await deleGenerate([{ id: record.id }]);
        createMessage.success('删除成功');
        reload();
      }

      function handleSuccess() {
        reload();
      }

      return {
        registerTable,
        registerCodeGenModal,
        registerCodeGenConfigModal,
        convertCodeGenType,
        handleCreate,
        handleEdit,
        handleConfig,
        handleGenerate,
        handleDelete,
        handleSuccess,
        codeGenType,
      };
    },
  });
</script>
