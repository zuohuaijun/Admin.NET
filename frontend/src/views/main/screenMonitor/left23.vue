<template>
  <div id="left23">
    <div class="water-level-chart-title">
      机构评估进度
      <!-- <dv-decoration-12 style="width:200px;height:20px;" /> -->
    </div>

    <dv-decoration-9>66%</dv-decoration-9>
  </div>
</template>

<script>
  export default {
    name: 'Left23',
    data() {
      return {}
    }
  }
</script>

<style lang="less">
  #left23 {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    background-color: rgba(6, 30, 93, 0.5);
    display: flex;
    flex-direction: column;

    .water-level-chart-title {
      position: absolute;
      left: 10px;
      font-size: 14px;
      top: 10px;
      color: #6BD4E0;
    }

    .dv-decoration-9 {
      width: 80%;
      height: 80%;
      position: absolute;
      left: 50%;
      top: 52%;
      transform: translate(-50%, -48%);
      color: #6BD4E0;
      font-weight: bold;
      font-size: 40px;
    }
  }
</style>
