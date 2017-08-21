高清动态无中心自组网综合应用管理平台（HDDNCONIAMP）:
1、单机版平台采用C#语言编程实现，X86架构，运行与.NET 4.0基础环境；
2、数据库采用SQLite；
3、日志记录采用log4net 2.0.8；
4、界面美化采用DotNetBar 12.12;
5、项目目录结构说明：
	References:项目引用外部DLL文件存储目录；
	Resources:项目用图标资源存储目录；
	Model:数据模型存储目录；
	UI:所有UI界面
		-Login:登录界面；
		-GISVideo:“GIS定位关联视频”模块界面；
		-AudioVideoProcess：“音视频综合处理”模块界面；
		-MeshManagement:“Mesh设备管理”模块界面；
		-UserSettings:“用户配置管理”模块界面；
	Utils:工具类存储目录；
