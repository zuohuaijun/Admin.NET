<template>
  <div id="left1">
    <dv-scroll-board :config="config" />
  </div>
</template>

<script>
  export default {
    name: 'Left1',
    data() {
      return {
        config: {
          // header: ['时间', '病害信息', '数量', '标段'],
          data: [
            [
              '<span style="color:#37a2da;">运城市张阿姨1老人在XXX养老社区购买了xxx服务<span style="font-size:5px;color:#32A1FB;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨2老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨3老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨4老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨5老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨6老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨7老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨8老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ],
            [
              '<span style="color:#37a2da;">运城市张阿姨8老人在XXX养老社区购买了xxx服务<span style="font-size:5px;">2019-07-02 17:25:00</span></span>'
            ]
          ],
          // index: true,
          // columnWidth: [45],
          align: ['left'],
          rowNum: 12,
          // headerBGC: '#1981f6',
          // headerHeight: 45,
          oddRowBGC: 'rgba(0, 44, 81, 0.0)',
          evenRowBGC: 'rgba(10, 29, 50, 0.0)'
        }
      }
    }
  }
</script>

<style lang="less">
  #left1 {
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    background-color: rgba(6, 30, 93, 0.5);

    .dv-scroll-board {
      position: absolute;
      top: 1%;
      bottom: 0px;
    }
  }
</style>
