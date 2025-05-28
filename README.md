# CosmicWorks

How to migrate a relational data model to Azure Cosmos DB, a distributed, horizontally scalable, NoSQL database.

This sample demonstrates how to migrate a relational database to a distributed, NoSQL database like Azure Cosmos DB.
This repo contains a Powerpoint presentation and a .NET project that represents the demos for this presentation.

## Steps to setup

### Using Azure Developer CLI (AZD)

This option deploys a serverless Cosmos DB account with all required databases and containers, and sets up RBAC permissions for both a managed identity and the current user.

1. Install the [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
2. Clone this repository to your local machine.
3. Run the following commands from the repository root:

```bash
# Login to Azure
azd auth login

# Initialize the environment (first time only)
azd init

# Provision resources and deploy
azd up
```

The deployment will automatically create an `appsettings.development.json` file in the *src* folder with the Cosmos DB endpoint.

1. Right click the 'CosmicWorks' project and set as start up. Then press F5 to start it.
1. On the main menu, press 'k' to load data. (Note, this can take some time when run locally over low bandwidth connections. Best performance is running in a GitHub Codespace or on a VM in the same region the Cosmos account was provisioned in.)
1. In the project, put breakpoints for any of the functions you want to step through, then press the corresponding menu item key to execute.

> [!IMPORTANT]
> This runs in a serverless Cosmos DB account so you will not incur RU charges when not running. But you will pay for storage, approximately .25c USD per container per month.
To minimize cost, you can run azd down --force --purge and remove the deployed Cosmos DB account then follow the steps above to redeploy.

## Source data

Below is the source data for the 4 versions of the Cosmos DB databases as it progresses through its evolution from a relational database to a highly scalable NoSQL database.

* [Cosmic Works version 1](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v1)
* [Cosmic Works version 2](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v2)
* [Cosmic Works version 3](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v3)
* [Cosmic Works version 4](https://github.com/AzureCosmosDB/CosmicWorks/tree/main/data/database-v4)
