// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'


/* 自定义axios */

//引用axios
import axios from 'axios'
Vue.prototype.axios = axios;

//配置全局的url
axios.defaults.baseURL ='http://api.iot.lcvue.com'
//跨域请求的全局凭证
//axios.defaults.withCredentials = true;

Vue.config.productionTip = false

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
