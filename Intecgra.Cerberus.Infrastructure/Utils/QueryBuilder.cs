using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using Intecgra.Cerberus.Infrastructure.Data.Attributes;

namespace Intecgra.Cerberus.Infrastructure.Utils
{
    public static class QueryBuilder
    {
        /// <summary>
        /// Construye un select de la tabla (entidad) y brinda la posibilidad
        /// de incluir la clausula where e in (any)
        /// </summary>
        /// <param name="where"></param>
        /// <param name="in"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildSelect<T>(Dictionary<string, dynamic> where = null, object @in = null)
        {
            var builder = new SqlBuilder();

            var tableName = GetTableName<T>();
            var columns = GetColumns<T>();
            foreach (var col in columns)
            {
                builder.Select(col.Name);
            }

            if (null != where)
            {
                foreach (var param in where)
                {
                    builder.Where($"{param.Key} = @{param.Key}");
                }
            }

            if (null != @in)
            {
                var inType = @in.GetType();
                foreach (var param in inType.GetProperties())
                {
                    builder.Where($"{param.Name} = ANY(@{param.Name})");
                }
            }

            var builderTemplate = builder.AddTemplate($"SELECT /**select**/ FROM {tableName} /**where**/ ");
            return builderTemplate.RawSql;
        }

        /// <summary>
        /// Obtiene la clave primaria de la tabla (entidad) y crear el query y los params para una consulara
        /// por id de manera sencilla
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string BuildById<T>(dynamic id, out Dictionary<string, dynamic> param)
        {
            var primaryKey = GetPrimaryKey<T>();
            var whereParams = new Dictionary<string, dynamic>
            {
                {primaryKey, id}
            };
            param = whereParams;
            return BuildSelect<T>(where: whereParams);
        }

        public static string BuildInsert<T>(T entity, out Dictionary<string, object> queryParams)
        {
            if (null == entity) throw new Exception("La entidad no puede ser nula.");
            var tableName = GetTableName<T>();
            var columns = GetColumns<T>();

            var builder = new SqlBuilder();
            var paramsBuilder = new SqlBuilder();
            foreach (var col in columns.Where(c => !c.Key))
            {
                builder.Select(col.Name);
                paramsBuilder.Select($"@{col.Name}");
            }

            var primaryKey = GetPrimaryKey<T>();
            var paramsTemplate = paramsBuilder.AddTemplate("/**select**/");
            var template = builder.AddTemplate(
                $"INSERT INTO {tableName}  (/**select**/) VALUES ({paramsTemplate.RawSql}) RETURNING {primaryKey}");


            var qParams = new Dictionary<string, object>();
            var properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var column = prop.GetCustomAttribute<Column>();
                var value = prop.GetValue(entity, null);
                if (column is {Key: false})
                {
                    qParams.Add(column.Name, value);
                }
            }

            queryParams = qParams;
            return template.RawSql;
        }

        public static string BuildUpdate<T>(T entity, out Dictionary<string, object> queryParams)
        {
            var tableName = GetTableName<T>();
            var columns = GetColumns<T>();
            var primaryKey = GetPrimaryKey<T>();
            var builder = new SqlBuilder();
            foreach (var column in columns.Where(m => !m.Key))
            {
                builder.Set($"{column.Name} = @{column.Name}");
            }

            builder.Where($"{primaryKey} = @{primaryKey}");


            var qParams = new Dictionary<string, object>();
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var column = prop.GetCustomAttribute<Column>();
                var value = prop.GetValue(entity, null);
                if (column is {Key: false})
                {
                    qParams.Add(column.Name, value);
                }
            }

            queryParams = qParams;
            var bt = builder.AddTemplate($"UPDATE {tableName} /**set**/ /**where**/ ");
            return bt.RawSql;
        }

        public static string BuildDelete<T>(T entity, out Dictionary<string, object> queryParams)
        {
            var primaryKey = GetPrimaryKey<T>();
            var tableName = GetTableName<T>();

            var qParams = new Dictionary<string, object>();
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var column = prop.GetCustomAttribute<Column>();
                var value = prop.GetValue(entity, null);
                if (column is {Key: true})
                {
                    qParams.Add(column.Name, value);
                }
            }
            if (qParams.Count > 1) throw new Exception("La entidad no puede contener mas de una clave primaria.");
            queryParams = qParams;
            return $"delete from {tableName} where {primaryKey} = @{primaryKey}";
        }

        private static IEnumerable<Column> GetColumns<T>()
        {
            var type = typeof(T);
            IEnumerable<PropertyInfo> props = type.GetProperties();
            return props.Select(prop => prop.GetCustomAttribute<Column>()).ToList();
        }

        private static string GetTableName<T>()
        {
            var type = typeof(T);
            return type.GetAttributeValue((Table tbl) => tbl.Name);
        }

        private static string GetPrimaryKey<T>()
        {
            var columns = GetColumns<T>();
            var primaryKey = columns.Where(m => m.Key).ToList();
            if (!primaryKey.Any()) throw new Exception("La entidad no contiene una clave primaria.");
            return primaryKey.First().Name;
        }

        private static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default(TValue);
        }
    }
}