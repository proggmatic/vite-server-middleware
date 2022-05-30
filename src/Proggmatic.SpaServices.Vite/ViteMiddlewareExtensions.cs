using System;

using Microsoft.AspNetCore.SpaServices;


namespace Proggmatic.SpaServices.Vite;

/// <summary>
/// Extension methods for enabling Vite development server middleware support.
/// Original template here: https://github.com/dotnet/aspnetcore/blob/main/src/Middleware/Spa/SpaServices.Extensions/src/ReactDevelopmentServer/ReactDevelopmentServerMiddlewareExtensions.cs
/// </summary>
public static class ViteMiddlewareExtensions
{
    /// <summary>
    /// Handles requests by passing them through to an instance of the Vite server.
    /// This means you can always serve up-to-date CLI-built resources without having
    /// to run the Vite server manually.
    ///
    /// This feature should only be used in development. For production deployments, be
    /// sure not to enable the Vite server.
    /// </summary>
    /// <param name="spaBuilder">The <see cref="ISpaBuilder"/>.</param>
    /// <param name="npmScript">The name of the script in your package.json file that launches the Vite server.</param>
    public static void UseViteServer(
        this ISpaBuilder spaBuilder,
        string npmScript = "dev")
    {
        if (spaBuilder == null)
        {
            throw new ArgumentNullException(nameof(spaBuilder));
        }

        var spaOptions = spaBuilder.Options;

        if (string.IsNullOrEmpty(spaOptions.SourcePath))
            spaOptions.SourcePath = "ClientApp";

        if (string.IsNullOrEmpty(spaOptions.PackageManagerCommand))
            spaOptions.PackageManagerCommand = "npm";

        ViteMiddleware.Attach(spaBuilder, npmScript);
    }
}