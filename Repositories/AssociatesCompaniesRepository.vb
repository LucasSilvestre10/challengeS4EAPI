Imports System.Data.SqlClient

Public Class AssociatesCompaniesRepository
    Private connectionString As String = ConnectionHelper.GetConnectionString()

    Public Sub AddAssociateCompany(associateId As Integer, companyId As Integer)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "INSERT INTO AssociatesCompanies (associateId, companyId) VALUES (@AssociateId, @CompanyId)"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@AssociateId", associateId)
                command.Parameters.AddWithValue("@CompanyId", companyId)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub RemoveAssociateCompanies(associateId As Integer)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "DELETE FROM AssociatesCompanies WHERE AssociateId = @AssociateId"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@AssociateId", associateId)

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub
End Class
