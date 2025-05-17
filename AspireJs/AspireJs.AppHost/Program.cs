var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposePublisher();

var moviesDb = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin(resource => { resource.WithUrlForEndpoint("http", u => u.DisplayText = "PG Admin"); })
    .AddDatabase("movies")
    .WithCreationScript("""
                        CREATE DATABASE Movies
                            ENCODING = 'UTF8';

                        CREATE TABLE Movies (
                            Id SERIAL PRIMARY KEY,
                            Title TEXT NOT NULL,
                            Year INT NOT NULL,
                            Genre TEXT NOT NULL
                        );
                        """);

var api = builder.AddProject<Projects.AspireJs_Api>("api")
    .WithHttpsHealthCheck("/health")
    .WithHttpsEndpoint()
    .WithExternalHttpEndpoints()
    .WithReplicas(2)
    .WithReference(moviesDb).WaitFor(moviesDb);

var web = builder.AddViteApp("web", "../AspireJs.Web")
    .WithNpmPackageInstallation()
    .WithExternalHttpEndpoints()
    .WithReference(api).WaitFor(api)
    .PublishAsDockerFile();

builder.Build().Run();