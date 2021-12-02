using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Intecgra.Cerberus.Domain.Ports.Repository;
using Intecgra.Cerberus.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Intecgra.Cerberus.Repository.Data;

public class GenericRepository<TE> : IDisposable, IGenericRepository<TE> where TE : class, new()
{
    private readonly IConfiguration _configuration;

    public GenericRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<TE>> GetAll()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        return await conn.GetAllAsync<TE>();
    }

    public async Task<IEnumerable<TE>> Get(string query = null, Dictionary<string, dynamic> where = null)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        if (string.IsNullOrEmpty(query)) query = QueryBuilder.BuildSelect<TE>(where);
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        return await conn.QueryAsync<TE>(query, where);
    }

    public async Task<IEnumerable<TE>> ExecuteQuery(string query, Dictionary<string, dynamic> queryParams)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        return await conn.QueryAsync<TE>(query, queryParams);
    }

    public async Task<IEnumerable<TE>> GetIn(string query = null, Dictionary<string, dynamic> @in = null,
        dynamic data = null, bool inferPk = false)
    {
        if (inferPk)
        {
            var pk = QueryBuilder.GetPrimaryKey<TE>();
            @in = new Dictionary<string, dynamic> {{pk, data}};
        }

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        if (string.IsNullOrEmpty(query)) query = QueryBuilder.BuildSelect<TE>(@in: @in);
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));

        var parameter = new DynamicParameters();
        if (@in != null)
        {
            foreach (var p in @in)
            {
                parameter.Add($"@{p.Key}", p.Value);
            }

            return await conn.QueryAsync<TE>(query, parameter);
        }

        return new List<TE>();
    }


    public async Task<TE> GetById(object id)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        var query = QueryBuilder.BuildById<TE>(id, out Dictionary<string, dynamic> queryParams);
        return await conn.QueryFirstOrDefaultAsync<TE>(query, queryParams);
    }


    public async Task<TP> Save<TP>(TE entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = QueryBuilder.BuildInsert(entity, out var queryParams);
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        return await conn.ExecuteScalarAsync<TP>(query, queryParams);
    }

    public async Task Update(TE entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = QueryBuilder.BuildUpdate(entity, out var queryParams);
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        await conn.QueryAsync(query, queryParams);
    }

    public async Task Delete(TE entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var query = QueryBuilder.BuildDelete(entity, out var queryParams);
        await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
        await conn.QueryAsync(query, queryParams);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }


    public async Task<IEnumerable<TP>> SaveRange<TP>(IEnumerable<TE> entity)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext")))
        {
            var response = new List<TP>();
            conn.Open();
            await using (var trans = await conn.BeginTransactionAsync())
            {
                foreach (var e in entity)
                {
                    var query = QueryBuilder.BuildInsert(e, out var queryParams);
                    response.Add(await conn.ExecuteScalarAsync<TP>(query, queryParams));
                }

                await trans.CommitAsync();
            }

            return response;
        }
    }

    public async Task DeleteRange(IEnumerable<TE> entity)
    {
    }

    public async Task UpdateRange(IEnumerable<TE> entity)
    {
    }
}