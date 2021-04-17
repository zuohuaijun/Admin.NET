<!--
 * @Author: your name
 * @Date: 2021-04-17 17:45:44
 * @LastEditTime: 2021-04-17 18:04:27
 * @LastEditors: your name
 * @Description: In User Settings Edit
 * @FilePath: \frontend\src\views\system\log\vislog\details.vue
-->
<template>
  <a-modal
    title="日志详情"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item
          label="具体消息"
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
        >
          <a-textarea :rows="4" v-decorator="['message']"/>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>
<script>
  export default {
    data () {
      return {
        labelCol: {
          xs: { span: 24 },
          sm: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 24 },
          sm: { span: 15 }
        },
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      details (record) {
        this.visible = true
        setTimeout(() => {
          this.form.setFieldsValue(
            {
              message: record.message
            }
          )
        }, 100)
      },
      handleCancel () {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
