using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Mobibox.ProviderSettings.API.Repository;

namespace Mobibox.ProviderSettings.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            // Add services to the container.



            // Configure AWS Services
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonDynamoDB>();
            builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

            // Register the repository
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();

            builder.Services.AddControllers();        

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseHttpsRedirection();  // Comment this out if necessary


            app.Run();
        }
    }
}
