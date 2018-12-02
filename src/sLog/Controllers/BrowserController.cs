using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sLog.Models;

namespace sLog.Controllers
{
    public class BrowserController : Controller
    {
        private readonly sLogContext _context;

        public BrowserController(sLogContext context)
        {
            _context = context;
        }

        // GET: Browser
        public async Task<IActionResult> Index()
        {
            var sLogContext = _context.Log.Include(l => l.Registration);
            return View();
            //TODO return db data
            //return View(await sLogContext.ToListAsync());
        }

        /// <summary>
        /// Executes the specified select command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="selectCommand">The SQL SELECT command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Execute(string connectionString, string selectCommand)
        {
            var data = GetData(connectionString, selectCommand);

            var result = new sLog.Models.Query();
            result.Rows = data;
            return View(result);


            //var sqlString = new RawSqlString(selectCommand);
            //var result = _context.Database.ExecuteSqlCommand(sqlString);
            ////TODO continue with showing the result data
            //return View();
            ////TODO return db data
            ////return View(await sLogContext.ToListAsync());
        }

        private IList<string> GetData(string connectionString, string selectCommand)
        {
            using (var sqlConnection1 = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = selectCommand,
                    CommandType = CommandType.Text,
                    Connection = sqlConnection1
                })
                {

                    //cmd.Parameters.Add("@id", SqlDbType.Int).Value = model.CandidateId
                    sqlConnection1.Open();

                    //TOOD which variant is simpler?
                    //How to extract and show meta information?

                    //using (var reader = cmd.ExecuteReader())
                    //{
                    //    //return ReadDataVariant1(reader);
                    //}

                    return ReadDataVariant2(cmd);

                }
            }
        }

        /// <summary>
        /// Reads the data variant2.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        private IList<string> ReadDataVariant2(SqlCommand cmd)
        {
            var data = new List<string>();

            //try https://stackoverflow.com/a/6073545/79485
            var dataTable = new DataTable();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);


            //Transform to list of strings
            foreach (DataRow row in dataTable.Rows)
            {
                data.Add(string.Join(",", row.ItemArray));
            }


            da.Dispose();
            return data;
        }

        /// <summary>
        /// Reads the data using variant1.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private IList<string> ReadDataVariant1(SqlDataReader reader)
        {
            var data = new List<string>();

            //Create a header row
            var columnCount = reader.FieldCount;
            DataTable schemaTable = reader.GetSchemaTable();
            string headerData = string.Empty;
            foreach (DataRow row in schemaTable.Rows)
            {
                headerData += row[0] + " "; //Field Name

                //foreach (DataColumn column in schemaTable.Columns)
                //{
                //    headerData += String.Format("{0} = {1}", column.ColumnName, row[column]) + " ";
                //}
            }
            data.Add(headerData);

            while (reader.Read())
            {
                //Read all data in row
                string rowData = String.Empty;
                for (int i = 0; i < columnCount; i++)
                {
                    rowData += reader[i] + " ";
                }
                data.Add(rowData);
            }
            return data;
        }
    }
}
