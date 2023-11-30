using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.WebEncoders;
using MobileInvitation.Config;
using MobileInvitation.Data.Coupon;
using MobileInvitation.Data.Invitation;
using MobileInvitation.Data.Mcard;
using MobileInvitation.Data.Member;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Order;
using MobileInvitation.Data.Product;
using MobileInvitation.Data.Template;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using MobileInvitation.Payment;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

/// <summary>
/// 어플리케이션의 진입점으로서, 환경설정을 지정하고 어플리케이션에서 사용할 서비스에 연결하는 작업을 할 수 있는 클래스
/// </summary>
namespace MobileInvitation
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;

        }

        public IConfiguration Configuration { get; }

        //  응용프로그램에 사용될 서비스들을 정의
        public void ConfigureServices(IServiceCollection services)
        {

            #region Application gateway x-forword-for 설정
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.ForwardLimit = 5;
                options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.16.100.0"), 24));
            });
            #endregion

            var DbConnectionString = Configuration.GetConnectionString("MBarunsonDBConn");
          
            services.AddDbContext<barunsonContext>(options =>
                 options.UseSqlServer(DbConnectionString));
			services.AddDbContext<ProtectKeysContext>(options =>
                options.UseSqlServer(DbConnectionString));
			services.AddDbContext<BarShopContext>(options =>
				 options.UseSqlServer(Configuration.GetConnectionString("BarShopDBConn")));

			//바른손 전용 구성 정보 등록
			services.AddSingleton<BarunnConfig>(Configuration.GetSection("BarunnConfig").Get<BarunnConfig>());

            //Toss 결제 상점 키 정보, API 호출 서비스 등록
            if (Configuration.GetSection("PgMertInfos").Value == null)
                services.AddSingleton<List<PgMertInfo>>(Configuration.GetSection("PgMertInfos").Get<List<PgMertInfo>>());
            else
                services.AddSingleton<List<PgMertInfo>>(JsonSerializer.Deserialize<List<PgMertInfo>>(Configuration.GetSection("PgMertInfos").Get<string>()));
            services.AddScoped<ITossPaymentService, TossPaymentService>();

            #region Kakao bank
            var kakoinfo = Configuration.GetSection("KakaoBank").Get<KakaoBankConfig>();
            services.AddSingleton(kakoinfo);
            #endregion

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();

            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IOrderRepository, OrderRepository> ();
            services.AddScoped<IOperationRepository, OperationRepository> ();
            services.AddScoped<ICouponRepository, CouponRepository> ();

            services.AddTransient<PathController>();

            services.AddScoped<IMcardRepository, McardRepository>();
            
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();// 디버깅 모드 실행하고 뷰를 수정 후 웹페이지에서 새로고침해도 변경되게 하는 옵션

            services.AddDataProtection()
                .PersistKeysToDbContext<ProtectKeysContext>()
                .SetApplicationName("barun.mobileinvitation");

            services.AddHttpClient();
            
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = "userAuth";
            }).AddCookie("userAuth", "userAuth", o =>
            {
                o.Cookie.Name = "userAuth";
                o.Cookie.Domain = ".barunsonmcard.com";
                o.LoginPath = new PathString("/Member/LogIn"); //" /Admin/Member/LogIn";
                o.LogoutPath = new PathString("/Member/LogOut");
                o.ExpireTimeSpan = TimeSpan.FromHours(1);

                o.Cookie.SecurePolicy = CookieSecurePolicy.None; /*추가*/
            });

            /**************추가*****************/
            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "userAuth";
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                o.Cookie.HttpOnly = false;

                o.ExpireTimeSpan = TimeSpan.FromDays(31);
                o.LoginPath = "/Member/LogIn";
                o.SlidingExpiration = true;
                o.Validate();
                o.Cookie.Domain = ".barunsonmcard.com";

            });
            
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); // 한글이 인코딩되는 문제 해결
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddHealthChecks()
                .AddSqlServer(DbConnectionString); 
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// IApplicationBuilder -> 어플리케이션의 요청 처리경로를 구축할 때 사용합니다.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app/*, IWebHostEnvironment env*//*, ILoggerFactory loggerFactory*/)
        {
            app.UseForwardedHeaders();
            
            if (_env.EnvironmentName.ToLower().Equals("local") || _env.EnvironmentName.ToLower().Equals("development") || _env.EnvironmentName.ToLower().Equals("dev"))
            {

                app.UseDeveloperExceptionPage(); //세부 오류 안내 페이지로 이동
                if (_env.EnvironmentName.ToLower().Equals("local"))
                {
                    //가상 경로 맵핑
                    app.UseFileServer(new FileServerOptions
                    {
                        FileProvider = new PhysicalFileProvider(@"\\172.16.4.4\devmcardshare\barunsonmcard\upload"),
                        RequestPath = new PathString("/upload"),
                        EnableDirectoryBrowsing = false
                    });
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); //사용자 지정 예외 처리 페이지 설정
                app.UseHsts();
                app.UseStatusCodePagesWithRedirects("/CustomErrors/{0}");
            }


            //정적 콘텐츠의 클라이언트 캐시 30일간 설정
            //참조: https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0
            var cacheMaxAge = (60 * 60 * 24 * 30).ToString();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append(
                         "Cache-Control", $"public, max-age={cacheMaxAge}");
                }
            });

            app.UseRouting(); // 요청을 라우팅하기위한 미들웨어 라우팅 
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization(); //사용자가 보안 자원에 액세스 할 수있는 권한을 부여합니다.
            app.UseCookiePolicy();

            //웹 브라우저 URL을 통해서 특정 요청을 수행하는 것을 라우팅(Routing)
            app.UseEndpoints(endpoints =>
            {
                //라우팅 미들웨어 설정하기
                endpoints.MapAreaControllerRoute(name: "default", areaName: "User", pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

        }
    }
}
