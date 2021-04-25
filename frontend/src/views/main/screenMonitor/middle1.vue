<template>
  <div id="middle1">
    <div class="digital-flop-item" v-for="item in digitalFlopData" :key="item.title">
      <div class="digital-flop">
        <dv-digital-flop :config="item.number" />
      </div>
      <div class="digital-flop-title">{{ item.title }}</div>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'Middle1',
    data() {
      return {
        digitalFlopData: []
      }
    },
    methods: {
      createData() {
        this.digitalFlopData = [{
            title: '机构总数',
            number: {
              number: [150],
              content: '{nt}',
              textAlign: 'center',
              style: {
                fill: '#40faee',
                fontWeight: 'bold'
              }
            },
            unit: '个'
          },
          {
            title: '床位总数',
            number: {
              number: [40000],
              content: '{nt}',
              textAlign: 'center',
              style: {
                fill: '#40faee',
                fontWeight: 'bold'
              }
            },
            unit: '个'
          },
          {
            title: '老人总数',
            number: {
              number: [98760],
              content: '{nt}',
              textAlign: 'center',
              style: {
                fill: '#40faee',
                fontWeight: 'bold'
              }
            },
            unit: '个'
          },
          {
            title: '员工总数',
            number: {
              number: [1200],
              content: '{nt}',
              textAlign: 'center',
              style: {
                fill: '#40faee',
                fontWeight: 'bold'
              }
            },
            unit: '个'
          }
        ]
      }
    },
    mounted() {
      const {
        createData
      } = this

      createData()
    }
  }
</script>

<style lang="less">
  #middle1 {
    height: 100%;
    padding: 5px 20px 5px 20px;
    flex-shrink: 0;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: rgba(6, 30, 93, 0.5);

    .digital-flop-item {
      width: 100%;
      //height: 60%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      border-left: 3px solid rgb(6, 30, 93);
      border-right: 3px solid rgb(6, 30, 93);
    }

    .digital-flop-title {
      font-size: 10px;
      margin-bottom: 10px;
      color: #40faee;
      margin-top: -20px;
      font-family: "微软雅黑";
    }

    .digital-flop {
      display: flex;
      font-family: "黑体";
    }
  }
</style>
