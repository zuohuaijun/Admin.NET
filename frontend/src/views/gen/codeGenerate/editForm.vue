<template>
  <a-modal
    title="编辑代码生成配置"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-show="false">
          <a-input v-decorator="['id']" />
        </a-form-item>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="生成表" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择数据库表"
                v-decorator="['tableName', {rules: [{ required: true, message: '请选择数据库表！' }]}]">
                <a-select-option
                  v-for="(item,index) in tableNameData"
                  :key="index"
                  :value="item.tableName"
                  @click="tableNameSele(item)">{{ item.tableName }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="业务名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input
                placeholder="请输入业务名"
                v-decorator="['busName', {rules: [{required: true, message: '请输入业务名！'}]}]" />
            </a-form-item>
          </a-col>
          <!--          <a-col :md="12" :sm="24">
            <a-form-item
              :labelCol="labelCol"
              :wrapperCol="wrapperCol"
              label="移除前缀"
            >
              <a-radio-group v-decorator="['tablePrefix',{rules: [{ required: true, message: '请选择是否移除前缀！' }]}]" >
                <a-radio v-for="(item,index) in tablePrefixData" :key="index" :value="item.code" @click="tablePrefixRadio(item.code)">{{ item.name }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col> -->
        </a-row>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="菜单分类" has-feedback>
              <a-select
                style="width: 100%"
                placeholder="请选择应用分类"
                v-decorator="['menuApplication', {rules: [{ required: true, message: '请选择应用分类！' }]}]">
                <a-select-option
                  v-for="(item,index) in appData"
                  :key="index"
                  :value="item.code"
                  @click="changeApplication(item.code)">{{ item.name }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <div>
              <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="父级菜单" has-feedback>
                <a-tree-select
                  v-decorator="['menuPid', {rules: [{ required: true, message: '请选择父级菜单！' }]}]"
                  style="width: 100%"
                  :dropdownStyle="{ maxHeight: '300px', overflow: 'auto' }"
                  :treeData="menuTreeData"
                  placeholder="请选择父级菜单"
                  treeDefaultExpandAll>
                  <span slot="title" slot-scope="{ id }">{{ id }}
                  </span>
                </a-tree-select>
              </a-form-item>
            </div>
          </a-col>
        </a-row>
        <!--        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item
              label="功能名"
              :labelCol="labelCol"
              :wrapperCol="wrapperCol"
              has-feedback
            >
              <a-input placeholder="请输入功能名" v-decorator="['tableComment', {rules: [{required: true, message: '请输入功能名！'}]}]" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item
              label="类名"
              :labelCol="labelCol"
              :wrapperCol="wrapperCol"
              has-feedback
            >
              <a-input placeholder="请输入类名" v-decorator="['className', {rules: [{required: true, message: '请输入类名！'}]}]" />
            </a-form-item>
          </a-col>
        </a-row> -->
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item label="命名空间" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input
                placeholder="请输入命名空间"
                v-decorator="['nameSpace', {rules: [{required: true, message: '请输入命名空间！'}]}]" />
            </a-form-item>
          </a-col>
          <a-col :md="12" :sm="24">
            <a-form-item label="作者姓名" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
              <a-input
                placeholder="请输入作者姓名"
                v-decorator="['authorName', {rules: [{required: true, message: '请输入作者姓名！'}]}]" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="24">
          <a-col :md="12" :sm="24">
            <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="生成方式">
              <a-radio-group v-decorator="['generateType',{rules: [{ required: true, message: '请选择生成方式！' }]}]">
                <a-radio
                  v-for="(item,index) in generateTypeData"
                  :key="index"
                  :value="item.code"
                  @click="generateTypeRadio(item.code)">{{ item.name }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    getAppList
  } from '@/api/modular/system/appManage'
  import {
    getMenuTree
  } from '@/api/modular/system/menuManage'
  import {
    codeGenerateInformationList,
    codeGenerateEdit
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
        tableNameData: [],
        appData: [],
        menuTreeData: [],
        // tablePrefixData: [],
        generateTypeData: [],
        confirmLoading: false,
        // tablePrefixValue: 'N',
        tableNameValue: '',
        // packageNameShow: true,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      edit(record) {
        this.visible = true
        this.codeGenerateInformationList()
        this.dataTypeItem()
        setTimeout(() => {
          this.form.setFieldsValue({
            id: record.id,
            tableName: record.tableName,
            // tablePrefix: record.tablePrefix,
            // tableComment: record.tableComment,
            // className: record.className,
            busName: record.busName,
            generateType: record.generateType,
            authorName: record.authorName,
            nameSpace: record.nameSpace,
            menuApplication: record.menuApplication,
            menuPid: record.menuPid
          })
        }, 100)
        this.tableNameValue = record.tableName
        // this.tablePrefixValue = record.tablePrefix

        // 获取系统应用列表
        this.getSysApplist()
        this.changeApplication(record.menuApplication)
      },
      /**
       * 获得菜单所属应用
       */
      getSysApplist() {
        return getAppList().then((res) => {
          if (res.success) {
            this.appData = res.data
          } else {
            this.$message.warning(res.message)
          }
        })
      },
      /**
       * 获得所有数据库的表
       */
      codeGenerateInformationList() {
        codeGenerateInformationList().then((res) => {
          this.tableNameData = res.data
        })
      },
      /**
       * 获取字典数据
       */
      dataTypeItem() {
        this.tablePrefixData = this.$options.filters['dictData']('yes_or_no')
        this.generateTypeData = this.$options.filters['dictData']('code_gen_create_type')
        this.generateTypeData.splice(0, 1) // 默认去掉从压缩包下载
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
            this.confirmLoading = true
            codeGenerateEdit(values).then((res) => {
              if (res.success) {
                this.$message.success('编辑成功')
                this.$emit('ok', values)
                this.handleCancel()
              } else {
                this.$message.error('编辑失败' + res.message)
              }
            }).finally((res) => {
              this.confirmLoading = false
            })
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
        this.tableNameValue = item.tableName
        this.form.setFieldsValue({
          className: item.tableComment
        })
        this.settingDefaultValue()
      },
      /**
       * 菜单所属应用change事件
       */
      changeApplication(value) {
        getMenuTree({
          'application': value
        }).then((res) => {
          if (res.success) {
            this.menuTreeData = [{
              'id': '-1',
              'parentId': '0',
              'title': '顶级',
              'value': '0',
              'pid': '0',
              'children': res.data
            }]
          } else {
            this.$message.warning(res.message)
          }
        })
      },
      // /**
      //  * 选择是否移除前缀触发
      //  */
      // tablePrefixRadio (tablePrefixType) {
      //   this.tablePrefixValue = tablePrefixType
      //   this.settingDefaultValue()
      // },
      // /**
      //  * 设置默认值
      //  */
      // settingDefaultValue () {
      //   const tableName = this.classNameToHump()
      //   this.form.setFieldsValue(
      //     {
      //       className: tableName,
      //       busName: tableName.toLowerCase()
      //     }
      //   )
      // },
      // /**
      //  * 设置类名为数据库表的驼峰命名
      //  */
      // classNameToHump () {
      //   const arr = this.tableNameValue.toLowerCase().split('_')
      //   if (this.tablePrefixValue === 'Y') {
      //     arr.splice(0, 1)
      //   }
      //   for (let i = 0; i < arr.length; i++) {
      //     // charAt()方法得到第一个字母，slice()得到第二个字母以后的字符串
      //     arr[i] = arr[i].charAt(0).toUpperCase() + arr[i].slice(1)
      //   }
      //   return arr.join('')
      // },
      /**
       * 选择生成方式
       */
      generateTypeRadio(generateType) {
        // if (generateType === '1') {
        //   this.packageNameShow = true
        // } else {
        //   this.packageNameShow = false
        //   this.form.setFieldsValue({ packageName: 'com.cn.xiaonuo' })
        // }
      }
    }
  }
</script>
