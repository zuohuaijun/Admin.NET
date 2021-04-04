using System.IO;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 代码生成配置
    /// </summary>
    public class Config
	{

		/// <summary>
		/// 路径分离（不通的机器，取不同的路径）
		/// </summary>
		public static string FILE_SEP = ""; // File.Separator;

		/// <summary>
		/// 存放vm模板位置
		/// </summary>
		public static string templatePath = "template" + FILE_SEP;

		/// <summary>
		/// 主键标识
		/// </summary>
		public static string DB_TABLE_COM_KRY = "PRI";

		/// <summary>
		/// 模块名（一般为modular，无特殊要求一般不改）
		/// </summary>
		public static string MODULAR_NAME = "modular";

		/// <summary>
		/// 本项目生成时是否覆盖
		/// </summary>
		public const bool FLAG = false;

		/// <summary>
		/// 大模块名称（生成到代码中哪个模块下）
		/// </summary>
		public static string BASE_MODULAR_NAME = "xiaonuo-main";

		/// <summary>
		/// java文件夹
		/// </summary>
		public static string BASE_JAVA_PAHT = FILE_SEP + "src" + FILE_SEP + "main" + FILE_SEP + "java" + FILE_SEP;

		/// <summary>
		/// vue文件夹
		/// </summary>
		public static string BASE_VUE_PAHT = FILE_SEP + "_web" + FILE_SEP + "src" + FILE_SEP;

		/// <summary>
		/// sql文件夹
		/// </summary>
		public static string BASE_SQL_PAHT = FILE_SEP + "_sql" + FILE_SEP;

		/// <summary>
		/// 代码生成路径
		/// </summary>
		public static string controllerPath;
		public static string entityPath;
		public static string enumsPath;
		public static string mapperPath;
		public static string mappingPath;
		public static string paramPath;
		public static string servicePath;
		public static string serviceImplPath;
		public static string manageJsPath;
		public static string vueIndexPath;
		public static string vueAddFromPath;
		public static string vueEditFromPath;
		public static string mysqlSqlPath;
		public static string oracleSqlPath;

		/// <summary>
		/// 各个代码存放路径文件夹
		/// </summary>
		public static string[] xnCodeGenFilePath(string busName, string packageName)
		{
			string packageNameString = packageName.Replace(".", FILE_SEP) + FILE_SEP;
			controllerPath = BASE_JAVA_PAHT + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "controller" + FILE_SEP;
			entityPath = BASE_JAVA_PAHT + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "entity" + FILE_SEP;
			enumsPath = BASE_JAVA_PAHT + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "enums" + FILE_SEP;
			mapperPath = BASE_JAVA_PAHT + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "mapper" + FILE_SEP;
			mappingPath = mapperPath + FILE_SEP + "mapping" + FILE_SEP;
			paramPath = BASE_JAVA_PAHT + FILE_SEP + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "param" + FILE_SEP;
			servicePath = BASE_JAVA_PAHT + FILE_SEP + packageNameString + MODULAR_NAME + FILE_SEP + busName + FILE_SEP + "service" + FILE_SEP;
			serviceImplPath = servicePath + FILE_SEP + "impl" + FILE_SEP;
			manageJsPath = BASE_VUE_PAHT + FILE_SEP + "api" + FILE_SEP + MODULAR_NAME + FILE_SEP + "main" + FILE_SEP + busName + FILE_SEP;
			vueIndexPath = BASE_VUE_PAHT + FILE_SEP + "views" + FILE_SEP + "main" + FILE_SEP + busName + FILE_SEP;
			vueAddFromPath = BASE_VUE_PAHT + FILE_SEP + "views" + FILE_SEP + "main" + FILE_SEP + busName + FILE_SEP;
			vueEditFromPath = BASE_VUE_PAHT + FILE_SEP + "views" + FILE_SEP + "main" + FILE_SEP + busName + FILE_SEP;
			mysqlSqlPath = BASE_SQL_PAHT;
			oracleSqlPath = BASE_SQL_PAHT;
			return new string[] { controllerPath, entityPath, enumsPath, mapperPath, mappingPath, paramPath, servicePath, serviceImplPath, manageJsPath, vueIndexPath, vueAddFromPath, vueEditFromPath, mysqlSqlPath, oracleSqlPath };
		}

		/// <summary>
		/// 模板文件
		/// </summary>
		public static string[] xnCodeGenTempFile = new string[] { "Controller.java.vm", "entity.java.vm", "ExceptionEnum.java.vm", "Mapper.java.vm", "Mapper.xml.vm", "Param.java.vm", "Service.java.vm", "ServiceImpl.java.vm", "Manage.js.vm", "index.vue.vm", "addForm.vue.vm", "editForm.vue.vm", "XnMysql.sql.vm", "XnOracle.sql.vm" };

        /// <summary>
        /// 本地项目根目录
        /// </summary>
        public static string LocalPath
        {
            get => Directory.GetCurrentDirectory(); // System.getProperty("user.dir") + FILE_SEP + BASE_MODULAR_NAME + FILE_SEP;			
        }

        /// <summary>
        /// vue前端
        /// </summary>
        public static string LocalFrontPath
		{
			get=> Directory.GetCurrentDirectory(); // System.getProperty("user.dir") + FILE_SEP;
			
		}
	}
}
