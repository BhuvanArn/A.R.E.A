import comp from "/home/barnaud/.Delivery/tek3/DEV/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/technical-doc.html.vue"
const data = JSON.parse("{\"path\":\"/technical-doc.html\",\"title\":\"Technical documentation\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[{\"level\":2,\"title\":\"Backend technical documentation\",\"slug\":\"backend-technical-documentation\",\"link\":\"#backend-technical-documentation\",\"children\":[]},{\"level\":2,\"title\":\"Frontend technical documentation\",\"slug\":\"frontend-technical-documentation\",\"link\":\"#frontend-technical-documentation\",\"children\":[]}],\"git\":{\"updatedTime\":1733852506000,\"contributors\":[{\"name\":\"Andriamanampisoa\",\"username\":\"Andriamanampisoa\",\"email\":\"toavina.andriamanampisoa@epitech.eu\",\"commits\":1,\"url\":\"https://github.com/Andriamanampisoa\"}]},\"filePathRelative\":\"technical-doc.md\"}")
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
