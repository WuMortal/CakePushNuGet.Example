# Install cake.tool
dotnet tool install --global cake.tool --version 0.33.0

# 输出将要执行的命命令
Write-Host "dotnet cake $SCRIPT $CAKE_ARGS $ARGS" -ForegroundColor GREEN

dotnet cake build\build.cake -verbosity=diagnostic