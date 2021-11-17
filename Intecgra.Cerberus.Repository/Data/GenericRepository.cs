using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Intecgra.Cerberus.Domain.Ports.Repository;
using Intecgra.Cerberus.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Intecgra.Cerberus.Repository.Data
{
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

        public async Task<IEnumerable<TE>> GetIn(string query = null, object @in = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            if (string.IsNullOrEmpty(query)) query = QueryBuilder.BuildSelect<TE>(@in: @in);
            await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
            return await conn.QueryAsync<TE>(query, @in);
        }


        public async Task<TE> GetById(object id)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("EntityContext"));
            var query = QueryBuilder.BuildById<TE>(id, out Dictionary<string, dynamic> queryParams);
            return await conn.QuerySingleAsync<TE>(query, queryParams);
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


        public async Task SaveRange(IEnumerable<TE> entity)
        {
        }

        public async Task DeleteRange(IEnumerable<TE> entity)
        {
        }

        public async Task UpdateRange(IEnumerable<TE> entity)
        {
        }
    }
}