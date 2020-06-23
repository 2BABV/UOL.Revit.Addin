using Cadac.ClientTools;
using Cadac.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using UOL.SDK;
using UOL.SDK.Controls;

namespace UOL.Revit.SampleAddin
{
    /// <summary>
    /// The class <c>IServiceCollectionExtension</c> is a class for the dependency injection calls.
    /// </summary>
    public static class RevitAddinExtensions
    {
        private const string ClientId = "???";
        private const string ClientSecret = "???";

        /// <summary>
        /// Adds all needed bindings for the addin.
        /// </summary>
        /// <param name="applicationContext">The Service collection to use for binding.</param>
        /// <returns>An <see cref="IServiceCollection"/> object with bindings.</returns>
        public static IServiceCollection AddUOLRevitAddIn(this IApplicationContext applicationContext)
        {
            /// Add dependency injection for the logging classes.
            applicationContext.AddCadacClientTools();

            /// Add dependency injection for the RevitHost.
            applicationContext.TryAddTransient<ICADHost, RevitHost>();

            ///Configure the uol client options that are needed.
            void configureOptions(UOLClientOptions options)
            {
                options.AuthenticationRedirectUrl = new Uri("https://uol-auth.2ba.nl");
                options.ClientId = ClientId;
                options.ClientSecret = ClientSecret;
                options.LicenseCode = Guid.NewGuid().ToString();
                options.ValidatorFunctionUrl = new Uri("https://uol-vf.2ba.nl/api/");
                options.ContentServiceBaseUrl = new Uri("https://uol-cs.2ba.nl/api/v1/");
                options.DataServiceBaseUrl = new Uri("https://uol-ds.2ba.nl/api/v1/");
            }

            applicationContext.Configure((Action<UOLClientOptions>)configureOptions);

            /// Add dependency injection for the uol SDK controls.
            applicationContext.AddUolSdkControls();
            return applicationContext;
        }
    }
}
