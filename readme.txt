临时目录使用情况
/temp
	/$app/$app-upgrade.zip（Http下载的升级包存放处）
	/$app/Upgrade/Guid/解压到这个目录下（解压到此目录下）

/bak


收集机器信息
1.机器配置
 主板
 CPU
 内存
 硬盘,分区信息
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
(废除)-main 主程序绝对路径，不指定使用app.config中的
(废除)-mainArgs 主程序启动参数，不指定使用app.config中的
(废除)-upgradeZip 本地升级时，指定的升级压缩包全路径。非必须，如果未指定会使用升级配置Json中的远程URL下载升级压缩包
-upgrade 升级配置Json的绝对路径，未指定时使用app.config中AppSetting.UpgradeJsonFullName的值

二.配置优先级
从高到底：命令行-->升级程序app.config-->升级Json配置.json


三.项目编译异常
 error MSB4044: 未给任务“Fody.WeavingTask”的必需参数“SolutionDir”赋值
 解决方案：卸载项目，重新加载项目即可
 
 凑个凑个字数
