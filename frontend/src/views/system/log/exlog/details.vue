<template>
  <a-modal title="日志详情" :width="900" :visible="visible" :confirmLoading="confirmLoading" @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="方法名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['methodName']" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="类名" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['className']" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="异常名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['exceptionName']" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="异常信息" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-textarea :rows="4" v-decorator="['exceptionMsg']" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>
<script>
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
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      details(record) {
        this.visible = true
        setTimeout(() => {
          this.form.setFieldsValue({
            className: record.className,
            methodName: record.methodName,
            exceptionMsg: record.exceptionMsg,
            exceptionName: record.exceptionName
          })
        }, 100)
      },
      handleCancel() {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
