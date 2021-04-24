<template>
  <a-modal
    title="编辑租户"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-form-item style="display: none;" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input v-decorator="['id']" />
        </a-form-item>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="公司名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="公司名称" v-decorator="['name', {rules: [{required: true, message: '请输入公司名称'}]}]" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="管理员姓名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入管理员姓名" v-decorator="['adminName', {rules: [{required: true, message: '请输入管理员姓名'}]}]" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="邮箱(账号)" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入电子邮箱" v-decorator="['email']" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="电话号码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入电话号码" v-decorator="['phone']" />
            </a-form-item>
          </a-col>
        </a-row>

        <!-- <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item label="数据库连接" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-textarea
                :rows="4"
                placeholder="请输入数据库连接"
                v-decorator="['connection', {rules: [{required: true, message: '请输入数据库连接字符串！'}]}]"></a-textarea>
            </a-form-item>
          </a-col>
        </a-row> -->

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form :form="form">
              <a-form-item label="备注" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
                <a-textarea :rows="2" placeholder="请输入备注" v-decorator="['remark']"></a-textarea>
              </a-form-item>
            </a-form>
          </a-col>
        </a-row>

      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    sysTenantEdit
  } from '@/api/modular/system/tenantManage'

  export default {
    data() {
      return {
        labelCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 6
          }
        },
        wrapperCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 16
          }
        },
        labelCol_JG: {
          xs: {
            span: 24
          },
          sm: {
            span: 3
          }
        },
        wrapperCol_JG: {
          xs: {
            span: 24
          },
          sm: {
            span: 20
          }
        },
        visible: false,
        confirmLoading: false,
        formLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      edit(record) {
        this.visible = true
        setTimeout(() => {
          this.form.setFieldsValue({
            id: record.id,
            name: record.name,
            adminName: record.adminName,
            host: record.host,
            email: record.email,
            phone: record.phone,
            remark: record.remark,
            connection: record.connection
          })
        }, 100)
      },

      handleSubmit() {
        const {
          form: {
            validateFields
          }
        } = this
        this.confirmLoading = true
        validateFields((errors, values) => {
          if (!errors) {
            sysTenantEdit(values).then((res) => {
              if (res.success) {
                this.$message.success('编辑成功')
                this.visible = false
                this.confirmLoading = false
                this.$emit('ok', values)
                this.form.resetFields()
              } else {
                this.$message.error('编辑失败：' + res.message)
              }
            }).finally((res) => {
              this.confirmLoading = false
            })
          } else {
            this.confirmLoading = false
          }
        })
      },
      handleCancel() {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
