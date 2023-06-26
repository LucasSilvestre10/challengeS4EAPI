Imports System.ComponentModel.DataAnnotations

Public Class Associates
    Public Property Id As Integer

    <Required(ErrorMessage:="Name is required")>
    Public Property Name As String

    <Required(ErrorMessage:="CPF is required")>
    Public Property Cpf As String

    <Required(ErrorMessage:="BirthDay is required")>
    Public Property BirthDay As Date

    Public Property CompanyIds As List(Of Integer)


    Public Property CompaniesList = New List(Of Companies)()

End Class
