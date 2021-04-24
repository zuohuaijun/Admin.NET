<template>
  <a-modal
    title="新增定时任务"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :label="jobNameLabel" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-select
                :placeholder="jobNamePlaceholder"
                :showArrow="true"
                :mode="jobNameMode"
                v-decorator="['jobName', { rules: [{ required: true, message: '请输入任务名称！' }] }]">
                <a-select-option
                  v-for="(item, index) in JobNameData"
                  :key="index"
                  :value="item.jobName"
                  @click="onChangeJobName(item)">{{ item.jobName }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol">
              <span slot="label">
                <a-tooltip title="Run类型：类名/方法名，http类型：URL地址">
                  <a-icon type="question-circle-o" />
                </a-tooltip>&nbsp; 请求地址
              </span>
              <a-input
                placeholder="请输入请地址"
                v-decorator="['requestUrl', { rules: [{ required: true, message: '请输入请求地址！' }] }]" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" label="请求类型">
              <a-radio-group v-decorator="['requestType', { rules: [{ required: true, message: '请选择请求类别！' }] }]">
                <a-radio
                  v-for="(item, index) in requestTypeEnumDataDropDown"
                  :key="index"
                  :value="parseInt(item.code)"
                  @click="onChangeRequestType(item.code)">{{ item.value }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <div v-show="showHeaders">
            <a-col :md="24" :sm="24">
              <a-form-item :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" label="请求头" has-feedback>
                <a-input placeholder="请输入请求头" style="width: 100%" v-decorator="['headers']" />
              </a-form-item>
            </a-col>
          </div>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="24" :sm="24">
            <a-form-item
              :labelCol="labelCol_JG"
              :wrapperCol="wrapperCol_JG"
              :label="requestParametersLabel"
              has-feedback>
              <a-textarea :rows="1" :placeholder="requestParametersPlaceholder" v-decorator="['requestParameters']">
              </a-textarea>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="定时器类型">
              <a-select
                style="width: 100%"
                placeholder="请选择定时器类型"
                @change="onChangeTimerType"
                v-decorator="['timerType', { rules: [{ required: true, message: '请选择定时器类型！' }] }]">
                <a-select-option v-for="(item, index) in spareTimeTypeDropDown" :key="index" :value="item.code">{{
                  item.value
                }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item
              :labelCol="labelCol"
              :wrapperCol="wrapperCol"
              label="执行间隔(秒)"
              has-feedback
              v-if="VisibleTimerType">
              <a-input-number
                placeholder="请输入执行间隔"
                style="width: 100%"
                v-decorator="['interval', { rules: [{ required: true, message: '请输入执行间隔！' }] }]"
                :min="1" />
            </a-form-item>
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="任务表达式" has-feedback v-else>
              <nobr>
                <a-input
                  placeholder="请输入任务表达式"
                  v-decorator="['cron', { rules: [{ required: true, message: '请输入任务表达式！' }] }]" />
                <a href="https://www.bejson.com/othertools/cron/" target="_Blank">参考</a>
              </nobr>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol">
              <span slot="label">
                <a-tooltip title="新增任务后立即执行，项目启动后立即执行">
                  <a-icon type="question-circle-o" />
                </a-tooltip>&nbsp; 立即执行
              </span>
              <a-switch
                id="startNow"
                checkedChildren="是"
                unCheckedChildren="否"
                v-decorator="['startNow', { valuePropName: 'checked' }]" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="只执行一次">
              <a-switch
                id="doOnce"
                checkedChildren="是"
                unCheckedChildren="否"
                v-decorator="['doOnce', { valuePropName: 'checked' }]" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol">
              <span slot="label">
                <a-tooltip title="并行执行不会等待当前任务完成，发起执行后立即开始下次任务的倒计时。串行执行会等待当前任务完成才开始下次任务的倒计时">
                  <a-icon type="question-circle-o" />
                </a-tooltip>&nbsp; 执行类型
              </span>
              <a-select
                style="width: 100%"
                placeholder="请选择执行类型"
                v-decorator="['executeType', { rules: [{ required: true, message: '请选择执行类型！' }] }]">
                <a-select-option v-for="(item, index) in executeTypeDropDown" :key="index" :value="item.code">{{
                  item.value
                }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="备注" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-input placeholder="请输入备注" v-decorator="['remark']"></a-input>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    sysTimersAdd,
    sysTimersLocalJobList
  } from '@/api/modular/system/timersManage'
  import {
    sysEnumDataList,
    sysEnumDataListByField
  } from '@/api/modular/system/enumManage'
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
        requestTypeEnumDataDropDown: [],
        spareTimeTypeDropDown: [],
        executeTypeDropDown: [],
        LocalJobsDropDown: [],
        JobNameData: [],
        formLoading: false,
        jobNameLabel: '任务名称',
        jobNameMode: 'combobox',
        jobNamePlaceholder: '请输入任务名称',
        requestParametersLabel: '请求参数',
        requestParametersPlaceholder: '请输入请求参数',
        showHeaders: true,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      add(record) {
        this.visible = true
        // this.formLoading = true
        this.formLoading = false
        this.sysEnumDataList()

        this.form.getFieldDecorator('requestType', {
          initialValue: 2
        })
        this.form.getFieldDecorator('timerType', {
          initialValue: 0
        })
        this.form.getFieldDecorator('executeType', {
          initialValue: 0
        })
      },

      /**
       * 获取枚举数据
       */
      sysEnumDataList() {
        sysEnumDataList({
          enumName: 'RequestTypeEnum'
        }).then(res => {
          this.requestTypeEnumDataDropDown = res.data
        })
        sysEnumDataListByField({
          EntityName: 'SysTimer',
          FieldName: 'TimerType'
        }).then(res => {
          this.spareTimeTypeDropDown = res.data
        })
        sysEnumDataListByField({
          EntityName: 'SysTimer',
          FieldName: 'ExecuteType'
        }).then(res => {
          this.executeTypeDropDown = res.data
        })
      },

      onChangeTimerType(e) {
        this.VisibleTimerType = e === 0
      },

      onChangeRequestType(e) {
        this.showHeaders = e !== 0
        if (!this.showHeaders) {
          if (this.LocalJobsDropDown.length === 0) {
            sysTimersLocalJobList().then(res => {
              this.LocalJobsDropDown = res.data
              this.JobNameData = res.data
            })
          } else {
            this.JobNameData = this.LocalJobsDropDown
          }
          this.jobNameLabel = '任务方法'
          this.jobNameMode = 'default'
          this.jobNamePlaceholder = '请选择任务方法'
          this.requestParametersLabel = '配置项参数'
          this.requestParametersPlaceholder = '请输入配置项参数'
        } else {
          this.JobNameData = []
          this.jobNameLabel = '任务名称'
          this.jobNameMode = 'combobox'
          this.jobNamePlaceholder = '请输入任务名称'
          this.requestParametersLabel = '请求参数'
          this.requestParametersPlaceholder = '请输入请求参数'
        }
        this.form.resetFields()
      },

      onChangeJobName(e) {
        this.onChangeTimerType(e.timerType)
        setTimeout(() => {
          this.form.setFieldsValue({
            requestUrl: e.requestUrl,
            startNow: e.startNow,
            doOnce: e.doOnce,
            interval: e.interval,
            timerType: e.timerType,
            remark: e.remark,
            executeType: e.executeType,
            cron: e.cron
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
            sysTimersAdd(values)
              .then(res => {
                if (res.success) {
                  this.$message.success('新增成功')
                  this.visible = false
                  this.confirmLoading = false
                  this.$emit('ok', values)
                  this.form.resetFields()
                } else {
                  this.$message.error('新增失败：' + res.message)
                }
              })
              .finally(res => {
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
