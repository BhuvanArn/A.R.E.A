import comp from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/index.html.vue"
const data = JSON.parse("{\"path\":\"/\",\"title\":\"Home\",\"lang\":\"en-US\",\"frontmatter\":{\"home\":true,\"title\":\"Home\",\"heroImage\":\"/assets/logo.png\",\"actions\":[{\"text\":\"Get Started\",\"link\":\"/get-started\",\"type\":\"primary\"},{\"text\":\"Introduction\",\"link\":\"/introduction\",\"type\":\"secondary\"}],\"features\":[{\"title\":\"Frontend technical documentation\",\"details\":\"The frontend technical documentation for the Area project. Detailed information about the frontend architecture, components, and more.\"},{\"title\":\"Frontend user documentation\",\"details\":\"The frontend user documentation for the Area project. Detailed information about how to use the Area project.\"},{\"title\":\"Backend technical documentation\",\"details\":\"The backend technical documentation for the Area project. Detailed information about the backend architecture. Contains information about the API, database, and more.\"}],\"footer\":\"MIT Licensed | Copyright © 2018-present VuePress Community\"},\"headers\":[],\"git\":{},\"filePathRelative\":\"README.md\"}")
export { comp, data }

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updatePageData) {
    __VUE_HMR_RUNTIME__.updatePageData(data)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ data }) => {
    __VUE_HMR_RUNTIME__.updatePageData(data)
  })
}
