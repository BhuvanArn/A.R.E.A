import comp from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/frontend-tech-doc.html.vue"
const data = JSON.parse("{\"path\":\"/frontend-tech-doc.html\",\"title\":\"Guide\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[{\"level\":2,\"title\":\"Working directory\",\"slug\":\"working-directory\",\"link\":\"#working-directory\",\"children\":[]},{\"level\":2,\"title\":\"Components\",\"slug\":\"components\",\"link\":\"#components\",\"children\":[]},{\"level\":2,\"title\":\"Views\",\"slug\":\"views\",\"link\":\"#views\",\"children\":[]},{\"level\":2,\"title\":\"Router\",\"slug\":\"router\",\"link\":\"#router\",\"children\":[]}],\"git\":{},\"filePathRelative\":\"frontend-tech-doc.md\"}")
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
