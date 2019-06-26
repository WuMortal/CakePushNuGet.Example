# 前言

大家好，我最近在想如何提交代码的时候自动的打包然后发布到 `AzureDevOps` 中的 `Artifacts`，在这个过程中踩了很多坑，也走了很多弯路，所以这次篇文章就是将我探索的结果和我遇到的一些问题整理分享给大家。 

我的上一篇关于 `CI/CD` 的文章[《使用 Gitlab CI/CD 实现自动化发布站点到 IIS》](https://www.cnblogs.com/AMortal/p/10845783.html) 中是使用脚本的形式实现的，后来有[园友](https://www.cnblogs.com/linianhui/)在下面评论说可以使用 [Cake（C# Make）](https://cakebuild.net/) 这个工具来实现其中的功能，所以本次就不用了脚本了。下面也会将使用 Cake 改造后的实现部署的代码放出。


整体思路：
1. 首先介绍下 `Cake`、`AzureDevops Pipelines/Artifacts` 怎么使用
2. 接着配置 AzureDevops Pipelines
3. 创建 AzureDevops Artifacts （NuGet 服务端）
4. AzureDevops 配置 PAT (Personal Access Tokens) 和 Pipelines 所需的 Variables（变量）
5. Cake 增加打包、推送 NuGet 包代码。
6. 最后查看运行结果

使用到的工具及版本：

> dotnet core 2.2
> cake 0.33.0
> PowerShell、NuGet、CredentialProvider
> AzureDevops Pipelines 和 AzureDevops Artifacts