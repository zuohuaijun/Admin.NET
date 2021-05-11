<template>
  <a-modal
    title="核酸检测"
    :width="800"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item style="display: none;">
              <a-input v-decorator="['id']" />
            </a-form-item>
            <a-form-item label="检测编号" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input placeholder="请输入检测编号" disabled v-decorator="['number']" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="证件号码" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input placeholder="请输入证件号码" disabled v-decorator="['idNumber']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="新冠ORFlab" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择新冠ORFlab"
                v-decorator="['xgOrflab', {rules: [{ required: true, message: '请选择新冠ORFlab！' }]}]">
                <a-select-option :value="1">阴性</a-select-option>
                <a-select-option :value="2">阳性</a-select-option>
              </a-select>
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="新冠N" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择新冠N"
                v-decorator="['xgN', {rules: [{ required: true, message: '请选择新冠N！' }]}]">
                <a-select-option :value="1">阴性</a-select-option>
                <a-select-option :value="2">阳性</a-select-option>
              </a-select>
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="抗体IgG" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入抗体IgG" v-decorator="['igG']" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="抗体IgM" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入抗体IgM" v-decorator="['igM']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    updateTestResult
  } from '@/api/modular/main/covid19/xgTest'

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
        // 机构行样式
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
        columns: [{
          title: '操作',
          key: 'action',
          scopedSlots: {
            customRender: 'operation'
          }
        }],
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      test(record) {
        this.visible = true
        setTimeout(() => {
          this.form.setFieldsValue({
            id: record.id,
            number: record.number,
            idNumber: record.idNumber,
            xgOrflab: record.xgOrflab,
            xgN: record.xgN,
            igG: record.igG,
            igM: record.igM
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
            updateTestResult(values).then((res) => {
              if (res.success) {
                this.$message.success('检测成功')
                this.confirmLoading = false
                this.$emit('ok', values)
                this.handleCancel()
              } else {
                this.$message.error('保存失败：' + res.message)
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
        this.birthdayString = []
      }
    }
  }
</script>
