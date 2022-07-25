<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    showFooter
    :title="getTitle"
    @ok="handleSubmit"
  >
    <Divider orientation="left" style="font-size: 14px">数据表信息</Divider>
    <BasicForm @register="registerForm" />
    <template v-if="!isUpdate">
      <Divider orientation="left" style="font-size: 14px">数据列信息</Divider>
      <BasicTable @register="registerEditColumn">
        <template #action="{ record, column }">
          <TableAction :actions="createActions(record, column)" />
        </template>
      </BasicTable>
      <a-row style="padding: 6px">
        <a-col :span="6">
          <a-button preIcon="carbon:add" type="dashed" style="width: 100%" @click="addPrimaryColumn"
            >新增主键字段</a-button
          >
        </a-col>
        <a-col :span="6">
          <a-button preIcon="carbon:add" type="dashed" style="width: 100%" @click="addColumn"
            >新增普通字段</a-button
          >
        </a-col>
        <a-col :span="6">
          <a-button preIcon="carbon:add" type="dashed" style="width: 100%" @click="addTenantColumn"
            >新增租户字段</a-button
          >
        </a-col>
        <a-col :span="6">
          <a-button preIcon="carbon:add" type="dashed" style="width: 100%" @click="addBaseColumn"
            >新增基础字段</a-button
          >
        </a-col>
      </a-row>
    </template>
  </BasicModal>
</template>
<script lang="ts">
  import { defineComponent, ref, computed, unref } from 'vue';
  import { Divider } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form/index';
  import { tableFormSchema, editColumn } from './database.data';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import {
    BasicTable,
    useTable,
    EditRecordRow,
    BasicColumn,
    ActionItem,
    TableAction,
  } from '/@/components/Table';
  import { addTable, updateTable } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'TableModal',
    components: { BasicModal, BasicForm, BasicTable, TableAction, Divider },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const isUpdate = ref(true);
      const [registerForm, { resetFields, setFieldsValue, updateSchema, validate }] = useForm({
        labelWidth: 300,
        schemas: tableFormSchema,
        showActionButtonGroup: false,
      });

      const [registerModal, { setModalProps, closeModal }] = useModalInner(async (data) => {
        resetFields();
        setModalProps({ confirmLoading: false });
        isUpdate.value = !!data?.isUpdate;

        updateSchema([
          {
            field: 'oldName',
            ifShow: true,
          },
        ]);
        if (unref(isUpdate)) {
          setFieldsValue({
            oldName: data.record.name,
            ...data.record,
          });
        }
        setFieldsValue({
          configId: data.configId,
        });
      });

      const getTitle = computed(() => (!unref(isUpdate) ? '新增表' : '编辑表'));

      var colIndex = 0;
      const [registerEditColumn, { getDataSource }] = useTable({
        columns: editColumn,
        showIndexColumn: false,
        bordered: false,
        pagination: false,
        maxHeight: 300,
        actionColumn: {
          width: 200,
          title: '操作',
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function addPrimaryColumn() {
        const data = getDataSource();
        const addRow: EditRecordRow = {
          columnDescription: '主键Id',
          dataType: 'bigint',
          dbColumnName: 'Id',
          decimalDigits: 0,
          isIdentity: 0,
          isNullable: 0,
          isPrimarykey: 1,
          length: 0,
          key: colIndex,
          editable: true,
          isNew: true,
        };
        data.push(addRow);
        colIndex++;
      }

      function addColumn() {
        const data = getDataSource();
        const addRow: EditRecordRow = {
          columnDescription: '',
          dataType: '',
          dbColumnName: '',
          decimalDigits: 0,
          isIdentity: 0,
          isNullable: 1,
          isPrimarykey: 0,
          length: 0,
          key: colIndex,
          editable: true,
          isNew: true,
        };
        data.push(addRow);
        colIndex++;
      }

      function addTenantColumn() {
        const data = getDataSource();
        const addRow: EditRecordRow = {
          columnDescription: '租户Id',
          dataType: 'bigint',
          dbColumnName: 'TenantId',
          decimalDigits: 0,
          isIdentity: 0,
          isNullable: 1,
          isPrimarykey: 0,
          length: 0,
          key: colIndex,
          editable: true,
          isNew: true,
        };
        data.push(addRow);
        colIndex++;
      }

      function addBaseColumn() {
        const fileds = [
          {
            dataType: 'datetime',
            name: 'CreateTime',
            desc: '创建时间',
          },
          {
            dataType: 'datetime',
            name: 'UpdateTime',
            desc: '更新时间',
          },
          {
            dataType: 'bigint',
            name: 'CreateUserId',
            desc: '创建者Id',
          },
          {
            dataType: 'bigint',
            name: 'UpdateUserId',
            desc: '修改者Id',
          },
          {
            dataType: 'bit',
            name: 'IsDelete',
            desc: '软删除',
            isNullable: 0,
          },
        ];

        const data = getDataSource();
        fileds.forEach((m: any) => {
          data.push({
            columnDescription: m.desc,
            dataType: m.dataType,
            dbColumnName: m.name,
            decimalDigits: 0,
            isIdentity: 0,
            isNullable: m.isNullable === 0 ? 0 : 1,
            isPrimarykey: 0,
            length: m.length || 0,
            key: colIndex,
            editable: true,
            isNew: true,
          });
          colIndex++;
        });
      }

      function createActions(record: EditRecordRow, column: BasicColumn): ActionItem[] {
        return [
          {
            icon: 'fluent:delete-24-regular',
            color: 'error',
            label: '删除',
            onClick: handleColDelete.bind(null, record, column),
          },
          {
            icon: 'fluent:arrow-up-12-filled',
            label: '上移',
            onClick: handleColTop.bind(null, record, column),
          },
          {
            icon: 'fluent:arrow-down-12-filled',
            label: '下移',
            onClick: handleColDown.bind(null, record, column),
          },
        ];
      }

      function handleColDelete(record: EditRecordRow) {
        if (record.isNew) {
          const data = getDataSource();
          const index = data.findIndex((item) => item.key === record.key);
          data.splice(index, 1);
        }
      }

      function handleColTop(record: EditRecordRow) {
        if (record.isNew) {
          const data = getDataSource();
          const index = data.findIndex((item) => item.key === record.key);
          var data1 = ChangeExForArray(index, index - 1, data);
          return data1;
        }
      }

      function handleColDown(record: EditRecordRow) {
        if (record.isNew) {
          const data = getDataSource();
          const index = data.findIndex((item) => item.key === record.key);
          return ChangeExForArray(index, index + 1, data);
        }
      }

      function ChangeExForArray(index1, index2, array: EditRecordRow) {
        let temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
        return array;
      }

      async function handleSubmit() {
        try {
          const values = await validate();
          debugger;
          setModalProps({ confirmLoading: true });
          if (!unref(isUpdate)) {
            let tbData: any = [];
            getDataSource().forEach((item: any) => {
              tbData.push(item.editValueRefs);
            });
            const body: any = {
              dbColumnInfoList: tbData,
              ...values,
            };
            await addTable(body);
          } else {
            await updateTable(values);
          }
          closeModal();
          emit('success');
        } finally {
          setModalProps({ confirmLoading: false });
        }
      }

      return {
        registerModal,
        isUpdate,
        registerForm,
        registerEditColumn,
        getTitle,
        handleSubmit,
        addPrimaryColumn,
        addColumn,
        addTenantColumn,
        addBaseColumn,
        createActions,
      };
    },
  });
</script>
