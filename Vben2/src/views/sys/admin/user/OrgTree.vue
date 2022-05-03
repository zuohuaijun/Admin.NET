<template>
  <div class="m-4 mr-0 overflow-hidden bg-white">
    <BasicTree
      title="机构列表"
      toolbar
      search
      :clickRowToExpand="true"
      :treeData="treeData"
      :fieldNames="{ key: 'id', title: 'name' }"
      @select="handleSelect"
      ref="treeAction"
    />
  </div>
</template>
<script lang="ts">
  import { defineComponent, onMounted, ref, unref, nextTick } from 'vue';
  import { BasicTree, TreeActionType, TreeItem } from '/@/components/Tree/index';

  import { getOrgList } from '/@/api/sys/admin';

  export default defineComponent({
    name: 'OrgTree',
    components: { BasicTree },

    emits: ['select'],
    setup(_, { emit }) {
      const treeData = ref<TreeItem[]>([]);
      const treeAction = ref<Nullable<TreeActionType>>(null);

      const appendNodeByKey = (parentKey: string, values) => {
        unref(treeAction).insertNodeByKey({
          parentKey: parentKey,
          node: values,
          // 往后插入
          push: 'push',
          // 往前插入
          // push:'unshift'
        });
      };

      const updateNodeByKey = (key: string, values) => {
        unref(treeAction).updateNodeByKey(key, values);
      };

      const deleteNodeByKey = (key: string) => {
        unref(treeAction).deleteNodeByKey(key);
      };

      async function fetch() {
        treeData.value = (await getOrgList()) as unknown as TreeItem[];
        nextTick(() => {
          unref(treeAction)?.filterByLevel(2);
        });
      }

      function handleSelect(keys, obj) {
        emit('select', keys[0], obj.selectedNodes[0]);
      }

      onMounted(() => {
        fetch();
      });
      return {
        treeData,
        handleSelect,
        treeAction,
        appendNodeByKey,
        updateNodeByKey,
        deleteNodeByKey,
        fetch,
      };
    },
  });
</script>
