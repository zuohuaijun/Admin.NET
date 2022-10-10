<template>
	<div class="sys-user-container">
		<el-dialog v-model="isShowDialog" width="769px">
			<template #header>
				<div style="font-size: large" v-drag="['.el-dialog','.el-dialog__header']">
					{{ title }}
				</div>
			</template>
			<el-form :model="ruleForm" :rules="ruleRules" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="账号名称" prop="userName">
							<el-input v-model="ruleForm.userName" placeholder="账号名称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="手机号码" prop="phone">
							<el-input v-model="ruleForm.phone" placeholder="手机号码" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="真实姓名" prop="realName">
							<el-input v-model="ruleForm.realName" placeholder="真实姓名" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="昵称" prop="nickName">
							<el-input v-model="ruleForm.nickName" placeholder="昵称" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="出生日期" prop="birthday">
							<el-date-picker v-model="ruleForm.birthday" type="date" placeholder="出生日期"
								format="YYYY-MM-DD" value-format="YYYY-MM-DD" style="width: 100%" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="性别" prop="sex">
							<el-radio-group v-model="ruleForm.sex">
								<el-radio :label="1">男</el-radio>
								<el-radio :label="2">女</el-radio>
							</el-radio-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="证件号码" prop="idCard">
							<el-input v-model="ruleForm.idCard" placeholder="证件号码" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="邮箱" prop="email">
							<el-input v-model="ruleForm.email" placeholder="邮箱" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb5">
						<el-form-item label="排序">
							<el-input-number v-model="ruleForm.order" controls-position="right" placeholder="排序"
								class="w100" />
						</el-form-item>
					</el-col>
					<el-divider border-style="dashed" content-position="center">
						<div style="color: #b1b3b8">其他</div>
					</el-divider>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="所属机构">
							<el-cascader :options="orgData" :props="{ checkStrictly: true, value: 'id', label: 'name' }"
								placeholder="所属机构" clearable class="w100" v-model="ruleForm.orgId">
								<template #default="{ node, data }">
									<span>{{ data.name }}</span>
									<span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
								</template>
							</el-cascader>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="职位" prop="posId">
							<el-select v-model="ruleForm.posId" placeholder="职位" style="width: 100%">
								<el-option v-for="d in posData" :key="d.id" :label="d.name" :value="d.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="工号" prop="jobNum">
							<el-input v-model="ruleForm.jobNum" placeholder="所属机构" clearable></el-input>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="岗位状态" prop="jobStatus">
							<el-select v-model="ruleForm.jobStatus" placeholder="岗位状态" style="width: 100%">
								<el-option v-for="dict in jobStatusType" :key="dict.value" :label="dict.label"
									:value="dict.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="个性签名" prop="signature">
							<el-input v-model="ruleForm.signature" placeholder="个性签名" clearable></el-input>
						</el-form-item>
					</el-col>
					<!-- <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="本人简介" prop="introduction">
							<el-input v-model="ruleForm.introduction" placeholder="本人简介" clearable></el-input>
						</el-form-item>
					</el-col> -->
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
import { reactive, toRefs, defineComponent, getCurrentInstance, ref, unref, onMounted } from 'vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysPosApi, SysUserApi } from '/@/api-services/api';

export default defineComponent({
	name: 'sysEditUser',
	components: {},
	props: {
		// 弹窗标题
		title: {
			type: String,
			default: () => "",
		},
		// 机构数据
		orgData: {
			type: Array,
			default: () => [],
		}
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref<HTMLElement | null>(null);
		const state = reactive({
			isShowDialog: false,
			ruleForm: {
				id: 0,
				userName: '',
				nickName: '',
				realName: '',
				birthday: undefined,
				sex: 1,
				phone: '',
				idCard: '',
				email: '',
				orgId: 0,
				posId: 0,
				jobNum: '',
				jobStatus: 1,
				signature: '',
				introduction: '',
				order: 10, // 排序
				remark: '', // 备注
			},
			jobStatusType: [{ value: 1, label: "在职" }, { value: 2, label: "离职" }, { value: 3, label: "请假" }], // 岗位状态
			posData: [] as any, // 职位数据
			ruleRules: {
				userName: [{ required: true, message: "账号名称不能为空", trigger: "blur" }],
				phone: [{ required: true, message: "手机号码不能为空", trigger: "blur" }],
				realName: [{ required: true, message: "真实姓名不能为空", trigger: "blur" }],
			},
		});
		onMounted(async () => {
			var res = await getAPI(SysPosApi).sysPosListGet();
			state.posData = res.data.result;
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
			const formWrap = unref(ruleFormRef) as any;
			if (!formWrap) return;

			// 取父节点Id
			if (Array.isArray(state.ruleForm.orgId))
				state.ruleForm.orgId = state.ruleForm.orgId[state.ruleForm.orgId.length - 1];
			formWrap.validate(async () => {
				if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
					await getAPI(SysUserApi).sysUserUpdatePost(state.ruleForm);
				}
				else {
					await getAPI(SysUserApi).sysUserAddPost(state.ruleForm);
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
	