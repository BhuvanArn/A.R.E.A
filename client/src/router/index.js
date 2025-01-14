import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('@/views/AreaHome.vue')
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/Login.vue')
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('@/views/Register.vue')
  },
  {
    path: '/services',
    name: 'services',
    component: () => import('@/views/ServicePage.vue')
  },
  {
    path: '/reset-password',
    name: 'reset-password',
    component: () => import('../views/ResetPwd.vue')
  },
  {
    path: '/forgot-password',
    name: 'forgot-password',
    component: () => import('../views/ForgotPwd.vue')
  },
  {
    path: '/mobile-services',
    name: 'mobile-services',
    component: () => import('../views/MobileServicePage.vue')
  },
  {
    path: '/profile',
    name: 'profile',
    component: () => import('../views/ProfilePage.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
