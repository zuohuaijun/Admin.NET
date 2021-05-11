<template>
  <a-modal
    title="新增检测项目"
    :width="500"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-form-item label="项目名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入项目名称" v-decorator="['name', {rules: [{required: true, message: '请输入项目名称！'}]}]" />
        </a-form-item>

        <a-form-item label="唯一编码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入唯一编码" v-decorator="['code']" />
        </a-form-item>

        <a-form-item label="项目类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['type',{rules: [{ required: true, message: '请选择类型！' }]}]">
            <a-radio :value="1">非定期检测</a-radio>
            <a-radio :value="2">定期检测</a-radio>
          </a-radio-group>
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="适用层级">
          <a-select
            style="width: 100%"
            placeholder="请选择适用层级"
            v-decorator="['area', {rules: [{ required: true, message: '请选择适用层级！' }]}]">
            <a-select-option v-for="(item,index) in areaList" :key="index" :value="item.id">{{ item.name }}
            </a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="排序">
          <a-input-number
            placeholder="请输入排序"
            style="width: 100%"
            v-decorator="['sort', { initialValue: 100 }]"
            :min="1"
            :max="1000" />
        </a-form-item>

        <a-form-item label="备注" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea :rows="4" placeholder="请输入备注" v-decorator="['remark']"></a-textarea>
        </a-form-item>

      </a-form>

    </a-spin>
  </a-modal>
</template>

<script>
  import {
    addXgTestItem
  } from '@/api/modular/main/covid19/xgTestItem'
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
        areaList: [{
          'id': '1',
          'name': '全省'
        }, {
          'id': '2',
          'name': '全市'
        }, {
          'id': '3',
          'name': '全县'
        }, {
          'id': '4',
          'name': '本医院'
        }],
        visible: false,
        confirmLoading: false,
        formLoading: true,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      add() {
        this.visible = true
        this.formLoading = false
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
            addXgTestItem(values).then((res) => {
              if (res.success) {
                this.$message.success('新增成功')
                this.visible = false
                this.confirmLoading = false
                this.$emit('ok', values)
                this.form.resetFields()
              } else {
                this.$message.error('新增失败：' + res.message)
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
