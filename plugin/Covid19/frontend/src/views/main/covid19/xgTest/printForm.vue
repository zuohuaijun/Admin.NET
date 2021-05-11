<template>
  <a-modal
    title="核酸报告"
    :width="800"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <template slot="footer">
      <a-button key="back" type="primary" icon="printer" @click="print()">
        打印检验报告
      </a-button>
    </template>
    <a-spin :spinning="confirmLoading" id="print" style="margin-left: 20px;margin-right: 20px;">
      <a-row :gutter="24">
        <a-col :md="24" :sm="24">
          <div style="text-align: center;font-size: 20px; font-weight: 520;">天津市XXX医院/机构核酸检验报告单</div>
        </a-col>
      </a-row>
      <br />
      <a-row :gutter="24">
        <table style="width: 100%;">
          <tr>
            <td><span v-html="'姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名:'"></span></td>
            <td>{{ name }}</td>
            <td><span v-html="'性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别:'"></span></td>
            <td>{{ sex }}</td>
            <td>采样编号：</td>
            <td>{{ number }}</td>
            <td>样本类型：</td>
            <td>{{ xgType }}</td>
          </tr>
          <tr>
            <td><span v-html="'标&nbsp;&nbsp;本&nbsp;&nbsp;号:'"></span></td>
            <td></td>
            <td><span v-html="'年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;龄:'"></span></td>
            <td>{{ age }}</td>
            <td>采样时间：</td>
            <td>{{ collectionTime }}</td>
            <td>样本状态：</td>
            <td>{{ xgStatus }}</td>
          </tr>
          <tr>
            <td>检验项目：</td>
            <td>新型冠状病毒检测</td>
            <td>送检单位：</td>
            <td>{{ siteName }}</td>
          </tr>
        </table>
      </a-row>
      <a-row :gutter="24" style="margin-top: 15px;">
        <table style="width: 100%;" frame="hsides">
          <tr style="border:1px solid black">
            <th>序号</th>
            <th>项目名称</th>
            <th>结果</th>
            <th>单位</th>
            <th>参考范围</th>
          </tr>
          <tr>
            <td>1</td>
            <td>新型冠状病毒基因ORFlab</td>
            <td>{{ xgOrflab }}</td>
            <td></td>
            <td>{{ xgOrflab }}</td>
          </tr>
          <tr>
            <td>2</td>
            <td>新型冠状病毒基因N</td>
            <td>{{ xgN }}</td>
            <td></td>
            <td>{{ xgN }}</td>
          </tr>
          <br />
          <br />
          <br />
          <br />
          <br />
          <br />
        </table>
      </a-row>
      <a-row :gutter="24">
        <table style="width: 100%;">
          <tr>
            <td>检验日期：</td>
            <td align="left">{{ testTime }}</td>
            <td>报告日期：</td>
            <td align="left">{{ nowTime }}</td>
            <td>检验员：</td>
            <td align="left">{{ testDoctor }}</td>
            <td>审核员：</td>
            <td align="left">{{ auditDoctor }}</td>
          </tr>
        </table>
      </a-row>
      <a-row :gutter="24">
        <br />
        声明：本次结果报告只针对本次送检样本负责<br />
        <div
          v-html="'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在感染初期，新型冠状病毒IgN、IgG抗体未产生或滴度很低会导致阴性结果'">
        </div>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>
  import printJS from 'print-js'
  import moment from 'moment'

  export default {
    data() {
      return {
        name: '',
        sex: '',
        number: '',
        xgType: '',
        age: '',
        collectionTime: '',
        xgStatus: '正常',
        siteName: '',
        xgOrflab: '',
        xgN: '',
        testTime: '',
        nowTime: moment(new Date()).format('YYYY-MM-DD HH:mm:ss'),
        testDoctor: '',
        auditDoctor: '',
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      init(record, orgList) {
        this.visible = true
        setTimeout(() => {
          this.name = record.name
          this.sex = record.sex === 1 ? '男' : '女'
          this.number = record.number
          this.age = moment().diff(moment(new Date(record.birthday.replace(/-/g, '/'))), 'years')
          this.collectionTime = record.collectionTime
          this.xgStatus = '正常'
          this.siteName = orgList.find(item => record.siteId === item.id).name
          this.xgOrflab = record.xgOrflab === 1 ? '阴性' : record.xgOrflab !== 0 ? '阳性' : '未检'
          this.xgN = record.xgN === 1 ? '阴性' : record.xgN !== 0 ? '阳性' : '未检'
          this.testTime = record.testTime
          this.testDoctor = record.testDoctor
          this.auditDoctor = record.auditDoctor
        }, 100)
      },
      print() {
        printJS({
          printable: 'print',
          type: 'html',
          // properties: [
          //    { field: 'name', displayName: '姓名', columnSize:`50%`},
          //    { field: 'sex', displayName: '性别',columnSize:`50%`},
          // ],
          // header: `<p class="custom-p"> 名单 </p>`,
          // style: '#printCons {width: 600px;} .no-print{width: 0px} .itemText1 { width: 200px } .itemText2 { width: 200px } .itemText3 { width: 200px } .itemText4 { width: 200px }',
          // gridHeaderStyle:'font-size:12px; padding:3px; border:1px solid; font-weight: 100; text-align:left;',
          // gridStyle:'font-size:12px; padding:3px; border:1px solid; text-align:left;',
          // repeatTableHeader: true,
          // scanStyles:true,
          targetStyles: ['*'],
          ignoreElements: ['no-print', 'bc', 'gb']
        })
      },
      handleSubmit() {
        this.handleCancel()
      },
      handleCancel() {
        this.visible = false
      }
    }
  }
</script>
