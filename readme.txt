支持功能
1.远程安装包升级
2.本地安装包升级

使用步骤
1.将本程序放在主程序目录下的升级文件夹中
2.主程序生成 UpgradeModel.json 文件，格式参考：项目/Demo/UpgradeModel.json
3.主程序拉起升级程序：MAutoUpdate.exe -main d:/main.exe -mainArgs "arg1" -upgradeZip d:/1.zip -upgrade d:/upgrade.json
 命令行参数含义，可参考附录一

关于备份
升级程序会自动将主程序根目录的文件、以及升级Json中的BackupDirs字段配置的文件夹备份

收集机器信息
1.机器配置
 主板
 CPU
 内存
 硬盘
 网卡
 显示器

2.计算机信息
 计算机名
 联网信息
3.操作系统信息
 64位？
 操作系统版本
附录
一.命令行参数
-main 主程序绝对路径，不指定使用app.config中的
-mainArgs 主程序启动参数，不指定使用app.config中的
-upgradeZip 本地升级时，指定的升级压缩包全路径。非必须，如果未指定会使用升级配置Json中的远程URL下载升级压缩包
-upgrade 升级配置绝对路径，不指定使用app.config中的
二.项目编译异常
 error MSB4044: 未给任务“Fody.WeavingTask”的必需参数“SolutionDir”赋值
 解决方案：卸载项目，重新加载项目即可