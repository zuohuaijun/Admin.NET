export interface UploadApiResult {
  id: number;
  // 仓储名称
  bucketName: string;
  //文件名称
  fileName: string;
  //文件后缀
  suffix: string;
  //存储路径
  filePath: string;
  //文件大小KB
  sizeKb: string;
  //文件大小信息-计算后的
  sizeInfo: string;
  //文件地址
  url: string;
}
