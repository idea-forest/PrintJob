module.exports = {
    apps: [
      {
        name: 'your-app-name',
        script: 'node_modules/.bin/next',
        args: 'start',
        instances: 1,
        autorestart: true,
        watch: false,
        max_memory_restart: '1G',
        env: {
          NODE_ENV: 'production',
        },
      },
    ],
  };
  