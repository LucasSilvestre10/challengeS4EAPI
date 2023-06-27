Imports System.Configuration
Imports Microsoft.SqlServer

Public Class ConnectionHelper
    Public Shared Function GetConnectionString() As String
        Return "Server=tcp:s4e.database.windows.net,1433;Initial Catalog=DataBaseChallenge;Persist Security Info=False;User ID=s4eadmin;Password=Admin@#s4e;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    End Function
End Class
