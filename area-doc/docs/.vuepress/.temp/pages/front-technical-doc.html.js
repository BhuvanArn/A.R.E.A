import comp from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/front-technical-doc.html.vue"
const data = JSON.parse("{\"path\":\"/front-technical-doc.html\",\"title\":\"Technical documentation\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[],\"git\":{},\"filePathRelative\":\"front-technical-doc.md\"}")
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
