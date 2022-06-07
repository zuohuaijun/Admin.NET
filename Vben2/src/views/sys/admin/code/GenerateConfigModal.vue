<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    showFooter
    title="配置"
    @ok="handleSubmit"
    :defaultFullscreen="true"
  >
    <a-table
      ref="table"
      size="middle"
      :columns="columns"
      :dataSource="tbData"
      :pagination="false"
      :alert="true"
      :loading="tbData.length > 0 ? false : true"
      :rowKey="(record) => record.id"
      :scroll="{ y: true }"
    >
      <template #columnComment="{ record }">
        <a-input v-model:value="record.columnComment" />
      </template>
      <template #effectType="{ record }">
        <a-select
          style="width: 150px"
          v-model:value="record.effectType"
          :disabled="judgeColumns(record)"
          :options="effectTypeData"
          :field-names="{ label: 'label', value: 'value' }"
          @change="effectTypeChange(record, $event)"
        />
      </template>
      <template #dictTypeCode="{ record }">
        <a-select
          style="width: 180px"
          v-model:value="record.dictTypeCode"
          :options="dictDataAll"
          :field-names="{ label: 'name', value: 'code' }"
          :disabled="
            record.effectType !== 'Radio' &&
            record.effectType !== 'Select' &&
            record.effectType !== 'Checkbox' &&
            record.effectType !== 'ConstSelector'
          "
        />
      </template>
      <template #whetherTable="{ record }">
        <a-checkbox v-model:checked="record.whetherTable" />
      </template>
      <template #whetherRetract="{ record }">
        <a-checkbox v-model:checked="record.whetherRetract" />
      </template>
      <template #whetherAddUpdate="{ record }">
        <a-checkbox v-model:checked="record.whetherAddUpdate" :disabled="judgeColumns(record)" />
      </template>
      <template #whetherRequired="{ record }">
        <a-checkbox v-model:checked="record.whetherRequired" :disabled="judgeColumns(record)" />
      </template>

      <template #queryWhether="{ record }">
        <a-switch v-model:checked="record.queryWhether">
          <template #checkedChildren><check-outlined /></template>
          <template #unCheckedChildren><close-outlined /></template>
        </a-switch>
      </template>
      <template #queryType="{ record }">
        <a-select
          style="width: 100px"
          v-model:value="record.queryType"
          :options="codeGenQueryTypeData"
          :field-names="{ label: 'label', value: 'value' }"
          :disabled="!record.queryWhether"
        />
      </template>
    </a-table>
  </BasicModal>
  <FkModal @register="registerFkModal" @success="fkHandleSuccess" />
  <TreeModal @register="registerTreeModal" @success="fkHandleSuccess" />
</template>
<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { columns } from './codeGenerate.data';
  import FkModal from './fkModal.vue';
  import TreeModal from './treeModal.vue';
  import { BasicModal, useModalInner, useModal } from '/@/components/Modal';
  import {
    getGenerateConfigList,
    updateGenerateConfig,
    getDictTypeList,
    getDictDataDropdown,
    getAllConstSelector,
  } from '/@/api/sys/admin';

  export default defineComponent({
    components: { BasicModal, FkModal, TreeModal },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const tbData = ref<any[]>([]);
      const effectTypeData = ref<any[]>();
      const dictDataAll = ref<any[]>();
      const codeGenQueryTypeData = ref<any[]>();
      const dictTypeList = ref<any[]>();
      const allConstSelector = ref<any[]>();
      onMounted(async () => {
        // 初始化下拉框数据源
        await loadDictTypeDropDown();
      });
      const [registerFkModal, { openModal: openFkModal }] = useModal();
      const [registerTreeModal, { openModal: openTreeModal }] = useModal();
      const [registerModal, { setModalProps, closeModal }] = useModalInner(async (data) => {
        setModalProps({ confirmLoading: false });
        getGenerateConfigList({ CodeGenId: data.id }).then((res) => {
          var data = res;
          data.forEach((item) => {
            for (const key in item) {
              if (item[key] === 'Y') {
                item[key] = true;
              }
              if (item[key] === 'N') {
                item[key] = false;
              }
            }
          });
          tbData.value = data;
        });
      });

      // 判断是否（用于是否能选择或输入等）
      function judgeColumns(data) {
        if (
          data.columnName.indexOf('createdUserName') > -1 ||
          data.columnName.indexOf('createdTime') > -1 ||
          data.columnName.indexOf('updatedUserName') > -1 ||
          data.columnName.indexOf('updatedTime') > -1 ||
          data.columnKey === 'True'
        ) {
          return true;
        }
        return false;
      }

      // 控件类型改变
      function effectTypeChange(data, value) {
        if (value === 'fk') {
          openFkModal(true, { data });
        } else if (value === 'ApiTreeSelect') {
          openTreeModal(true, { data });
        } else if (value === 'Select') {
          data.dictTypeCode = '';
          dictDataAll.value = dictTypeList.value;
        } else if (value === 'ConstSelector') {
          data.dictTypeCode = '';
          dictDataAll.value = allConstSelector.value;
        }
      }
      async function loadDictTypeDropDown() {
        effectTypeData.value = await getDictDataDropdown('code_gen_effect_type');
        const data = await getDictTypeList();
        dictTypeList.value = data;
        dictDataAll.value = data;
        allConstSelector.value = await getAllConstSelector();
        codeGenQueryTypeData.value = await getDictDataDropdown('code_gen_query_type');
      }

      // 外键弹窗回调
      function fkHandleSuccess(data) {
        let index = tbData.value.findIndex((c) => c.columnName == data.columnName);
        tbData[index] = data;
      }

      // 提交
      async function handleSubmit() {
        try {
          setModalProps({ confirmLoading: true });
          var lst = tbData.value;
          lst.forEach((item) => {
            // 必填那一项转换
            for (const key in item) {
              if (item[key] === true) {
                item[key] = 'Y';
              }
              if (item[key] === false) {
                item[key] = 'N';
              }
            }
          });
          await updateGenerateConfig(lst);
          closeModal();
          emit('success');
        } finally {
          setModalProps({ confirmLoading: false });
        }
      }

      return {
        registerModal,
        tbData,
        columns,
        handleSubmit,
        effectTypeData,
        dictDataAll,
        judgeColumns,
        effectTypeChange,
        codeGenQueryTypeData,
        registerFkModal,
        registerTreeModal,
        fkHandleSuccess,
        dictTypeList,
      };
    },
  });
</script>
