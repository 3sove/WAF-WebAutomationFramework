﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WAF.Framework.HelperClasses
{
    public static class DatabaseHelper
    {
        // Database Test Class
        //class DatabaseTesting
        //{
        //    // Database table
        //    static internal string table = "TestTable";
        //    // SQL Queries
        //    internal string selectQuery = "SELECT * FROM " + table;
        //    string insertQuery = "INSERT INTO " + table + " (column1, column2, column3) VALUES (10, 20, 30), (22, 33, 44), (77, 88, 99);";
        //    string deleteTableQuery = "DELETE FROM " + table;
        //    // Report Log
        //    protected ExtentReports ReportLog;
        //    protected static ExtentTest TestLog;

        //    [Test]
        //    public void DBTest()
        //    {
        //        // Connect to Database
        //        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ToString());
        //        connection.Open();

        //        // ExecuteNonQuery - use this method when you don’t expect a result (perhaps and update statement, or a call to a Stored Procedure that returns no resultset)
        //        var command = connection.CreateCommand();
        //        command.CommandText = deleteTableQuery;
        //        command.ExecuteNonQuery();
        //        command.CommandText = insertQuery;
        //        command.ExecuteNonQuery();

        //        // SqlDataReader - which represents a forward-only stream of rows from the database, columns of each row can be accessed by index or name
        //        SqlCommand cmd = new SqlCommand(selectQuery, connection);
        //        SqlDataReader dataReader = cmd.ExecuteReader();

        //        // Run the Test
        //        if (dataReader.HasRows)
        //        {
        //            int count = 1;
        //            while (dataReader.Read())
        //            {
        //                var dbValue = dataReader.GetValue(0);
        //                var exlValue = ExcelReader.ReadFrom(1, 1, count);
        //                count = count + 1;
        //                try
        //                {
        //                    Assert.AreEqual(dbValue, Convert.ToInt32(exlValue));
        //                    TestLog.Log(LogStatus.Pass, "Database data <b>'" + dbValue + "'</b> is equal to Excel data <b>'" + exlValue + "'</b>");
        //                }
        //                catch (Exception)
        //                {
        //                    TestLog.Log(LogStatus.Fail, "Database data <b>'" + dbValue + "'</b> is not equal to Excel data <b>'" + exlValue + "'</b>");
        //                }
        //            }
        //        }
        //        // Close connection
        //        connection.Close();
        //    }

        //    [SetUp]
        //    public void setup()
        //    {
        //        string testName = (TestContext.CurrentContext.Test.Name);
        //        ReportLog = ReportHelper.ReportInstance;
        //        TestLog = ReportLog.StartTest(TestContext.CurrentContext.Test.Name);
        //    }
        //    [TearDown]
        //    public void cleanup()
        //    {
        //        var status = TestContext.CurrentContext.Result.Outcome.Status;
        //        var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
        //                ? ""
        //                : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.Message);
        //        LogStatus logstatus;
        //        switch (status)
        //        {
        //            case TestStatus.Failed:
        //                logstatus = LogStatus.Fail;
        //                string screenName = ScreenshotHelper.TakeScreenshot();
        //                string screenshotPath = TestLog.AddScreenCapture("screenshots//" + screenName);
        //                TestLog.Log(LogStatus.Fail, "Screenshot on Fail", screenshotPath);
        //                break;
        //            case TestStatus.Inconclusive:
        //                logstatus = LogStatus.Warning;
        //                TestLog.Log(LogStatus.Warning, "Warning");
        //                break;
        //            case TestStatus.Skipped:
        //                logstatus = LogStatus.Skip;
        //                break;
        //            default:
        //                logstatus = LogStatus.Pass;
        //                break;
        //        }
        //        TestLog.Log(logstatus, "Test ended with <b>" + logstatus + stacktrace);
        //        ReportLog.EndTest(TestLog);
        //        ReportLog.Flush();
        //    }
        //}


        //Open the connection

        public static SqlConnection DBConnect()
        {
            try
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ToString());
                connect.Open();
                return connect;
                //sqlConnection = new SqlConnection(connectionString);
                //sqlConnection.Open();
                //return sqlConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: " + e.Message);
            }

            return null;
        }

        //Closing the connection 
        public static void DBClose(this SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: " + e.Message);
            }
        }

        //Execution
        public static DataTable ExecuteQuery(this SqlConnection sqlConnection, string queryString)
        {

            DataSet dataset;
            try
            {
                //Checking the state of the connection
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                SqlDataAdapter dataAdaptor = new SqlDataAdapter();
                dataAdaptor.SelectCommand = new SqlCommand(queryString, sqlConnection);
                dataAdaptor.SelectCommand.CommandType = CommandType.Text;

                dataset = new DataSet();
                dataAdaptor.Fill(dataset, "table");
                sqlConnection.Close();
                return dataset.Tables["table"];
            }
            catch (Exception)
            {
                dataset = null;
                sqlConnection.Close();
                //LogHelpers.Write("ERROR :: " + e.Message);
                return null;
            }
            finally
            {
                sqlConnection.Close();
                dataset = null;
            }


        }

    }
}