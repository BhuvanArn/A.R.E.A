# area-test

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

## Running the Mobile Part

To set up and run the mobile part of the project, follow these steps:

1. **Install the dependencies**:
    ```sh
    npm install
    ```

2. **Add Android and iOS platforms** (if not already added, means if android and ios folders are not present):
    ```sh
    npx cap add android
    npx cap add ios
    ```

3. **Build the project**:
    ```sh
    npm run build
    ```

4. **Copy the web assets to the native platforms**:
    ```sh
    npx cap copy
    ```

5. **Open the native IDEs to run the app**:
    - For Android:
        ```sh
        npx cap open android
        ```
    - For iOS:
        ```sh
        npx cap open ios
        ```

Follow the instructions in the respective IDEs (Android Studio for Android and Xcode for iOS) to build and run the app on a device or emulator.

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
