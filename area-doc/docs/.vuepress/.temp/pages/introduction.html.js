import comp from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/introduction.html.vue"
const data = JSON.parse("{\"path\":\"/introduction.html\",\"title\":\"Introduction\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[{\"level\":2,\"title\":\"What is the Area project ?\",\"slug\":\"what-is-the-area-project\",\"link\":\"#what-is-the-area-project\",\"children\":[]},{\"level\":2,\"title\":\"Techinical stack\",\"slug\":\"techinical-stack\",\"link\":\"#techinical-stack\",\"children\":[{\"level\":3,\"title\":\"Backend\",\"slug\":\"backend\",\"link\":\"#backend\",\"children\":[]},{\"level\":3,\"title\":\"Frontend\",\"slug\":\"frontend\",\"link\":\"#frontend\",\"children\":[]}]},{\"level\":2,\"title\":\"Documentation\",\"slug\":\"documentation\",\"link\":\"#documentation\",\"children\":[]}],\"git\":{},\"filePathRelative\":\"introduction.md\"}")
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
