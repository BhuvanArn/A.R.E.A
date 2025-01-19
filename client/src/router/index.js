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
    path: '/panel',
    name: 'panel',
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
  },
  {
    path: '/oauth/spotify/callback',
    name: 'spotify-callback',
    component: () => import('../views/SpotifyCallback.vue'),
    props: (route) => ({ code: route.query.code })
  },
  {
    path: '/oauth/dropbox/callback',
    name: 'dropbox-callback',
    component: () => import('../views/DropboxCallback.vue'),
    props: (route) => ({ code: route.query.code })
  },
  {
    path: '/oauth/github/callback',
    name: 'github-callback',
    component: () => import('../views/GithubCallback.vue'),
    props: (route) => ({ code: route.query.code })
  },
  {
    path: '/oauth/reddit/callback',
    name: 'reddit-callback',
    component: () => import('../views/RedditCallback.vue'),
    props: (route) => ({ code: route.query.code })
  },
  {
    path: '/client.apk',
    name: 'AreaApk',
    component: () => import('../views/apk.vue')
  },
  {
    path: '/service-details/:serviceName',
    name: 'service-details',
    component: () => import('../views/ServiceDetails.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router