<template>
  <a-modal
    title="选择外键关系"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="外键表" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择数据库表"
                v-decorator="['tableName', { rules: [{ required: true, message: '请选择数据库表！' }] }]">
                <a-select-option
                  v-for="(item, index) in tableNameData"
                  :key="index"
                  :value="item.tableName"
                  @click="tableNameSele(item)">{{ item.tableName }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="显示字段" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择显示字段"
                v-decorator="['columnName', { rules: [{ required: true, message: '请选择显示字段！' }] }]">
                <a-select-option v-for="(item, index) in cloumnNameData" :key="index" :value="item.columnName">{{
                  item.columnName
                }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    codeGenerateInformationList,
    codeGenerateColumnList
  } from '@/api/modular/gen/codeGenerateManage'
  export default {
    data() {
      return {
        labelCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 5
          }
        },
        wrapperCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 15
          }
        },
        visible: false,
        confirmLoading: false,
        tableNameData: [],
        cloumnNameData: [],
        row: undefined,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      show(row) {
        this.row = row
        this.visible = true
        this.codeGenerateInformationList()
      },
      /**
       * 获得所有数据库的表
       */
      codeGenerateInformationList() {
        codeGenerateInformationList().then(res => {
          this.confirmLoading = true
          this.tableNameData = res.data
          this.confirmLoading = false
        })
      },
      /**
       * 获得表下的所有列
       */
      codeGenerateColumnList(tableName) {
        codeGenerateColumnList(tableName).then(res => {
          this.confirmLoading = true
          this.cloumnNameData = res.data
          this.confirmLoading = false
        })
      },

      /**
       * 提交表单
       */
      handleSubmit() {
        const {
          form: {
            validateFields
          }
        } = this
        validateFields((errors, values) => {
          if (!errors) {
            this.row.fkEntityName = values.tableName
            this.row.fkColumnName = values.columnName
            this.row.fkColumnNetType = this.cloumnNameData.find(e => e.columnName === values.columnName).netType

            this.handleCancel()
          }
        })
      },
      handleCancel() {
        this.form.resetFields()
        this.visible = false
      },
      /**
       * 选择数据库列表
       */
      tableNameSele(item) {
        this.codeGenerateColumnList(item.tableName)
      }
    }
  }
</script>
