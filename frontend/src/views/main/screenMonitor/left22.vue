<template>
  <div class="left22">
    <div class="chart-name">
      事件统计
      <dv-decoration-3 style="width:200px;height:20px;" />
    </div>
    <dv-charts :option="option" />
  </div>
</template>

<script>
  export default {
    name: 'Left22',
    data() {
      return {
        option: {
          legend: {
            data: [{
                name: '商品订单',
                color: '#00baff'
              },
              {
                name: '呼叫',
                color: '#ff5ca9'
              },
              {
                name: '服务工单',
                color: '#3de7c9'
              }
            ],
            textStyle: {
              fill: '#fff'
            }
          },
          xAxis: {
            data: [
              '12/01', '12/02', '12/03', '12/04', '12/05', '12/06', '12/07'
            ],
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
            },
            min: 0,
            max: 8
          },
          series: [{
              name: '商品订单',
              data: [
                2.5, 3.5, 6.5, 6.5, 7.5, 6.5, 2.5
              ],
              type: 'bar',
              barStyle: {
                fill: 'rgba(0, 186, 255, 0.4)'
              }
            },
            {
              name: '呼叫',
              data: [
                2.5, 3.5, 6.5, 6.5, 7.5, 6.5, 2.5
              ],
              type: 'line',
              lineStyle: {
                stroke: '#ff5ca9'
              },
              linePoint: {
                radius: 4,
                style: {
                  fill: '#ff5ca9',
                  stroke: 'transparent'
                }
              }
            },
            {
              name: '服务工单',
              data: [
                1.3, 2.3, 5.3, 5.3, 6.3, 5.3, 1.3
              ],
              type: 'line',
              smooth: true,
              lineArea: {
                show: true,
                gradient: ['rgba(55, 162, 218, 0.6)', 'rgba(55, 162, 218, 0)']
              },
              lineStyle: {
                lineDash: [5, 5]
              },
              linePoint: {
                radius: 4,
                style: {
                  fill: '#00db95'
                }
              }
            }
          ]
        }
      }
    }
  }
</script>

<style lang="less">
  .left22 {
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
      width: 98%;
      position: absolute;
      top: 1%;
      left: 1%;
      bottom: 0px;
    }
  }
</style>
