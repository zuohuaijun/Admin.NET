<template>
  <div>
    <x-card v-if="hasPerm('xgCollector:page')">
      <div slot="content" class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="5" :sm="24">
              <a-form-item label="姓名">
                <a-input v-model="queryParam.name" allow-clear placeholder="请输入姓名" />
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="证件号码">
                <a-input v-model="queryParam.idNumber" allow-clear placeholder="请输入证件号码" />
              </a-form-item>
            </a-col>
            <a-col :md="5" :sm="24">
              <a-form-item label="电话">
                <a-input v-model="queryParam.phone" allow-clear placeholder="请输入电话" />
              </a-form-item>
            </a-col>
            <a-col :md="!advanced && 8 || 24" :sm="24">
              <span
                class="table-page-search-submitButtons"
                :style="advanced && { float: 'right', overflow: 'hidden' } || {} ">
                <a-button type="primary" @click="$refs.table.refresh(true)">查询</a-button>
                <a-button style="margin-left: 8px" @click="() => queryParam = {}">重置</a-button>
              </span>
            </a-col>
          </a-row>
        </a-form>
      </div>
    </x-card>

    <a-card :bordered="false">
      <s-table
        ref="table"
        size="default"
        :scroll="{ x: 1800 }"
        :columns="columns"
        :data="loadData"
        :alert="true"
        :rowKey="(record) => record.id"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }">
        <template slot="operator" v-if="hasPerm('xgCollector:add')">
          <a-button type="primary" v-if="hasPerm('xgCollector:add')" icon="plus" @click="$refs.addForm.add()">新增样本
          </a-button>
          <a-upload :multiple="true" :showUploadList="false" name="file" v-if="hasPerm('xgCollector:import')">
            <a-button icon="up-circle">导入</a-button>
          </a-upload>
          <a-button v-if="hasPerm('xgCollector:export')" icon="down-circle">导出
          </a-button>
        </template>
        <span slot="idNumber" slot-scope="idNumber">
          <a-tag :color="Number(idNumber.substr(idNumber.length-2, 1)) % 2 != 0 ? 'geekblue' : 'pink'">
            {{ idNumber }}
          </a-tag>
        </span>
        <span slot="siteId" slot-scope="siteId">
          {{ siteIdFilter(siteId) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('xgCollector:print')" @click="$refs.printForm.init(record)">条码</a>
          <a-divider type="vertical" v-if="hasPerm('xgCollector:edit') || hasPerm('xgCollector:delete')" />
          <a-dropdown v-if="hasPerm('xgCollector:edit') || hasPerm('xgCollector:delete')">
            <a class="ant-dropdown-link">
              更多
              <a-icon type="down" />
            </a>
            <a-menu slot="overlay">
              <a-menu-item v-if="hasPerm('xgCollector:edit')">
                <a @click="$refs.editForm.edit(record)">编辑</a>
              </a-menu-item>
              <a-menu-item v-if="hasPerm('xgCollector:delete')">
                <a-popconfirm placement="topRight" title="确认删除？" @confirm="() => deleteXgCollector(record)">
                  <a>删除</a>
                </a-popconfirm>
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </span>
      </s-table>

      <add-form ref="addForm" @ok="handleOk" />
      <edit-form ref="editForm" @ok="handleOk" />
      <print-form ref="printForm" @ok="handleOk" />
    </a-card>
  </div>
</template>

<script>
  import {
    STable,
    XCard
  } from '@/components'
  import {
    getXgCollectorPage,
    deleteXgCollector
  } from '@/api/modular/main/covid19/xgCollector'
  import {
    getOrgList
  } from '@/api/modular/system/orgManage'
  import addForm from './addForm'
  import editForm from './editForm'
  import printForm from './printForm'

  export default {
    components: {
      STable,
      XCard,
      addForm,
      editForm,
      printForm
    },

    data() {
      return {
        // 高级搜索 展开/关闭
        advanced: false,
        // 查询参数
        queryParam: {},
        // 表头
        columns: [{
            title: '采集日期',
            dataIndex: 'collectionTime',
            fixed: 'left',
            width: 180
          },
          {
            title: '姓名',
            dataIndex: 'name',
            width: 100,
            sorter: (a, b) => a.name.length - b.name.length
          },
          {
            title: '证件号码',
            dataIndex: 'idNumber',
            width: 150,
            scopedSlots: {
              customRender: 'idNumber'
            }
          },
          {
            title: '电话',
            dataIndex: 'phone'
          },
          {
            title: '采集编号',
            dataIndex: 'number',
            width: 180
          },
          {
            title: '站点名称',
            dataIndex: 'siteId',
            scopedSlots: {
              customRender: 'siteId'
            }
          },
          {
            title: '类别编码',
            dataIndex: 'typeCode'
          },
          {
            title: '住址',
            dataIndex: 'address',
            width: 250
          },
          {
            title: '备注',
            dataIndex: 'remark'
          }
        ],
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return getXgCollectorPage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        orgList: [],
        selectedRowKeys: [],
        selectedRows: []
      }
    },

    created() {
      getOrgList().then((res) => {
        this.orgList = res.data
      })

      if (this.hasPerm('xgCollector:edit') || this.hasPerm('xgCollector:delete') || this.hasPerm('xgCollector:print')) {
        this.columns.push({
          title: '操作',
          dataIndex: 'action',
          fixed: 'right',
          scopedSlots: {
            customRender: 'action'
          }
        })
      }
    },

    methods: {
      siteIdFilter(siteId) {
        if (this.orgList == null) return ''
        return this.orgList.find(item => siteId === item.id)?.name
      },
      deleteXgCollector(record) {
        deleteXgCollector(record).then((res) => {
          if (res.success) {
            this.$message.success('删除成功')
            this.$refs.table.refresh()
          } else {
            this.$message.error('删除失败：' + res.message)
          }
        }).catch((err) => {
          this.$message.error('删除错误：' + err.message)
        })
      },

      toggleAdvanced() {
        this.advanced = !this.advanced
      },
      handleOk() {
        this.$refs.table.refresh()
      },
      onSelectChange(selectedRowKeys, selectedRows) {
        this.selectedRowKeys = selectedRowKeys
        this.selectedRows = selectedRows
      }
    }

  }
</script>

<style lang="less">
  .table-operator {
    margin-bottom: 18px;
  }

  button {
    margin-right: 8px;
  }
</style>
