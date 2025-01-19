FROM node:20

# Install Android SDK
RUN apt-get update && apt-get install -y \
    wget \
    unzip \
    openjdk-17-jdk \
    lib32stdc++6 \
    lib32z1 \
    && apt-get clean

# Install Android CLI
RUN wget https://dl.google.com/android/repository/commandlinetools-linux-9477386_latest.zip -O android-sdk.zip
RUN mkdir -p /usr/local/android-sdk/cmdline-tools/latest
RUN unzip android-sdk.zip -d /usr/local/android-sdk/cmdline-tools/latest
RUN rm android-sdk.zip

RUN mv /usr/local/android-sdk/cmdline-tools/latest/cmdline-tools/* /usr/local/android-sdk/cmdline-tools/latest/ && \
    rmdir /usr/local/android-sdk/cmdline-tools/latest/cmdline-tools

ENV ANDROID_HOME /usr/local/android-sdk
ENV PATH $ANDROID_HOME/cmdline-tools/latest/bin:$ANDROID_HOME/platform-tools:$ANDROID_HOME/build-tools/34.0.0:$PATH

# Accept SDK licenses
RUN yes | sdkmanager --licenses

RUN sdkmanager "platform-tools" "platforms;android-34" "build-tools;34.0.0"

WORKDIR /app

COPY . .
COPY android/app/my-release-key.jks android/

RUN npm install
RUN npm run build

# Sync Capacitor project platforms
RUN npx cap sync

# Get keystore for release
ARG KEYSTORE_PASSWORD
ARG KEY_ALIAS
ARG KEY_PASSWORD

# Build APK
RUN cd android && ./gradlew clean && ./gradlew assembleRelease

# Share APK
RUN mkdir -p /shared
RUN cp android/app/build/outputs/apk/release/app-release.apk /shared/client.apk

CMD ["echo", "[SUCCESS] APK generated at /shared/client.apk"]

