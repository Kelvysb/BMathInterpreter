
'Copyright 2018 Kelvys B. Pantaleão

'This file is part of BMath

'BMath Is free software: you can redistribute it And/Or modify
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

Public Class uscVariable

#Region "Declarations"
    Private strKey As String
    Private strValue As String
#End Region

#Region "Constructor"
    Public Sub New(p_strName As String, p_strValue As String)
        Try
            InitializeComponent()
            strKey = p_strName
            strValue = p_strValue
            lblName.Content = strKey
            txtValue.Text = strValue
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Events"
    Private Sub txtValue_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtValue.PreviewTextInput
        Dim objRegEx As Text.RegularExpressions.Regex
        Try
            'Uses a regex to filter the input (note de negate '^', if the char is not in the range 0-9 or is not a parentesis the handled will be set true)
            objRegEx = New Text.RegularExpressions.Regex("[^0-9 | \( | \) | \+ | \- | \/ | \* | ¬ | % | \\ | \^]+")
            e.Handled = objRegEx.IsMatch(e.Text)
            strValue = txtValue.Text
            RaiseEvent evChanged(sender)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As RoutedEventArgs) Handles btnRemove.Click
        Try
            RaiseEvent evRemove(Me)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnInsert_Click(sender As Object, e As RoutedEventArgs) Handles btnInsert.Click
        Try
            RaiseEvent evInsert(Me)
        Catch ex As Exception

        End Try
    End Sub

    Public Event evChanged(sender As Object)

    Public Event evRemove(sender As Object)

    Public Event evInsert(sender As Object)
#End Region

#Region "Functions"

#End Region

#Region "Properties"
    Public Property Key As String
        Get
            Return strKey
        End Get
        Set(value As String)
            strKey = value
        End Set
    End Property

    Public Property Value As String
        Get
            Return strValue
        End Get
        Set(value As String)
            strValue = value
        End Set
    End Property
#End Region

End Class
