# zsq.Auth
dotnet core Auth Sample


Add-Migration InitConfiguration -Context ConfigurationDbContext -OutputDir Data\Migrations\IdentityServer\ConfigurationDb

Add-Migration InitPersistedGrant -Context PersistedGrantDbContext -OutputDir Data\Migrations\IdentityServer\PersistedGrantDb

Update-Database -Context ConfigurationDbContext

Update-Database -Context PersistedGrantDbContext