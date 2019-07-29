Imports System.Data.SqlClient

Partial Public Class DBHelper

    ''' <summary>
    ''' Retrieves a boolean scalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameter">An optional SqlParameter to send.</param>
    ''' <returns>A boolean value.</returns>
    Public Function GetBoolean(query As String, Optional parameter As SqlParameter = Nothing) As Boolean
        Return Convert.ToBoolean(GetSingleValue(Of Boolean)(query, parameter))
    End Function

    ''' <summary>
    ''' Retrieves a boolean sclalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameterArray">An array of SqlParameter values to send.</param>
    ''' <returns>A boolean value.</returns>
    Public Function GetBoolean(query As String, parameterArray As SqlParameter()) As Boolean
        Return Convert.ToBoolean(GetSingleValue(Of Boolean)(query, parameterArray))
    End Function

    ''' <summary>
    ''' Retrieves an integer scalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameter">An optional SqlParameter to send.</param>
    ''' <returns>An integer value.</returns>
    Public Function GetInteger(query As String, Optional parameter As SqlParameter = Nothing) As Integer
        Return GetSingleValue(Of Integer)(query, parameter)
    End Function

    ''' <summary>
    ''' Retrieves an integer sclalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameterArray">An array of SqlParameter values to send.</param>
    ''' <returns>An integer value.</returns>
    Public Function GetInteger(query As String, parameterArray As SqlParameter()) As Integer
        Return GetSingleValue(Of Integer)(query, parameterArray)
    End Function

    ''' <summary>
    ''' Retrieves a string scalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameter">An optional SqlParameter to send.</param>
    ''' <returns>A string value.</returns>
    Public Function GetString(query As String, Optional parameter As SqlParameter = Nothing) As String
        Return GetSingleValue(Of String)(query, parameter)
    End Function

    ''' <summary>
    ''' Retrieves a string scalar value from the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameterArray">An array of SqlParameter values to send.</param>
    ''' <returns>A string value.</returns>
    Public Function GetString(query As String, parameterArray As SqlParameter()) As String
        Return GetSingleValue(Of String)(query, parameterArray)
    End Function

    ''' <summary>
    ''' Retrieves a single value of the specified type from the database.
    ''' </summary>
    ''' <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameter">An optional SqlParameter to send.</param>
    ''' <returns>A value of the specified type.</returns>
    Public Function GetSingleValue(Of T)(query As String, Optional parameter As SqlParameter = Nothing) As T
        Dim parameterArray As SqlParameter() = Nothing

        If parameter IsNot Nothing Then
            parameterArray = {parameter}
        End If

        Return GetSingleValue(Of T)(query, parameterArray)
    End Function

    ''' <summary>
    ''' Retrieves a single value of the specified type from the database.
    ''' </summary>
    ''' <typeparam name="T">The expected type of the value retrieved from the database.</typeparam>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameterArray">An array of SqlParameter values to send.</param>
    ''' <returns>A value of the specified type.</returns>
    Public Function GetSingleValue(Of T)(query As String, parameterArray As SqlParameter()) As T
        Dim result As Object = QExecuteScalar(query, parameterArray)

        Return DBUtilities.GetNullable(Of T)(result)
    End Function

    ''' <summary>
    ''' Determines whether a value as indicated by the SQL query exists in the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameter">An optional SqlParameter array to send.</param>
    ''' <returns>A boolean value signifying whether the indicated value exists and is not DBNull.</returns>
    Public Function ValueExists(query As String, Optional parameter As SqlParameter = Nothing) As Boolean
        Dim parameterArray As SqlParameter() = Nothing

        If parameter IsNot Nothing Then
            parameterArray = {parameter}
        End If

        Return ValueExists(query, parameterArray)
    End Function

    ''' <summary>
    ''' Determines whether a value as indicated by the SQL query exists in the database.
    ''' </summary>
    ''' <param name="query">The SQL query to send.</param>
    ''' <param name="parameterArray">An array of SqlParameter values to send.</param>
    ''' <returns>A boolean value signifying whether the indicated value exists and is not DBNull.</returns>
    Public Function ValueExists(query As String, parameterArray As SqlParameter()) As Boolean
        Dim result As Object = QExecuteScalar(query, parameterArray)

        Return result IsNot Nothing AndAlso Not IsDBNull(result)
    End Function

End Class
