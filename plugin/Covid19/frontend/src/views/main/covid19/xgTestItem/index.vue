<template>
  <div>
    <x-card v-if="hasPerm('xgTestItem:page')">
      <div slot="content" class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="项目名称">
                <a-input v-model="queryParam.name" allow-clear placeholder="请输入项目名称" />
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
        :columns="columns"
        :data="loadData"
        :alert="true"
        :rowKey="(record) => record.id"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }">
        <template slot="operator" v-if="hasPerm('xgTestItem:add')">
          <a-button type="primary" v-if="hasPerm('xgTestItem:add')" icon="plus" @click="$refs.addForm.add()">新增检测项目
          </a-button>
        </template>
        <span slot="type" slot-scope="type">
          {{ typeFilter(type) }}
        </span>
        <span slot="area" slot-scope="area">
          {{ areaFilter(area) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('xgTestItem:edit')" @click="$refs.editForm.edit(record)">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('xgTestItem:delete')" />
          <a-popconfirm
            v-if="hasPerm('xgTestItem:delete')"
            placement="topRight"
            title="确认删除？"
            @confirm="() => deleteXgTestItem(record)">
            <a>删除</a>
          </a-popconfirm>
        </span>

      </s-table>

      <add-form ref="addForm" @ok="handleOk" />
      <edit-form ref="editForm" @ok="handleOk" />
    </a-card>
  </div>
</template>

<script>
  import {
    STable,
    XCard
  } from '@/components'
  import {
    getXgTestItemPage,
    deleteXgTestItem
  } from '@/api/modular/main/covid19/xgTestItem'
  import addForm from './addForm'
  import editForm from './editForm'

  export default {
    components: {
      STable,
      XCard,
      addForm,
      editForm
    },

    data() {
      return {
        // 高级搜索 展开/关闭
        advanced: false,
        // 查询参数
        queryParam: {},
        // 表头
        columns: [{
            title: '项目名称',
            dataIndex: 'name'
          },
          {
            title: '唯一编码',
            dataIndex: 'code'
          },
          {
            title: '类型',
            dataIndex: 'type',
            scopedSlots: {
              customRender: 'type'
            }
          },
          {
            title: '适用层级',
            dataIndex: 'area',
            scopedSlots: {
              customRender: 'area'
            }
          },
          {
            title: '排序',
            dataIndex: 'sort'
          },
          {
            title: '备注',
            dataIndex: 'remark'
          }
        ],
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return getXgTestItemPage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        selectedRowKeys: [],
        selectedRows: []
      }
    },

    created() {
      if (this.hasPerm('xgTestItem:edit') || this.hasPerm('xgTestItem:delete')) {
        this.columns.push({
          title: '操作',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        })
      }
    },

    methods: {
      typeFilter(type) {
        if (Number(type) === 1) {
          return '非定期检测'
        } else if (Number(type) === 2) {
          return '定期检测'
        }
      },
      areaFilter(area) {
        if (Number(area) === 1) {
          return '全省'
        } else if (Number(area) === 2) {
          return '全市'
        } else if (Number(area) === 3) {
          return '全县'
        } else if (Number(area) === 4) {
          return '本医院'
        }
      },

      deleteXgTestItem(record) {
        deleteXgTestItem(record).then((res) => {
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

<style>
</style>
