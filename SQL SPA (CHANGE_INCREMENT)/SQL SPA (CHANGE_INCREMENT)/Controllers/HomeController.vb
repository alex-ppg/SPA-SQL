Imports System.Data.SqlClient

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    ReadOnly connectionRegex As String = "^Data Source=[()a-zA-Z]+;Initial Catalog=[a-zA-Z]+;Integrated Security=False;User Id=[a-zA-z]+;Password=[a-zA-Z]+;MultipleActiveResultSets=True$"

    Function Index(ByVal columnName As String, ByVal tableName As String, ByVal connectionURL As String, Optional ByVal resetValue As Int16 = 1) As ActionResult
        ViewBag.Title = "Query Creation"
        If (IsNothing(connectionURL) Or IsNothing(columnName) Or IsNothing(tableName) Or tableName = "" Or columnName = "") Then
            Return View()
        End If
        Dim connectionRegExp As Regex
        connectionRegExp = New Regex(connectionRegex)
        If connectionRegExp.IsMatch(connectionURL) Then
            changeSqlIncrement(connectionURL, resetValue, tableName, columnName)
        Else
            ViewBag.Title = "Malformed Input"
        End If
        Return View()
    End Function

    Sub changeSqlIncrement(ByVal connectionURL As String, ByVal resetValue As Int16, ByVal table As String, ByVal column As String)
        Dim rowsAffected As Integer
        Dim session As New SqlConnection(connectionURL)
        Dim newCommand As New SqlCommand("CHANGE_INCREMENT")
        newCommand.Parameters.Add("@table", SqlDbType.VarChar).Value = table
        newCommand.Parameters.Add("@column", SqlDbType.VarChar).Value = column
        newCommand.Parameters.Add("@resetValue", SqlDbType.Int).Value = resetValue
        newCommand.Connection = session
        newCommand.CommandType = CommandType.StoredProcedure
        Try
            newCommand.Connection.Open()
            rowsAffected = newCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Database Error: " & ex.Message)
        End Try
        newCommand.Connection.Close()
    End Sub
End Class