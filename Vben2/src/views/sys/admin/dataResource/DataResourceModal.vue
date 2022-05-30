<!--
 * @Author: kenny 362270511@qq.com
 * @Date: 2022-05-30 11:36:19
 * @LastEditors: kenny 362270511@qq.com
 * @LastEditTime: 2022-05-30 16:00:49
 * @FilePath: \frontend\src\views\sys\admin\dataResource\org\DataResourceModal.vue
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
-->
<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="getTitle" @ok="handleSubmit">
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>
<script lang="ts">
  import { defineComponent, ref, computed, unref } from 'vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form/index';

  import { formSchema } from './dataResource.data';
  import { getDataResourceList, addDataResource, updateDataResource } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'DataResourceModal',
    components: { BasicModal, BasicForm },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const isUpdate = ref(true);
      let rowId: number;

      const [registerForm, { resetFields, setFieldsValue, updateSchema, validate }] = useForm({
        labelWidth: 100,
        schemas: formSchema,
        showActionButtonGroup: false,
      });

      const [registerModal, { setModalProps, closeModal }] = useModalInner(async (data) => {
        resetFields();
        setModalProps({ confirmLoading: false });
        isUpdate.value = !!data?.isUpdate;

        const treeData = await getDataResourceList({ id: data.parentId || 0 });
        debugger;
        if (!data.parentId) {
          treeData.push({
            id: 0,
            name: '根节点',
            pid: 0,
            remark: '根节点',
            children: [],
          });
        }
        updateSchema({
          field: 'pid',
          componentProps: { treeData },
        });

        if (unref(isUpdate)) {
          rowId = data.record.id;
          setFieldsValue({
            ...data.record,
          });
        } else {
          setFieldsValue({ pid: data.parentId });
        }
      });

      const getTitle = computed(() => (!unref(isUpdate) ? '新增数据资源' : '编辑数据资源'));

      async function handleSubmit() {
        try {
          const values = await validate();
          setModalProps({ confirmLoading: true });

          if (unref(isUpdate)) {
            values.id = rowId;
            await updateDataResource(values);
          } else {
            rowId = await addDataResource(values);
          }

          closeModal();
          emit('success', {
            isUpdate: unref(isUpdate),
            values: { ...values, id: rowId },
          });
        } finally {
          setModalProps({ confirmLoading: false });
        }
      }

      return { registerModal, registerForm, getTitle, handleSubmit };
    },
  });
</script>
