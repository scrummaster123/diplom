import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5182', // адрес бэкенда
        changeOrigin: true,
        secure: false, // если бэк не использует HTTPS
        rewrite: (path) => path.replace(/^\/api/, ''), // убираем /api из пути
      },
    },
  },

});