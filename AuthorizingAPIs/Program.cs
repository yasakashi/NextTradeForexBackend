using NextTradeAPIs.Interfaces;
using NextTradeAPIs.Services;
using Base.Common.Convertors;
using Base.Common.Encryption;
using DataLayers;
using Entities.Systems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Text;

string ConnStr = string.Empty;
string LogConnStr = string.Empty;
string fpath = string.Empty;
Base.Common.Encryption.KeyInfo _keyInfo = null;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

//fpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DotnetFrameworkAdo.dll";
//if (System.IO.File.Exists(fpath))
//    ConnStr = Base.Common.Encryption.Encryption.DecryptString(File.ReadAllText(fpath)).Replace("\r", "").Replace("\n", "").Trim();
//else
    ConnStr = configuration.GetConnectionString("ConnStr");

builder.Services.AddDbContext<SBbContext>(options =>
    options.UseSqlServer(
        ConnStr,
        b => b.MigrationsAssembly(typeof(SBbContext).Assembly.FullName)
        ), ServiceLifetime.Transient);

//fpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DotnetFrameworkLAdo.dll";
//if (System.IO.File.Exists(fpath))
//    LogConnStr = Base.Common.Encryption.Encryption.DecryptString(File.ReadAllText(fpath)).Replace("\r", "").Replace("\n", "").Trim();
//else
    LogConnStr = configuration.GetConnectionString("LConnStr");

builder.Services.AddDbContext<LogSBbContext>(options =>
    options.UseSqlServer(
        LogConnStr,
        b => b.MigrationsAssembly(typeof(LogSBbContext).Assembly.FullName)
        ), ServiceLifetime.Transient);


//fpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DotnetFrameworkKey.dll";
//if (System.IO.File.Exists(fpath))
//{
//    FKeyInfo keyInfo = JsonConvert.DeserializeObject<FKeyInfo>(Base.Common.Encryption.Encryption.DecryptString(File.ReadAllText(fpath)).Replace("\r", "").Replace("\n", "").Trim());
//    _keyInfo = new Base.Common.Encryption.KeyInfo(keyInfo.Key, keyInfo.Iv);
//}
//else
//{
    _keyInfo = new Base.Common.Encryption.KeyInfo("15KLO2soJhvBwz09kBrMlShkaL40vUSGaqr/WBu3+Va=",
                            "kh3fn+orShicGDMLksabAQ==");
//}
var encryptionProvider = new AesProvider(_keyInfo.Key, _keyInfo.Iv);


Jwt jwt = new Jwt()
{
    Audience = configuration["JWT:Audience"],
    Issuer = configuration["JWT:Issuer"],
    UseRsa = false,
    RsaPrivateKeyXML = string.Empty,
    RsaPublicKeyXML = string.Empty,
    Key = configuration["JWT:Secret"]
};

builder.Services.Configure<Jwt>(configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = configuration["JWT:Audience"],
        ValidIssuer = configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddRazorPages();



#region IoC
builder.Services.AddEndpointsApiExplorer();

//On Publish Needed Info
//DomainName_Fa = Configuration["DomainName_Fa"];
//SMSTemplate = Configuration["SMSTemplate"];
//Shaparakiin = Configuration["Shaparakiin"];
//ShaparakAcceptorCode = Configuration["ShaparakAcceptorCode"];
//ShaparakUsername = Configuration["ShaparakUsername"];
//ShaparakPassword = Configuration["ShaparakPassword"];
//Server COnfiguration Injection
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();

builder.Services.AddTransient<AuthorizationService, AuthorizationService>();

builder.Services.AddScoped<IJwtHandler, JwtHandler>();

builder.Services.AddTransient<BaseInformationServices, BaseInformationServices>();

builder.Services.AddTransient<UserServices, UserServices>();
builder.Services.AddTransient<SystemLogServices, SystemLogServices>();
builder.Services.AddTransient<PeopleServices, PeopleServices>();
builder.Services.AddTransient<BlockedIPServices, BlockedIPServices>();

builder.Services.AddTransient<UserTypeServices, UserTypeServices>();

builder.Services.AddTransient<ForumsServices, ForumsServices>();
builder.Services.AddTransient<CommunityGroupServices, CommunityGroupServices>();
builder.Services.AddTransient<SignalChannelServices, SignalChannelServices>();
builder.Services.AddTransient<CourseServices, CourseServices>();

builder.Services.AddTransient<TicketServices, TicketServices>();
builder.Services.AddTransient<WalletServices, WalletServices>();

builder.Services.AddTransient<SiteMessageServices, SiteMessageServices>();

#endregion

builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    //app.UseSwagger(c =>
    //{
    //    c.SerializeAsV2 = true;
    //});
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Next Trade Forex API Call v1");
    });
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
};


app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true)
);

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
