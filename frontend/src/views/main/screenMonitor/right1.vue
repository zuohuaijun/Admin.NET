<template>
  <div id="right1">
    <div class="rose-chart-title">
      年龄分布
      <dv-decoration-3 style="width:200px;height:20px;" />
    </div>
    <dv-charts :option="option" />
  </div>
</template>

<script>
  export default {
    name: 'Right1',
    data() {
      return {
        option: {}
      }
    },
    methods: {
      createData() {
        const {
          randomExtend
        } = this

        this.option = {
          series: [{
            type: 'pie',
            radius: '80%',
            roseSort: false,
            data: [{
                name: '60-69岁',
                value: randomExtend(40, 70)
              },
              {
                name: '70-79岁',
                value: randomExtend(20, 30)
              },
              {
                name: '80-89岁',
                value: randomExtend(10, 50)
              },
              {
                name: '90-99岁',
                value: randomExtend(5, 20)
              },
              {
                name: '其他',
                value: randomExtend(40, 50)
              }
            ],
            insideLabel: {
              show: true
            },
            outsideLabel: {
              formatter: '{name} {percent}%',
              labelLineEndLength: 10,
              style: {
                fill: '#fff'
              },
              labelLineStyle: {
                stroke: '#fff'
              }
            }
            // roseType: true
          }],
          color: ['#00baff', '#fa3600', '#3de7c9', '#ff724c', '#ffc530', '#469f4b', '#a02200', '#5d1400', '#b72700']
        }
      },
      randomExtend(minNum, maxNum) {
        if (arguments.length === 1) {
          return parseInt(Math.random() * minNum + 1, 10)
        } else {
          return parseInt(Math.random() * (maxNum - minNum + 1) + minNum, 10)
        }
      }
    },
    mounted() {
      const {
        createData
      } = this

      createData()

      setInterval(createData, 30000)
    }
  }
</script>

<style lang="less">
  #right1 {
    width: 100%;
    height: 100%;
    //box-sizing: border-box;
    background-color: rgba(6, 30, 93, 0.5);

    .rose-chart-title {
      position: absolute;
      left: 10px;
      font-size: 14px;
      top: 10px;
      color: #6BD4E0;
    }

    .dv-charts-container {
      height: calc(~"100% - 50px");
      padding-top: 13%;
    }
  }
</style>
