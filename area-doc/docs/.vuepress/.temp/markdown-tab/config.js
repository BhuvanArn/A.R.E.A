import { CodeTabs } from "/home/barnaud/.Delivery/tek3/DEV/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/components/CodeTabs.js";
import { Tabs } from "/home/barnaud/.Delivery/tek3/DEV/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/components/Tabs.js";
import "/home/barnaud/.Delivery/tek3/DEV/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/styles/vars.css";

export default {
  enhance: ({ app }) => {
    app.component("CodeTabs", CodeTabs);
    app.component("Tabs", Tabs);
  },
};
