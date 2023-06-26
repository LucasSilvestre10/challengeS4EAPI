Imports System.ComponentModel.DataAnnotations

Public Class Companies
    Public Property Id As Integer

    <Required(ErrorMessage:="Name is required")>
    Public Property Name As String

    <Required(ErrorMessage:="Cnpj is required")>
    Public Property Cnpj As String


    Public Property AssociatesIds As List(Of Integer)
    Public Property AssociatesList = New List(Of Associates)()
End Class
