# 生成根证书
openssl genrsa -out admin.com.key 2048
openssl req -x509 -new -nodes -key  admin.com.key -subj "/C=US/ST=Utah/L=Lehi/O=ABC Company,Inc./OU=IT/CN=admin.com" -days 5000 -out admin.com.crt
# 生成服务证书请求
openssl genrsa -out abc.admin.com.key 2048
openssl req -new -key abc.admin.com.key -subj "/C=US/ST=Utah/L=Lehi/O=ABC Company,Inc./OU=IT/CN=abc.admin.com" -out abc.admin.com.csr
# 用根证书签发服务器证书
openssl x509 -req -in abc.admin.com.csr -CA admin.com.crt -CAkey admin.com.key -CAcreateserial -out abc.admin.com.crt -days 5000 -extfile <(printf "subjectAltName=IP:127.0.0.1,IP:192.168.1.1,IP:192.168.0.252,DNS:abc.admin.com,DNS:mx.admin.com,DNS:localhost")
# extfile可选，可以为服务器证书关联多个IP或者域名

openssl req -x509 -new -nodes -key admin.com.key -sha256 -days 36500 -out admin.com.pem -subj "/C=US/ST=Utah/L=Lehi/O=ABC Company,Inc./OU=IT/CN=admin.com"
openssl x509 -req -in abc.admin.com.csr -CA admin.com.pem -CAkey admin.com.key -CAcreateserial -out abc.admin.com.pem -days 36500 -sha256

