export const redirects = JSON.parse("{}")

export const routes = Object.fromEntries([
  ["/", { loader: () => import(/* webpackChunkName: "index.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/index.html.js"), meta: {"title":"Home"} }],
  ["/get-started.html", { loader: () => import(/* webpackChunkName: "get-started.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/get-started.html.js"), meta: {"title":"Get Started"} }],
  ["/introduction.html", { loader: () => import(/* webpackChunkName: "introduction.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/introduction.html.js"), meta: {"title":"Introduction"} }],
  ["/technical-doc.html", { loader: () => import(/* webpackChunkName: "technical-doc.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/technical-doc.html.js"), meta: {"title":"Technical documentation"} }],
  ["/404.html", { loader: () => import(/* webpackChunkName: "404.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/404.html.js"), meta: {"title":""} }],
  ["/frontend-tech-doc.html", { loader: () => import(/* webpackChunkName: "frontend-tech-doc.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/frontend-tech-doc.html.js"), meta: {"title":"Guide"} }],
  ["/backend-tech-doc.html", { loader: () => import(/* webpackChunkName: "backend-tech-doc.html" */"/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/docs/.vuepress/.temp/pages/backend-tech-doc.html.js"), meta: {"title":""} }],
]);

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updateRoutes) {
    __VUE_HMR_RUNTIME__.updateRoutes(routes)
  }
  if (__VUE_HMR_RUNTIME__.updateRedirects) {
    __VUE_HMR_RUNTIME__.updateRedirects(redirects)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ routes, redirects }) => {
    __VUE_HMR_RUNTIME__.updateRoutes(routes)
    __VUE_HMR_RUNTIME__.updateRedirects(redirects)
  })
}
