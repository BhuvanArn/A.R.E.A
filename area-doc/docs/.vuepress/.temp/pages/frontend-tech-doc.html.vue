<template><div><h1 id="guide" tabindex="-1"><a class="header-anchor" href="#guide"><span>Guide</span></a></h1>
<p>You will find in this documentation all the information you need to understand and customize the frontend of the Area project.</p>
<p>The frontend of the Area project is a Vue.js application. The frontend is divided into several parts, such as the components, the pages and so on.</p>
<h2 id="working-directory" tabindex="-1"><a class="header-anchor" href="#working-directory"><span>Working directory</span></a></h2>
<p>The frontend of the Area project is located in the <code v-pre>client</code> directory, from the root of the project directory. This directory contains all the files related to the frontend.</p>
<h2 id="components" tabindex="-1"><a class="header-anchor" href="#components"><span>Components</span></a></h2>
<p>The components of the frontend are located in the <code v-pre>client/src/components</code> directory, from the root of the project directory. You will find in this directory all the components used in the frontend. Feel free to see the current componenets. You can create new components in this directory.</p>
<h2 id="views" tabindex="-1"><a class="header-anchor" href="#views"><span>Views</span></a></h2>
<p>The views of the frontend are located in the <code v-pre>client/src/views</code> directory, from the root of the project directory. You will find in this directory all the views used in the frontend. Feel free to see the current views. You can create new views in this directory.</p>
<p>Basically each view is a page of the application. For example if you want to create a new page, &quot;MyPage&quot;, you can create a new file <code v-pre>MyPage.vue</code> in the <code v-pre>client/src/views</code> directory.
All the HTML, CSS and JavaScript code of the page will be in this file.</p>
<div class="language-html line-numbers-mode" data-highlighter="prismjs" data-ext="html" data-title="html"><pre v-pre><code><span class="line"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>template</span><span class="token punctuation">></span></span></span>
<span class="line">    <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>body</span><span class="token punctuation">></span></span></span>
<span class="line">        <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>div</span><span class="token punctuation">></span></span></span>
<span class="line">            MyPage</span>
<span class="line">        <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>div</span><span class="token punctuation">></span></span></span>
<span class="line">    <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>body</span><span class="token punctuation">></span></span></span>
<span class="line"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>template</span><span class="token punctuation">></span></span></span>
<span class="line"></span>
<span class="line"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>script</span><span class="token punctuation">></span></span><span class="token script"><span class="token language-javascript"></span>
<span class="line"></span></span><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>script</span><span class="token punctuation">></span></span></span>
<span class="line"></span>
<span class="line"><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>style</span><span class="token punctuation">></span></span><span class="token style"><span class="token language-css"></span>
<span class="line"></span></span><span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>style</span><span class="token punctuation">></span></span></span>
<span class="line"></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="router" tabindex="-1"><a class="header-anchor" href="#router"><span>Router</span></a></h2>
<p>The router of the frontend is located in the <code v-pre>client/src/router</code> directory, from the root of the project directory. You will find in this directory the file <code v-pre>index.js</code> which contains the routes of the frontend.
For our example &quot;MyPage&quot;, you will have to add a new route in this file.</p>
<div class="language-javascript line-numbers-mode" data-highlighter="prismjs" data-ext="js" data-title="js"><pre v-pre><code><span class="line"><span class="token punctuation">{</span></span>
<span class="line">    <span class="token literal-property property">path</span><span class="token operator">:</span> <span class="token string">'/my-page'</span><span class="token punctuation">,</span></span>
<span class="line">    <span class="token literal-property property">name</span><span class="token operator">:</span> <span class="token string">'my-page'</span><span class="token punctuation">,</span></span>
<span class="line">    <span class="token function-variable function">component</span><span class="token operator">:</span> <span class="token punctuation">(</span><span class="token punctuation">)</span> <span class="token operator">=></span> <span class="token keyword">import</span><span class="token punctuation">(</span><span class="token string">'../views/MyPage.vue'</span><span class="token punctuation">)</span></span>
<span class="line"><span class="token punctuation">}</span></span>
<span class="line"></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p>Launch the frontend with the command <code v-pre>npm run serve</code> and go to the URL displayed in the console.</p>
<p>Navigate to the URL <code v-pre>http://localhost:your-port/my-page</code> to see your new page. Replace &quot;<strong>your-port</strong>&quot; by the port displayed in the console.</p>
<p><img src="/assets/mypage.png" alt="MyPage"></p>
<h2 id="to-go-further" tabindex="-1"><a class="header-anchor" href="#to-go-further"><span>To go further</span></a></h2>
<p>You can find more information about the Vue.js framework in the <a href="https://vuejs.org/guide/introduction.html" target="_blank" rel="noopener noreferrer">official documentation</a>.</p>
</div></template>


