# Guide

You will find in this documentation all the information you need to understand and customize the frontend of the Area project.

The frontend of the Area project is a Vue.js application. The frontend is divided into several parts, such as the components, the pages and so on.

## Working directory

The frontend of the Area project is located in the `client` directory, from the root of the project directory. This directory contains all the files related to the frontend.

## Components

The components of the frontend are located in the `client/src/components` directory, from the root of the project directory. You will find in this directory all the components used in the frontend. Feel free to see the current componenets. You can create new components in this directory.

## Views

The views of the frontend are located in the `client/src/views` directory, from the root of the project directory. You will find in this directory all the views used in the frontend. Feel free to see the current views. You can create new views in this directory.

Basically each view is a page of the application. For example if you want to create a new page, "MyPage", you can create a new file `MyPage.vue` in the `client/src/views` directory.
All the HTML, CSS and JavaScript code of the page will be in this file.

```html
<template>
    <body>
        <div>
            MyPage
        </div>
    </body>
</template>

<script>
</script>

<style>
</style>
```

## Router

The router of the frontend is located in the `client/src/router` directory, from the root of the project directory. You will find in this directory the file `index.js` which contains the routes of the frontend.
For our example "MyPage", you will have to add a new route in this file.

```js
{
    path: '/my-page',
    name: 'my-page',
    component: () => import('../views/MyPage.vue')
}
```

Launch the frontend with the command `npm run serve` and go to the URL displayed in the console.

Navigate to the URL `http://localhost:your-port/my-page` to see your new page. Replace "**your-port**" by the port displayed in the console.

![MyPage](/assets/mypage.png)

## To go further

You can find more information about the Vue.js framework in the [official documentation](https://vuejs.org/guide/introduction.html).

