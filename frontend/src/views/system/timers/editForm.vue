<template>
  <a-modal
    title="编辑定时任务"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-row :gutter="24" style="display: none;">
          <a-col :md="12" :sm="24">
            <a-form-item>
              <a-input v-decorator="['id']" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="任务名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入任务名称" v-decorator="['jobName', {rules: [{required: true, message: '请输入任务名称！'}]}]" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="备注" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入备注" v-decorator="['remark']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item label="请求地址" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-input placeholder="请输入请地址" style="width: 100%" v-decorator="['requestUrl', {rules: [{required: true, message: '请输入请求地址！'}]}]" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" label="请求类型">
              <a-radio-group v-decorator="['requestType',{rules: [{ required: true, message: '请选择请求类别！' }]}]">
                <a-radio v-for="(item,index) in requestTypeDictTypeDropDown" :key="index" :value="parseInt(item.code)">{{ item.value }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="定时器类型">
              <a-select style="width: 100%" placeholder="请选择定时器类型" @change="onChangeTimerType" v-decorator="['timerType', {rules: [{ required: true, message: '请选择定时器类型！' }]}]">
                <a-select-option :value="0">Interval</a-select-option>
                <a-select-option :value="1">Cron</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="执行间隔" has-feedback v-if="VisibleTimerType">
              <a-input-number
                placeholder="请输入执行间隔"
                style="width: 100%"
                v-decorator="['interval', {rules: [{ required: true, message: '请输入执行间隔！' }]}]"
                :min="1" />
            </a-form-item>
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="任务表达式" has-feedback v-else>
              <nobr>
                <a-input placeholder="请输入任务表达式" v-decorator="['cron', {rules: [{ required: true, message: '请输入任务表达式！' }]}]" />
                <a href="https://www.bejson.com/othertools/cron/" target="_Blank">参考</a>
              </nobr>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" label="请求头" has-feedback>
              <a-input placeholder="请输入请求头" style="width: 100%" v-decorator="['headers']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" label="请求参数" has-feedback>
              <a-textarea :rows="1" placeholder="请输入请求参数" v-decorator="['requestParameters']"></a-textarea>
            </a-form-item>
          </a-col>
        </a-row>

      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    sysTimersEdit
  } from '@/api/modular/system/timersManage'
  import {
    sysDictTypeDropDown
  } from '@/api/modular/system/dictManage'

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
        VisibleTimerType: true,
        requestTypeDictTypeDropDown: [],
        formLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      edit(record) {
        this.visible = true
        this.formLoading = true
        this.sysDictTypeDropDown()

        // eslint-disable-next-line eqeqeq
        this.VisibleTimerType = record.timerType == 0

        setTimeout(() => {
          this.form.setFieldsValue({
            id: record.id,
            jobName: record.jobName,
            requestUrl: record.requestUrl,
            requestType: record.requestType,
            timerType: record.timerType,
            cron: record.cron,
            interval: record.interval,
            headers: record.headers,
            requestParameters: record.requestParameters,
            remark: record.remark
          })
        }, 100)
      },

      sysDictTypeDropDown() {
        sysDictTypeDropDown({
          code: 'request_type'
        }).then((res) => {
          this.requestTypeDictTypeDropDown = res.data
        })
        this.formLoading = false
      },

      onChangeTimerType(e) {
        this.VisibleTimerType = e === 0
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
            sysTimersEdit(values).then((res) => {
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
