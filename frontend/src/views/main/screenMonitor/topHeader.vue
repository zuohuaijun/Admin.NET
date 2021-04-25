<template>
  <div id="top-header">
    <dv-decoration-8 class="header-left-decoration" />
    <dv-decoration-5 class="header-center-decoration" />
    <dv-decoration-8 class="header-right-decoration" :reverse="true" />
    <div class="center-title">智慧养老服务数据中心</div>
    <div class="title-time">{{ dateFormat(date) }}</div>
  </div>
</template>

<script>
  export default {
    name: 'TopHeader',
    data() {
      return {
        date: new Date()
      }
    },
    methods: {
      dateFormat(time) {
        var date = new Date(time)
        return date.toLocaleString('zh-Hans-CN', {
          // timeZone:['UTC'],
          weekday: 'long',
          hours12: false,
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
          hour: '2-digit',
          minute: '2-digit',
          second: '2-digit'
        })

        // var year = date.getFullYear();
        // /* 在日期格式中，月份是从0开始的，因此要加0
        //  * 使用三元表达式在小于10的前面加0，以达到格式统一  如 09:11:05
        //  * */
        // var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        // var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        // var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        // var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        // var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        // var show_day = new Array('星期一', '星期二', '星期三', '星期四', '星期五', '星期六', '星期日');
        // // 拼接
        // return year + "年" + month + "月" + day + "日 " + hours + ":" + minutes + ":" + seconds;
      }
    },
    mounted() {
      const _this = this
      this.timer = setInterval(() => {
        _this.date = new Date()
      }, 1000)
    },
    beforeDestroy() {
      if (this.timer) {
        clearInterval(this.timer)
      }
    }
  }
</script>

<style lang="less">
  #top-header {
    position: relative;
    width: 100%;
    height: 100px;
    display: flex;
    justify-content: space-between;
    flex-shrink: 0;

    .header-center-decoration {
      width: 40%;
      height: 80px;
      margin-top: 30px;
    }

    .header-left-decoration,
    .header-right-decoration {
      width: 25%;
      height: 60px;
    }

    .center-title {
      position: absolute;
      font-size: 28px;
      font-weight: bold;
      left: 50%;
      top: 15px;
      transform: translateX(-50%);
      color: #40FAEE;
    }

    .title-time {
      position: absolute;
      font-size: 10px;
      //font-weight: bold;
      left: 50%;
      top: 55px;
      color: #6BD4E0;
      transform: translateX(-50%);
    }
  }
</style>
