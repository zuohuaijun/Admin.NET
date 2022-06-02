<template>
  <BasicDrawer v-bind="$attrs" @register="register" title="日志详情" width="50%">
    <Tabs tab-position="left" v-model:activeKey="activeKey">
      <TabPane key="SQL语句" tab="SQL语句">
        {{ log.sql }}
      </TabPane>
      <TabPane key="变更前数据" tab="变更前数据">
        <JsonPreview v-if="log.beforeData" :data="JSON.parse(log.beforeData)" />
      </TabPane>
      <TabPane key="变更后数据" tab="变更后数据">
        <JsonPreview v-if="log.afterData" :data="JSON.parse(log.afterData)" />
      </TabPane>
      <TabPane key="业务对象" tab="业务对象">
        <JsonPreview
          v-if="log.businessData && log.businessData != 'null'"
          :data="JSON.parse(log.businessData)"
        />
      </TabPane>
    </Tabs>
  </BasicDrawer>
</template>
<script lang="ts">
  import { defineComponent, ref, unref } from 'vue';
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer';
  import { JsonPreview } from '/@/components/CodeEditor';
  import { Tabs, TabPane } from 'ant-design-vue';
  export default defineComponent({
    components: { BasicDrawer, JsonPreview, Tabs, TabPane },
    setup() {
      const activeKey = ref('SQL语句');
      const log = ref({});
      const [register] = useDrawerInner((data) => {
        log.value = unref(data);
      });

      return { activeKey, register, log };
    },
  });
</script>
