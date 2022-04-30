<template>
  <BasicDrawer
    v-bind="$attrs"
    @register="registerDrawer"
    showFooter
    :title="getTitle"
    width="40%"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicDrawer>
</template>
<script lang="ts">
  import { defineComponent, ref, computed, unref } from 'vue';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';

  import { formSchema } from './config.data';
  import { addConfig, updateConfig } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'ConfigDrawer',
    components: { BasicDrawer, BasicForm },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const isUpdate = ref(true);
      let rowId: number;

      const [registerForm, { resetFields, setFieldsValue, validate, updateSchema }] = useForm({
        labelWidth: 100,
        schemas: formSchema,
        showActionButtonGroup: false,
        //baseColProps: { lg: 12, md: 24 },
      });

      const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner(async (data) => {
        resetFields();
        setDrawerProps({ confirmLoading: false });
        isUpdate.value = !!data?.isUpdate;

        if (unref(isUpdate)) {
          rowId = data.record.id;
          setFieldsValue({
            ...data.record,
          });

          const isEnable: Boolean = data.record.sysFlag == 1 ? true : false;
          updateFormProps(isEnable);
        } else {
          updateFormProps(false);
        }
      });

      function updateFormProps(isEnable: Boolean) {
        updateSchema([
          {
            field: 'sysFlag',
            componentProps: { disabled: isEnable },
          },
          {
            field: 'code',
            componentProps: { disabled: isEnable },
          },
          {
            field: 'groupCode',
            componentProps: { disabled: isEnable },
          },
        ]);
      }

      const getTitle = computed(() => (!unref(isUpdate) ? '新增配置' : '编辑配置'));

      async function handleSubmit() {
        try {
          const values = await validate();
          setDrawerProps({ confirmLoading: true });

          if (unref(isUpdate)) {
            values.id = rowId;
            await updateConfig(values);
          } else {
            await addConfig(values);
          }

          closeDrawer();
          emit('success', {
            isUpdate: unref(isUpdate),
            values: { ...values, id: rowId },
          });
        } finally {
          setDrawerProps({ confirmLoading: false });
        }
      }

      return { registerDrawer, registerForm, getTitle, handleSubmit };
    },
  });
</script>
