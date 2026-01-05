# Cosmic Works

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=AzureCosmosDB/CosmicWorks)
[![Run with Docker](https://img.shields.io/badge/Run%20with-Docker-blue)](https://github.com/AzureCosmosDB/CosmicWorks#run-with-docker)
[![Deploy with AZD](https://img.shields.io/badge/Deploy%20with-Azure%20Developer%20CLI-blue)](https://github.com/AzureCosmosDB/CosmicWorks#deploy-to-azure-azd)

This sample demonstrates how to migrate a relational database to a distributed, NoSQL database like Azure Cosmos DB.
This repo contains a Powerpoint presentation and a .NET Project that represents the demos for this presentation. You can watch this presentation from [Igloo Conf 2022](https://youtu.be/TvQEG52eVrI?si=rbXrAV_SwwtbCIX_&t=49)

The main components of this sample include:

* **Program.cs**: A console application. The menu items coincide with demos for the presentation that highlight the evolution of the data models from a normalized relational data model (v1) to a fully denormalized NoSQL data model (v4).

* **ChangeFeed.cs**: The class implements Cosmos DB's change feed processor to monitor the product categories container for changes and then propagates those to the products container. This highlights how to maintain referential integrity between entities.

* **Models.cs**: This project contains all of the data models (and versions of them) for the entities.

* **CosmosManagement.cs**: This class contains the Cosmos DB management SDK classes used to delete and recreate the databases and containers.

## Quick Start Options

Choose one of these options to get started:

1. **[Run in GitHub Codespaces](#run-in-codespaces)** - The fastest way to get started with zero local setup
2. **[Run with Docker](#run-with-docker)** - Run locally in a container with minimal setup
3. **[Run locally](#run-locally)** - Traditional local development experience
4. **[Deploy to Azure (AZD)](#deploy-to-azure-azd)** - Deploy to Azure and run with real Azure Cosmos DB resources

### Run in CodeSpaces

1. Click the **Open in GitHub Codespaces** button at the top of this README
1. Wait for the Codespace to initialize (this may take a few minutes)
1. Deploy to Azure using [Deploy to Azure (AZD)](#deploy-to-azure-azd)
1. Once deployed, open the integrated terminal and run:

   ```bash
   cd src
   dotnet run
   ```

GitHub Codespaces provides the best performance for data loading since it runs in the cloud, closer to the Azure Cosmos DB resources.

## Run with Docker

To run CosmicWorks in a containerized environment:

1. Make sure [Docker](https://www.docker.com/products/docker-desktop/) is installed on your system
2. Clone the repository:

   ```bash
   git clone https://github.com/AzureCosmosDB/CosmicWorks.git
   cd CosmicWorks
   ```

3. Deploy to Azure first (this creates the necessary appsettings.development.json file):

   See [Deploy to Azure (AZD)](#deploy-to-azure-azd).

4. Build and run the container:

   ```bash
   docker-compose up --build
   ```

You can also build and run the Docker container manually:

```bash
docker build -t cosmicworks .
docker run -it --volume "./src/appsettings.development.json:/app/appsettings.development.json:ro" cosmicworks
```

## Run Locally

To run CosmicWorks directly on your local machine:

1. Ensure [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) is installed
1. Clone the repository:

   ```bash
   git clone https://github.com/AzureCosmosDB/CosmicWorks.git
   cd CosmicWorks
   ```

1. Deploy to Azure (this creates the necessary appsettings.development.json file):

   See [Deploy to Azure (AZD)](#deploy-to-azure-azd).

1. Run the application:

   ```bash
   cd src
   dotnet run
   ```

1. On the main menu, press 'k' to load data
1. Explore the different functions by pressing the corresponding menu keys

## Deploy to Azure (AZD)

This option deploys a serverless Cosmos DB account with all required databases and containers, and sets up RBAC permissions for both a managed identity and the current user.

The deployment will automatically:

1. Create a serverless Cosmos DB account
1. Set up RBAC permissions
1. Create an `appsettings.development.json` file in the *src* folder
1. Configure everything needed to run the application

### Deploy steps

Run these commands once per environment (and any time you need to reprovision):

```bash
azd auth login
azd init
azd up
```

### Managing Costs

> [!IMPORTANT]
> This solution doesn't charge for RU when not in use but there are data storage costs of $0.25 per container per month. If you are not going to run this sample for some time you can return to the main menu of the application and select option 'L' to delete all the databases. When you return you can recreate these with option 'M', then reload the data with option 'K'

## Source Data

The sample data represents 4 versions of the Cosmos DB databases as they progress through the migration from a relational database to a highly scalable NoSQL database:

* [Cosmic Works version 1](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v1)
* [Cosmic Works version 2](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v2)
* [Cosmic Works version 3](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v3)
* [Cosmic Works version 4](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v4)
