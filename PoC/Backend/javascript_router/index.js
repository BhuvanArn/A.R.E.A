const express = require('express');
const { writeSync, closeSync } = require('fs');

const port = process.env.PORT || 8082;

(() =>
{
    const expressApp = express();

    // single testing route
    expressApp.get('/', (req, res) => {
        res.status(200).send('hello from the server');
    });

    process.on('SIGINT', function() {
        process.stdout.write('\b\b  \b\b');

        writeSync(process.stdout.fd, 'Server stopped.\n');
        closeSync(process.stdout.fd);

        process.exit(0);
    });

    expressApp.listen(port, () => {
        writeSync(process.stdout.fd, 'Server started on port ' + port + '\n');
    });
})();
