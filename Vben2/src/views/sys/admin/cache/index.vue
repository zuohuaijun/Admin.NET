<template>
  <PageWrapper dense contentFullHeight fixedHeight contentClass="flex">
    <div class="m-4 mr-0 overflow-hidden bg-white w-1/4 xl:w-1/5" style="overflow: auto">
      <BasicTree
        title="缓存键列表"
        toolbar
        search
        :clickRowToExpand="true"
        :treeData="treeData"
        :fieldNames="{ key: 'key', title: 'name' }"
        @select="handleSelect"
        ref="treeAction"
      />
    </div>
    <div class="w-3/4 xl:w-4/5 m-4">
      <div class="bg-white h-full">
        <div class="p-4 text-right flex">
          <div class="flex-1 text-left leading-8 font-300 text-lg">
            当前键：{{ currentNode.key }}
          </div>
          <PopConfirmButton
            title="确定要删除此缓存吗?"
            ok-text="删除"
            cancel-text="取消"
            @confirm="onDeleteCache"
            v-if="currentNode && currentNode.pid"
            color="warning"
          >
            删除此缓存
          </PopConfirmButton>
          <PopConfirmButton
            title="确定要清空此缓存吗?"
            ok-text="删除"
            cancel-text="取消"
            @confirm="onEmptyCache"
            v-if="currentNode && currentNode.pid == 0"
            color="error"
          >
            清空此缓存
          </PopConfirmButton>
        </div>
        <ScrollContainer ref="scrollRef" style="height: calc(100% - 64px)">
          <div class="p-4 pt-0">
            <JsonPreview v-if="isJson" :data="jsonData" />
            <div v-else>{{ jsonData }}</div>
          </div>
        </ScrollContainer>
      </div>
    </div>
  </PageWrapper>
</template>
<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { PageWrapper } from '/@/components/Page';
  import { PopConfirmButton } from '/@/components/Button';

  import {
    getAllCacheKeys,
    getCacheStringAsync,
    removeCacheAsync,
    delByParentKeyAsync,
  } from '/@/api/sys/admin';
  import { BasicTree, TreeActionType, TreeItem } from '/@/components/Tree/index';
  import { JsonPreview } from '/@/components/CodeEditor';
  import { ScrollContainer } from '/@/components/Container/index';
  import { useLoading } from '/@/components/Loading';

  export default defineComponent({
    name: 'OrgManagement',
    components: {
      PageWrapper,
      BasicTree,
      JsonPreview,
      ScrollContainer,
      PopConfirmButton,
    },
    setup() {
      const { hasPermission } = usePermission();

      const treeData = ref<TreeItem[]>([]);
      const treeAction = ref<Nullable<TreeActionType>>(null);
      const jsonData = ref<any>('');
      const isJson = ref(true);
      const currentNode = ref<any>({});
      const [openFullLoading, closeFullLoading] = useLoading({
        tip: '请稍后...',
      });

      async function fetch() {
        let keys = await getAllCacheKeys();
        let cacheData: any[] = [];
        for (let i = 0; i < keys.length; i++) {
          let keyArr = keys[i].split(':');
          let p = keyArr[0];
          let parentKey = keyArr.length > 1 ? `${p}:` : p;
          if (cacheData.filter((x) => x.key == parentKey).length === 0) {
            cacheData.push({
              pid: 0,
              children: [],
              name: p,
              key: parentKey,
            });
          }
          if (keyArr.length > 1) {
            let parent = cacheData.filter((x) => x.key == parentKey)[0] || {};
            parent.children.push({
              pid: p,
              name: keyArr[1],
              key: keys[i],
            });
          }
        }
        treeData.value = cacheData;
        console.log(treeData);
      }

      async function handleSelect(keys, obj) {
        if (obj == undefined || keys.length == 0) return;
        console.log('select', keys[0], obj.selectedNodes[0]);
        currentNode.value = obj.selectedNodes[0];
        //根节点且没有：则代表没有子节点
        if ((currentNode.value?.pid || 0) == 0 && currentNode.value?.key.indexOf(':') > 0) {
          jsonData.value = '';
          isJson.value = false;
          return;
        }

        var res = await getCacheStringAsync(keys[0]);
        try {
          jsonData.value = JSON.parse(res);
          isJson.value = true;
        } catch (error) {
          isJson.value = false;
          jsonData.value = res;
        }
      }

      async function onDeleteCache() {
        openFullLoading();
        await removeCacheAsync(currentNode.value.key);
        closeFullLoading();
        currentNode.value = {};
        isJson.value = false;
        jsonData.value = '';
        fetch();
      }

      async function onEmptyCache() {
        openFullLoading();
        await delByParentKeyAsync(currentNode.value.key);
        closeFullLoading();
        currentNode.value = {};
        isJson.value = false;
        jsonData.value = '';
        fetch();
      }

      fetch();

      return {
        treeData,
        treeAction,
        jsonData,
        hasPermission,
        handleSelect,
        isJson,
        currentNode,
        onDeleteCache,
        onEmptyCache,
      };
    },
  });
</script>
