#/bin/bash
home=$(dirname `readlink -f $0`)
\cp -f $home/.env.production  $home/../Web/.env.production
cd $home/../Web
cnpm i
cnpm run build
\cp -rf dist $home/nginx/
