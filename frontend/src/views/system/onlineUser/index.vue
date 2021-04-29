<template>
  <a-card :bordered="false">
    <a-table size="middle" :columns="columns" :dataSource="loadData" :pagination="false" :loading="loading">
      <span slot="lastLoginAddress" slot-scope="text">
        <ellipsis :length="20" tooltip>{{ text }}</ellipsis>
      </span>
      <span slot="lastLoginBrowser" slot-scope="text">
        <ellipsis :length="20" tooltip>{{ text }}</ellipsis>
      </span>
      <span slot="action" slot-scope="text, record">
        <a-popconfirm
          v-if="hasPerm('sysOnlineUser:forceExist')"
          placement="topRight"
          title="是否强制下线该用户？"
          @confirm="() => forceExist(record)">
          <a>强制下线</a>
        </a-popconfirm>
      </span>
    </a-table>
  </a-card>
</template>
<script>
  import {
    STable,
    Ellipsis
  } from '@/components'
  import {
    sysOnlineUserForceExist,
    sysOnlineUserList
  } from '@/api/modular/system/onlineUserManage'
  export default {
    components: {
      STable,
      Ellipsis
    },
    data() {
      return {
        // 查询参数
        queryParam: {},
        // 表头
        columns: [{
            title: '用户Id',
            dataIndex: 'id'
          },
          {
            title: '账号',
            dataIndex: 'account'
          },
          {
            title: '昵称',
            dataIndex: 'nickName'
          },
          {
            title: '登录IP',
            dataIndex: 'lastLoginIp'
          },
          {
            title: '登录时间',
            dataIndex: 'lastLoginTime'
          }
          // {
          //   title: '浏览器',
          //   dataIndex: 'lastLoginBrowser',
          //   scopedSlots: {
          //     customRender: 'lastLoginBrowser'
          //   }
          // },
          // {
          //   title: '操作系统',
          //   dataIndex: 'lastLoginOs'
          // }
        ],
        loading: true,
        loadData: [],
        selectedRowKeys: [],
        selectedRows: []
      }
    },
    // 进页面加载
    created() {
      if (this.hasPerm('sysOnlineUser:forceExist')) {
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
    mounted() {
      if (this.hasPerm('sysOnlineUser:list')) {
        // TODO: 可能直接刷新是获取不到自己的, 原因是 socket 还没连接, 先去后台读取数据了
        // 可能需要一个表示已连接的全局变量, 这里轮询等待
        // 现在先用 setTimeout 解决
        setTimeout(() => {
          sysOnlineUserList().then(res => {
            this.loadData = res.data
            this.loading = false
          })
        }, 1000)
      }
    },
    methods: {
      forceExist(record) {
        sysOnlineUserForceExist(record)
          .then(res => {
            if (res.success) {
              this.$message.success('强制下线成功')
              // 重新加载表格
              this.loadDataList()
            } else {
              this.$message.error('强制下线失败：' + res.message)
            }
          })
          .catch(err => {
            this.$message.error('强制下线错误：' + err.message)
          })
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
