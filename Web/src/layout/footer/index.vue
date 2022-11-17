<template>
	<div class="layout-footer pb15">
		<div class="layout-footer-warp">
			<div>Admin.NET</div>
			<div class="mt5">Copyright © 2022 Dilon All rights reserved.</div>
		</div>
	</div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
	name: 'layoutFooter',
});
</script>

<style scoped lang="scss">
.layout-footer {
	width: 100%;
	display: flex;
	&-warp {
		margin: auto;
		color: var(--el-text-color-secondary);
		text-align: center;
		animation: error-num 0.3s ease;
	}
}
</style>
