# CosmicWorks

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=AzureCosmosDB/CosmicWorks)
[![Run with Docker](https://img.shields.io/badge/Run%20with-Docker-blue)](https://github.com/AzureCosmosDB/CosmicWorks#running-with-docker)
[![Deploy with AZD](https://img.shields.io/badge/Deploy%20with-Azure%20Developer%20CLI-blue)](https://github.com/AzureCosmosDB/CosmicWorks#deploying-with-azure-developer-cli-azd)

This sample demonstrates how to migrate a relational data model to Azure Cosmos DB, a distributed, horizontally scalable, NoSQL database. The repository contains a PowerPoint presentation and a .NET project that demonstrates the evolution of data models from relational to NoSQL.

## Quick Start Options

Choose one of these options to get started:

1. **[Run in GitHub Codespaces](#running-in-github-codespaces)** - The fastest way to get started with zero local setup
2. **[Run with Docker](#running-with-docker)** - Run locally in a container with minimal setup
3. **[Run locally](#running-locally)** - Traditional local development experience
4. **[Deploy with Azure Developer CLI](#deploying-with-azure-developer-cli-azd)** - Deploy to Azure and run with real Azure Cosmos DB resources

## Running in GitHub Codespaces

This is the quickest way to get started with CosmicWorks:

1. Click the **Open in GitHub Codespaces** button at the top of this README
2. Wait for the Codespace to initialize (this may take a few minutes)
3. Once the environment is ready, open the integrated terminal and run:
   ```bash
   # Deploy to Azure
   azd auth login
   azd init
   azd up
   
   # Run the application
   cd src
   dotnet run
   ```
4. On the main menu, press 'k' to load data
5. Explore the different functions by pressing the corresponding menu keys

GitHub Codespaces provides the best performance for data loading since it runs in the cloud, closer to the Azure Cosmos DB resources.

## Running with Docker

To run CosmicWorks in a containerized environment:

1. Make sure [Docker](https://www.docker.com/products/docker-desktop/) is installed on your system
2. Clone the repository:
   ```bash
   git clone https://github.com/AzureCosmosDB/CosmicWorks.git
   cd CosmicWorks
   ```
3. Deploy to Azure first (this creates the necessary appsettings.development.json file):
   ```bash
   azd auth login
   azd init
   azd up
   ```
4. Build and run the container:
   ```bash
   docker-compose up --build
   ```

You can also build and run the Docker container manually:
```bash
docker build -t cosmicworks .
docker run -it --volume "./src/appsettings.development.json:/app/appsettings.development.json:ro" cosmicworks
```

## Running Locally

To run CosmicWorks directly on your local machine:

1. Ensure [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) is installed
2. Clone the repository:
   ```bash
   git clone https://github.com/AzureCosmosDB/CosmicWorks.git
   cd CosmicWorks
   ```
3. Deploy to Azure (to create the Cosmos DB resources):
   ```bash
   azd auth login
   azd init
   azd up
   ```
4. Run the application:
   ```bash
   cd src
   dotnet run
   ```
5. On the main menu, press 'k' to load data (Note: this can take time when run locally over low bandwidth connections)
6. Explore the different functions by pressing the corresponding menu keys

## Deploying with Azure Developer CLI (AZD)

This option deploys a serverless Cosmos DB account with all required databases and containers, and sets up RBAC permissions for both a managed identity and the current user.

1. Install the [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
2. Clone this repository to your local machine
3. Run the following commands from the repository root:

```bash
# Login to Azure
azd auth login

# Initialize the environment (first time only)
azd init

# Provision resources and deploy
azd up
```

The deployment will automatically:
- Create a serverless Cosmos DB account with all necessary containers
- Set up RBAC permissions
- Create an `appsettings.development.json` file in the *src* folder with the Cosmos DB endpoint
- Configure everything needed to run the application

### Managing Costs

> [!IMPORTANT]
> This project uses a serverless Cosmos DB account so you will not incur RU charges when not running. However, you will pay for storage, approximately $0.25 USD per container per month.
>
> To minimize costs, you can remove the deployed Azure resources when not in use:
> ```bash
> azd down --force --purge
> ```

## Source Data

The sample data represents 4 versions of the Cosmos DB databases as they progress through the migration from a relational database to a highly scalable NoSQL database:

* [Cosmic Works version 1](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v1)
* [Cosmic Works version 2](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v2)
* [Cosmic Works version 3](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v3)
* [Cosmic Works version 4](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v4)
