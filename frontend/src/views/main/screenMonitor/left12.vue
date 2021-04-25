<template>
  <div id="left12">
    <div class="ranking-board-title">
      机构评估等级
      <dv-decoration-6 style="width:200px;height:20px;" />
    </div>
    <dv-scroll-ranking-board class="board" :config="config" />
  </div>
</template>

<script>
  export default {
    name: 'Left12',
    data() {
      return {
        config: {
          data: [{
              name: 'XXX机构',
              value: 55
            },
            {
              name: 'XXX机构',
              value: 120
            },
            {
              name: 'XXX机构',
              value: 78
            },
            {
              name: 'XXX机构',
              value: 66
            },
            {
              name: 'XXX机构',
              value: 80
            },
            {
              name: 'XXX机构',
              value: 45
            },
            {
              name: 'XXX机构',
              value: 29
            },
            {
              name: 'XXX机构',
              value: 29
            },
            {
              name: 'XXX机构',
              value: 29
            }
          ],
          unit: '分',
          rowNum: 8
        }
      }
    }
  }
</script>

<style lang="less">
  #left12 {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    display: flex;
    flex-direction: column;
    background-color: rgba(6, 30, 93, 0.5);

    .ranking-board-title {
      position: absolute;
      left: 10px;
      font-size: 14px;
      top: 10px;
      color: #6BD4E0;
    }

    .board {
      width: 90%;
      position: absolute;
      top: 50px;
      bottom: 40px;
      padding: 0px 15px;
    }
  }
</style>
