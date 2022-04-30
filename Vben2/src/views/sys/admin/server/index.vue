<template>
  <PageWrapper title="系统监控">
    <template #headerContent>
      <div class="flex justify-between items-center">
        <span class="flex-1">
          <a href="https://gitee.com/zuohuaijun/Admin.NET" target="_blank">Admin.NET</a>
          基于.NET6/Furion实现的通用管理平台，前端采用Vue3/Vben框架。整合最新技术，模块插件式开发，前后端分离，开箱即用。让开发更简单、更通用、更流行！
        </span>
      </div>
    </template>
    <a-row>
      <a-col :span="12">
        <Description @register="register1" class="mt-2 mr-2" />
      </a-col>
      <a-col :span="12">
        <Description @register="register2" class="mt-2" />
      </a-col>
    </a-row>
    <a-row>
      <a-col :span="24">
        <Description @register="register3" class="mt-2" />
      </a-col>
    </a-row>
  </PageWrapper>
</template>
<script lang="ts">
  import { Row, Col } from 'ant-design-vue';
  import { defineComponent, onBeforeMount, ref } from 'vue';
  import { Description, useDescription } from '/@/components/Description/index';
  import { PageWrapper } from '/@/components/Page';

  import { BaseSchema, NetWorkSchema, UseSchema } from './server.data';
  import { serverBase, serverUse, serverNetWork } from '/@/api/sys/admin';

  export default defineComponent({
    components: { Description, PageWrapper, [Row.name]: Row, [Col.name]: Col },
    setup() {
      let baseData: Recordable = ref();
      let netWorkData: Recordable = ref();
      let useData: Recordable = ref();

      onBeforeMount(async () => {
        baseData.value = (await serverBase()) as any as Recordable;
        useData.value = (await serverUse()) as any as Recordable;
        netWorkData.value = (await serverNetWork()) as any as Recordable;
      });

      const [register1] = useDescription({
        title: '系统信息',
        collapseOptions: { canExpand: true, helpMessage: '系统信息' },
        column: 2,
        data: baseData,
        schema: BaseSchema,
      });

      const [register2] = useDescription({
        title: '网络信息',
        collapseOptions: { canExpand: true, helpMessage: '网络信息' },
        column: 2,
        data: netWorkData,
        schema: NetWorkSchema,
      });

      const [register3] = useDescription({
        title: '其他信息',
        collapseOptions: { canExpand: true, helpMessage: '其他信息' },
        column: 2,
        data: useData,
        schema: UseSchema,
      });

      // async function getServerData() {
      //   baseData = await serverBase();
      //   useData = await serverUse();
      //   netWorkData = await serverNetWork();

      //   console.log(baseData);
      //   console.log(netWorkData);
      //   console.log(useData);
      // }

      return { register1, register2, register3 };
    },
  });
</script>
