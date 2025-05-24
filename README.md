# Aspire JavaScript Docker Example

This repo is to acompany my blog post on [Using Aspire with Docker Hosts](https://intrepid-developer.com/blog/aspire-with-docker).

This project demonstrates how to use [Aspire](https://github.com/dotnet/aspire) to orchestrate and deploy a multi-service application using Docker. The stack includes:
- A .NET API backend
- A PostgreSQL database
- A Vue.js frontend built with Vite

**No Azure resources are required**, this setup works with any Docker compatible host, such as Digital Ocean, AWS EC2, or your own server. You can get $200 free credit with Digital Ocean using my [Referral Link](https://m.do.co/c/1791153d5ad6).

## Overview

In this guide, you'll learn how to:
- Use Aspire to manage a .NET API, PostgreSQL, and a JavaScript frontend.
- Publish your Aspire project to Docker Compose.
- Deploy your application to a Digital Ocean droplet (or any Docker host).

## Why Use Aspire?

.NET Aspire is Microsoft’s orchestration tooling to really help with the developer experience. Many believe it’s only for .NET applications, but it can be used for pretty much any language. There are lots of integrations, both official and in the community toolkit, that give nice typed experiences. But if there isn’t a dedicated integration it’s not a problem, as long as you can run your application in Docker then Aspire has you covered. 

To highlight this, we’re going to be publishing and deploying our Aspire application to a Digital Ocean droplet (though any VPS provider will work). For this demo, we have a basic .NET API, a Vue.js Vite web app, and a Postgres database. Everything we’re using here can be run on Docker and has no need for Azure or any specific cloud provider.
## Prerequisites

- [Docker](https://www.docker.com/get-started) installed locally.
- [Aspire CLI](https://learn.microsoft.com/en-us/dotnet/aspire/whats-new/dotnet-aspire-9.2#-aspire-cli-preview) installed.
- A [Digital Ocean](https://m.do.co/c/1791153d5ad6) account and a provisioned droplet (or any Docker-compatible host).
- Basic familiarity with .NET, JavaScript, and Docker.

## Install the Aspire CLI
To install the Aspire CLI, you can use the .NET CLI tool. Open your terminal and run:

```sh
dotnet tool install --global aspire.cli —prerelease
```

## Running Locally with Aspire

1. **Clone this repository:**
   ```sh
   git clone https://github.com/intrepid-developer/aspire-javascript-docker.git
   cd aspire-javascript-docker
   ```

2. **Run it locally:**
   ```sh
   aspire run
   ```
![AspireRun](images/aspire-run.png)
3. **Access your Vue.js frontend** at [http://localhost:PORT](http://localhost:PORT) (replace `PORT` with the port your frontend is configured to use).
![App](images/app-local.png)

## Running Locally with Docker Compose

1. **Clone this repository:**
   ```sh
   git clone https://github.com/intrepid-developer/aspire-javascript-docker.git
   cd aspire-javascript-docker
   ```

2. **Publish Aspire project to Docker Compose:**
   ```sh
   aspire publish -o /infra
   ```

3. **Start the application:**
   ```sh
   cd infra
   docker compose up
   ```
![DockerCompose](images/docker-compose.png)
4. **Access your Vue.js frontend** at [http://localhost:PORT](http://localhost:PORT) (replace `PORT` with the port your frontend is configured to use).

## Deploying to Digital Ocean (or Any Docker Host)

1. **Push Docker images** to your container registry (e.g., Digital Ocean Container Registry in this example).

```sh
doctl registry login
```
> Note: You can follow the Digital Ocean Guide on [How to Push and Pull Images](https://docs.digitalocean.com/products/container-registry/getting-started/quickstart/) if you need help with this step.

Next, tag the new `api` and `web` images and push them to your registry (replace `<your registry>` with your actual registry name):

```sh
docker tag api registry.digitalocean.com/<your registry>/api
docker tag web registry.digitalocean.com/<your registry>/web

docker push registry.digitalocean.com/<your registry>/api
docker push registry.digitalocean.com/<your registry>/web
```

2. **Copy your `docker-compose.yml` and `.env`** to your target host.

3. **SSH into your host:**
   ```sh
   ssh root@your_droplet_ip
   ```

4. **Pull images and start services:**
   Make sure you have `docker` and `doctl` installed on your droplet and that you're logged in to the Registry.
   ```sh
   doctl registry login
   
   docker compose pull
   docker compose up -d
   ```

5. **Verify your app is running** by visiting your droplet's public IP in a browser `http://your_droplet_ip:8006`
![AppDeployed](images/app.png)

## Bonus: Cutdown Aspire Dashboard
This example also includes a cutdown version of the Aspire dashboard, which provides a simple web interface to view and manage your Aspire applications.
This gets published in the docker-compose.yml file and can be accessed at [http://localhost:8006](http://localhost:8006) when running locally, or at `http://your_droplet_ip:8007` when deployed to a Docker host.
![AspireDashboard](images/basic-dashboard.png)
You will need to get the Dashboard token from. To do that you can run this command on the droplet:
```sh
docker logs -f aspire-app-dashboard-1
```

And then look for:
```sh
Login to the dashboard at http://localhost:18888/login?t=f3867cc7bf09f0c048dade2703f6bc95. The URL may need changes depending on how network access to the container is configured.
```

You'll have to update the URL with your droplets ip address and the default port should be `8007` instead of `18888`.

## Key Takeaways

- Aspire can orchestrate and deploy multi-language, multi-service applications, not just .NET.
- No Azure resources are required—works with any Docker-compatible host.
- Docker Compose makes it easy to run your Aspire-managed apps locally and in the cloud.

## Resources

- [Aspire Documentation](https://learn.microsoft.com/dotnet/aspire/)
- [Docker Compose Docs](https://docs.docker.com/compose/)
- [Digital Ocean Droplets](https://www.digitalocean.com/products/droplets/)

---
Happy coding!
