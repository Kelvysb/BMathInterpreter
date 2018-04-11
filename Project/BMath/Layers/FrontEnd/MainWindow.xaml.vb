
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

Imports BMathInterpreter
Imports Microsoft.Win32

Class MainWindow

#Region "Declarations"
    Private objVariables As List(Of uscVariable)
#End Region

#Region "Constructor"

#End Region

#Region "Events"
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            Call sbLoadScreen()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub txtInput_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtInput.PreviewTextInput
        Dim objRegEx As Text.RegularExpressions.Regex
        Try
            'Uses a regex to filter the input (note de negate '^', if the char is not in the range 0-9 or is not a acceptable operator or parentesis the handled will be set true)
            objRegEx = New Text.RegularExpressions.Regex("[^0-9 | \( | \) | \+ | \- | \/ | \* | ¬ | % | \\ | \^]+")
            e.Handled = objRegEx.IsMatch(e.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtInput_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtInput.TextChanged
        Try
            Call sbEvaluate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub variable_Changed(sender As uscVariable)
        Try
            Call sbEvaluate()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub variable_Remove(sender As uscVariable)
        Try
            Call sbRemoveVariable(sender)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub variable_Insert(sender As uscVariable)
        Try
            Call sbInputValue(sender.Key)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnAddVariable_Click(sender As Object, e As RoutedEventArgs) Handles btnAddVariable.Click
        Try
            Call sbAddVariable()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub Keys_Click(sender As Object, e As RoutedEventArgs) Handles btn0.Click, btn1.Click, btn2.Click,
                                                                            btn3.Click, btn4.Click, btn5.Click,
                                                                            btn6.Click, btn7.Click, btn8.Click,
                                                                            btn9.Click, btnPlus.Click, btnMinus.Click,
                                                                            btnMultiply.Click, btnDivide.Click,
                                                                            btnParentesisOpen.Click, btnParentesisClose.Click,
                                                                            btnComma.Click, btnPot.Click, btnPerc.Click
        Try
            Call sbInputValue(DirectCast(sender, Button).Content.Trim)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnSqr_Click(sender As Object, e As RoutedEventArgs) Handles btnSqr.Click
        Try
            Call sbInputValue("¬")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnMod_Click(sender As Object, e As RoutedEventArgs) Handles btnMod.Click
        Try
            Call sbInputValue("\")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        Try
            Call sbSave()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As RoutedEventArgs) Handles btnOpen.Click
        Try
            Call sbOpen()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnResultoTovariable_Click(sender As Object, e As RoutedEventArgs) Handles btnResultoTovariable.Click
        Try
            Call sbAddVariable(txtResult.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnInputToVariable_Click(sender As Object, e As RoutedEventArgs) Handles btnInputToVariable.Click
        Try
            Call sbAddVariable(txtInput.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnParentesis_Click(sender As Object, e As RoutedEventArgs) Handles btnParentesis.Click
        Try
            Call sbSetParentesis()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As RoutedEventArgs) Handles btnClear.Click
        Try
            Call sbLoadScreen()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Title)
        End Try
    End Sub
#End Region

#Region "Functions"
    Private Sub sbLoadScreen()
        Try
            objVariables = New List(Of uscVariable)
            stkVarables.Children.Clear()
            txtVariableName.Text = ""
            txtInput.Text = ""
            txtResult.Text = "0"
            txtInput.Focus()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbAddVariable()
        Try
            sbAddVariable("0")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbAddVariable(p_strValue As String)
        Try
            If fnAddVariable(txtVariableName.Text, p_strValue) = True Then
                txtVariableName.Text = ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function fnAddVariable(p_strKey As String, p_strValue As String) As Boolean

        Dim strAuxVariableName As String
        Dim blnReturn As Boolean
        Dim intCounter As Integer

        Try

            If Not p_strValue.Contains("#") Then
                strAuxVariableName = p_strKey
                blnReturn = False

                intCounter = objVariables.Count + 1
                If strAuxVariableName.Trim = "" Then
                    Do
                        strAuxVariableName = "#Var" & intCounter & ";"
                        intCounter = intCounter + 1
                    Loop While objVariables.Find(Function(Item As uscVariable) Item.Key.ToString.Trim.ToUpper = strAuxVariableName.Trim.ToUpper) IsNot Nothing
                End If

                If strAuxVariableName.StartsWith("#") = False Then
                    strAuxVariableName = "#" & strAuxVariableName
                End If
                If strAuxVariableName.EndsWith(";") = False Then
                    strAuxVariableName = strAuxVariableName & ";"
                End If

                If objVariables.Find(Function(Item As uscVariable) Item.Key.ToString.Trim.ToUpper = strAuxVariableName.Trim.ToUpper) Is Nothing Then
                    objVariables.Add(New uscVariable(strAuxVariableName.Trim, p_strValue))
                    objVariables.Last.Margin = New Thickness(2)
                    AddHandler objVariables.Last.evChanged, AddressOf variable_Changed
                    AddHandler objVariables.Last.evRemove, AddressOf variable_Remove
                    AddHandler objVariables.Last.evInsert, AddressOf variable_Insert
                    stkVarables.Children.Add(objVariables.Last)
                Else
                    MsgBox(My.Resources.msgVariableDeclared, MsgBoxStyle.Exclamation, My.Resources.Title)
                End If
            Else
                MsgBox(My.Resources.msgSubVariables, MsgBoxStyle.Exclamation, My.Resources.Title)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub sbRemoveVariable(p_objVariable As uscVariable)
        Try
            stkVarables.Children.Remove(p_objVariable)
            objVariables.Remove(p_objVariable)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbInputValue(p_strValue As String)
        Dim intPosition As Integer
        Try
            intPosition = txtInput.CaretIndex
            txtInput.Text = txtInput.Text.Insert(txtInput.CaretIndex, p_strValue)
            txtInput.CaretIndex = intPosition + 1
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbSetParentesis()
        Dim intPosition As Integer
        Try
            intPosition = txtInput.CaretIndex
            If txtInput.SelectedText.Trim <> "" Then
                txtInput.SelectedText = "(" & txtInput.SelectedText & ")"
            Else
                Call sbInputValue("()")
            End If
            txtInput.CaretIndex = intPosition + 2
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbEvaluate()

        Dim objVariablesDict As Dictionary(Of String, String)

        Try

            objVariablesDict = New Dictionary(Of String, String)

            If txtInput.Text.Trim <> "" Then
                If objVariables.Count > 0 Then
                    objVariables.ForEach(Sub(Item As uscVariable) objVariablesDict.Add(Item.Key, Item.Value))
                    txtResult.Text = Interpreter.fnEvaluate(txtInput.Text, objVariablesDict)
                Else
                    txtResult.Text = Interpreter.fnEvaluate(txtInput.Text)
                End If
            Else
                txtResult.Text = 0
            End If
        Catch ex As Exception
            txtResult.Text = ex.Message
        End Try
    End Sub

    Private Sub sbSave()

        Dim objExpression As clsExpression
        Dim objDialog As SaveFileDialog
        Dim objVariablesDict As Dictionary(Of String, String)

        Try

            objDialog = New SaveFileDialog()
            objDialog.Title = "Save expression"
            objDialog.FileName = "Expression_" & Now.ToString("ddMMyyyyHHmmss") & ".exp"
            objDialog.Filter = "Expression|*.exp"

            If objDialog.ShowDialog = True Then
                objVariablesDict = New Dictionary(Of String, String)
                objVariables.ForEach(Sub(Item As uscVariable) objVariablesDict.Add(Item.Key, Item.Value))
                objExpression = New clsExpression(txtInput.Text, objVariablesDict)
                objExpression.Save(objDialog.FileName)
            End If

            objDialog = Nothing

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub sbOpen()

        Dim objExpression As clsExpression
        Dim objDialog As OpenFileDialog
        Dim objVariablesDict As Dictionary(Of String, String)

        Try

            objDialog = New OpenFileDialog()
            objDialog.Title = "Open expression"
            objDialog.Filter = "Expression|*.exp"

            If objDialog.ShowDialog = True Then

                objExpression = clsExpression.Load(objDialog.FileName)
                objVariablesDict = objExpression.VariablesDict

                Call sbLoadScreen()

                For Each Item As KeyValuePair(Of String, String) In objVariablesDict
                    Call fnAddVariable(Item.Key, Item.Value)
                Next

                txtInput.Text = objExpression.Expression

                Call sbEvaluate()

            End If

            objDialog = Nothing

        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

#End Region

End Class
