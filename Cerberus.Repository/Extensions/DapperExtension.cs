using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cerberus.Infrastructure.Data.Attributes;
using Dapper;
using static TypeMerger.TypeMerger;

namespace Cerberus.Repository.Extensions;

public static class DapperExtension
{
    /// <summary>
    ///     Returns a single entity by a single id from table "Ts" asynchronously using Task. T must be of interface type.
    ///     Id must be marked with [Key] attribute.
    ///     Created entity is tracked/intercepted for changes and used by the Update() extension.
    /// </summary>
    /// <typeparam name="T">Interface type to create and populate</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
    /// <returns>Entity of T</returns>
    public static async Task<T> GetByIdAsync<T>(this IDbConnection connection, object id,
        IDbTransaction? transaction = null, int? commandTimeout = null) where T : class
    {
        var builder = new SqlBuilder();
        var (_, key) = GetPrimaryKey<T>();
        var select = BuildSelect<T>();

        var builderTemplate = builder.AddTemplate($"{select} /**where**/ ");
        builder.Where($"{key} = @id");

        var dynParams = new DynamicParameters();
        dynParams.Add("@id", id);

        return await connection.QueryFirstAsync<T>(builderTemplate.RawSql, dynParams, transaction, commandTimeout);
    }

    /// <summary>
    ///     Returns a list of entities from table "Ts".
    ///     Id of T must be marked with [Key] attribute.
    ///     Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
    ///     for optimal performance.
    /// </summary>
    /// <typeparam name="T">Interface or type to create and populate</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
    /// <returns>Entity of T</returns>
    public static async Task<IEnumerable<T>> GetAllAsync<T>(this IDbConnection connection,
        IDbTransaction? transaction = null, int? commandTimeout = null) where T : class
    {
        var select = BuildSelect<T>();
        return await connection.QueryAsync<T>(select, transaction: transaction, commandTimeout: commandTimeout);
    }

    /// <summary>
    ///     Inserts an entity into table "Ts" asynchronously using Task and returns identity id.
    /// </summary>
    /// <typeparam name="T">The type being inserted.</typeparam>
    /// <typeparam name="TK">The type of a key column</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="entityToInsert">Entity to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
    /// <returns>Identity of inserted entity</returns>
    public static Task<TK> InsertAsync<T, TK>(this IDbConnection connection, T entityToInsert,
        IDbTransaction? transaction = null,
        int? commandTimeout = null) where T : class
    {
        if (null == entityToInsert) throw new Exception("The entity cannot be null.");
        var tableName = GetTableName<T>();
        var columns = GetColumns<T>();
        var (key, snakeKey) = GetPrimaryKey<T>();

        var builder = new SqlBuilder();
        var paramsBuilder = new SqlBuilder();
        foreach (var col in columns.Where(c => c.Name != key))
        {
            builder.Select(col.Name.ToSnakeCase());
            paramsBuilder.Select($"@{col.Name}");
        }

        var paramsTemplate = paramsBuilder.AddTemplate("/**select**/");
        var template =
            builder.AddTemplate(
                $"INSERT INTO {tableName}  (/**select**/) VALUES ({paramsTemplate.RawSql}) RETURNING {snakeKey}");
        return connection.ExecuteScalarAsync<TK>(template.RawSql, entityToInsert, transaction, commandTimeout);
    }

    /// <summary>
    ///     Updates entity in table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked
    ///     by the Get() extension.
    /// </summary>
    /// <typeparam name="T">Type to be updated</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="entityToUpdate">Entity to be updated</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
    /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
    public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entityToUpdate,
        IDbTransaction? transaction = null, int? commandTimeout = null) where T : class
    {
        var tableName = GetTableName<T>();
        var columns = GetColumns<T>();
        var (key, snakeKey) = GetPrimaryKey<T>();
        var builder = new SqlBuilder();
        foreach (var column in columns.Where(m => m.Name != key))
            builder.Set($"{column.Name.ToSnakeCase()} = @{column.Name}");

        builder.Where($"{snakeKey} = @{key}");

        var bt = builder.AddTemplate($"UPDATE {tableName} /**set**/ /**where**/ ");
        return await connection.ExecuteAsync(bt.RawSql, entityToUpdate, transaction, commandTimeout) > 0;
    }


    /// <summary>
    ///     Delete entity in table "Ts" asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="entityId">Entity to delete</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
    /// <returns>true if deleted, false if not found</returns>
    public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, int entityId,
        IDbTransaction? transaction = null, int? commandTimeout = null) where T : class
    {
        var (_, snakeKey) = GetPrimaryKey<T>();
        var tableName = GetTableName<T>();

        var dynParams = new DynamicParameters();
        dynParams.Add("@id", entityId);

        var cmd = $"delete from {tableName} where {snakeKey} = @id";
        try
        {
            return await connection.ExecuteAsync(cmd, dynParams, transaction, commandTimeout) > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<IEnumerable<T>> WhereAsync<T>(this IDbConnection connection, object whereCondition,
        object? inCondition = null, IDbTransaction? transaction = null, int? commandTimeout = null) where T : class
    {
        var select = BuildSelect<T>();
        var builder = new SqlBuilder();
        var properties = whereCondition.GetType().GetProperties();
        for (var i = 0; i < properties.Count(); i++)
        {
            var property = properties.ElementAt(i);
            builder.Where($"{property.Name.ToSnakeCase()} = @{property.Name}");
        }

        var mergedParams = whereCondition;

        if (inCondition != null)
        {
            var inProperties = inCondition.GetType().GetProperties();
            for (var i = 0; i < inProperties.Count(); i++)
            {
                var property = inProperties.ElementAt(i);
                builder.Where($"{property.Name.ToSnakeCase()} = ANY(@{property.Name})");
            }

            mergedParams = Merge(whereCondition, inCondition);
        }

        var builderTemplate = builder.AddTemplate($"{select} /**where**/ ");
        return await connection.QueryAsync<T>(builderTemplate.RawSql, mergedParams, transaction,
            commandTimeout);
    }

    private static string BuildSelect<T>()
    {
        var builder = new SqlBuilder();

        var tableName = GetTableName<T>();
        var columns = GetColumns<T>();
        foreach (var col in columns) builder.Select(col.Name.ToSnakeCase());

        var builderTemplate = builder.AddTemplate($"SELECT /**select**/ FROM {tableName}");
        return builderTemplate.RawSql;
    }

    private static string GetTableName<T>()
    {
        var type = typeof(T);
        return type.GetAttributeValue((Table tbl) => tbl.Name);
    }

    private static (string, string) GetPrimaryKey<T>()
    {
        var columns = GetColumns<T>();
        var primaryKey = columns.Where(m => m.GetCustomAttributes<Key>().Any()).ToList();
        if (!primaryKey.Any()) throw new Exception("The entity must contain a primary key");
        return (primaryKey.First().Name, primaryKey.First().Name.ToSnakeCase());
    }

    private static IEnumerable<PropertyInfo> GetColumns<T>()
    {
        var type = typeof(T);
        return type.GetProperties();
    }

    private static string ToSnakeCase(this string str)
    {
        var pattern = new Regex(@"([A-Z][a-z]+|[A-Z]+(?![a-z])|[0-9]+)");
        return string.Join("_", pattern.Matches(str)).ToLower();
    }

    private static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
        Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
    {
        if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            return valueSelector(att);

        return default!;
    }
}