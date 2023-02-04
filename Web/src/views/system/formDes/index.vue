<template>
	<div class="sys-formDes-container">
		<v-form-designer ref="vFormDesignRef"></v-form-designer>
	</div>
</template>

<script lang="ts" setup name="sysFormDes">
import { ref } from 'vue';

const vFormDesignRef = ref(null);
</script>

<style lang="scss" scoped>
body {
	margin: 0; // 去除页面垂直滚动条
}
</style>
