using Common.DatabaseExecution;
using Moq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace UnitTest.CommonTest
{
    public class CommonProject_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //IDatabaseExecution<SqlConnection> testobj = new SQLDatabaseExecution();
            ////int ExecuteQuery<T, U>(string sql, U[] parameter);
            //var parameter = new SqlParameter[]
            //{
            //CreateParameter("@Shift", SqlDbType.NVarChar, string.Empty),
            //CreateParameter("@fromdate", SqlDbType.NVarChar, string.Empty),
            //CreateParameter("@todate", SqlDbType.NVarChar, string.Empty)
            //};
            //List<TableObjectForTest> expectedResults = new List<TableObjectForTest>();

            //// Create mock database manager
            //var mockDatabaseManager = new Mock<IDatabaseExecution<SqlConnection>>();
            ////mockDatabaseManager.Setup(m => m.ExecuteQuery<int, SqlParameter>("hau test", parameter))
            ////               .Returns(expectedResults);
            ////List<T> IDatabaseExecution<SqlConnection>.LoadGridByStr<T>(string sqlQuery)
            //mockDatabaseManager.Setup(m => m.LoadGridByStr<TableObjectForTest>("hau test"))
            //               .Returns(expectedResults);
            //var a = testobj.LoadGridByStr<TableObjectForTest>("hau test");


            // Arrange
            string validQuery = "SELECT * FROM TableName";

            List<TableObjectForTest> expectedResults = new List<TableObjectForTest>
            {
                new TableObjectForTest { Name = "Value1", Course = "Value2" },
                new TableObjectForTest { Name = "Value3", Course = "Value4" }
            };  

            // Create a mock SqlDataReader
            var mockDataReader = new Mock<SqlDataReader>();
            mockDataReader.SetupSequence(m => m.Read())
                          .Returns(true)
                          .Returns(true)
                          .Returns(false); // Simulate two rows

            mockDataReader.Setup(m => m["Column1"]).Returns("Value1");
            mockDataReader.Setup(m => m["Column2"]).Returns("Value2");

            mockDataReader.Setup(m => m["Column1"]).Returns("Value3");
            mockDataReader.Setup(m => m["Column2"]).Returns("Value4");

            // Create a mock SqlCommand
            //var mockSqlCommand = new Mock<SqlCommand>();
            //mockSqlCommand.Setup(m => m.ExecuteReader()).Returns(mockDataReader.Object);

            //// Create a mock SqlConnection
            //var mockSqlConnection = new Mock<SqlConnection>();
            //mockSqlConnection.Setup(m => m.CreateCommand()).Returns(mockSqlCommand.Object);

            var _mockDatabaseExecution = new Mock<IDatabaseExecution<SqlConnection>>();
            _mockDatabaseExecution.Setup(m => m.LoadGridByStr<TableObjectForTest>(validQuery))
                                  .Returns(expectedResults);

            // Act
            var actualResults = _mockDatabaseExecution.Object.LoadGridByStr<TableObjectForTest>(validQuery);

            // Assert
            Assert.AreEqual(expectedResults.Count, actualResults.Count);
            Assert.AreEqual(expectedResults[0].Name, actualResults[0].Name);
            Assert.AreEqual(expectedResults[0].Course, actualResults[0].Course);
            Assert.AreEqual(expectedResults[1].Name, actualResults[1].Name);
            Assert.AreEqual(expectedResults[1].Course, actualResults[1].Course);
            //Assert.Pass();
        }

        public static SqlParameter CreateParameter(string parameterName, SqlDbType type, object value)
        {
            SqlParameter parameter = new SqlParameter();

            try
            {
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = type;
                parameter.Value = value;

                return parameter;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}