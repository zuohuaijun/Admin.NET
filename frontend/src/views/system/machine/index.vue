<template>
  <div v-if="hasPerm('sysMachine:query')">
    <!-- 系统信息  Java信息-->
    <a-row :gutter="24">
      <a-col :md="12" :sm="24">
        <a-card :loading="loading" title="系统信息" style="margin-bottom: 20px" :bordered="false">
          <table class="sysInfo_table">
            <tr>
              <td class="sysInfo_td">主机名称：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.hostName }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">操作系统：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.systemOs }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">系统架构：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.osArchitecture }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">运行框架：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.frameworkDescription }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">CPU核数：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.processorCount }}</td>
            </tr>
          </table>
        </a-card>
      </a-col>
      <a-col :md="12" :sm="24">
        <a-card :loading="loading" title="网络信息" style="margin-bottom: 20px" :bordered="false">
          <table class="sysInfo_table">
            <tr>
              <td class="sysInfo_td">外网信息：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.wanIp }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">IPv4地址：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.lanIp }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">网卡MAC：</td>
              <td class="sysInfo_td">{{ this.machineBaseInfo.ipMac }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">流量统计：</td>
              <td class="sysInfo_td">{{ this.machineNetworkInfo.sendAndReceived }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">网络速度：</td>
              <td class="sysInfo_td">{{ this.machineNetworkInfo.networkSpeed }}</td>
            </tr>
          </table>
        </a-card>
      </a-col>
    </a-row>
    <a-card :loading="loading" title="其他信息" :bordered="false">
      <table class="sysInfo_table">
        <tr>
          <td class="sysInfo_td">运行时间：</td>
          <td class="sysInfo_td">{{ this.machineUseInfo.runTime }}</td>
          <td class="sysInfo_td">CPU使用率：</td>
          <td class="sysInfo_td">{{ this.machineUseInfo.cpuRate }}%</td>
        </tr>
        <tr>
          <td class="sysInfo_td">总内存：</td>
          <td class="sysInfo_td">{{ this.machineUseInfo.totalRam }}</td>
          <td class="sysInfo_td">内存使用率：</td>
          <td class="sysInfo_td">{{ this.machineUseInfo.ramRate }}%</td>
        </tr>
      </table>
    </a-card>
  </div>
</template>
<script>
  import {
    sysMachineBase,
    sysMachineUse,
    sysMachineNetwork
  } from '@/api/modular/system/machineManage'
  export default {
    data() {
      return {
        loading: true,
        machineBaseInfo: [],
        machineUseInfo: [],
        machineNetworkInfo: []
      }
    },
    // 进页面加载
    created() {
      this.loadMachineBaseInfo()
      this.loadMachineUseInfo()
    },
    methods: {
      // 加载数据方法
      loadMachineBaseInfo() {
        sysMachineBase().then((res) => {
          this.loading = false
          this.machineBaseInfo = res.data
        })
      },
      loadMachineUseInfo() {
        sysMachineUse().then((res) => {
          this.loading = false
          this.machineUseInfo = res.data
        })
      },
      loadMachineNetworkInfo() {
        sysMachineNetwork().then((res) => {
          this.loading = false
          this.machineNetworkInfo = res.data
        })
      },
      refreshData() {
        this.loadMachineUseInfo()
        this.loadMachineNetworkInfo()
      }
    },
    mounted() {
      this.timer = setInterval(this.refreshData, 3000)
    },
    beforeDestroy() {
        clearInterval(this.timer)
    }
  }
</script>
<style lang="less">
  .sysInfo_table {
    width: 100%;
    min-height: 45px;
    line-height: 45px;
    text-align: center;
  }

  .sysInfo_td {
    border-bottom: 1px solid #e8e8e8;
  }
</style>
