Imports System.Data.SqlClient

Public Class AssociatesRepository
    Private connectionString As String = ConnectionHelper.GetConnectionString()

    Public Function GetAssociates() As List(Of Associates)
        Dim associates As New List(Of Associates)()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT a.Id, a.Name, a.Cpf, a.BirthDay, c.Id AS CompanyId, c.Name AS CompanyName FROM Associates AS a LEFT JOIN AssociatesCompanies AS ac ON a.Id = ac.AssociateId LEFT JOIN Companies AS c ON ac.CompanyId = c.Id"

            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim currentAssociate As Associates = Nothing

                    While reader.Read()
                        Dim associateId As Integer = Convert.ToInt32(reader("Id"))

                        If currentAssociate Is Nothing OrElse currentAssociate.Id <> associateId Then
                            currentAssociate = New Associates()
                            currentAssociate.Id = associateId
                            currentAssociate.Name = reader("Name").ToString()
                            currentAssociate.Cpf = reader("Cpf").ToString()
                            currentAssociate.BirthDay = Convert.ToDateTime(reader("BirthDay"))

                            associates.Add(currentAssociate)
                        End If

                        If Not IsDBNull(reader("CompanyId")) Then
                            Dim companyId As Integer = Convert.ToInt32(reader("CompanyId"))
                            Dim companyName As String = reader("CompanyName").ToString()

                            Dim company As New Companies()
                            company.Id = companyId
                            company.Name = companyName

                            currentAssociate.CompaniesList.Add(company)
                        End If
                    End While
                End Using
            End Using
        End Using

        Return associates
    End Function

    Public Function GetAssociateById(id As Integer) As Associates
        Dim associate As New Associates()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT a.Id, a.Name, a.Cpf, a.BirthDay, c.Id AS CompanyId, c.Name AS CompanyName FROM Associates AS a LEFT JOIN AssociatesCompanies AS ac ON a.Id = ac.AssociateId LEFT JOIN Companies AS c ON ac.CompanyId = c.Id WHERE a.Id = @Id"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Id", id)

                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim currentAssociateId As Integer = 0

                    While reader.Read()
                        Dim associateId As Integer = Convert.ToInt32(reader("Id"))

                        If associateId <> currentAssociateId Then
                            associate.Id = associateId
                            associate.Name = reader("Name").ToString()
                            associate.Cpf = reader("Cpf").ToString()
                            associate.BirthDay = Convert.ToDateTime(reader("BirthDay"))
                            currentAssociateId = associateId
                        End If
                        If Not IsDBNull(reader("CompanyId")) AndAlso Not String.IsNullOrEmpty(reader("CompanyId").ToString()) Then
                            Dim companyId As Integer = Convert.ToInt32(reader("CompanyId"))
                            Dim companyName As String = reader("CompanyName").ToString()

                            If companyId > 0 Then
                                Dim company As New Companies()
                                company.Id = companyId
                                company.Name = companyName
                                associate.CompaniesList.Add(company)
                            End If
                        End If


                    End While
                End Using
            End Using
        End Using

        If associate.Id = 0 Then
            Throw New Exception("Associado não encontrado.")
        End If

        Return associate
    End Function


    Public Function GetAssociatesByFilters(name As String, cpf As String) As List(Of Associates)
        Dim associates As New List(Of Associates)()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT Id, Name, Cpf, BirthDay FROM Associates WHERE 1=1"

            If Not String.IsNullOrEmpty(name) Then
                query += " AND Name LIKE @Name"
            End If

            If Not String.IsNullOrEmpty(cpf) Then
                query += " AND Cpf LIKE @Cpf"
            End If

            Using command As New SqlCommand(query, connection)
                If Not String.IsNullOrEmpty(name) Then
                    command.Parameters.AddWithValue("@Name", "%" + name + "%")
                End If

                If Not String.IsNullOrEmpty(cpf) Then
                    command.Parameters.AddWithValue("@Cpf", "%" + cpf + "%")
                End If

                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim associate As New Associates()
                        associate.Id = Convert.ToInt32(reader("Id"))
                        associate.Name = reader("Name").ToString()
                        associate.Cpf = reader("Cpf").ToString()
                        associate.BirthDay = Convert.ToDateTime(reader("BirthDay"))
                        associates.Add(associate)
                    End While
                End Using
            End Using
        End Using

        Return associates
    End Function



    Public Sub AddAssociate(associate As Associates, companyIds As List(Of Integer))
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "INSERT INTO Associates (Name, Cpf, BirthDay) VALUES (@Name, @Cpf, @BirthDay); SELECT SCOPE_IDENTITY()"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Name", associate.Name)
                command.Parameters.AddWithValue("@Cpf", associate.Cpf)
                command.Parameters.AddWithValue("@BirthDay", associate.BirthDay)

                Dim associateId As Integer = Convert.ToInt32(command.ExecuteScalar())

                ' Vincula o associado às empresas
                If companyIds IsNot Nothing AndAlso companyIds.Count > 0 Then
                    Dim associatesCompaniesRepository As New AssociatesCompaniesRepository()
                    For Each companyId As Integer In companyIds
                        associatesCompaniesRepository.AddAssociateCompany(associateId, companyId)
                    Next
                End If
            End Using
        End Using
    End Sub

    Public Sub UpdateAssociate(associate As Associates, companyIds As List(Of Integer))
        Dim existingAssociate = GetAssociateById(associate.Id)

        If existingAssociate IsNot Nothing Then
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As String = "UPDATE Associates SET Name = @Name, Cpf = @Cpf, BirthDay = @BirthDay WHERE Id = @Id"

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Id", associate.Id)
                    command.Parameters.AddWithValue("@Name", associate.Name)
                    If existingAssociate.Cpf IsNot associate.Cpf Then
                        command.Parameters.AddWithValue("@Cpf", associate.Cpf)
                    End If
                    command.Parameters.AddWithValue("@BirthDay", associate.BirthDay)

                    command.ExecuteNonQuery()
                End Using

                ' Remove todas as associações anteriores
                Dim deleteQuery As String = "DELETE FROM AssociatesCompanies WHERE AssociateId = @AssociateId"

                Using deleteCommand As New SqlCommand(deleteQuery, connection)
                    deleteCommand.Parameters.AddWithValue("@AssociateId", associate.Id)
                    deleteCommand.ExecuteNonQuery()
                End Using

                ' Adiciona as novas associações
                If companyIds IsNot Nothing AndAlso companyIds.Count > 0 Then
                    Dim associatesCompaniesRepository As New AssociatesCompaniesRepository()
                    For Each companyId As Integer In companyIds
                        associatesCompaniesRepository.AddAssociateCompany(associate.Id, companyId)
                    Next
                End If
            End Using
        Else
            Throw New Exception("Associado não encontrado.")
        End If
    End Sub



    Public Sub DeleteAssociate(id As Integer)

        Dim existingAssociate = GetAssociateById(id)

        If existingAssociate IsNot Nothing Then
            ' Remover os vínculos do associado com as empresas
            Dim associatesCompaniesRepository As New AssociatesCompaniesRepository()
            associatesCompaniesRepository.RemoveAssociateCompanies(id)

            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As String = "DELETE FROM Associates WHERE Id = @Id"

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Id", id)

                    command.ExecuteNonQuery()
                End Using
            End Using
        Else
            Throw New Exception("Associado não encontrado.")
        End If
    End Sub
End Class
