namespace AutocompleteApi.Migrations;

public interface IDbMigration
{
    Task MigrateAsync();
}

