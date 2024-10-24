using SelfFunded.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<BrokerDal>(); // Registering BrokerDal
builder.Services.AddSingleton<MenuDal>();
builder.Services.AddSingleton<CommonDal>();
builder.Services.AddSingleton<UserDal>();
builder.Services.AddSingleton<CorporateDal>();
builder.Services.AddSingleton<InsuranceMasterDal>();
builder.Services.AddSingleton<PolicyDal>();
builder.Services.AddSingleton<PolicyPemiumInfoDal>();
builder.Services.AddSingleton<PolicyLiveDetailsDal>();
builder.Services.AddSingleton<ImpPolicyInfoDal>();
builder.Services.AddSingleton<BenefitConfigDal>();
builder.Services.AddSingleton<CrmDetailsDal>();
builder.Services.AddSingleton<PlanDetailsDal>();
builder.Services.AddSingleton<CoinsuranceDetailsDal>();
builder.Services.AddSingleton<BenefitCategoryDal>();
builder.Services.AddSingleton<ContactDetailsDal>();
builder.Services.AddSingleton<IntimationSheetInboundDal>();
builder.Services.AddSingleton<PreAuthDal>();
builder.Services.AddSingleton<ClaimsCashlessDal>();
builder.Services.AddSingleton<ClaimReimbursementDal>();
builder.Services.AddSingleton<EnrollmentDal>();
builder.Services.AddSingleton<MasterReportDal>();
builder.Services.AddSingleton<PhmWeeklyReportDal>();
builder.Services.AddSingleton<ClaimSettlementReportDal>();
builder.Services.AddSingleton<SavingReportDal>();
builder.Services.AddSingleton<BalanceSumInsuredReportDal>();
builder.Services.AddSingleton<ClientMISReportDal>();
builder.Services.AddSingleton<FundReceivedReportDal>();
builder.Services.AddSingleton<TDSReportDal>();
builder.Services.AddSingleton<DailyProductivityReportDal>();
builder.Services.AddSingleton<HospitalOutstandiingReportDal>();
builder.Services.AddSingleton<DebitNoteDal>();
builder.Services.AddSingleton<DebitSummaryDal>();
builder.Services.AddSingleton<DashboardDal>();




// Registering IConfiguration
builder.Services.AddScoped<InvestigationReportDal>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var commonDal = provider.GetRequiredService<CommonDal>();
    var folderName = "InvestigationReport";
    var licenseKey = "F5mKmI2ImIqJj5iPloiYi4mWiYqWgYGBgZiI";
    return new InvestigationReportDal(configuration, commonDal, folderName, licenseKey);
});
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SelfFunded  API", Version = "v1" }); // Here, I've set the title to "Broker API"
    c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Menu API", Version = "v2" }); // Here, I've set the title to "Menu API"
    c.SwaggerDoc("v3", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Common API", Version = "v3" });
    c.SwaggerDoc("v4", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "User API", Version = "v4" });
    c.SwaggerDoc("v5", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Corporate API", Version = "v5" });
    c.SwaggerDoc("v6", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Insurance API", Version = "v6" });
    c.SwaggerDoc("v7", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Policy API", Version = "v7" });
    c.SwaggerDoc("v8", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PolicyPemiumInfo API", Version = "v8" });
    c.SwaggerDoc("v9", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PolicyLiveDetailsDal API", Version = "v9" });
    c.SwaggerDoc("v10", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ImpPolicyInfo API", Version = "v10" });
    c.SwaggerDoc("v11", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BenefitConfig API", Version = "v11" });
    c.SwaggerDoc("v12", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CrmDetails API", Version = "v12" });
    c.SwaggerDoc("v13", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PlanDetails API", Version = "v13" });
    c.SwaggerDoc("v14", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Coinsurance API", Version = "v14" });
    c.SwaggerDoc("v15", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BenefitCategory API", Version = "v15" });
    c.SwaggerDoc("v16", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ContactDetails API", Version = "v16" });
    c.SwaggerDoc("v17", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "IntimationSheetInbound API", Version = "v17" });
    c.SwaggerDoc("v18", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PreAuth API", Version = "v18" });
    c.SwaggerDoc("v19", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ClaimsCashlessAPI", Version = "v19" });
    c.SwaggerDoc("v20", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ClaimsReimbursementAPI", Version = "v20" });
    c.SwaggerDoc("v21", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "EnrollmentAPI", Version = "v21" });
    c.SwaggerDoc("v22", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MasterReportAPI", Version = "v22" });
    c.SwaggerDoc("v23", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PhmWeeklyReportAPI", Version = "v23" });
    c.SwaggerDoc("v24", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ClaimSettlementReportAPI", Version = "v24" });
    c.SwaggerDoc("v25", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SavingReportAPI", Version = "v25" });
    c.SwaggerDoc("v26", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BalanceSumInsuredReportAPI", Version = "v26" });
    c.SwaggerDoc("v27", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ClientMISReportAPI", Version = "v27" });
    c.SwaggerDoc("v28", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FundReceivedReportAPI", Version = "v28" });
    c.SwaggerDoc("v29", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TDSReportAPI", Version = "v29" });
    c.SwaggerDoc("v30", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DailyProductivityReportAPI", Version = "v30" });
    c.SwaggerDoc("v31", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HospitalOutstandiingReportAPI", Version = "v31" });
    c.SwaggerDoc("v32", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "InvestigationReportAPI", Version = "v32" });
    c.SwaggerDoc("v33", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DebitNoteAPI", Version = "v33" });
    c.SwaggerDoc("v34", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DebitSummaryAPI", Version = "v34" });
    c.SwaggerDoc("v35", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DashboardAPI", Version = "v35" });



});


var app = builder.Build();
// Enable CORS
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1")); // Here, I've set the title to "Broker API"
}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1")); // Here, I've set the title to "Broker API"
// Redirect root URL to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();


app.Run();