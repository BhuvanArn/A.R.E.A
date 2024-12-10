import { CodeTabs } from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/components/CodeTabs.js";
import { Tabs } from "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/components/Tabs.js";
import "/home/andriamanampisoa/Epitech/TECH_3/App-Dev/A.R.E.A/area-doc/node_modules/@vuepress/plugin-markdown-tab/lib/client/styles/vars.css";

export default {
  enhance: ({ app }) => {
    app.component("CodeTabs", CodeTabs);
    app.component("Tabs", Tabs);
  },
};
