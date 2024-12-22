const express = require('express');
const swaggerUi = require('swagger-ui-express');
const yaml = require('js-yaml');
const fs = require('fs');

function buildSwaggerDoc() {
  const routesPath = 'routes.yml';
  const routes = yaml.load(fs.readFileSync(routesPath, 'utf8')).routes;

  const swaggerDoc = {
    openapi: '3.0.0',
    info: {
      title: 'AREA - LastChance Backend API Documentation',
      version: '1.0.0',
      description: 'API documentation for the LastChance backend of the AREA project',
    },
    paths: {},
    tags: [
      { name: 'Authentication Service', description: 'Endpoints for user authentication' },
      { name: 'Area Service', description: 'Endpoints for area-related operations' },
    ],
  };

  routes
    .filter((route) => !route.name.startsWith('swagger'))
    .forEach((route) => {
      const path = route.realPath || route.condition
        .replace(/^\^/, '') // Remove '^'
        .replace(/\$$/, '') // Remove '$'
        .replace(/\(\[\^\/\]\+\)/g, '{param}');
        // replace is used when the realPath is not defined in the route
        // (should be defined when the route has params in the path)

      const method = (route.method || 'get').toLowerCase();
      if (!swaggerDoc.paths[path]) swaggerDoc.paths[path] = {};

      const tag = route.name.startsWith('auth') ? 'Authentication Service' : 'Area Service';

      // extract params from path
      const pathParams = [];
      const paramMatches = [...path.matchAll(/\{([^}]+)\}/g)];
      if (paramMatches) {
        paramMatches.forEach((match) => {
          pathParams.push({
            name: match[1],
            in: 'path',
            required: true,
            schema: {
              type: 'string',
            },
          });
        });
      }

      swaggerDoc.paths[path][method] = {
        tags: [tag],
        summary: route.summary || `Route for ${route.name}`,
        description: route.description || `Handles ${route.name}`,
        parameters: [...(route.parameters || []), ...pathParams],
        requestBody: route.requestBody || undefined,
        responses: route.responses || {
          200: { description: 'OK' },
        },
      };
    });

  return swaggerDoc;
}

const app = express();

app.get('/swagger/swagger.json', (req, res) => {
  const swaggerDoc = buildSwaggerDoc();
  res.json(swaggerDoc);
});

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(null, { swaggerUrl: '/swagger/swagger.json' }));

const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Swagger doc is running at http://localhost:${PORT}/swagger`);
});
