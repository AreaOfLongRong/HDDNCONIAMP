高清动态无中心自组网综合应用管理平台（HDDNCONIAMP）:
1、单机版平台采用C#语言编程实现，X86架构，运行与.NET 4.0基础环境；
2、数据库采用SQLite；
3、日志记录采用log4net 2.0.8；
4、界面美化采用DotNetBar 12.12;
5、项目目录结构说明：
	DB:数据模型及数据库操作相关类库目录；
	References:项目引用外部DLL文件存储目录；
	Resources:项目用图标资源存储目录；
	Events:存放事件参数类；
	Mesh:Mesh设备业务逻辑操作目录；
	Model:数据模型存储目录；
	UI:所有UI界面
		-GISVideo:“GIS定位关联视频”子系统界面；
		-AudioVideoProcess：“音视频综合处理”子系统界面；
		-MeshManagement:“Mesh设备管理”子系统界面；
		-UserSettings:“用户配置管理”子系统界面；
	Utils:工具类存储目录；
	Video:视频设备及视频存取业务逻辑操作目录；
	FormMain.cs:应用程序主界面；
	log4net.config:log4net日志类库配置文件；
	Program.cs:应用程序启动文件。
