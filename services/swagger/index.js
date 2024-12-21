// Import required modules
const express = require('express');
const swaggerUi = require('swagger-ui-express');
const YAML = require('yamljs');

defaultOpenAPIDoc = {
  openapi: '3.0.0',
  info: {
    title: 'API Documentation',
    version: '1.0.0',
    description: 'Auto-generated API documentation',
  },
  paths: {},
};
const app = express();

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(defaultOpenAPIDoc));

const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Swagger doc is running at http://localhost:${PORT}/swagger`);
});
