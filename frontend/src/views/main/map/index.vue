<template style="margin: 0;">
  <page-view>
    <div id="map"></div>
  </page-view>
</template>

<script>
  import L from 'leaflet'
  import {
    LMap,
    LTileLayer,
    LMarker
  } from 'vue2-leaflet'
  import 'leaflet/dist/leaflet.css'

  export default {
    name: 'Map',
    components: {
      LMap,
      LTileLayer,
      LMarker
    },
    data() {
      return {
        map: ''
      }
    },
    mounted() {
      this.initMap()
    },
    methods: {
      initMap() {
        this.map = L.map('map', {
          center: [39.063076, 117.06969],
          zoom: 15,
          zoomControl: false, // 禁用 + - 按钮
          doubleClickZoom: false, // 禁用双击放大
          attributionControl: false // 移除右下角leaflet标识
        })
        this.baseLayer = L.tileLayer(
          'http://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}', {
            subdomains: '1234'
          })
        this.map.addLayer(this.baseLayer)
      }
    }
  }
</script>
<style lang="less">
  #map {
    width: 100%;
    height: calc(100vh);
    z-index: 1;
  }
</style>
