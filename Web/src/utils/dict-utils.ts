import { storeToRefs } from 'pinia';
import { useUserInfo } from '/@/stores/userInfo';


const stores = useUserInfo();
const { dictList } = storeToRefs(stores);

// 用于在 Table 中把字段的代码转换为名称，示例如下：
/*
import { getDictDataItem as di, getDictDataList as dl } from '/@/utils/dict-utils';

<el-table-column prop="字段名" label="描述" width="140">
    <template #default="scope">
    <el-tag :type="di('字典名代码', scope.row.credentialsType)?.tagType"> [{{di("字典名代码", scope.row.credentialsType)?.code}}]{{di("字典名代码", scope.row.credentialsType)?.value}} </el-tag>
    </template>
</el-table-column>
*/
export function getDictDataItem(dicName:string, dicItemCode:any): any{
    const dict = dictList.value.filter(item => item.code === dicName);
    if (dict.length === 0)
        return null;
    const dictData = dict[0].children.filter(item => item.code == dicItemCode);
    if (dictData.length === 0)
        return null;
    return dictData[0];
}

// select 控件使用，用于获取字典列表，示例如下：
/*
import { getDictDataItem as di, getDictDataList as dl } from '/@/utils/dict-utils';

<el-select clearable v-model="ruleForm.字段" placeholder="请选择证件提示">
    <el-option v-for="(item,index) in  dl('字段名名码')"  :key="index" :value="item.code" :label="`[${item.code}] ${item.value}`"></el-option>
</el-select>
*/
export function getDictType(dicName:string): any{
    const dict = dictList.value.filter(item => item.code === dicName);
    if (dict.length === 0)
        return null;
    return dict[0];
}

export function getDictDataList(dicName:string): any{
    const result = getDictType(dicName)?.children;
    return result ?? [];
}
