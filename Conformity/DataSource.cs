using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Conformity
{
    internal class DataSource
    {
        private readonly string connectionString;

        public DataSource(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString is Required", nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        public async Task<List<Dictionary<string, object>>> GetDataAsync(Job job)
        {
            if (string.IsNullOrWhiteSpace(job.PrimaryProcedure))
            {
                throw new ArgumentException("PrimaryProcedure is Required.", nameof(job.PrimaryProcedure));
            }

            using(var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var primaryResults = await GetResultAsList(connection, job.PrimaryProcedure);

                foreach (var item in job.SecondaryProcedures)
                {
                    foreach (var resultItem in primaryResults)
                    {
                        var secondaryResults = await GetResultAsList(connection, item.Value, new Tuple<string, object>(job.PrimaryKey, resultItem[job.PrimaryKey]));
                        resultItem.Add(item.Key, secondaryResults);
                    }
                }

                return primaryResults;
            }
        }

        private async Task<List<Dictionary<string, object>>> GetResultAsList(SqlConnection connection, string storedProcedure, Tuple<string, object> parameter = null)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                if (parameter != null)
                {
                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                }

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    var schemaTable = reader.GetSchemaTable();
                    var result = new List<Dictionary<string, object>>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(
                                Enumerable.Range(0, reader.FieldCount)
                                .ToDictionary(i => reader.GetName(i), i => reader.GetValue(i))
                            );
                        }
                    }

                    return result;
                }
            }
        }
    }


}
