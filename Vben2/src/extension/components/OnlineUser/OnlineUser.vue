<template>
  <div>
    <div @click="toggleDrawer">{{ onlineUserList.length }}人在线</div>

    <Drawer title="在线人员" width="600px" v-model:visible="drawerShow">
      <List item-layout="horizontal" :data-source="onlineUserList">
        <template #renderItem="{ item }">
          <ListItem>
            <ListItemMeta>
              <template #title>
                <div style="display: flex">
                  <div style="flex: 1">{{ item.name }} ({{ item.account }}) </div>
                  <Popconfirm
                    title="确定要强制此用户下线吗?"
                    ok-text="确定"
                    cancel-text="取消"
                    @confirm="onForceExist(item.connectionId)"
                  >
                    <a-button v-if="hasPermission('online:ForceExistUser')" type="link" danger
                      >强制下线</a-button
                    >
                  </Popconfirm>
                </div>
              </template>
              <template #description>
                <!-- {{ item }} -->
                <div class="extra-wrapper">
                  <Space>
                    <span>
                      <ClockCircleOutlined />{{
                        formatToDateTime(item.lastTime, 'YYYY/MM/DD HH:mm:ss')
                      }}
                      <Divider type="vertical" />
                    </span>
                    <span>
                      <LaptopOutlined />{{ item.lastLoginOs }}
                      <Divider type="vertical" />
                    </span>
                    <span>
                      <IeOutlined />{{ item.lastLoginBrowser }}
                      <Divider type="vertical" />
                    </span>
                    <span> <ClusterOutlined />{{ item.lastLoginIp }} </span>
                  </Space>
                </div>
              </template>
            </ListItemMeta>
          </ListItem>
        </template>
      </List>
    </Drawer>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import * as SignalR from '@microsoft/signalr';
  import { getAppEnvConfig } from '/@/utils/env';
  import { getToken } from '/@/utils/auth';
  import { useUserStore } from '/@/store/modules/user';
  import { usePermission } from '/@/hooks/web/usePermission';
  //使用ant 原生组件 方便后期移植
  import {
    Drawer,
    notification,
    List,
    ListItem,
    ListItemMeta,
    Space,
    Divider,
    Popconfirm,
  } from 'ant-design-vue';
  import {
    IeOutlined,
    LaptopOutlined,
    ClusterOutlined,
    ClockCircleOutlined,
  } from '@ant-design/icons-vue';

  import { formatToDateTime } from '/@/utils/dateUtil';
  const { VITE_GLOB_SIGNALR_URL } = getAppEnvConfig();
  const userStore = useUserStore();
  const { hasPermission } = usePermission();
  const onlineUserList = ref<any>([]);
  const drawerShow = ref(false);
  //console.log(userStore.getUserInfo);

  function toggleDrawer() {
    drawerShow.value = !drawerShow.value;
  }

  async function onForceExist(connectionId) {
    console.log(connectionId);
    await connection.send('ForceExistUser', { connectionId }).catch(function (err) {
      console.log(err);
    });
  }

  const messages = ref('');
  const reciveMessage = (msg: any) => {
    console.log('msg', msg);
  };

  //初始化signalr HubConnection对象
  const connection = new SignalR.HubConnectionBuilder()
    .configureLogging(SignalR.LogLevel.Information)
    .withUrl(`${VITE_GLOB_SIGNALR_URL}?access_token=${getToken()}`)
    .withAutomaticReconnect({
      nextRetryDelayInMilliseconds: () => {
        //每5秒重连一次
        return 5000;
      },
    })
    .build();

  connection.keepAliveIntervalInMilliseconds = 15000; //定时PING服务器，避免掉线

  //注册web端方法以供后端调用
  connection.on('ReceiveMessage', reciveMessage);
  connection.on('ForceExist', async (x: any) => {
    console.log('强制下线', x);
    await connection.stop();
    userStore.logout(true);
  });
  connection.on('OnlineUserChanged', (data: any) => {
    console.log('人员变动', data);
    onlineUserList.value = data.list;
    notification.open({
      message: `${data.offline ? `${data.name}离开了` : `欢迎${data.name}上线`}`,
      placement: 'bottomRight',
    });
  });

  connection.onclose(async () => {
    //连接断开
  });
  connection.onreconnecting(() => {
    //掉线重连中
  });
  connection.onreconnected(() => {
    //重新连接成功
  });
  connection.start().then(() => {
    //第一次连接成功
  });
</script>

<style>
  /*兼容vben浅色主题 */
  .vben-layout-header--light .online-user-wrapper {
    color: #000;
  }
</style>

<style scoped>
  .extra-wrapper ::v-deep(.anticon) {
    margin-right: 8px;
  }
</style>