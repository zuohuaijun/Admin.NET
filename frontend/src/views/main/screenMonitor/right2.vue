<template>
  <div class="right2">
    <div class="chart-name">
      老人类型
      <dv-decoration-1 style="width:200px;height:20px;" />
    </div>
    <dv-conical-column-chart :config="config" />
  </div>
</template>

<script>
  export default {
    name: 'Right2',
    data() {
      return {
        config: {
          data: [{
              name: '三五老人',
              value: 3500
            },
            {
              name: '五保老人',
              value: 2500
            },
            {
              name: '社会代养',
              value: 2000
            },
            {
              name: '其他',
              value: 1000
            }
          ],
          img: [],
          showValue: true
        }
      }
    }
  }
</script>

<style lang="less">
  .right2 {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    background-color: rgba(6, 30, 93, 0.5);

    .chart-name {
      position: absolute;
      left: 10px;
      font-size: 14px;
      top: 10px;
      color: #6BD4E0;
    }

    .dv-conical-column-chart {
      // width: 98%;
      // position: absolute;
      // top: 20%;
      // left: 2px;
      // bottom: 0px;
    }
  }
</style>
