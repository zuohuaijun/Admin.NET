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
            <a-form-item label="任务分组" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入任务分组" disabled v-decorator="['jobGroup', {rules: [{required: true, message: '请输入任务分组！'}]}]" />
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
            <a-form-item label="开始时间" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-date-picker placeholder="请选择开始时间" @change="onChangeBeginTime" style="width: 100%" showTime v-decorator="['beginTime']" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="结束时间" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-date-picker placeholder="请选择结束时间" @change="onChangeEndTime" style="width: 100%" showTime v-decorator="['endTime']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="触发器类型">
              <a-select style="width: 100%" placeholder="请选择触发器类型" @change="onChangeTriggerType" v-decorator="['triggerType', {rules: [{ required: true, message: '请选择触发器类型！' }]}]">
                <a-select-option :value="1">Simple</a-select-option>
                <a-select-option :value="2">Cron</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="执行间隔" has-feedback v-if="VisibleTriggerType">
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

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item label="备注" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-textarea :rows="1" placeholder="请输入备注" v-decorator="['remark']"></a-textarea>
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
  import moment from 'moment'

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
        beginTimeString: [],
        endTimeString: [],
        timeFormat: 'YYYY-MM-DD HH:mm:ss',
        VisibleTriggerType: true,
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
        this.VisibleTriggerType = record.triggerType == 1

        setTimeout(() => {
          this.form.setFieldsValue({
            id: record.id,
            jobName: record.jobName,
            jobGroup: record.jobGroup,
            requestUrl: record.requestUrl,
            requestType: record.requestType,
            // beginTime: record.beginTime,
            // endTime: record.endTime,
            triggerType: record.triggerType,
            cron: record.cron,
            interval: record.interval,
            headers: record.headers,
            requestParameters: record.requestParameters,
            remark: record.remark
          })
        }, 100)
        // 时间单独处理
        if (record.beginTime != null) {
          this.form.getFieldDecorator('beginTime', {
            initialValue: moment(record.beginTime, this.timeFormat)
          })
        }
        this.beginTimeString = moment(record.beginTime).format(this.timeFormat)
        if (record.endTime != null) {
          this.form.getFieldDecorator('endTime', {
            initialValue: moment(record.endTime, this.timeFormat)
          })
        }
        this.endTimeString = moment(record.endTime).format(this.timeFormat)
      },

      sysDictTypeDropDown() {
        sysDictTypeDropDown({
          code: 'request_type'
        }).then((res) => {
          this.requestTypeDictTypeDropDown = res.data
        })
        this.formLoading = false
      },

      onChangeTriggerType(e) {
        this.VisibleTriggerType = e === 1
      },

      onChangeBeginTime(date, dateString) {
        if (date == null) {
          this.beginTimeString = []
        } else {
          this.beginTimeString = moment(date).format(this.timeFormat)
        }
      },
      onChangeEndTime(date, dateString) {
        if (date == null) {
          this.endTimeString = []
        } else {
          this.endTimeString = moment(date).format(this.timeFormat)
        }
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
            // eslint-disable-next-line eqeqeq
            if (this.beginTimeString == 'Invalid date') {
              this.beginTimeString = null
            }
            // eslint-disable-next-line eqeqeq
            if (this.endTimeString == 'Invalid date') {
              this.endTimeString = null
            }
            values.beginTime = this.beginTimeString
            values.endTime = this.endTimeString
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
