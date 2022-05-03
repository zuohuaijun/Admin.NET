<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="树选择配置" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>
<script lang="ts">
  import { defineComponent } from 'vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { treeFormSchema } from './codeGenerate.data';

  export default defineComponent({
    components: { BasicModal, BasicForm },
    emits: ['success'],
    setup(_, { emit }) {
      var row: any = {};
      const [registerForm, { resetFields, validate }] = useForm({
        labelWidth: 100,
        schemas: treeFormSchema,
        showActionButtonGroup: false,
        actionColOptions: {
          span: 23,
        },
      });
      const [registerModal, { setModalProps, closeModal }] = useModalInner(async (data) => {
        resetFields();
        row = data.data;
        setModalProps({ confirmLoading: false });
      });

      async function handleSubmit() {
        const values = await validate();
        row.fkTableName = values.tableName;
        row.fkEntityName = values.entityName;
        row.displayColumn = values.displayColumn;
        row.valueColumn = values.valueColumn;
        row.pidColumn = values.pidColumn;
        emit('success', row);
        setModalProps({ confirmLoading: false });
        closeModal();
      }

      return { registerModal, registerForm, handleSubmit };
    },
  });
</script>
