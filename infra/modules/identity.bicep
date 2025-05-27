// Parameters
param location string
param tags object = {}
param namePrefix string
param uniqueSuffix string

// Variables
var managedIdentityName = '${namePrefix}-identity-${uniqueSuffix}'

// Create a user-assigned managed identity for the application
resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: managedIdentityName
  location: location
  tags: tags
}

// Outputs
output managedIdentityPrincipalId string = identity.properties.principalId
