<template>
	<div class="table-search-container" v-if="props.search.length > 0">
		<el-form ref="tableSearchRef" :model="model" label-width="100px" class="table-form">
			<el-row :gutter="20">
				<!-- <el-col :xs="12" :sm="8" :md="8" :lg="6" :xl="4" class="mb20"></el-col> -->
				<el-col :xs="12" :sm="5" :md="5" :lg="6" :xl="4" class="mb20" v-for="(val, key) in search" :key="key" v-show="key < 3 || state.isToggle">
					<template v-if="val.type !== ''">
						<el-form-item
							label-width="auto"
							:label="val.label"
							:prop="val.prop"
							:rules="[{ required: val.required, message: `${val.label}不能为空`, trigger: val.type === 'input' ? 'blur' : 'change' }]"
						>
							<el-input
								v-model="model[val.prop]"
								v-bind="val.comProps"
								:placeholder="val.placeholder"
								:clearable="!val.required"
								v-if="val.type === 'input'"
								@keyup.enter="onSearch(tableSearchRef)"
								@change="val.change"
								class="w100"
							/>
							<el-date-picker
								v-model="model[val.prop]"
								v-bind="val.comProps"
								type="date"
								:placeholder="val.placeholder"
								:clearable="!val.required"
								v-else-if="val.type === 'date'"
								@change="val.change"
								class="w100"
							/>
							<el-date-picker
								v-model="model[val.prop]"
								v-bind="val.comProps"
								type="monthrange"
								value-format="YYYY/MM/DD"
								:placeholder="val.placeholder"
								:clearable="!val.required"
								v-else-if="val.type === 'monthrange'"
								@change="val.change"
								class="w100"
							/>
							<el-date-picker
								v-model="model[val.prop]"
								v-bind="val.comProps"
								type="daterange"
								value-format="YYYY/MM/DD"
								:placeholder="val.placeholder"
								:clearable="!val.required"
								v-else-if="val.type === 'daterange'"
								@change="val.change"
								class="w100"
							/>
							<el-select v-model="model[val.prop]" v-bind="val.comProps" :clearable="!val.required" :placeholder="val.placeholder" v-else-if="val.type === 'select'" @change="val.change" class="w100">
								<el-option v-for="item in val.options" :key="item.value" :label="item.label" :value="item.value"> </el-option>
							</el-select>
							<el-cascader
								v-else-if="val.type === 'cascader' && val.cascaderData"
								:options="val.cascaderData"
								:clearable="!val.required"
								filterable
								:props="val.cascaderProps ? val.cascaderProps : state.cascaderProps"
								:placeholder="val.placeholder"
								@change="val.change"
								class="w100"
								v-bind="val.comProps"
								v-model="model[val.prop]"
							>
							</el-cascader>
						</el-form-item>
					</template>
				</el-col>
				<el-col :xs="12" :sm="9" :md="9" :lg="6" :xl="4" class="mb20">
					<el-form-item class="table-form-btn" label-width="auto">
						<template #label>
							<div v-if="search.length > 3">
								<div class="table-form-btn-toggle" @click="state.isToggle = !state.isToggle">
									<span>{{ state.isToggle ? '收起' : '展开' }}</span>
									<SvgIcon :name="state.isToggle ? 'ele-ArrowUp' : 'ele-ArrowDown'" />
								</div>
							</div>
						</template>
						<div>
							<!-- 使用el-button-group会导致具有type属性的按钮的右边框无法显示 -->
							<el-button-group>
								<el-button plain type="primary" icon="ele-Search" @click="onSearch(tableSearchRef)"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="onReset(tableSearchRef)" style="margin-left: 12px"> 重置 </el-button>
							</el-button-group>
						</div>
					</el-form-item>
				</el-col>
			</el-row>
		</el-form>
	</div>
</template>

<script setup lang="ts" name="makeTableDemoSearch">
import { reactive, ref } from 'vue';
import type { FormInstance } from 'element-plus';
import { saulVModel } from '/@/utils/saulVModel';

// 定义父组件传过来的值
const props = defineProps({
	// 搜索表单,type-控件类型（input,select,cascader,date）,options-type为selct时需传值，cascaderData,cascaderProps-type为cascader时需传值，属性同elementUI,cascaderProps不传则使用state默认。
	// 可带入comProps属性，和使用的控件属性对应
	search: {
		type: Array<TableSearchType>,
		default: () => [],
	},
	reset: {
		type: Object,
		default: () => {},
	},
	modelValue: {
		type: Object,
		default: () => {},
	},
});

// 定义子组件向父组件传值/事件
const emit = defineEmits(['search', 'reset', 'update:modelValue']);

// 定义变量内容
const tableSearchRef = ref<FormInstance>();
const state = reactive({
	isToggle: false,
	cascaderProps: { checkStrictly: true, emitPath: false, value: 'id', label: 'name', expandTrigger: 'hover' },
});

const model = saulVModel(props, 'modelValue', emit);

// 查询
const onSearch = (formEl: FormInstance | undefined) => {
	if (!formEl) return;
	formEl.validate((valid: boolean) => {
		if (valid) {
			emit('search');
		} else {
			return false;
		}
	});
};

// 重置
const onReset = (formEl: FormInstance | undefined) => {
	if (!formEl) return;
	formEl.resetFields();
	emit('reset');
};
</script>

<style scoped lang="scss">
.table-search-container {
	display: flex;

	.table-form {
		flex: 1;

		.table-form-btn-toggle {
			white-space: nowrap;
			user-select: none;
			display: flex;
			align-items: center;
			color: var(--el-color-primary);
		}
	}
}
</style>
