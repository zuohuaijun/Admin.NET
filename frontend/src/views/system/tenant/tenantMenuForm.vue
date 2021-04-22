<template>
  <a-modal
    title="租户授权菜单"
    :width="600"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="formLoading">
      <a-form :form="form">
        <a-form-item label="菜单权限" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-tree
            v-model="checkedKeys"
            multiple
            checkable
            :auto-expand-parent="autoExpandParent"
            :expanded-keys="expandedKeys"
            :tree-data="menuTreeData"
            :selected-keys="selectedKeys"
            :replaceFields="replaceFields"
            @expand="onExpand"
            @select="onSelect"
            @check="treeCheck" />
          </a-tree>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    SysMenuTreeForGrant
  } from '@/api/modular/system/menuManage'
  import {
    sysTenantOwnMenu,
    sysTenantGrantMenu
  } from '@/api/modular/system/tenantManage'

  export default {
    data() {
      return {
        labelCol: {
          style: {
            'padding-right': '20px'
          },
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
        menuTreeData: [],
        expandedKeys: [],
        checkedKeys: [],
        halfCheckedKeys: [],
        visible: false,
        confirmLoading: false,
        formLoading: true,
        autoExpandParent: true,
        selectedKeys: [],
        subValues: [],
        tenantEntity: [],
        replaceFields: {
          key: 'id'
        },
        form: this.$form.createForm(this)
      }
    },

    methods: {
      // 初始化方法
      tenantMenu(record) {
        this.formLoading = true
        this.tenantEntity = record
        this.visible = true
        this.getMenuTree()
        this.expandedMenuKeys(record)
      },

      /**
       * 获取菜单列表
       */
      getMenuTree() {
        SysMenuTreeForGrant().then((res) => {
          if (res.success) {
            this.menuTreeData = res.data
            // 默认展开目录级
            this.menuTreeData.forEach(item => {
              this.expandedKeys.push(item.id)
            })
          }
        })
      },

      /**
       * 此租户已有菜单权限
       */
      expandedMenuKeys(record) {
        sysTenantOwnMenu({
          id: record.id
        }).then((res) => {
          if (res.success) {
            this.checkedKeys = res.data
            this.findAllChildren(this.menuTreeData)
          }
          this.formLoading = false
        })
      },

      treeCheck(checkKeys, event) {
        this.halfCheckedKeys = event.halfCheckedKeys
      },
      onExpand(expandedKeys) {
        this.expandedKeys = expandedKeys
        this.autoExpandParent = false
      },
      onCheck(checkedKeys) {
        this.checkedKeys = checkedKeys
      },
      onSelect(selectedKeys, info) {
        this.selectedKeys = selectedKeys
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
            sysTenantGrantMenu({
              id: this.tenantEntity.id,
              grantMenuIdList: this.checkedKeys.concat(this.halfCheckedKeys)
            }).then((res) => {
              if (res.success) {
                this.$message.success('授权成功')
                this.confirmLoading = false
                this.$emit('ok', values)
                this.handleCancel()
              } else {
                this.$message.error('授权失败：' + res.message)
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
        // 清空已选择的
        this.checkedKeys = []
        // 清空已展开的
        this.expandedKeys = []
        this.visible = false
      },

      // 遍历树形然后获取到对父节点进行移除，使用子节点，而且将父节点加入到mainMenuList
      findAllChildren(data) {
        data.forEach((item, index) => {
          if (item.children.length !== 0) {
            for (let i = 0; i < this.checkedKeys.length; i++) {
              if (item.id === this.checkedKeys[i]) {
                this.checkedKeys.splice(i, 1)
              }
            }
            this.findAllChildren(item.children)
          }
        })
      }
    }
  }
</script>
