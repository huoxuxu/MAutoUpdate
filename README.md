参考了大佬的项目：https://www.cnblogs.com/OMango/p/8509436.html
感谢前人栽树！！！


每次更新都是全量更新<br/>
主程序根目录的文件会改名后更新，<br/>
主程序根目录的文件夹会在解压时覆盖更新<br/>
<br/>

核心逻辑<br/>
1.主程序将升级模型写入升级程序根目录，文件名称 ServerInfo.json<br/>
2.升级程序启动后解析 ServerInfo.json<br/>
<br/>
http://localhost:666/sf/4.7.0_test_0928.zip<br/>

启动方式：<br/>
1.主程序拉起<br/>
  
2.手动双击启动<br/>
  拉起后调用主窗体，内容显示 检查升级中... 立即更新按钮置灰<br/>
  如果需要升级，则将 立即更新按钮常亮<br/>
<br/>
