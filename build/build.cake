#reference "NuGet.Packaging"

#load nuget.tool.cake

var target = Argument ("target", "PushPack");

var rootPath = "../";
var srcPath = rootPath + "src/";
var solution = srcPath + "Wigor.CakePushNuGet.Example.sln";
var project = GetFiles (srcPath + "Wigor.CakePushNuGet.HelloWorld/*.csproj");
var nugetPakcageDirectory = $"{srcPath}nugetPackage/";

var nugetTool = NuGetTool.FromCakeContext (Context);

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
      NoRestore = true,
        Configuration = "Release"
    });
  });

Task ("UnitTest")
  .Description ("单元测试")
  .Does (() => {
    Information ("开始执行单元测试任务");
    
    DotNetCoreTest(solution);
  });

Task ("Pack")
  .Description ("Nuget 打包")
  .Does (() => {
    Information ("开始执行打包任务");

    // 确保目录存在
    EnsureDirectoryExists (nugetPakcageDirectory);

    var packageFilePaths = project.Select (_ => _.FullPath).ToList ();

    nugetTool.Pack (packageFilePaths, nugetPakcageDirectory);
  });

Task ("Push")
  .Description ("Nuget 发布")
  .Does (() => {
    Information ("开始执行 Nuget 包发布任务");
    var repositoryApiUrl = EnvironmentVariable ("NUGET_REPOSITORY_API_URL");
    var repositoryApiKey = EnvironmentVariable ("NUGET_REPOSITORY_API_KEY");
    var userName = EnvironmentVariable ("USERNAME");
    var accessToken = EnvironmentVariable ("PASSWORD");
    var packageFilePaths = GetFiles ($"{nugetPakcageDirectory}*.symbols.nupkg").Select (_ => _.FullPath).ToList ();

    nugetTool.Push(packageFilePaths);
  });

Task ("PushPack")
  .Description ("发布 Nuget 包")
  .IsDependentOn ("Restore")
  .IsDependentOn ("Build")
  .IsDependentOn ("Pack")
  // .IsDependentOn ("Push")
  .Does (() => {
    Information ("所有任务完成");
  });

RunTarget (target);