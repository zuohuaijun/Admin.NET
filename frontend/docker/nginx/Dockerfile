# build
FROM node:latest AS builder

# 切换编译目录
WORKDIR /build
COPY ./ ./
# 编译项目 测试
RUN yarn config set registry https://registry.npm.taobao.org/ \
  && yarn install \
  && yarn run build:development

# 编译项目 正式
#RUN yarn config set registry https://registry.npm.taobao.org/ \
#  && yarn install \
#  && yarn run build

# web
FROM nginx:alpine

# 时区
ENV TZ=Asia/Shanghai
RUN echo "http://mirrors.aliyun.com/alpine/v3.4/main/" > /etc/apk/repositories \
  && apk --no-cache add tzdata zeromq \
  && ln -snf /usr/share/zoneinfo/$TZ /etc/localtime \
  && echo '$TZ' > /etc/timezone

COPY ./docker/nginx/logs /var/log/nginx
COPY ./docker/nginx/vhost /etc/nginx/conf.d/
COPY ./docker/nginx/nginx.conf /etc/nginx/nginx.conf

# 拷贝项目
COPY --from=builder /build/dist/ /var/www/html