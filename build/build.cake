var rootPath = "../";   //根目录
var srcPath = rootPath + "src/";    
var solution = srcPath + "Wigor.CakePushNuGet.Example.sln";   //解决方案文件
//需要执行的目标任务
var target = Argument ("target", "Demo");

Task ("Restore")
  .Description ("还原项目依赖")
  .Does (() => {
    //Restore
    Information ("开始执行还原项目依赖任务");
    DotNetCoreRestore (solution);
  });

Task ("Build")
  .Description ("编译项目")
  .Does (() => {
    Information ("开始执行编译生成项目任务");
    //Build
    DotNetCoreBuild (solution, new DotNetCoreBuildSettings {
        NoRestore = true,   //不执行还原，上一步已经还原过了
        Configuration = "Release"
    });
  });

// 执行的任务
Task ("Demo")
  .IsDependentOn ("Restore")    //1. 执行上面的 Restore 任务
  .IsDependentOn ("Build")      //2. 需要执行 上面的 Build 任务
  .Does (() => {
    Information ("所有任务完成");
  });

//运行目标任务 Demo
RunTarget (target);