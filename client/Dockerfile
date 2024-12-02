FROM node:20

WORKDIR /app

COPY . .

RUN npm i

EXPOSE $PORT

CMD ["npm", "run", "serve"]
