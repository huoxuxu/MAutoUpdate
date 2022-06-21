<h1>
  参考了大佬的项目：<br/>
  https://www.cnblogs.com/OMango/p/8509436.html
</h1>
<h1>感谢前人栽树！！！</h1>

<h2>核心逻辑</h2>
<p>每次更新都是全量更新，所以升级包务必打包应用运行所需的文件和文件夹</p>
<p>主程序根目录的文件会在更新前改名为：xx.dll-backup</p>
<p>主程序根目录的文件和文件夹会在解压时覆盖更新</p>
<br/>

<h2>使用步骤</h2>
<p>1.生成 UpgradeModel.json 文件，<b>格式参考：项目/Demo/UpgradeModel.json</b></p>
<p>2.启动升级程序：MAutoUpdate.exe -upgrade d:/upgrade.json
&nbsp;&nbsp;命令行参数含义，可参考附录一</p>
<br/>


<h2>功能</h2>
<p>1.远程升级包更新</p>
<p>2.本地升级包更新</p>

