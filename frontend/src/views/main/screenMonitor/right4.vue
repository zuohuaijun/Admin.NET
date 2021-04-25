<template>
  <div class="right4">
    <div class="chart-name">
      服务器状态
      <dv-decoration-3 style="width:200px;height:20px;" />
    </div>
    <dv-charts :option="option" />
  </div>
</template>

<script>
  export default {
    name: 'Right4',
    data() {
      return {
        option: {
          // title: {
          // 	text: '周销售额趋势'
          // },
          xAxis: {
            // name: '第二周',
            data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
            axisLine: {
              style: {
                stroke: '#F6F8FA'
              }
            },
            axisLabel: {
              style: {
                fill: '#F6F8FA'
              }
            },
            axisTick: {
              show: false
            }
          },
          yAxis: {
            // name: '销售额',
            data: 'value',
            splitLine: {
              show: false
            },
            axisLine: {
              style: {
                stroke: '#F6F8FA'
              }
            },
            axisLabel: {
              style: {
                fill: '#F6F8FA'
              }
            },
            axisTick: {
              show: false
            }
          },
          series: [{
            data: [1200, 2230, 1900, 2100, 3500, 4200, 3985],
            type: 'line',
            lineArea: {
              show: true,
              gradient: ['rgba(55, 162, 218, 0.6)', 'rgba(55, 162, 218, 0)']
            },
            label: {
              show: true,
              formatter: '{value}'
            },
            smooth: true,
            lineStyle: {
              lineDash: [5, 5]
            },
            linePoint: {
              radius: 0
            }
          }]
        }
      }
    }
  }
</script>

<style lang="less">
  .right4 {
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

    .dv-charts-container {
      //height: calc(~"100% - 40px");
      //padding-top: 35px 10px 10px 10px;
      height: 100%;
      top: 10%;
      left: 4%;
      position: absolute;
    }
  }
</style>
