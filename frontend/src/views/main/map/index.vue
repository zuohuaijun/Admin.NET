<template>
  <div>
    <div id="map">
      <a-drawer
        title="图元属性"
        placement="right"
        :width="600"
        :closable="false"
        :visible="drawerVisible"
        :get-container="false"
        :wrap-style="{ position: 'absolute' }"
        @close="onCloseDrawer">
        <p>{{ geosjonData }}</p>
      </a-drawer>
    </div>
  </div>
</template>

<script>
  import L from 'leaflet'
  import {
    LMap,
    LTileLayer,
    LMarker,
    LControlLayers
  } from 'vue2-leaflet'
  import 'leaflet/dist/leaflet.css'

  import 'leaflet.pm'
  import 'leaflet.pm/dist/leaflet.pm.css'

  export default {
    name: 'Map',
    components: {
      LMap,
      LTileLayer,
      LMarker,
      LControlLayers
    },
    data() {
      return {
        map: '',
        drawerVisible: false,
        geosjonData: ''
      }
    },
    mounted() {
      this.initMap()
      this.initMapPm()
      this.getlatLngs()
    },
    methods: {
      initMap() {
        this.map = L.map('map', {
          center: [39.064576, 117.06969],
          zoom: 15,
          zoomControl: true,
          doubleClickZoom: true,
          attributionControl: false // 移除右下角leaflet标识
        })
        var mapLayers = {
          '高德/卫星': L.layerGroup([
            L.tileLayer('https://webst0{s}.is.autonavi.com/appmaptile?style=6&x={x}&y={y}&z={z}', {
              maxZoom: 20,
              maxNativeZoom: 18,
              minZoom: 3,
              attribution: '高德地图 AutoNavi.com',
              subdomains: '1234'
            }),
            L.tileLayer('https://webst0{s}.is.autonavi.com/appmaptile?style=8&x={x}&y={y}&z={z}', {
              maxZoom: 20,
              maxNativeZoom: 18,
              minZoom: 3,
              attribution: '高德地图 AutoNavi.com',
              subdomains: '1234',
              opacity: 0.5
            })
          ]).addTo(this.map),
          '高德/街道': L.tileLayer(
            'https://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}', {
              maxZoom: 20,
              maxNativeZoom: 18,
              minZoom: 3,
              attribution: '高德地图 AutoNavi.com',
              subdomains: '1234'
            }),
          '智图/街道': L.tileLayer(
            'https://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineStreetPurplishBlue/MapServer/tile/{z}/{y}/{x}', {
              maxZoom: 20,
              maxNativeZoom: 16,
              minZoom: 3,
              attribution: '智图 GeoQ.cn'
            })
        }
        L.control.layers(mapLayers, {}, {
          position: 'topright',
          collapsed: true
        }).addTo(this.map)
      },
      initMapPm() {
        this.map.pm.addControls({
          position: 'topleft',
          drawPolygon: true, // 添加绘制多边形
          drawMarker: true, // 添加按钮以绘制标记
          drawCircleMarker: true, // 添加按钮以绘制圆形标记
          drawPolyline: true, // 添加按钮绘制线条
          drawRectangle: true, // 添加按钮绘制矩形
          drawCircle: true, //  添加按钮绘制圆圈
          editMode: true, //  添加按钮编辑多边形
          dragMode: true, //   添加按钮拖动多边形
          cutPolygon: true, // 添加一个按钮以删除图层里面的部分内容
          removalMode: true // 清除图层
        })
        // 设置绘制后的线条颜色等
        this.map.pm.setPathOptions({
          color: 'orange',
          fillColor: 'green',
          fillOpacity: 0.4
        })
        this.map.pm.setLang('zh') // 设置语言  en, de, it, ru, ro, es, fr, pt_br, zh , nl
      },
      getlatLngs() {
        this.map.on('pm:drawstart', e => {})
        this.map.on('pm:drawend', e => {})
        this.map.on('pm:create', e => {
          console.log(e.layer._latlngs[0], '绘制坐标')
          this.GetGeoJson()
        })
      },
      // 生成geojson数据
      GetGeoJson() {
        var layerArray = []
        this.map.eachLayer(function(layer) {
          if (layer.pm !== 'undefined' && layer.pm != null && layer.pm !== '') {
            if (layer.pm._enabled === false && layer.pm.options.draggable === true) {
              layerArray.push(layer)
            }
          }
        })
        var geojson = L.layerGroup(layerArray).toGeoJSON()
        for (var n = 0; n < geojson.features.length; n++) {
          var nowJson = JSON.stringify(geojson.features[n])
          for (var m = n + 1; m < geojson.features.length; m++) {
            var nextJson = JSON.stringify(geojson.features[m])
            if (nowJson === nextJson) {
              geojson.features.splice(n, 1)
            }
          }
        }
        this.drawerVisible = true
        this.geosjonData = geojson
        return geojson
        // 重新加载geojson数据
        // L.geoJson(geojson).addTo(map);
      },
      onCloseDrawer() {
        this.drawerVisible = false
      }
    }
  }
</script>
<style lang="less">
  #map {
    width: 100%;
    height: calc(100vh);
    z-index: 1;
    margin-left: -24px;
    margin-top: -24px;
    position: fixed;
  }

  .leaflet-right .leaflet-control {
    position: fixed;
    right: -5px;
  }
</style>
