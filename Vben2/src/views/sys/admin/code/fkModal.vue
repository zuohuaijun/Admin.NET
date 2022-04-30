<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="外键配置" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>
<script lang="ts">
  import { defineComponent, ref, computed, unref } from 'vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { fkFormSchema } from './codeGenerate.data';

  export default defineComponent({
    components: { BasicModal, BasicForm },
    emits: ['success'],
    setup(_, { emit }) {
      var row: any = {};
      const [registerForm, { resetFields, validate, updateSchema }] = useForm({
        labelWidth: 100,
        schemas: fkFormSchema,
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
        row.fkColumnName = values.columnName;
        row.fkColumnNetType = values.columnNetType;
        emit('success', row);
        setModalProps({ confirmLoading: false });
        closeModal();
      }
      return { registerModal, registerForm, handleSubmit };
    },
  });
</script>
