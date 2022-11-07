<template>
	<div class="sys-dbTable-container">
		<el-dialog v-model="isShowDialog" title="表编辑" draggable width="1400px">
			<el-divider content-position="left">数据表信息</el-divider>
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="表名称" prop="tableName" :rules="[{ required: true, message: '名称不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.tableName" placeholder="表名称" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="描述" prop="description" :rules="[{ required: true, message: '描述不能为空', trigger: 'blur' }]">
							<el-input v-model="ruleForm.description" placeholder="描述" clearable type="textarea" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<el-divider content-position="left">数据列信息</el-divider>
			<el-table :data="tableData" style="width: 100%" max-height="400">
				<el-table-column fixed prop="dbColumnName" label="字段名" width="150">
					<template #default="scope">
						<el-input v-model="scope.row.dbColumnName" autocomplete="off" />
					</template>
				</el-table-column>
				<el-table-column prop="columnDescription" label="描述" width="200">
					<template #default="scope">
						<el-input v-model="scope.row.columnDescription" autocomplete="off" />
					</template>
				</el-table-column>
				<el-table-column prop="isPrimarykey" label="主键" >
					<template #default="scope">
						<el-select v-model="scope.row.isPrimarykey" class="m-2" placeholder="Select">
							<el-option v-for="item in isOrNotSelect()" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
						<!-- <el-switch v-model="scope.row.isPrimarykey" active-text="是" inactive-text="否" /> -->
					</template>
				</el-table-column>
				<el-table-column prop="isIdentity" label="自增" >
					<template #default="scope">
						<el-select v-model="scope.row.isIdentity" class="m-2" placeholder="Select">
							<el-option v-for="item in isOrNotSelect()" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
						<!-- <el-switch v-model="scope.row.isIdentity" active-text="是" inactive-text="否" /> -->
					</template>
				</el-table-column>
				<el-table-column prop="dataType" label="类型" >
					<template #default="scope">
						<el-select v-model="scope.row.dataType" class="m-2" placeholder="Select">
							<el-option v-for="item in apiTypeSelect()" :key="item.value" :label="item.value" :value="item.value" />
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="isNullable" label="可空" >
					<template #default="scope">
						<el-select v-model="scope.row.isNullable" class="m-2" placeholder="Select">
							<el-option v-for="item in isOrNotSelect()" :key="item.value" :label="item.label" :value="item.value" />
						</el-select>
						<!-- <el-switch v-model="scope.row.isNullable" active-text="是" inactive-text="否" /> -->
					</template>
				</el-table-column>
				<el-table-column prop="length" label="长度" >
					<template #default="scope">
						<el-input-number v-model="scope.row.length" size="small" />
					</template>
				</el-table-column>
				<el-table-column prop="decimalDigits" label="保留几位小数">
					<template #default="scope">
						<el-input-number v-model="scope.row.decimalDigits" size="small" />
					</template>
				</el-table-column>
				<el-table-column fixed="right" label="操作" width="120">
					<template #default="scope">
						<el-button link type="primary" size="small" @click.prevent="deleteRow(scope.$index)"> Remove </el-button>
					</template>
				</el-table-column>
			</el-table>
			<div style="text-align: center">
				<el-button type="primary" text bg @click="addPrimaryColumn">新增主键字段</el-button>
				<el-button type="primary" text bg @click="addColumn">新增普通字段</el-button>
				<el-button type="primary" text bg @click="addTenantColumn">新增租户字段</el-button>
				<el-button type="primary" text bg @click="addBaseColumn">新增基础字段</el-button>
			</div>

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

import { getAPI } from '/@/utils/axios-utils';
import { SysDatabaseApi } from '/@/api-services/api';
import { UpdateDbTableInput } from '/@/api-services/models';

export default defineComponent({
	name: 'sysEditTable',
	components: {},
	setup() {
		var colIndex = 0;
		/**
		 *
		 * @export
		 * @interface EditRecordRow
		 */
		interface EditRecordRow {
			columnDescription?: string | null;
			dataType?: string | null;
			dbColumnName?: string | null;
			decimalDigits: number;
			isIdentity: number;
			isNullable: number;
			isPrimarykey: number;
			length: number;
			key?: number;
			editable?: boolean;
			isNew: boolean;
		}
		const { proxy } = getCurrentInstance() as any;
		const ruleFormRef = ref();
		const tableData = reactive([]) as Array<EditRecordRow>;
		const state = reactive({
			isShowDialog: false,
			ruleForm: {} as UpdateDbTableInput,
		});
		// 打开弹窗
		const openDialog = (row: any) => {
			state.ruleForm = row;
			state.isShowDialog = true;
		};
		// 关闭弹窗
		const closeDialog = () => {
			proxy.mittBus.emit('submitRefreshTable');
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
				await getAPI(SysDatabaseApi).sysDatabaseTableUpdatePost(state.ruleForm);
				closeDialog();
			});
		};

		const apiTypeSelect = () => {
			return [
				{
					value: 'text',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'varchar',
					hasLength: true,
					hasDecimalDigits: false,
				},
				{
					value: 'nvarchar',
					hasLength: true,
					hasDecimalDigits: false,
				},
				{
					value: 'char',
					hasLength: true,
					hasDecimalDigits: false,
				},
				{
					value: 'nchar',
					hasLength: true,
					hasDecimalDigits: false,
				},
				{
					value: 'timestamp',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'int',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'smallint',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'tinyint',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'bigint',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'bit',
					hasLength: false,
					hasDecimalDigits: false,
				},
				{
					value: 'decimal',
					hasLength: true,
					hasDecimalDigits: true,
				},
				{
					value: 'datetime',
					hasLength: false,
					hasDecimalDigits: false,
				},
			];
		};

		const isOrNotSelect = () => {
			return [
				{
					label: '是',
					value: 1
				},
				{
					label: '否',
					value: 0
				}
			];
		};

		const deleteRow = (index: number) => {
			tableData.splice(index, 1);
		};

		function addPrimaryColumn() {
			const addRow: EditRecordRow = {
				columnDescription: '主键Id',
				dataType: 'bigint',
				dbColumnName: 'Id',
				decimalDigits: 0,
				isIdentity: 0,
				isNullable: 0,
				isPrimarykey: 1,
				length: 0,
				key: colIndex,
				editable: true,
				isNew: true,
			};
			tableData.push(addRow);
			colIndex++;
		}

		function addColumn() {
			const addRow: EditRecordRow = {
				columnDescription: '',
				dataType: '',
				dbColumnName: '',
				decimalDigits: 0,
				isIdentity: 0,
				isNullable: 1,
				isPrimarykey: 0,
				length: 0,
				key: colIndex,
				editable: true,
				isNew: true,
			};
			tableData.push(addRow);
			colIndex++;
		}

		function addTenantColumn() {
			const addRow: EditRecordRow = {
				columnDescription: '租户Id',
				dataType: 'bigint',
				dbColumnName: 'TenantId',
				decimalDigits: 0,
				isIdentity: 0,
				isNullable: 1,
				isPrimarykey: 0,
				length: 0,
				key: colIndex,
				editable: true,
				isNew: true,
			};
			tableData.push(addRow);
			colIndex++;
		}

		function addBaseColumn() {
			const fileds = [
				{
					dataType: 'datetime',
					name: 'CreateTime',
					desc: '创建时间',
				},
				{
					dataType: 'datetime',
					name: 'UpdateTime',
					desc: '更新时间',
				},
				{
					dataType: 'bigint',
					name: 'CreateUserId',
					desc: '创建者Id',
				},
				{
					dataType: 'bigint',
					name: 'UpdateUserId',
					desc: '修改者Id',
				},
				{
					dataType: 'bit',
					name: 'IsDelete',
					desc: '软删除',
					isNullable: 0,
				},
			];

			fileds.forEach((m: any) => {
				tableData.push({
					columnDescription: m.desc,
					dataType: m.dataType,
					dbColumnName: m.name,
					decimalDigits: 0,
					isIdentity: 0,
					isNullable: m.isNullable === 0 ? 0 : 1,
					isPrimarykey: 0,
					length: m.length || 0,
					key: colIndex,
					editable: true,
					isNew: true,
				});
				colIndex++;
			});
		}
		return {
			ruleFormRef,
			openDialog,
			closeDialog,
			cancel,
			submit,
			...toRefs(state),
			tableData,
			deleteRow,
			apiTypeSelect,
			addPrimaryColumn,
			addColumn,
			addTenantColumn,
			addBaseColumn,
			isOrNotSelect
		};
	},
});
</script>
