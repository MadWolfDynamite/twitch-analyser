using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System;
using TwitchStreamAnalyser.Api.Mapping;
using TwitchStreamAnalyser.Api.Services;
using TwitchStreamAnalyser.Domain.Repositories;
using TwitchStreamAnalyser.Domain.Services;
using TwitchStreamAnalyser.Persistence.Repositories;
using TwitchStreamAnalyser.TwitchApi.Contracts;
using TwitchStreamAnalyser.TwitchApi;

namespace TwitchStreamAnalyser.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHttpClient(
                "twitch-api",
                client =>
                {
                    client.BaseAddress = new Uri("https://api.twitch.tv/helix/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

            services.AddHttpClient(
                "twitch-oauth",
                client =>
                {
                    client.BaseAddress = new Uri("https://id.twitch.tv/");
                });

            services.AddScoped<ITwitchAccountRepository, TwitchAccountRepository>();
            services.AddScoped<ITwitchAccountService, TwitchAccountService>();

            services.AddScoped<ITwitchTokenRepository, TwitchTokenRepository>();
            services.AddScoped<ITwitchTokenService, TwitchTokenService>();

            services.AddScoped<ITwitchApiClient, TwitchApiClient>();
            services.AddScoped<ITwitchTokenClient, TwitchTokenClient>();

            services.AddAutoMapper(typeof(ModelToResourceProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
