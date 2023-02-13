openssl genrsa -out rootCA.key 2048
openssl req -x509 -new -nodes -key rootCA.key -sha256 -days 36500 -out rootCA.pem -subj "/C=US/ST=Utah/L=Lehi/O=Your Company, Inc./OU=IT/CN=rootca.com"
openssl genrsa -out server.key 2048
openssl req -new -key server.key -out server.csr -subj "/C=US/ST=Utah/L=Lehi/O=Your Company, Inc./OU=IT/CN=test-redis"
openssl x509 -req -in server.csr -CA rootCA.pem -CAkey rootCA.key -CAcreateserial -out server.pem -days 36500 -sha256

