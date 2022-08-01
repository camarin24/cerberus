using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Domain.Ports.Repository;
using Cerberus.Repository.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Cerberus.Repository.Data;

public class GenericRepository<TE> : IGenericRepository<TE> where TE : class
{
    private readonly string? _connectionString;

    public GenericRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("EntityContext");
    }

    public async Task<IEnumerable<TE>> GetAll()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.GetAllAsync<TE>();
    }

    public async Task<IEnumerable<TE>> Where(object whereConditions, object? inCondition = null)
    {
        if (whereConditions == null) throw new ArgumentNullException(nameof(whereConditions));
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.WhereAsync<TE>(whereConditions, inCondition);
    }

    public async Task<TE> GetById(object id)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.GetByIdAsync<TE>(id);
    }

    public async Task<TK> Create<TK>(TE entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.InsertAsync<TE, TK>(entity);
    }

    public async Task Create(IEnumerable<TE> entities)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        await using var transaction = await conn.BeginTransactionAsync();
        foreach (var entity in entities) await conn.InsertAsync<TE, dynamic>(entity, transaction);
        await transaction.CommitAsync();
    }

    public async Task<bool> Update(TE entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.UpdateAsync(entity);
    }

    public async Task<bool> Update(IEnumerable<TE> entities)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.UpdateAsync(entities);
    }

    public async Task<bool> Delete(int entityId)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.DeleteAsync<TE>(entityId);
    }

    public async Task<IEnumerable<TE>> Query(string query, object parameters)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<TE>(query, parameters);
    }
}