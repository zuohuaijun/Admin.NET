<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    showFooter
    :title="getTitle"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>
<script lang="ts">
  import { defineComponent, ref, unref, computed } from 'vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { columnFormSchema } from './database.data';
  import { addColumn, updateColumn } from '/@/api/sys/admin';

  export default defineComponent({
    components: { BasicModal, BasicForm },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const isUpdate = ref(true);
      const [registerForm, { resetFields, setFieldsValue, updateSchema, validate }] = useForm({
        labelWidth: 120,
        schemas: columnFormSchema,
        showActionButtonGroup: false,
      });

      const [registerModal, { setModalProps, closeModal }] = useModalInner(async (data) => {
        resetFields();
        setModalProps({ confirmLoading: false });
        isUpdate.value = !!data?.isUpdate;
        setFieldsValue({
          tableName: data.tableName,
        });
        if (unref(isUpdate)) {
          updateSchema([
            {
              field: 'oldName',
              ifShow: true,
            },
            {
              field: 'isPrimarykey',
              ifShow: false,
            },
            {
              field: 'isIdentity',
              ifShow: false,
            },
            {
              field: 'dataType',
              ifShow: false,
            },
            {
              field: 'isNullable',
              ifShow: false,
            },
            {
              field: 'length',
              ifShow: false,
            },
            {
              field: 'decimalDigits',
              ifShow: false,
            },
          ]);
          setFieldsValue({
            oldName: data.record.dbColumnName,
            ...data.record,
          });
        }
      });

      const getTitle = computed(() => (!unref(isUpdate) ? '新增列' : '编辑列'));

      async function handleSubmit() {
        try {
          const values = await validate();
          setModalProps({ confirmLoading: true });
          if (!unref(isUpdate)) {
            await addColumn(values);
          } else {
            await updateColumn(values);
          }
          closeModal();
          emit('success');
        } finally {
          setModalProps({ confirmLoading: false });
        }
      }

      return {
        getTitle,
        registerModal,
        registerForm,
        handleSubmit,
      };
    },
  });
</script>
