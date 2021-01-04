import Vue from 'vue'
import Router from 'vue-router'

import Index from '@/components/Index'
import admin_index from '@/view/admin/index'
import admin_student_list from '@/view/admin/student/list'
import admin_student_upload from '@/view/admin/student/upload'



import ElementUi from 'element-ui'	//引入elementui组件
import 'element-ui/lib/theme-chalk/index.css'		//引入样式文件
Vue.use(ElementUi)	//使用

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Index',
      component: Index
    },
    {
      path: '/admin',
      name: 'admin',
      component: admin_index
    },
    {
      path: '/admin/student',
      name: 'admin/student',
      component: admin_student_list
    },
    {
      path: '/admin/student/upload',
      name: 'admin/student/upload',
      component: admin_student_upload
    }
  ]
})
