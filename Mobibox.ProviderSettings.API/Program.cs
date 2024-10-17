using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Mobibox.ProviderSettings.API.Repository;
using Amazon.Extensions.NETCore.Setup;


using Microsoft.Extensions.Options;
using Amazon.Runtime;
using Mobibox.ProviderSettings.API.Model;
using Microsoft.AspNetCore.Builder;
using System.Net;

namespace Mobibox.ProviderSettings.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind the AWS section from appsettings.json to the AwsSettings class
            builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));




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
            builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
          
            // Register the Amazon DynamoDB client
            builder.Services.AddScoped<IAmazonDynamoDB>(sp =>
            {
                // Retrieve the AWS settings
                var awsOptions = sp.GetRequiredService<IOptions<AwsSettings>>().Value;

                if (string.IsNullOrWhiteSpace(awsOptions.AccessKey) ||
                    string.IsNullOrWhiteSpace(awsOptions.SecretKey) ||
                    string.IsNullOrWhiteSpace(awsOptions.Region))
                {
                    throw new ArgumentException("AWS settings are not properly configured.");
                }

                // Create AWS credentials
                var credentials = new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey);

                // Create and return the DynamoDB client with the specified region
                return new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.GetBySystemName(awsOptions.Region));
            });


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
