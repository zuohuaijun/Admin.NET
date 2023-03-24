#!/bin/sh

currPath=$(pwd)
parentPath=$(dirname "$currPath")
apiServicesPath=${parentPath}/src/api-services/

echo "================================ 生成目录 ${apiServicesPath} ================================"

# 判断目录是否存在
if test -d "$apiServicesPath"; then
  echo "================================ 删除目录 api-services ================================"
  rm -rf "${apiServicesPath}"
fi

echo "================================ 开始生成 api-services ================================"

java -jar "${currPath}"/swagger-codegen-cli.jar generate -i http://localhost:5005/swagger/All%20Groups/swagger.json -l typescript-axios -o "${apiServicesPath}"

rm -rf "${apiServicesPath}".swagger-codegen
rm -f "${apiServicesPath}".gitignore
rm -f "${apiServicesPath}".npmignore
rm -f "${apiServicesPath}".swagger-codegen-ignore
rm -f "${apiServicesPath}"git_push.sh
rm -f "${apiServicesPath}"package.json
rm -f "${apiServicesPath}"README.md
rm -f "${apiServicesPath}"tsconfig.json

echo "================================ 生成结束 ================================"
