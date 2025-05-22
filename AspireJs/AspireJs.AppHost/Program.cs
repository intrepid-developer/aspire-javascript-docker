var builder = DistributedApplication.CreateBuilder(args);

var env = builder.AddDockerComposeEnvironment("movies-env");


#pragma warning disable ASPIRECOMPUTE001
var moviesDb = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithComputeEnvironment(env)
    .WithPgAdmin(resource => { resource.WithUrlForEndpoint("http", u => u.DisplayText = "PG Admin"); })
    .AddDatabase("movies");

var api = builder.AddProject<Projects.AspireJs_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(moviesDb).WaitFor(moviesDb)
    .WithComputeEnvironment(env);

var web = builder.AddViteApp("web", "../AspireJs.Web")
    .WithNpmPackageInstallation()
    .WithExternalHttpEndpoints()
    .WithReference(api).WaitFor(api)
    .PublishAsDockerFile()
    .WithComputeEnvironment(env);
#pragma warning restore ASPIRECOMPUTE001

builder.Build().Run();