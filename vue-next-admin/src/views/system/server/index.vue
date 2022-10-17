<template>
  <div class="sys-server-container">
    <el-row :gutter="8">
      <el-col :md="12" :sm="24">
        <el-card shadow="hover" header="系统信息" style="margin-bottom;: 8px">
          <table class="sysInfo_table">
            <tr>
              <td class="sysInfo_td">主机名称：</td>
              <td class="sysInfo_td">{{ machineBaseInfo.hostName }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">操作系统：</td>
              <td class="sysInfo_td">{{ machineBaseInfo.systemOs }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">系统架构：</td>
              <td class="sysInfo_td">{{ machineBaseInfo.osArchitecture }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">CPU核数：</td>
              <td class="sysInfo_td">{{ machineBaseInfo.processorCount }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">运行框架：</td>
              <td class="sysInfo_td">{{ machineBaseInfo.frameworkDescription }}</td>
            </tr>
          </table>
        </el-card>
      </el-col>
      <el-col :md="12" :sm="24">
        <el-card shadow="hover" header="网络信息" style="margin-bottom;: 8px">
          <table class="sysInfo_table">
            <tr>
              <td class="sysInfo_td">外网信息：</td>
              <td class="sysInfo_td">{{ machineNetworkInfo.wanIp }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">IPv4地址：</td>
              <td class="sysInfo_td">{{ machineNetworkInfo.localIp }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">网卡MAC：</td>
              <td class="sysInfo_td">{{ machineNetworkInfo.ipMac }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">流量统计：</td>
              <td class="sysInfo_td">{{ machineNetworkInfo.sendAndReceived }}</td>
            </tr>
            <tr>
              <td class="sysInfo_td">网络速度：</td>
              <td class="sysInfo_td">{{ machineNetworkInfo.networkSpeed }}</td>
            </tr>
          </table>
        </el-card>
      </el-col>
    </el-row>
    <el-card shadow="hover" header="其他信息" style="margin-top: 8px">
      <table class="sysInfo_table">
        <tr>
          <td class="sysInfo_td">运行时间：</td>
          <td class="sysInfo_td">{{ machineUseInfo.runTime }}</td>
          <td class="sysInfo_td">CPU使用率：</td>
          <td class="sysInfo_td">{{ machineUseInfo.cpuRate }}</td>
        </tr>
        <tr>
          <td class="sysInfo_td">总内存：</td>
          <td class="sysInfo_td">{{ machineUseInfo.totalRam }}</td>
          <td class="sysInfo_td">内存使用率：</td>
          <td class="sysInfo_td">{{ machineUseInfo.ramRate }}</td>
        </tr>
      </table>
    </el-card>
  </div>
</template>

<script lang="ts">
import { toRefs, reactive, defineComponent, onActivated, onDeactivated } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysServerApi } from '/@/api-services';

export default defineComponent({
  name: 'sysServer',
  components: {},
  setup() {
    const state = reactive({
      loading: true,
      machineBaseInfo: [] as any,
      machineUseInfo: [] as any,
      machineNetworkInfo: [] as any,
      timer: null as any,
    });

    // 服务器基本配置
    const loadMachineBaseInfo = async () => {
      var res = await getAPI(SysServerApi).serverBaseGet();
      state.machineBaseInfo = res.data.result;
    };
    // 服务器使用资源
    const loadMachineUseInfo = async () => {
      var res = await getAPI(SysServerApi).serverUseGet();
      state.machineUseInfo = res.data.result;
    };
    // 服务器网络信息
    const loadMachineNetworkInfo = async () => {
      var res = await getAPI(SysServerApi).serverNetworkGet();
      state.machineNetworkInfo = res.data.result;
    };
    const refreshData = () => {
      loadMachineUseInfo();
      loadMachineNetworkInfo();
    };
    onActivated(() => {
      state.timer = setInterval(() => {
        refreshData();
      }, 3000);
    });
    onDeactivated(() => {
      clearInterval(state.timer);
      // state.timer = null;
    });

    loadMachineBaseInfo();
    loadMachineUseInfo();
    loadMachineNetworkInfo();
    return {
      loadMachineBaseInfo,
      loadMachineUseInfo,
      loadMachineNetworkInfo,
      refreshData,
      ...toRefs(state),
    };
  },
});
</script>

<style lang="scss">
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
