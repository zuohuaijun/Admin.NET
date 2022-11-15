<template>
	<div class="sys-codeGenerate-container">
		<el-dialog v-model="isShowDialog" :title="title" draggable width="700px">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="库定位器" prop="configId">
							<el-select clearable v-model="ruleForm.configId" placeholder="库名" filterable @change="DbChanged()">
								<el-option v-for="item in dbData" :key="item.configId" :label="item.configId" :value="item.configId" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="数据库类型" prop="dbType" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.dbTypeString" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="链接串" prop="connectionString">
							<el-input v-model="ruleForm.connectionString" disabled clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="生成表" prop="tableName" :rules="[{ required: true, message: '生成表不能为空', trigger: 'blur' }]">
							<el-select v-model="ruleForm.tableName" class="m-2" filterable clearable>
								<el-option v-for="item in tableData" :key="item.entityName" :label="item.tableName" :value="item.entityName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="业务名" prop="busName">
							<el-input v-model="ruleForm.busName" placeholder="请输入" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="父级菜单" prop="menuPid">
							<el-cascader
								:options="menuData"
								:props="{ checkStrictly: true, emitPath: false, value: 'id', label: 'title' }"
								placeholder="请选择上级菜单"
								clearable
								class="w100"
								v-model="ruleForm.menuPid"
							>
								<template #default="{ node, data }">
									<span>{{ data.title }}</span>
									<span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
								</template>
							</el-cascader>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="命名空间" prop="nameSpace">
							<el-input v-model="ruleForm.nameSpace" clearable placeholder="请输入" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="作者姓名" prop="authorName">
							<el-input v-model="ruleForm.authorName" clearable placeholder="请输入" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="生成方式" prop="generateType">
							<el-select v-model="ruleForm.generateType" class="m-2" filterable clearable>
								<el-option v-for="item in codeGenerateTypeList" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
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
import { reactive, toRefs, onMounted, defineComponent, getCurrentInstance, ref } from 'vue';

import { getDatabaseList, getTableList, addGenerate, updateGenerate } from '/@/api/system/admin';
import { AddCodeGenInput } from '/@/api/system/interface';

export default defineComponent({
	name: 'codeGenerateDialog',
	components: {},
	props: {
		title: {
			type: String,
			default: '',
		},
		menuData: {
			type: Array,
			default: () => [],
		},
		codeGenerateTypeList: {
			type: Array,
			default: () => [] as any,
		},
	},
	setup() {
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as AddCodeGenInput,
			tableData: [] as any,
			dbData: [] as any,
		});

		onMounted(async () => {
			var res = await getDatabaseList();
			state.dbData = res.data.result;
		});

		// DbChanged
		const DbChanged = async () => {
			if (state.ruleForm.configId === '') return;
			let res = await getTableList(state.ruleForm.configId as string);
			state.tableData = res.data.result ?? [];

			let db = state.dbData.filter((u: any) => u.configId == state.ruleForm.configId);
			state.ruleForm.connectionString = db[0].connectionString;
			state.ruleForm.dbType = db[0].dbType.toString();
			state.ruleForm.dbTypeString = convertDbType(db[0].dbType);
		};

		// 打开弹窗
		const openDialog = (addRow: AddCodeGenInput) => {
			state.ruleForm = addRow;
			if (state.ruleForm.type === '1') {
				state.ruleForm.nameSpace = 'Admin.NET.Application';
				state.ruleForm.authorName = 'one';
				state.ruleForm.generateType = '2';
			} else {
				state.ruleForm.dbTypeString = convertDbType(parseInt(state.ruleForm.dbType));
			}
			state.isShowDialog = true;
		};

		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit('submitRefresh');
			state.isShowDialog = false;
		};

		// 取消
		const cancel = () => {
			state.isShowDialog = false;
		};

		// 提交
		const submit = () => {
			ruleFormRef.value.validate(async (valid: boolean) => {
				if (!valid) return;

				if (state.ruleForm.type === '1') {
					await addGenerate(state.ruleForm);
				} else {
					await updateGenerate(state.ruleForm);
				}
				closeDialog();
			});
		};

		const convertDbType = (dbType: number) => {
			let result = '';
			switch (dbType) {
				case 0:
					result = 'MySql';
					break;
				case 1:
					result = 'SqlServer';
					break;
				case 2:
					result = 'Sqlite';
					break;
				case 3:
					result = 'Oracle';
					break;
				case 4:
					result = 'PostgreSql';
					break;
				case 5:
					result = 'Dm';
					break;
				case 6:
					result = 'Kdbndp';
					break;
				case 7:
					result = 'Oscar';
					break;
				case 8:
					result = 'MySqlConnector';
					break;
				case 9:
					result = 'Access';
					break;
				default:
					result = 'Custom';
					break;
			}
			return result;
		};

		const isOrNotSelect = () => {
			return [
				{
					label: '是',
					value: 1,
				},
				{
					label: '否',
					value: 0,
				},
			];
		};

		return {
			ruleFormRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			...toRefs(state),
			convertDbType,
			isOrNotSelect,
			DbChanged,
		};
	},
});
</script>
