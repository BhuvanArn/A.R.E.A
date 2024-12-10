import { defaultTheme } from '@vuepress/theme-default'
import { defineUserConfig } from 'vuepress/cli'
import { viteBundler } from '@vuepress/bundler-vite'

import VuepressApiPlayground from 'vuepress-api-playground'

export default defineUserConfig({
  lang: 'en-US',

  title: 'Area documentation',
  description: 'Documentation for the Area project',

  theme: defaultTheme({
    logo: '/assets/logo.png',

    navbar: ['/', '/introduction', '/technical-doc'],
  }),

  bundler: viteBundler(),

  enhance({ app }) {
    app.component('VuepressApiPlayground', VuepressApiPlayground)
  },
})
