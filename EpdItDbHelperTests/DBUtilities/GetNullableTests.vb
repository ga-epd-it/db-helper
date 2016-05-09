﻿Imports System.Text
Imports EpdItDbHelper
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class GetNullableTests

#Region "ConvertFromDbVal"
    <TestMethod>
    Public Sub GetNullable_ToString_FromDBNull()
        Dim result As String = DBUtilities.GetNullable(Of String)(DBNull.Value)
        Assert.AreEqual(Nothing, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToBool_FromDBNull()
        Dim result As Boolean = DBUtilities.GetNullable(Of Boolean)(DBNull.Value)
        Assert.AreEqual(False, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToInteger_FromDBNull()
        Dim result As Integer = DBUtilities.GetNullable(Of Integer)(DBNull.Value)
        Assert.AreEqual(0, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToString_FromNull()
        Dim result As String = DBUtilities.GetNullable(Of String)(Nothing)
        Assert.AreEqual(Nothing, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToBool_FromNull()
        Dim result As Boolean = DBUtilities.GetNullable(Of Boolean)(Nothing)
        Assert.AreEqual(False, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToInteger_FromNull()
        Dim result As Integer = DBUtilities.GetNullable(Of Integer)(Nothing)
        Assert.AreEqual(0, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToString_FromString()
        Dim result As String = DBUtilities.GetNullable(Of String)("test")
        Assert.AreEqual("test", result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToBool_FromBool()
        Dim result As Boolean = DBUtilities.GetNullable(Of Boolean)(True)
        Assert.AreEqual(True, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToInteger_FromInteger()
        Dim result As Integer = DBUtilities.GetNullable(Of Integer)(12)
        Assert.AreEqual(12, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToNullableBool_FromNull()
        Dim result As System.Nullable(Of Boolean) = DBUtilities.GetNullable(Of System.Nullable(Of Boolean))(Nothing)
        Assert.AreEqual(Nothing, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToNullableInteger_FromNull()
        Dim result As System.Nullable(Of Integer) = DBUtilities.GetNullable(Of System.Nullable(Of Integer))(Nothing)
        Assert.AreEqual(Nothing, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToNullableBool_FromDBNull()
        Dim result As System.Nullable(Of Boolean) = DBUtilities.GetNullable(Of System.Nullable(Of Boolean))(DBNull.Value)
        Assert.AreEqual(Nothing, result)
    End Sub

    <TestMethod>
    Public Sub GetNullable_ToNullableInteger_FromDBNull()
        Dim result As System.Nullable(Of Integer) = DBUtilities.GetNullable(Of System.Nullable(Of Integer))(DBNull.Value)
        Assert.AreEqual(Nothing, result)
    End Sub
#End Region

End Class