
'Copyright 2018 Kelvys B. Pantaleão

'This file is part of BMathInterpreter

'BMathInterpreter Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program.  If Not, see <http://www.gnu.org/licenses/>.


'Este arquivo é parte Do programa BMathInterpreter

'BMathInterpreter é um software livre; você pode redistribuí-lo e/ou 
'modificá-lo dentro dos termos da Licença Pública Geral GNU como 
'publicada pela Fundação Do Software Livre (FSF); na versão 3 da 
'Licença, ou(a seu critério) qualquer versão posterior.

'Este programa é distribuído na esperança de que possa ser  útil, 
'mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO
'a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a
'Licença Pública Geral GNU para maiores detalhes.

'Você deve ter recebido uma cópia da Licença Pública Geral GNU junto
'com este programa, Se não, veja <http://www.gnu.org/licenses/>.

'GitHub: https://github.com/Kelvysb/BMathInterpreter

Imports System.IO
Imports Newtonsoft.Json

Public Class clsExpression
#Region "Declarations"
    Private strExpression As String
    Private strVariables As List(Of List(Of String))
#End Region

#Region "Constructor"

    Public Sub New()
        Try
            strExpression = ""
            strVariables = New List(Of List(Of String))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(p_strExpression As String, p_objVariables As Dictionary(Of String, String))
        Try
            strExpression = p_strExpression
            strVariables = New List(Of List(Of String))

            If p_objVariables.Keys.Count > 0 Then
                strVariables = (From Item As KeyValuePair(Of String, String) In p_objVariables
                                Select {Item.Key, Item.Value}.ToList).ToList

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Functions"
    Public Function Clone() As clsExpression
        Try
            Return New clsExpression(strExpression, VariablesDict)
        Catch Ex As Exception
            Throw Ex
        End Try
    End Function

    Public Function Serialize() As String
        Dim Retorno As String
        Try
            Retorno = JsonConvert.SerializeObject(Me)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Serialize(p_objBloco As clsExpression) As String
        Dim Retorno As String
        Try
            Retorno = JsonConvert.SerializeObject(p_objBloco)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Deserialize(p_strJson As String) As clsExpression
        Dim Retorno As clsExpression
        Try
            Retorno = JsonConvert.DeserializeObject(Of clsExpression)(p_strJson)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function Load(p_strPath As String) As clsExpression
        Dim objReturn As clsExpression
        Dim objFile As StreamReader
        Dim strAuxFile As String

        Try
            objReturn = Nothing

            If File.Exists(p_strPath) Then

                objFile = New StreamReader(p_strPath)
                strAuxFile = objFile.ReadToEnd
                objFile.Close()
                objFile.Dispose()
                objFile = Nothing
                objReturn = clsExpression.Deserialize(strAuxFile)

            Else
                Throw New Exception("File not found: " & p_strPath)
            End If

            Return objReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub Save(p_strPath As String)
        Dim objFile As StreamWriter
        Try
            objFile = New StreamWriter(p_strPath)
            Call objFile.WriteLine(Serialize)
            objFile.Close()
            objFile.Dispose()
            objFile = Nothing
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function fnMoutDictionary() As Dictionary(Of String, String)
        Dim objReturn As Dictionary(Of String, String)
        Try
            objReturn = New Dictionary(Of String, String)
            strVariables.ForEach(Sub(item As List(Of String)) objReturn.Add(item.First, item.Last))
            Return objReturn
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Properties"
    <JsonProperty("EXPRESSION")>
    Public Property Expression As String
        Get
            Return strExpression
        End Get
        Set(value As String)
            strExpression = value
        End Set
    End Property

    <JsonProperty("VARIABLES")>
    Public Property Variables As List(Of List(Of String))
        Get
            Return strVariables
        End Get
        Set(value As List(Of List(Of String)))
            strVariables = value
        End Set
    End Property

    <JsonIgnore>
    Public ReadOnly Property VariablesDict As Dictionary(Of String, String)
        Get
            Return fnMoutDictionary()
        End Get
    End Property
#End Region
End Class
