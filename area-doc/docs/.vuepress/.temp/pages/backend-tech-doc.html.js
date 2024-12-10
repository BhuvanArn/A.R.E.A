import comp from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/backend-tech-doc.html.vue"
const data = JSON.parse("{\"path\":\"/backend-tech-doc.html\",\"title\":\"\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[],\"git\":{},\"filePathRelative\":\"backend-tech-doc.md\"}")
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
