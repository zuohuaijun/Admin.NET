<template>
	<div class="sys-editCodeGen-container">
		<el-dialog v-model="isShowDialog" draggable width="700px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ title }} </span>
				</div>
			</template>
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="库定位器" prop="configId">
							<el-select v-model="ruleForm.configId" placeholder="库名" filterable @change="DbChanged()" class="w100">
								<el-option v-for="item in dbData" :key="item.configId" :label="item.configId" :value="item.configId" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="库类型" prop="dbType" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-select v-model="ruleForm.dbType" placeholder="数据库类型" clearable disabled class="w100">
								<el-option label="MySql" :value="'0'" />
								<el-option label="SqlServer" :value="'1'" />
								<el-option label="Sqlite" :value="'2'" />
								<el-option label="Oracle" :value="'3'" />
								<el-option label="PostgreSQL" :value="'4'" />
								<el-option label="Dm" :value="'5'" />
								<el-option label="Kdbndp" :value="'6'" />
								<el-option label="Oscar" :value="'7'" />
								<el-option label="MySqlConnector" :value="'8'" />
								<el-option label="Access" :value="'9'" />
								<el-option label="OpenGauss" :value="'10'" />
								<el-option label="QuestDB" :value="'11'" />
								<el-option label="HG" :value="'12'" />
								<el-option label="ClickHouse" :value="'13'" />
								<el-option label="GBase" :value="'14'" />
								<el-option label="Custom" :value="'900'" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="库地址" prop="connectionString">
							<el-input v-model="ruleForm.connectionString" disabled clearable type="textarea" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="生成表" prop="tableName" :rules="[{ required: true, message: '生成表不能为空', trigger: 'blur' }]">
							<el-select v-model="ruleForm.tableName" filterable clearable class="w100">
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
							<el-select v-model="ruleForm.generateType" filterable class="w100">
								<el-option v-for="item in codeGenTypeList" :key="item.value" :label="item.label" :value="item.value" />
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
import { reactive, toRefs, onMounted, defineComponent, ref } from 'vue';
import mittBus from '/@/utils/mitt';

import { getAPI } from '/@/utils/axios-utils';
import { SysCodeGenApi, SysDictDataApi, SysMenuApi } from '/@/api-services/api';
import { UpdateCodeGenInput, AddCodeGenInput, SysMenu } from '/@/api-services/models';

export default defineComponent({
	name: 'sysEditCodeGen',
	components: {},
	props: {
		title: {
			type: String,
			default: '',
		},
	},
	setup() {
		const ruleFormRef = ref();
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as UpdateCodeGenInput,
			tableData: [] as any,
			dbData: [] as any,
			menuData: [] as Array<SysMenu>,
			codeGenTypeList: [] as any,
		});

		onMounted(async () => {
			var resDb = await getAPI(SysCodeGenApi).sysCodeGenDatabaseListGet();
			state.dbData = resDb.data.result;

			let resMenu = await getAPI(SysMenuApi).sysMenuListGet();
			state.menuData = resMenu.data.result ?? [];

			let resDicData = await getAPI(SysDictDataApi).sysDictDataDictDataDropdownCodeGet('code_gen_create_type');
			state.codeGenTypeList = resDicData.data.result;
		});

		// DbChanged
		const DbChanged = async () => {
			if (state.ruleForm.configId === '') return;
			let res = await getAPI(SysCodeGenApi).sysCodeGenTableListConfigIdGet(state.ruleForm.configId as string);
			state.tableData = res.data.result ?? [];

			let db = state.dbData.filter((u: any) => u.configId == state.ruleForm.configId);
			state.ruleForm.connectionString = db[0].connectionString;
			state.ruleForm.dbType = db[0].dbType.toString();
		};

		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = JSON.parse(JSON.stringify(row));
			state.isShowDialog = true;
		};

		// 关闭弹窗
		const closeDialog = () => {
			mittBus.emit('submitRefresh');
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
				if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
					await getAPI(SysCodeGenApi).sysCodeGenUpdatePost(state.ruleForm as UpdateCodeGenInput);
				} else {
					await getAPI(SysCodeGenApi).sysCodeGenAddPost(state.ruleForm as AddCodeGenInput);
				}
				closeDialog();
			});
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
			isOrNotSelect,
			DbChanged,
			...toRefs(state),
		};
	},
});
</script>
