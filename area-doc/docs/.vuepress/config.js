import { defaultTheme } from '@vuepress/theme-default'
import { defineUserConfig } from 'vuepress/cli'
import { viteBundler } from '@vuepress/bundler-vite'

export default defineUserConfig({
  lang: 'en-US',

  title: 'Area documentation',
  description: 'Documentation for the Area project',

  theme: defaultTheme({
    logo: '/assets/logo.png',

    navbar: ['/', '/get-started'],
  }),

  bundler: viteBundler(),
})
