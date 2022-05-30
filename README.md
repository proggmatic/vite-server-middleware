# Vite Middleware

[![](https://buildstats.info/nuget/Proggmatic.SpaServices.Vite)](https://www.nuget.org/packages/Proggmatic.SpaServices.Vite/)

Provides dev-time support for [Vite](https://vitejs.dev/) in ASP.NET Core's SPA scenarios. 

Only .NET 6.0 and higher will be supported.

This is mostly copied and modified from ASP.net Core's implementation for React:
[https://github.com/dotnet/aspnetcore/tree/main/src/Middleware/Spa/SpaServices.Extensions/src/ReactDevelopmentServer](https://github.com/dotnet/aspnetcore/tree/main/src/Middleware/Spa/SpaServices.Extensions/src/ReactDevelopmentServer).

# Usage

## ASP.NET Project

Install the `Proggmatic.SpaServices.Vite` NuGet package on the
ASP.NET Core web project, then modify the `Startup.cs` file similar to the following.


```cs
using Proggmatic.SpaServices.Vite;                  //-- new addition --//


public void ConfigureServices(IServiceCollection services)
{
  // ... other .NET configuration skipped


  //-- new addition --//
  services.AddSpaStaticFiles(configuration =>
  {
    configuration.RootPath = "ClientApp/dist";
  });
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  // ... other .NET configuration skipped here

  app.UseStaticFiles();

  app.UseSpaStaticFiles();                            //-- new addition --//
  
  // ... more default stuff

  app.UseEndpoints(routes =>
  {
      // you app routes...
  }


  //-- new addition --//
  app.UseSpa(spa =>
  {
    // spa.Options.SourcePath = "ClientApp";          // Optional. If this string is commented, "ClientApp" will be used
    // spa.Options.PackageManagerCommand = "yarn";    // Optional. If this string is commented, "npm" will be used. You may use yarn instead of npm.

    if (env.IsDevelopment())
    {
      spa.UseViteServer();
      
      // Or to build not by starting this application but manually uncomment next lines and comment line above
      // spa.ApplicationBuilder.UseFixSpaPathBaseBugMiddleware();     // Uncomment this, if you want to use non-root url for proxying (like http://localhost:8080/my-custom-path)
      // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
    }
  });
}
```


## Vite Project

The Vite project is a typical one created by vite cli such as `npm create vite@latest` and
placed inside the ASPNET site's project folder. More scaffoldings [here on vitejs.dev](https://vitejs.dev/guide/#scaffolding-your-first-vite-project)



## .csproj Configuration

If publishing the ASPNET Core's project is needed then edit the .csproj file like below.
Change `SpaRoot` value to the actual vite project's folder name. Change yarn to npm if necessary.

```xml
  <PropertyGroup>
    <SpaRoot>ClientApp\</SpaRoot>
    <DefaultItemExcludes>$(DefaultItemExcludes);$(SpaRoot)node_modules\**</DefaultItemExcludes>
  </PropertyGroup>

  <ItemGroup>
    <!-- Don't publish the SPA source files, but do show them in the project files list -->
    <Content Remove="$(SpaRoot)**" />
    <None Remove="$(SpaRoot)**" />
    <None Include="$(SpaRoot)**" Exclude="$(SpaRoot)node_modules\**" />
  </ItemGroup>

  <Target Name="DebugEnsureNodeEnv" BeforeTargets="Build" Condition=" '$(Configuration)' == 'Debug' And !Exists('$(SpaRoot)node_modules') ">

    <!-- Ensure Node.js is installed -->
    <Exec Command="node --version" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
    <Error Condition="'$(ErrorCode)' != '0'" Text="Node.js is required to build and run this project. To continue, please install Node.js from https://nodejs.org/, and then restart your command prompt or IDE." />

    <!-- Or ensure Yarn is installed -->
    <Exec Command="yarn --version" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
    <Error Condition="'$(ErrorCode)' != '0'" Text="Yarn is required to build and run this project." />
    <Message Importance="high" Text="Restoring dependencies using 'yarn'. This may take several minutes..." />
    <Exec WorkingDirectory="$(SpaRoot)" Command="yarn install" />
  </Target>

  <Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish">
    <!-- As part of publishing, ensure the JS resources are freshly built in production mode -->
    <Exec WorkingDirectory="$(SpaRoot)" Command="yarn install" />
    <Exec WorkingDirectory="$(SpaRoot)" Command="yarn build" />

    <!-- Include the newly-built files in the publish output -->
    <ItemGroup>
      <DistFiles Include="$(SpaRoot)dist\**" />
      <ResolvedFileToPublish Include="@(DistFiles->'%(FullPath)')" Exclude="@(ResolvedFileToPublish)">
        <RelativePath>%(DistFiles.Identity)</RelativePath>
        <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
        <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
      </ResolvedFileToPublish>
    </ItemGroup>
  </Target>
```

# Notes

* To get hot-module-reloading to work, both Vite dev server and aspnet's 
site need to be on the same protocol (http or https).
* When using https in dev server, it needs to use a trusted certificate or
aspnet will refuse to connect to it.
* Progress of `dev` command writes to logger with name Microsoft.AspNetCore.SpaServices.
* Since dev server's progress is written to stderror there will be lots of "fail"s logged in dotnet. 
To minimize this add `logLevel: 'silent'` to the `devServer` section in `vite.config.js` file. 
See [this page](https://vitejs.dev/config/#loglevel) on how to add it.