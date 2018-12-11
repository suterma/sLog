﻿using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using sLog.Models;

namespace sLog.Controllers.DbBrowser
{
    public class QueryController : Controller
    {
        // GET: Browser
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     Executes the specified select command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="selectCommand">The SQL SELECT command.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string connectionString, string selectCommand)
        {
            var dataAndSchema = GetDataSetAndSchema(connectionString, selectCommand);

            //Build the model with the parameters
            var result = new Query
            {
                DataSet = dataAndSchema.Item1,
                TableSchema = dataAndSchema.Item2
            };
            return View(result);
        }

        /// <summary>
        ///     Gets the data set as result from the select command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="selectCommand">The select command.</param>
        /// <returns></returns>
        private Tuple<DataTable, DataTable> GetDataSetAndSchema(string connectionString, string selectCommand)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand
                {
                    CommandText = selectCommand,
                    CommandType = CommandType.Text,
                    Connection = sqlConnection
                })
                {
                    sqlConnection.Open();

                    var dataTable = new DataTable();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dataTable);
                    }

                    var schemaTable = GetSchema(cmd);
                    return new Tuple<DataTable, DataTable>(dataTable, schemaTable);
                }
            }
        }

        /// <summary>
        ///     Gets the schema as result from the select command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        private DataTable GetSchema(SqlCommand cmd)
        {
            {
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read(); //TODO justo toread
                    var schemaTable = reader.GetSchemaTable();
                    return schemaTable;
                }
            }
        }
    }
}