#!/usr/bin/env bash

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

cd client

# Check npm installed
if ! [ -x "$(command -v npm)" ]; then
  echo -e "[${RED}ERROR${NC}] npm is not installed. Please install npm." >&2
  exit 84
fi

echo -e "[${YELLOW}INFO${NC}] Installing dependencies..."
npm install > /dev/null 2>&1

# Add Android/iOS platforms
echo -e "[${YELLOW}INFO${NC}] Adding Android and iOS platforms..."
npx cap add android > /dev/null 2>&1
npx cap add ios > /dev/null 2>&1

# Build project
echo -e "[${YELLOW}INFO${NC}] Building project..."
npm run build > /dev/null 2>&1

# Sync project with the native platforms
echo -e "[${YELLOW}INFO${NC}] Syncing project with native platforms..."
npx cap sync > /dev/null 2>&1

echo -e "[${GREEN}SUCCESS${NC}] Setup complete."

echo -e "[${YELLOW}INFO${NC}] To run the app on Android, open client/android/ folder in Android Studio and run the app."
