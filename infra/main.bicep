targetScope = 'subscription'

// Parameters
@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('The location for all resources')
param location string

@description('The name prefix for all resources')
param namePrefix string = 'cosmicworks'

@description('Owner tag for resource tagging')
param owner string = 'defaultuser@example.com'

@description('Id of the user or app to assign application roles')
param principalId string

// Variables
var uniqueSuffix = uniqueString(subscription().id, environmentName, location, principalId)

var tags = {
  'azd-env-name': environmentName
  owner: owner
}

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${namePrefix}-${uniqueSuffix}'
  location: location
  tags: tags
}

// Module imports
module identity './modules/identity.bicep' = {
  name: 'identityDeploy'
  params: {
    location: location
    namePrefix: namePrefix
    uniqueSuffix: uniqueSuffix
    tags: tags
  }
  scope: rg
}

module cosmosDb './modules/cosmos.bicep' = {
  name: 'cosmosDbDeploy'
  params: {
    location: location
    namePrefix: namePrefix
    uniqueSuffix: uniqueSuffix
    managedIdentityPrincipalId: identity.outputs.managedIdentityPrincipalId
    currentUserPrincipalId: principalId
    tags: tags
  }
  scope: rg
}

// Outputs
// AZD will reliably surface outputs prefixed with AZURE_ as environment variables for hooks.
output AZURE_SUBSCRIPTION_ID string = subscription().subscriptionId
output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_LOCATION string = rg.location
output AZURE_COSMOSDB_ENDPOINT string = cosmosDb.outputs.cosmosEndpoint
output AZURE_COSMOSDB_ACCOUNT_NAME string = cosmosDb.outputs.cosmosAccount

