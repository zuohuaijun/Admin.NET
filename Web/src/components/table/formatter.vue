<template>
    <!-- 将render函数变量写在temolate标签中 -->
    <render></render>
</template>
  
<script lang="ts" setup>
import { ref, watch,h } from 'vue';
// 定义父组件传过来的值
const props = defineProps<{
	fn: any;
}>();
const render=ref();
watch(
	props,
	async () => {
        render.value=h("div",null,props.fn)
	},
	{
		deep: true, //确认是否深入监听
		immediate: true, //确认是否以当前的初始值执行handler的函数
	}
);

</script>