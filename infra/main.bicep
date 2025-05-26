targetScope = 'resourceGroup'

// Parameters
@description('The location for all resources')
param location string = resourceGroup().location

@description('The name prefix for all resources')
param namePrefix string = 'cosmicworks'

@description('Id of the user or app to assign application roles')
param principalId string

// Variables
var uniqueSuffix = uniqueString(resourceGroup().id)

// Module imports
module identity './modules/identity.bicep' = {
  name: 'identityDeploy'
  params: {
    location: location
    namePrefix: namePrefix
    uniqueSuffix: uniqueSuffix
  }
}

module cosmosDb './modules/cosmos.bicep' = {
  name: 'cosmosDbDeploy'
  params: {
    location: location
    namePrefix: namePrefix
    uniqueSuffix: uniqueSuffix
    managedIdentityPrincipalId: identity.outputs.managedIdentityPrincipalId
    currentUserPrincipalId: principalId
  }
}

// Outputs
output AZURE_COSMOSDB_ENDPOINT string = cosmosDb.outputs.cosmosDbEndpoint
