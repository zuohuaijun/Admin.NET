1、迁移默认数据库命令
Add-Migration Init -Context DefaultDbContext
update-database -Context DefaultDbContext

2、迁移租户数据库命令
Add-Migration Init -Context MultiTenantDbContext
update-database -Context MultiTenantDbContext

dotnet run --urls="http://*:5566"
