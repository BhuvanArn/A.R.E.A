import comp from "/home/barnaud/.Delivery/tek3/DEV/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/backend-tech-doc.html.vue"
const data = JSON.parse("{\"path\":\"/backend-tech-doc.html\",\"title\":\"\",\"lang\":\"en-US\",\"frontmatter\":{},\"headers\":[],\"git\":{\"updatedTime\":1733852506000,\"contributors\":[{\"name\":\"Andriamanampisoa\",\"username\":\"Andriamanampisoa\",\"email\":\"toavina.andriamanampisoa@epitech.eu\",\"commits\":3,\"url\":\"https://github.com/Andriamanampisoa\"},{\"name\":\"StevenGandon\",\"username\":\"StevenGandon\",\"email\":\"steven.gandon@epitech.eu\",\"commits\":1,\"url\":\"https://github.com/StevenGandon\"}]},\"filePathRelative\":\"backend-tech-doc.md\"}")
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
