# 执行的文件
[string]$SCRIPT = 'build/build.cake'

[string]$CAKE_VERSION = '0.33.0'

# 配置 NuGet 环境变量
$NUGET_EXE = "build/tool/NuGet.exe"
$NUGET_DIRECTORY = Get-ChildItem -Path $NUGET_EXE
$NUGET_DIRECTORY_NAME=$NUGET_DIRECTORY.DirectoryName
$env:Path += ";$NUGET_DIRECTORY_NAME"

# Install cake.tool
dotnet tool install --global cake.tool --version $CAKE_VERSION

# 参数：显需要执行cake 执行信息
[string]$CAKE_ARGS = "-verbosity=diagnostic"

# 输出将要执行的命命令
Write-Host "dotnet cake $SCRIPT $CAKE_ARGS $ARGS" -ForegroundColor GREEN

dotnet cake $SCRIPT $CAKE_ARGS $ARGS
