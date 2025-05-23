using AspireJs.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var env = builder.AddDockerComposeEnvironment("movies-env");

#pragma warning disable ASPIRECOMPUTE001

// Register our Postgres Database
var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB","movies")
    .WithDataVolume()
    .WithComputeEnvironment(env)
    .WithPgAdmin(resource => { resource.WithUrlForEndpoint("http", u => u.DisplayText = "PG Admin"); });

var database = postgres.AddDatabase("movies");

// Register our .Net Api
var api = builder.AddProject<Projects.AspireJs_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(database).WaitFor(database)
    .WithComputeEnvironment(env)
    .PublishAsDockerComposeService((_, service) =>
    {
        service.Restart = "always";
    });

// Register our Vue.js Web App
var web = builder.AddViteApp("web", "../AspireJs.Web/")
    .WithNpmPackageInstallation()
    .WithExternalHttpEndpoints()
    .WithReference(api).WaitFor(api)
    .PublishAsDockerFile()
    .WithComputeEnvironment(env);
#pragma warning restore ASPIRECOMPUTE001

//Add Aspire Dashboard for use with Docker
builder.AddDashboard();

builder.Build().Run();