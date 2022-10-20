<template>
	<div class="sys-formDesign-container">
		<v-form-designer ref="vFormDesignRef"></v-form-designer>
	</div>
</template>

<script lang="ts">
import { toRefs, reactive, defineComponent, ref } from 'vue';

export default defineComponent({
	name: 'sysFormDesign',
	components: {},
	setup() {
		const vFormDesignRef = ref(null);
		const state = reactive({
			loading: true,
		});

		return {
			vFormDesignRef,
			...toRefs(state),
		};
	},
});
</script>

<style lang="scss">
body {
	margin: 0; // 去除页面垂直滚动条
}
</style>
