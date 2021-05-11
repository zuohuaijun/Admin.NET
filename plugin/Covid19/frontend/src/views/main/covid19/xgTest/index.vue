<template>
  <div>
    <x-card v-if="hasPerm('xgTest:page')">
      <div slot="content" class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="6" :sm="24">
              <a-form-item label="姓名">
                <a-input v-model="queryParam.name" allow-clear placeholder="请输入姓名" />
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="证件号码">
                <a-input v-model="queryParam.idNumber" allow-clear placeholder="请输入证件号码" />
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="新冠ORFlab">
                <a-select
                  style="width: 100%"
                  placeholder="请选择新冠ORFlab"
                  v-decorator="['queryParam.xgOrflab', {rules: [{ required: true, message: '请选择新冠ORFlab！' }]}]">
                  <a-select-option :value="1">阴性</a-select-option>
                  <a-select-option :value="2">阳性</a-select-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="新冠N">
                <a-select
                  style="width: 100%"
                  placeholder="请选择新冠N"
                  v-decorator="['queryParam.xgN', {rules: [{ required: true, message: '请选择新冠N！' }]}]">
                  <a-select-option :value="1">阴性</a-select-option>
                  <a-select-option :value="2">阳性</a-select-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="抗体IgG">
                <a-input v-model="queryParam.igG" allow-clear placeholder="请输入抗体IgG" />
              </a-form-item>
            </a-col>
            <a-col :md="6" :sm="24">
              <a-form-item label="抗体IgM">
                <a-input v-model="queryParam.igM" allow-clear placeholder="请输入抗体IgM" />
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

    <a-card>
      <s-table
        ref="table"
        size="default"
        :scroll="{ x: 1500 }"
        :columns="columns"
        :data="loadData"
        :alert="true"
        :rowKey="(record) => record.id"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }">
        <template slot="operator" v-if="hasPerm('xgTest:updateNegative') || hasPerm('xgTest:checkTestResult')">
          <a-popconfirm
            v-if="hasPerm('xgTest:updateNegative')"
            placement="topRight"
            title="确认设置阴性？"
            @confirm="() => updateNegative()">
            <a-button type="primary" icon="experiment">阴性设置</a-button>
          </a-popconfirm>
          <a-popconfirm
            v-if="hasPerm('xgTest:checkTestResult')"
            placement="topRight"
            title="确认已审核数据？"
            @confirm="() => checkTestResult()">
            <a-button type="primary" icon="check-circle">审核数据</a-button>
          </a-popconfirm>
          <a-popconfirm
            v-if="hasPerm('xgTest:uploadData')"
            placement="topRight"
            title="确认上传报告？"
            @confirm="() => uploadData()">
            <a-button type="primary" icon="up-circle">上传报告</a-button>
          </a-popconfirm>
        </template>
        <span slot="idNumber" slot-scope="idNumber">
          <a-tag :color="Number(idNumber.substr(idNumber.length-2, 1)) % 2 != 0 ? 'geekblue' : 'pink'">
            {{ idNumber }}
          </a-tag>
        </span>
        <span slot="xgOrflab" slot-scope="xgOrflab">
          <a-tag :color="xgOrflab == 1? 'green' : xgOrflab == 0? '':'#f50'">
            {{ xgONFilter(xgOrflab) }}
          </a-tag>
        </span>
        <span slot="xgN" slot-scope="xgN">
          <a-tag :color="xgN == 1? 'green' : xgN == 0? '':'#f50'">
            {{ xgONFilter(xgN) }}
          </a-tag>
        </span>
        <span slot="igG" slot-scope="igG">
          <div :style="igG >= 1? 'color:red' : ''">
            {{ igFilter(igG) }}
          </div>
        </span>
        <span slot="igM" slot-scope="igM">
          <div :style="igM >= 1? 'color:red' : ''">
            {{ igFilter(igM) }}
          </div>
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('xgTest:updateTestResult')" @click="$refs.testForm.test(record)">检测</a>
          <a-divider type="vertical" v-if="hasPerm('xgTest:print')" />
          <a v-if="hasPerm('xgTest:print')" @click="$refs.printForm.init(record, orgList)">打印</a>
          <!-- <a-divider type="vertical" v-if="hasPerm('xgTest:edit') & hasPerm('xgTest:delete')" />
        <a-popconfirm v-if="hasPerm('xgTest:delete')" placement="topRight" title="确认删除？" @confirm="() => deleteXgTest(record)">
          <a>删除</a>
        </a-popconfirm> -->
        </span>
      </s-table>

      <test-form ref="testForm" @ok="handleOk" />
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
    getXgTestPage,
    deleteXgTest,
    updateNegative,
    checkTestResult
  } from '@/api/modular/main/covid19/xgTest'
  import {
    getOrgList
  } from '@/api/modular/system/orgManage'
  import testForm from './testForm'
  import printForm from './printForm'

  export default {
    components: {
      STable,
      XCard,
      testForm,
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
            title: '采集编号',
            dataIndex: 'number',
            fixed: 'left',
            width: 180,
            align: 'center',
            sorter: (a, b) => a.number.length - b.number.length
          },
          {
            title: '姓名',
            dataIndex: 'name',
            align: 'center',
            width: 100
          },
          {
            title: '证件号码',
            dataIndex: 'idNumber',
            align: 'center',
            width: 170,
            scopedSlots: {
              customRender: 'idNumber'
            }
          },
          {
            title: '电话',
            dataIndex: 'phone',
            align: 'center'
          },
          {
            title: '新冠ORFlab',
            dataIndex: 'xgOrflab',
            align: 'center',
            scopedSlots: {
              customRender: 'xgOrflab'
            }
          },
          {
            title: '新冠N',
            dataIndex: 'xgN',
            align: 'center',
            scopedSlots: {
              customRender: 'xgN'
            }
          },
          {
            title: '抗体IgG',
            dataIndex: 'igG',
            align: 'center',
            scopedSlots: {
              customRender: 'igG'
            }
          },
          {
            title: '抗体IgM',
            dataIndex: 'igM',
            align: 'center',
            scopedSlots: {
              customRender: 'igM'
            }
          },
          {
            title: '检验医生',
            dataIndex: 'testDoctor',
            align: 'center'
          },
          {
            title: '审核医生',
            dataIndex: 'auditDoctor',
            align: 'center'
          }
        ],
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return getXgTestPage(Object.assign(parameter, this.queryParam)).then((res) => {
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

      if (this.hasPerm('xgTest:print') || this.hasPerm('xgTest:updateTestResult')) {
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
      xgONFilter(xgon) {
        if (xgon === 0) {
          return '未检'
        } else {
          return xgon === 1 ? '阴性' : '阳性'
        }
      },
      igFilter(ig) {
        return ig >= 1 ? ig + '▲' : ig
      },
      updateNegative() {
        if (this.selectedRowKeys.length < 1) {
          this.$message.success('请先选择要设置的行')
          return
        }
        updateNegative({
          ids: this.selectedRowKeys
        }).then((res) => {
          if (res.success) {
            this.$message.success('阴性设置成功')
            this.$refs.table.refresh()
          } else {
            this.$message.error('阴性设置失败：' + res.message)
          }
        }).catch((err) => {
          this.$message.error('阴性设置错误：' + err.message)
        })
      },
      uploadData() {
        if (this.selectedRowKeys.length < 1) {
          this.$message.success('请先选择要设置的行')
          return
        }
        this.$message.success('报告上传成功')
      },
      checkTestResult() {
        if (this.selectedRowKeys.length < 1) {
          this.$message.success('请先选择要设置的行')
          return
        }
        checkTestResult({
          ids: this.selectedRowKeys
        }).then((res) => {
          if (res.success) {
            this.$message.success('审核数据成功')
            this.$refs.table.refresh()
          } else {
            this.$message.error('审核数据失败：' + res.message)
          }
        }).catch((err) => {
          this.$message.error('审核数据错误：' + err.message)
        })
      },
      deleteXgTest(record) {
        deleteXgTest(record).then((res) => {
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
