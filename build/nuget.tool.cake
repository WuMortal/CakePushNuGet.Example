using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.List;
using Cake.Core;
using NuGet.Packaging;

public class NuGetTool {
    public ICakeContext CakeContext { get; }

    public string RepositoryApiUrl { get; }

    public string RepositoryApiKey { get; }

    public string UserName { get; set; }

    public string Password { get; set; }

    private NuGetListSettings ListSettings => new NuGetListSettings {
        AllVersions = true,
        Source = new string[] { this.RepositoryApiUrl }
    };

    private DotNetCorePackSettings BuildPackSettings (string packOutputDirectory) => new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = packOutputDirectory,
        IncludeSource = true,
        IncludeSymbols = true,
        NoBuild = false
    };

    private NuGetTool (ICakeContext cakeContext) {
        CakeContext = cakeContext;
        RepositoryApiUrl = cakeContext.Environment.GetEnvironmentVariable ("NUGET_REPOSITORY_API_URL");
        RepositoryApiKey = cakeContext.Environment.GetEnvironmentVariable ("NUGET_REPOSITORY_API_KEY");
        UserName = cakeContext.Environment.GetEnvironmentVariable ("USERNAME");
        Password = cakeContext.Environment.GetEnvironmentVariable ("PASSWORD");
        CakeContext.Information ($"获取所需参数成功：{RepositoryApiUrl}");
    }

    public static NuGetTool FromCakeContext (ICakeContext cakeContext) {
        return new NuGetTool (cakeContext);
    }

    public void Pack (List<string> projectFilePaths, string packOutputDirectory) {
        projectFilePaths.ForEach (_ => CakeContext.DotNetCorePack (_, BuildPackSettings (packOutputDirectory)));
    }

    public void Push (List<string> packageFilePaths) {

        foreach (var packageFilePath in packageFilePaths) {
            CakeContext.NuGetAddSource (
                "wigor",
                this.RepositoryApiUrl,
                new NuGetSourcesSettings {
                    UserName = this.UserName,
                    Password = this.Password
                });

            CakeContext.NuGetPush (packageFilePath, new NuGetPushSettings {
                Source = "wigor",
                ApiKey = this.RepositoryApiKey
            });

        }

    }
}
