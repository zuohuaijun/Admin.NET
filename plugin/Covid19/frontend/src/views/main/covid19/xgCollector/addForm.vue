<template>
  <a-modal
    title="增加样本"
    :width="700"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <template slot="footer">
      <a-button key="readCard" type="primary" icon="idcard" @click="readCard">
        读取身份证
      </a-button>
      <a-button key="back" @click="handleCancel">
        取消
      </a-button>
      <a-button key="submit" type="primary" @click="handleSubmit">
        确定
      </a-button>
    </template>
    <a-spin :spinning="confirmLoading">
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="姓名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input
                placeholder="请输入姓名"
                v-decorator="['name', {rules: [{required: true, min: 2, message: '请输入至少2个字符！'}]}]" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="性别" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-radio-group v-decorator="['sex',{rules: [{ required: true, message: '请选择性别！' }], initialValue: 1}]">
                <a-radio :value="1">男</a-radio>
                <a-radio :value="2">女</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="证件类型" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择证件类型"
                v-decorator="['idType', {rules: [{ required: true, message: '请选择证件类型！' }], initialValue: 1}]">
                <a-select-option :value="1">身份证</a-select-option>
                <a-select-option :value="2">护照</a-select-option>
                <a-select-option :value="3">其他</a-select-option>
              </a-select>
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="证件号码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input
                placeholder="请输入证件号码"
                v-decorator="['idNumber', {rules: [{required: true, message: '请输入证件号码！'}]}]" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="生日" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-date-picker
                placeholder="请选择生日"
                @change="onChange"
                style="width: 100%"
                v-decorator="['birthday', {rules: [{required: true, message: '请输入生日！'}]}]" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="电话" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入电话" v-decorator="['phone', {rules: [{required: true, message: '请输入电话！'}]}]" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="24" :sm="24">
          <a-form :form="form">
            <a-form-item label="住址" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-input placeholder="请输入住址" v-decorator="['address']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="工作单位" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入工作单位" v-decorator="['workUnit']" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="工作岗位" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入工作岗位" v-decorator="['job']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="站点名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-tree-select
                v-decorator="['siteId', {rules: [{ required: true, message: '请选择站点名称！' }]}]"
                style="width: 100%"
                :dropdownStyle="{ maxHeight: '300px', overflow: 'auto' }"
                :treeData="orgTree"
                placeholder="请选择站点名称"
                treeDefaultExpandAll
                @change="e => initrOrgName(e)">
                <span slot="title" slot-scope="{ id }">{{ id }}</span>
              </a-tree-select>
            </a-form-item>
            <a-form-item v-show="false">
              <a-input v-decorator="['siteName']" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="疫情地区" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入疫情地区" v-decorator="['epidemicArea']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="社会信用码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入社会信用码" v-decorator="['socialCode']" />
            </a-form-item>
          </a-form>
        </a-col>
        <a-col :md="12" :sm="24">
          <a-form :form="form">
            <a-form-item label="类别编码" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input placeholder="请输入类别编码" v-decorator="['typeCode']" />
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :md="24" :sm="24">
          <a-form :form="form">
            <a-form-item label="备注" :labelCol="labelCol_JG" :wrapperCol="wrapperCol_JG" has-feedback>
              <a-input :rows="4" placeholder="请输入备注" v-decorator="['remark']"></a-input>
            </a-form-item>
          </a-form>
        </a-col>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    addXgCollector
  } from '@/api/modular/main/covid19/xgCollector'
  import {
    getOrgTree,
    getOrgList
  } from '@/api/modular/system/orgManage'
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
        visible: false,
        confirmLoading: false,
        orgTree: [],
        orgList: [],
        form: this.$form.createForm(this),
        birthdayString: []
      }
    },
    methods: {
      // 初始化方法
      add() {
        this.visible = true
        this.getOrgDate()
      },
      // 去读身份证信息
      readCard() {
        setTimeout(() => {
          this.form.setFieldsValue({
            name: 'zuohuaijun',
            sex: 1,
            idType: 1,
            idNumber: '130430198611111111',
            address: '天津市西青区张家窝镇保利诺丁山',
            siteId: '142307070910539',
            typeCode: 'A1'
          })
          // 时间单独处理
          var birthday = '1986-06-28'
          if (birthday != null) {
            this.form.getFieldDecorator('birthday', {
              initialValue: moment(birthday, 'YYYY-MM-DD')
            })
          }
          this.birthdayString = moment(birthday).format('YYYY-MM-DD')
          this.$message.success('身份证读取成功')
        }, 100)
      },

      /**
       * 获取机构树，并加载于表单中
       */
      getOrgDate() {
        getOrgTree().then((res) => {
          this.orgTree = res.data
        })
        getOrgList().then((res) => {
          this.orgList = res.data
        })
      },

      /**
       * 选择树机构，初始化机构名称于表单中
       */
      initrOrgName(value) {
        this.form.getFieldDecorator('SiteName', {
          initialValue: this.orgList.find(item => value === item.id).name
        })
      },

      /**
       * 日期需单独转换
       */
      onChange(date, dateString) {
        if (date == null) {
          this.birthdayString = []
        } else {
          this.birthdayString = moment(date).format('YYYY-MM-DD')
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
            if (this.birthdayString.length > 0) {
              values.birthday = this.birthdayString
            }
            addXgCollector(values).then((res) => {
              if (res.success) {
                this.$message.success('新增成功')
                this.confirmLoading = false
                this.$emit('ok', values)
                this.handleCancel()
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
        this.birthdayString = []
      }
    }
  }
</script>
