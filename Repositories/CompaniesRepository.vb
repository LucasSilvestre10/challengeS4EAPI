Imports System.Data.SqlClient

Public Class CompaniesRepository
    Private connectionString As String = ConnectionHelper.GetConnectionString()

    Public Function GetCompanies() As List(Of Companies)
        Dim companies As New List(Of Companies)()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT c.Id, c.Name, c.Cnpj, a.Id AS AssociateId, a.Name AS AssociateName, a.Cpf AS Cpf FROM Companies c LEFT JOIN AssociatesCompanies ac ON c.Id = ac.CompanyId LEFT JOIN Associates a ON ac.AssociateId = a.Id"

            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim currentCompanyId As Integer = 0
                    Dim currentCompany As Companies = Nothing

                    While reader.Read()
                        Dim companyId As Integer = Convert.ToInt32(reader("Id"))

                        ' Verifica se é uma nova empresa
                        If companyId <> currentCompanyId Then
                            ' Cria uma nova instância de Companies
                            currentCompany = New Companies()
                            currentCompany.Id = companyId
                            currentCompany.Name = reader("Name").ToString()
                            currentCompany.Cnpj = reader("Cnpj").ToString()

                            companies.Add(currentCompany)
                            currentCompanyId = companyId
                        End If

                        ' Verifica se há um associado vinculado
                        If reader("AssociateId") IsNot DBNull.Value Then
                            Dim associate As New Associates()
                            associate.Id = Convert.ToInt32(reader("AssociateId"))
                            associate.Name = reader("AssociateName").ToString()
                            associate.Cpf = reader("Cpf").ToString()

                            currentCompany.AssociatesList.Add(associate)
                        End If
                    End While
                End Using
            End Using
        End Using

        Return companies
    End Function


    Public Function GetCompanyById(id As Integer) As Companies
        Dim company As New Companies()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT c.Id, c.Name, c.Cnpj, a.Id AS AssociateId, a.Name AS AssociateName, a.Cpf " &
                              "FROM Companies c " &
                              "LEFT JOIN AssociatesCompanies ac ON c.Id = ac.CompanyId " &
                              "LEFT JOIN Associates a ON ac.AssociateId = a.Id " &
                              "WHERE c.Id = @Id"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Id", id)

                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        If company.Id = 0 Then
                            company.Id = Convert.ToInt32(reader("Id"))
                            company.Name = reader("Name").ToString()
                            company.Cnpj = reader("Cnpj").ToString()
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("AssociateId")) Then
                            Dim associate As New Associates()
                            associate.Id = Convert.ToInt32(reader("AssociateId"))
                            associate.Name = reader("AssociateName").ToString()
                            associate.Cpf = reader("Cpf").ToString()
                            company.AssociatesList.Add(associate)
                        End If
                    End While
                End Using
            End Using
        End Using

        If company.Id = 0 Then
            Throw New Exception("Companhia não encontrada.")
        End If

        Return company
    End Function




    Public Function GetCompaniesByFilters(name As String, cnpj As String) As List(Of Companies)
        Dim companies As New List(Of Companies)()

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim query As String = "SELECT c.Id, c.Name, c.Cnpj, a.Id AS AssociateId, a.Name AS AssociateName FROM Companies c LEFT JOIN AssociatesCompanies ac ON c.Id = ac.CompanyId LEFT JOIN Associates a ON ac.AssociateId = a.Id WHERE 1=1"

            If Not String.IsNullOrEmpty(name) Then
                query += " AND c.Name LIKE @Name"
            End If

            If Not String.IsNullOrEmpty(cnpj) Then
                query += " AND c.Cnpj LIKE @Cnpj"
            End If

            Using command As New SqlCommand(query, connection)
                If Not String.IsNullOrEmpty(name) Then
                    command.Parameters.AddWithValue("@Name", "%" + name + "%")
                End If

                If Not String.IsNullOrEmpty(cnpj) Then
                    command.Parameters.AddWithValue("@Cnpj", "%" + cnpj + "%")
                End If

                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim currentCompanyId As Integer = 0
                    Dim company As Companies = Nothing

                    While reader.Read()
                        Dim companyId As Integer = Convert.ToInt32(reader("Id"))

                        If companyId <> currentCompanyId Then
                            company = New Companies()
                            company.Id = companyId
                            company.Name = reader("Name").ToString()
                            company.Cnpj = reader("Cnpj").ToString()
                            companies.Add(company)
                            currentCompanyId = companyId
                        End If
                        If Not reader.IsDBNull(reader.GetOrdinal("AssociateId")) Then
                            Dim associateId As Integer = Convert.ToInt32(reader("AssociateId"))
                            Dim associateName As String = reader("AssociateName").ToString()

                            If associateId > 0 Then
                                Dim associate As New Associates()
                                associate.Id = associateId
                                associate.Name = associateName
                                company.AssociatesList.Add(associate)
                            End If
                        End If
                    End While
                End Using
            End Using
        End Using

        Return companies
    End Function



    Public Sub AddCompany(company As Companies, associateIds As List(Of Integer))
        ' Verifica se a empresa já existe pelo ID



        Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As String = "INSERT INTO Companies (Name, Cnpj) VALUES (@Name, @Cnpj); SELECT SCOPE_IDENTITY();"

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Name", company.Name)
                    command.Parameters.AddWithValue("@Cnpj", company.Cnpj)

                    ' Executa a consulta e obtém o ID da empresa recém-inserida
                    Dim companyId As Integer = Convert.ToInt32(command.ExecuteScalar())

                    ' Verifica se há associados para adicionar
                    If associateIds IsNot Nothing AndAlso associateIds.Count > 0 Then
                        ' Insere as associações entre empresa e associados na tabela de associações
                        For Each associateId In associateIds
                            query = "INSERT INTO AssociatesCompanies (AssociateId, CompanyId) VALUES (@AssociateId, @CompanyId)"
                            command.Parameters.Clear()
                            command.Parameters.AddWithValue("@AssociateId", associateId)
                            command.Parameters.AddWithValue("@CompanyId", companyId)
                            command.CommandText = query
                            command.ExecuteNonQuery()
                        Next
                    End If
                End Using
            End Using

    End Sub



    Public Sub UpdateCompany(companyData As Companies)
        Dim existingCompany = GetCompanyById(companyData.Id)

        If existingCompany IsNot Nothing Then
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim transaction As SqlTransaction = connection.BeginTransaction()

                Try
                    Dim query As String = "UPDATE Companies SET Name = @Name, Cnpj = @Cnpj WHERE Id = @Id"

                    Using command As New SqlCommand(query, connection, transaction)
                        command.Parameters.AddWithValue("@Id", companyData.Id)
                        command.Parameters.AddWithValue("@Name", companyData.Name)
                        command.Parameters.AddWithValue("@Cnpj", companyData.Cnpj)

                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

                        If rowsAffected = 0 Then
                            transaction.Rollback()
                            Throw New Exception("Companhia não encontrada.")
                        End If
                    End Using

                    ' Remove associados existentes
                    Dim removeAssociatesQuery As String = "DELETE FROM AssociatesCompanies WHERE CompanyId = @CompanyId"

                    Using removeAssociatesCommand As New SqlCommand(removeAssociatesQuery, connection, transaction)
                        removeAssociatesCommand.Parameters.AddWithValue("@CompanyId", companyData.Id)
                        removeAssociatesCommand.ExecuteNonQuery()
                    End Using

                    ' Adiciona associados
                    If companyData.AssociatesIds IsNot Nothing AndAlso companyData.AssociatesIds.Count > 0 Then
                        Dim addAssociatesQuery As String = "INSERT INTO AssociatesCompanies (AssociateId, CompanyId) VALUES (@AssociateId, @CompanyId)"

                        For Each associateId As Integer In companyData.AssociatesIds
                            Using addAssociatesCommand As New SqlCommand(addAssociatesQuery, connection, transaction)
                                addAssociatesCommand.Parameters.AddWithValue("@AssociateId", associateId)
                                addAssociatesCommand.Parameters.AddWithValue("@CompanyId", companyData.Id)
                                addAssociatesCommand.ExecuteNonQuery()
                            End Using
                        Next
                    End If

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw ex
                End Try
            End Using
        End If
    End Sub



    Public Sub DeleteCompany(id As Integer)
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Dim transaction As SqlTransaction = connection.BeginTransaction()

            Try
                ' Verifica se a empresa existe
                Dim checkCompanyQuery As String = "SELECT COUNT(*) FROM Companies WHERE Id = @Id"

                Using checkCompanyCommand As New SqlCommand(checkCompanyQuery, connection, transaction)
                    checkCompanyCommand.Parameters.AddWithValue("@Id", id)
                    Dim companyCount As Integer = Convert.ToInt32(checkCompanyCommand.ExecuteScalar())

                    If companyCount = 0 Then
                        Throw New Exception("Companhia não encontrada.")
                    End If
                End Using

                ' Remove associados vinculados à empresa
                Dim removeAssociatesQuery As String = "DELETE FROM AssociatesCompanies WHERE CompanyId = @CompanyId"

                Using removeAssociatesCommand As New SqlCommand(removeAssociatesQuery, connection, transaction)
                    removeAssociatesCommand.Parameters.AddWithValue("@CompanyId", id)
                    removeAssociatesCommand.ExecuteNonQuery()
                End Using

                ' Remove a empresa
                Dim removeCompanyQuery As String = "DELETE FROM Companies WHERE Id = @Id"

                Using removeCompanyCommand As New SqlCommand(removeCompanyQuery, connection, transaction)
                    removeCompanyCommand.Parameters.AddWithValue("@Id", id)
                    removeCompanyCommand.ExecuteNonQuery()
                End Using

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw ex
            End Try
        End Using
    End Sub


End Class
