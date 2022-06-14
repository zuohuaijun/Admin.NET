<template>
  <Dropdown placement="bottom">
    <SwapOutlined />
    <template #overlay>
      <Menu @click="handleMenuClick">
        <MenuItem> 应用12345 </MenuItem>
        <MenuItem> 应用12345</MenuItem>
        <MenuItem> 应用12345 </MenuItem>
      </Menu>
    </template>
  </Dropdown>
</template>
<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { Dropdown, Menu, MenuItem } from 'ant-design-vue';
  import { SwapOutlined } from '@ant-design/icons-vue';
  export default defineComponent({
    components: {
      Dropdown,
      Menu,
      MenuItem,
      SwapOutlined,
    },
    setup() {
      function handleMenuClick(e) {}
      return {
        handleMenuClick,
      };
    },
  });
</script>
