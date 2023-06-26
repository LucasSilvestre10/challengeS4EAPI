Imports System.Configuration
Imports Microsoft.SqlServer

Public Class ConnectionHelper
    Public Shared Function GetConnectionString() As String
        Return "Server=DESKTOP-VQ35O8J\SQLEXPRESS; Database=DataBaseChallenge; User Id=sa;Password=135085"
    End Function
End Class
