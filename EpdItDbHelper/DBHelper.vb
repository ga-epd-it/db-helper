﻿Public Class DBHelper

    Private Property ConnectionString As String

    Public Const SPReturnValueParameterName As String = "@return_value_argument"

    Public Sub New(DbConnectionString As String)
        ConnectionString = DbConnectionString
    End Sub

End Class
