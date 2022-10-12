<template>
	<div class="sys-menu-container">
		<el-dialog v-model="isShowDialog" width="769px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog','.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="上级菜单">
							<el-cascader :options="menuData"
								:props="{ checkStrictly: true, value: 'id', label: 'title' }" placeholder="请选择上级菜单"
								clearable class="w100" v-model="ruleForm.pid">
								<template #default="{ node, data }">
									<span>{{ data.title }}</span>
									<span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
								</template>
							</el-cascader>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="菜单类型" prop="type">
							<el-radio-group v-model="ruleForm.type">
								<el-radio v-for="dict in menuType" :key="dict.value" :label="dict.value">
									{{ dict.label }}
								</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="菜单名称" prop="title">
							<el-input v-model="ruleForm.title" placeholder="菜单名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<template v-if="ruleForm.type === 1 || ruleForm.type === 2">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="路由名称">
								<el-input v-model="ruleForm.name" placeholder="路由名称" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="路由路径">
								<el-input v-model="ruleForm.path" placeholder="路由路径" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="组件路径">
								<el-input v-model="ruleForm.component" placeholder="组件路径" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="菜单图标">
								<IconSelector v-model="ruleForm.icon" placeholder="菜单图标" type="all" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="重定向">
								<el-input v-model="ruleForm.redirect" placeholder="重定向地址" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="链接地址">
								<el-input v-model="ruleForm.outLink" placeholder="外链/内嵌时链接地址" clearable>
								</el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="菜单排序">
								<el-input-number v-model="ruleForm.order" placeholder="排序" class="w100" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="是否隐藏">
								<el-radio-group v-model="ruleForm.isHide">
									<el-radio :label="true">隐藏</el-radio>
									<el-radio :label="false">不隐藏</el-radio>
								</el-radio-group>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="是否缓存">
								<el-radio-group v-model="ruleForm.isKeepAlive">
									<el-radio :label="true">缓存</el-radio>
									<el-radio :label="false">不缓存</el-radio>
								</el-radio-group>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="是否固定">
								<el-radio-group v-model="ruleForm.isAffix">
									<el-radio :label="true">固定</el-radio>
									<el-radio :label="false">不固定</el-radio>
								</el-radio-group>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="是否内嵌">
								<el-radio-group v-model="ruleForm.isIframe">
									<el-radio :label="true">内嵌</el-radio>
									<el-radio :label="false">不内嵌</el-radio>
								</el-radio-group>
							</el-form-item>
						</el-col>
					</template>
					<template v-if="ruleForm.type === 3">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="权限标识">
								<el-input v-model="ruleForm.permission" placeholder="权限标识" clearable></el-input>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
							<el-form-item label="菜单排序">
								<el-input-number v-model="ruleForm.order" placeholder="排序" class="w100" />
							</el-form-item>
						</el-col>
					</template>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="是否启用">
							<el-radio-group v-model="ruleForm.status">
								<el-radio :label="1">启用</el-radio>
								<el-radio :label="2">不启用</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注内容" clearable type="textarea">
							</el-input>
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts">
import { reactive, toRefs, defineComponent, getCurrentInstance, ref } from 'vue';
import IconSelector from '/@/components/iconSelector/index.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysMenuApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditMenu',
	components: { IconSelector },
	props: {
		// 弹窗标题
		title: {
			type: String,
			default: "",
		},
		// 菜单数据
		menuData: {
			type: Array,
			default: () => [],
		}
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {
				id: 0, // Id
				pid: 0, // 父节点Id
				type: 1, // 菜单类型
				name: '', // 路由名称(全局唯一)
				component: '', // 组件路径
				redirect: '', // 路由重定向(有子集 children 时)
				permission: '', // 权限标识				
				path: '', // 路由路径
				title: '', // 菜单名称
				icon: '', // 菜单图标
				isHide: false, // 是否隐藏
				isKeepAlive: true, // 是否缓存
				isAffix: false, // 是否固定
				outLink: '', // 外链/内嵌时链接地址
				isIframe: false, // 是否内嵌
				order: 100, // 排序
				status: 1, // 是否启用
				remark: '', // 备注
			},
			menuType: [{ value: 1, label: "目录" }, { value: 2, label: "菜单" }, { value: 3, label: "按钮" }],
			ruleRules: {
				type: [{ required: true, message: "菜单类型不能为空", trigger: "blur" }],
				title: [{ required: true, message: "菜单名称不能为空", trigger: "blur" }],
			},
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit("submitRefresh");
			state.isShowDialog = false;
		};
		// 取消
		const cancel = () => {
			state.isShowDialog = false;
		};
		// 提交
		const submit = () => {
			// 上级菜单Id
			if (Array.isArray(state.ruleForm.pid))
				state.ruleForm.pid = state.ruleForm.pid[state.ruleForm.pid.length - 1];
			ruleFormRef.value.validate(async (valid: boolean) => {
				if (!valid) return;
				if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
					await getAPI(SysMenuApi).sysMenuUpdatePost(state.ruleForm);
				}
				else {
					await getAPI(SysMenuApi).sysMenuAddPost(state.ruleForm);
				}
				closeDialog();
			})
		};
		return {
			ruleFormRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			...toRefs(state),
		};
	},
});
</script>
