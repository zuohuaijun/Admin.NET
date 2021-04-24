<template>
  <div>
    <x-card v-if="hasPerm('sysTimers:page')">
      <div slot="content" class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="任务名称">
                <a-input v-model="queryParam.timerName" allow-clear placeholder="请输入任务名称" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="$refs.table.refresh(true)">查询</a-button>
              <a-button style="margin-left: 8px" @click="() => queryParam = {}">重置</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
    </x-card>
    <a-card :bordered="false">
      <s-table
        ref="table"
        :columns="columns"
        :data="loadData"
        :alert="true"
        :rowKey="(record) => record.id"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }">
        <template slot="operator" v-if="hasPerm('sysTimers:add')">
          <a-button @click="$refs.addForm.add()" icon="plus" type="primary" v-if="hasPerm('sysTimers:add')">新增定时任务
          </a-button>
        </template>
        <span slot="actionClass" slot-scope="text">
          <ellipsis :length="10" tooltip>{{ text }}</ellipsis>
        </span>
        <span slot="remark" slot-scope="text">
          <ellipsis :length="10" tooltip>{{ text }}</ellipsis>
        </span>
        <span slot="requestType" slot-scope="requestType">
          {{ requestTypeFilter(requestType) }}
        </span>
        <span slot="jobStatus" slot-scope="text,record" v-if="hasPerm('sysTimers:start') || hasPerm('sysTimers:stop')">
          <a-popconfirm
            placement="top"
            :title="text===0? '确定停止该任务？':'确定启动该任务？'"
            @confirm="() => editjobStatusStatus(text,record)">
            <a-badge :status="text===0? 'processing':'default'" />
            <a>{{ jobStatusFilter(text) }}</a>
          </a-popconfirm>
        </span>
        <span slot="jobStatus" v-else>
          <a-badge :status="text===0? 'processing':'default'" />
          {{ jobStatusFilter(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('sysTimers:edit')" @click="$refs.editForm.edit(record)">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('sysTimers:edit') & hasPerm('sysTimers:delete')" />
          <a-popconfirm
            v-if="hasPerm('sysTimers:delete')"
            placement="topRight"
            title="确认删除？"
            @confirm="() => sysTimersDelete(record)">
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
    Ellipsis,
    XCard
  } from '@/components'
  import {
    sysTimersPage,
    sysTimersDelete,
    sysTimersStart,
    sysTimersStop
  } from '@/api/modular/system/timersManage'
  import addForm from './addForm'
  import editForm from './editForm'
  import {
    sysEnumDataList
  } from '@/api/modular/system/enumManage'
  export default {
    // name: 'PosIndex',
    components: {
      XCard,
      STable,
      Ellipsis,
      addForm,
      editForm
    },
    data() {
      return {
        // 查询参数
        queryParam: {},
        // 表头
        columns: [{
            title: '任务名称',
            dataIndex: 'jobName'
          },
          {
            title: '请求地址',
            dataIndex: 'requestUrl'
          },
          {
            title: '请求类型',
            dataIndex: 'requestType',
            scopedSlots: {
              customRender: 'requestType'
            }
          },
          {
            title: '请求参数',
            dataIndex: 'requestParameters'
          },
          {
            title: '间隔',
            dataIndex: 'interval'
          },
          {
            title: 'Cron',
            dataIndex: 'cron'
          },
          {
            title: '状态',
            dataIndex: 'timerStatus',
            scopedSlots: {
              customRender: 'jobStatus'
            }
          },
          {
            title: '执行次数',
            dataIndex: 'runNumber'
          },
          {
            title: '备注',
            dataIndex: 'remark',
            width: 100
          }
        ],
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return sysTimersPage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        selectedRowKeys: [],
        selectedRows: [],
        requestTypeEnumDataDropDown: []
      }
    },
    created() {
      this.sysEnumDataList()
      if (this.hasPerm('sysTimers:edit') || this.hasPerm('sysTimers:delete')) {
        this.columns.push({
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: {
            customRender: 'action'
          }
        })
      }
    },
    methods: {
      /**
       * 获取枚举数据
       */
      sysEnumDataList() {
        sysEnumDataList({
          enumName: 'RequestTypeEnum'
        }).then((res) => {
          this.requestTypeEnumDataDropDown = res.data
        })
      },
      requestTypeFilter(requestType) {
        // eslint-disable-next-line eqeqeq
        const values = this.requestTypeEnumDataDropDown.filter(item => item.code == requestType)
        if (values.length > 0) {
          return values[0].value
        }
      },
      jobStatusFilter(jobStatus) {
        if (jobStatus === 0) {
          return '运行中'
        } else if (jobStatus === 1) {
          return '已停止'
        } else if (jobStatus === 2) {
          return '执行失败'
        } else if (jobStatus === 3) {
          return '已取消'
        }
      },
      /**
       * 启动停止
       */
      editjobStatusStatus(code, record) {
        // eslint-disable-next-line eqeqeq
        if (code === 0) {
          sysTimersStop({
            id: record.id,
            jobName: record.jobName
          }).then(res => {
            if (res.success) {
              this.$message.success('停止成功')
              this.$refs.table.refresh()
            } else {
              this.$message.error('停止失败：' + res.message)
            }
          })
          // eslint-disable-next-line eqeqeq
        } else if (code != 0) {
          sysTimersStart(record).then(res => {
            if (res.success) {
              this.$message.success('启动成功')
              this.$refs.table.refresh()
            } else {
              this.$message.error('启动失败：' + res.message)
            }
          })
        }
      },
      sysTimersDelete(record) {
        sysTimersDelete(record).then((res) => {
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
    },
    mounted() {
      // this.timer = setInterval(this.loadData, 5000)
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
