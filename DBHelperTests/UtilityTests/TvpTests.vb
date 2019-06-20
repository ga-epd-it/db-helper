Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports Microsoft.SqlServer.Server

<TestClass()>
Public Class TvpTests

    <TestMethod>
    Public Sub TvpTest()
        Dim values As Integer() = {1, 2}
        Dim tvp As SqlParameter = TvpSqlParameter("parameterName", values, "dbTableTypeName", "dbColumnName")

        Dim records As IEnumerable(Of SqlDataRecord) = CType(tvp.Value, IEnumerable(Of SqlDataRecord))

        Assert.AreEqual(tvp.SqlDbType, SqlDbType.Structured)
        Assert.IsNull(tvp.SqlValue)
        Assert.AreEqual(records.Count, values.Count)

        Assert.AreEqual(records.ToList().Item(0).FieldCount, 1)
        Assert.AreEqual(records.ToList().Item(0).GetSqlFieldType(0), GetType(SqlTypes.SqlInt32))
        Assert.AreEqual(records.ToList().Item(0).GetSqlValue(0), CType(values(0), SqlTypes.SqlInt32))
        Assert.AreEqual(records.ToList().Item(1).GetSqlValue(0), CType(values(1), SqlTypes.SqlInt32))
    End Sub

End Class
