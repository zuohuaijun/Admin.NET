<template>
  <a-modal
    title="样本条形码"
    :width="400"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <template slot="footer">
      <a-button key="back" type="primary" icon="printer" @click="print()">
        打印
      </a-button>
    </template>
    <a-spin :spinning="confirmLoading" id="print" style="margin-left: 20px;margin-right: 20px;">
      <img id="barcode" />
      <a-row style="text-align: center;">
        <span>{{ name }}</span><span v-html="'&nbsp;&nbsp;&nbsp;&nbsp;'"></span>
        <span>{{ age }}</span><span v-html="'&nbsp;&nbsp;&nbsp;&nbsp;'"></span>
        <span>{{ sex }}</span><span v-html="'&nbsp;&nbsp;&nbsp;&nbsp;'"></span>
        <span>{{ nowTime }}</span><br /><br />
        <span>新型冠状病毒核酸检测</span>
      </a-row>
    </a-spin>
  </a-modal>
</template>

<script>
  import JsBarcode from 'jsbarcode'
  import printJS from 'print-js'
  import moment from 'moment'

  export default {
    data() {
      return {
        name: '',
        sex: '',
        age: '',
        nowTime: moment(new Date()).format('YYYY-MM-DD HH:mm:ss'),
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      init(record) {
        this.visible = true
        setTimeout(() => {
          this.name = record.name
          this.sex = record.sex === 1 ? '男' : '女'
          this.age = moment().diff(moment(new Date(record.birthday.replace(/-/g, '/'))), 'years')

          JsBarcode('#barcode', record.number, {
            format: 'CODE128', // 选择要使用的条形码类型
            width: 2, // 设置条之间的宽度
            height: 50, // 高度
            displayValue: true, // 是否在条形码下方显示文字
            // fontOptions: 'bold', //使文字加粗体或变斜体
            // font: 'fantasy', //设置文本的字体
            // textAlign: 'left', //设置文本的水平对齐方式
            // textPosition: 'top', //设置文本的垂直位置
            // textMargin: 5, //设置条形码和文本之间的间距
            fontSize: 14 // 设置文本的大小
            // background: '#eee', //设置条形码的背景
            // lineColor: '#2196f3', //设置条和文本的颜色。
            // margin: 10, //设置条形码周围的空白边距
          })
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
