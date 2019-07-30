Imports System.Data.SqlClient
Imports EpdIt.DBUtilities

Partial Public Class DBHelper

    ''' <summary>
    ''' Executes a set of non-query SQL statements on the database.
    ''' </summary>
    ''' <param name="queryList">The SQL statements to execute.</param>
    ''' <param name="parametersList">A List of SqlParameter arrays to send.</param>
    ''' <param name="rowsAffectedList">A List of rows affected by each SQL statement.</param>
    ''' <returns>True if command ran successfully. Otherwise, false.</returns>
    Private Function QExecuteNonQuery(queryList As List(Of String),
                                      parametersList As List(Of SqlParameter()),
                                      ByRef rowsAffectedList As List(Of Integer)
                                      ) As Boolean

        If queryList.Count <> parametersList.Count Then
            Throw New ArgumentException("The number of queries does not match the number of SqlParameter sets")
        End If

        If queryList.Count = 0 Then
            Throw New ArgumentException("At least one query must be specified.", "query")
        End If

        rowsAffectedList = New List(Of Integer)
        rowsAffectedList.Clear()

        Dim success As Boolean = True
        Dim rowsAffected As Integer

        Using connection As New SqlConnection(ConnectionString)
            connection.Open()

            Using transaction As SqlTransaction = connection.BeginTransaction()
                Try
                    Using command As SqlCommand = connection.CreateCommand()
                        command.CommandType = CommandType.Text
                        command.Transaction = transaction

                        For index As Integer = 0 To queryList.Count - 1
                            command.CommandText = queryList(index)

                            If parametersList(index) IsNot Nothing Then
                                DBNullifyParameters(parametersList(index))
                                command.Parameters.AddRange(parametersList(index))
                            End If

                            rowsAffected = command.ExecuteNonQuery()

                            rowsAffectedList.Insert(index, rowsAffected)
                            command.Parameters.Clear()
                        Next
                    End Using
                Catch ee As SqlException
                    success = False
                    rowsAffectedList.Clear()
                    Throw ee
                Finally
                    If success Then
                        transaction.Commit()
                    Else
                        If transaction IsNot Nothing Then transaction.Rollback()
                    End If
                End Try
            End Using

            connection.Close()
        End Using

        Return success
    End Function

    ''' <summary>
    ''' Retrieves a DataSet containing one or more DataTables selected from the database by calling a stored procedure.
    ''' (Adds the necessary columns and primary key information to complete the schema.)
    ''' </summary>
    ''' <param name="query">The name of the stored procedure to execute.</param>
    ''' <param name="parameterArray">An array of SqlParameter values.</param>
    ''' <returns>A DataSet.</returns>
    Private Function QFillDataTable(query As String, parameterArray As SqlParameter()) As DataTable
        If String.IsNullOrEmpty(query) Then Throw New ArgumentException("The query must be specified.", "query")

        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                command.CommandType = CommandType.Text

                If parameterArray IsNot Nothing AndAlso parameterArray.Count > 0 Then
                    DBNullifyParameters(parameterArray)
                    command.Parameters.AddRange(parameterArray)
                End If

                Dim dataTable As New DataTable

                Using adapter As New SqlDataAdapter(command)
                    adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    adapter.Fill(dataTable)
                End Using

                command.Parameters.Clear()

                Return dataTable
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Retrieves a single value from the database by calling a stored procedure.
    ''' </summary>
    ''' <param name="query">The name of the stored procedure to execute.</param>
    ''' <param name="parameterArray">An array of SqlParameter values.</param>
    ''' <returns>The first column of the first row in the result set, or a null reference (Nothing
    ''' in Visual Basic) if the result set is empty.</returns>
    Private Function QExecuteScalar(query As String, parameterArray As SqlParameter()) As Object
        If String.IsNullOrEmpty(query) Then Throw New ArgumentException("The query must be specified.", "query")

        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                command.CommandType = CommandType.Text

                If parameterArray IsNot Nothing AndAlso parameterArray.Count > 0 Then
                    DBNullifyParameters(parameterArray)
                    command.Parameters.AddRange(parameterArray)
                End If

                command.Connection.Open()
                Dim result As Object = command.ExecuteScalar()

                command.Connection.Close()
                command.Parameters.Clear()

                Return result
            End Using
        End Using
    End Function

End Class
