
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

Public Class Interpreter

#Region "Declarations"

#End Region

#Region "Constructors"
    Private Sub New()

    End Sub
#End Region

#Region "Functions"

    ''' <summary>
    ''' Evaluate the expression
    ''' </summary>
    ''' <param name="p_objExpression">Expression class</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnEvaluate(p_objExpression As clsExpression) As Double
        Try
            Return fnEvaluate(p_objExpression.Expression, p_objExpression.VariablesDict)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Evaluate the expression
    ''' </summary>
    ''' <param name="p_strExpression">Math Expression</param>
    ''' <param name="p_objVaribles">Expression variables</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnEvaluate(p_strExpression As String, p_objVaribles As Dictionary(Of String, String)) As Double

        Dim strExpression As String
        Dim dblReturn As Double

        Try

            dblReturn = 0

            strExpression = p_strExpression.Replace(" ", "")

            For Each Key As String In p_objVaribles.Keys
                strExpression = strExpression.Replace(Key, p_objVaribles(Key))
            Next

            dblReturn = fnInterpreter(strExpression)

            Return dblReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    '''  Evaluate the expression
    ''' </summary>
    ''' <param name="p_strExpression">Math Expression</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnEvaluate(p_strExpression As String) As Double
        Try
            Return fnEvaluate(p_strExpression, New Dictionary(Of String, String))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Validate the expression
    ''' </summary>
    ''' <param name="p_objExpression">Expression class</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnValidate(p_objExpression As clsExpression) As Double
        Try
            Return fnValidate(p_objExpression.Expression, p_objExpression.VariablesDict)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Validate the expression
    ''' </summary>
    ''' <param name="p_strExpression">Math Expression</param>
    ''' <param name="p_objVaribles">Expression variables</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnValidate(p_strExpression As String, p_objVaribles As Dictionary(Of String, String)) As String

        Dim retorno As String
        Dim strExpression As String

        Try

            retorno = ""

            strExpression = p_strExpression.Replace(" ", "")

            For Each Key As String In p_objVaribles.Keys
                strExpression = strExpression.Replace(Key, p_objVaribles(Key))
            Next

            Try
                Call fnInterpreter(strExpression)
                retorno = ""
            Catch ex As Exception
                retorno = ex.Message
            End Try

            Return retorno

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Validate the expression
    ''' </summary>
    ''' <param name="p_strExpression">Math Expression</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnValidate(p_strExpression As String) As String
        Try
            Return fnValidate(p_strExpression, New Dictionary(Of String, String))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Break variables from a expression.
    ''' </summary>
    ''' <param name="p_strExpressions">Expression</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnBreakVariables(p_strExpressions As String) As Dictionary(Of String, String)

        Dim objReturn As Dictionary(Of String, String)
        Dim strAuxVariable As String
        Dim blnStarted As Boolean

        Try

            objReturn = New Dictionary(Of String, String)

            strAuxVariable = ""
            blnStarted = False
            For i = 0 To p_strExpressions.Length - 1

                If p_strExpressions(i) = "#" Then
                    blnStarted = True
                    strAuxVariable = p_strExpressions(i)
                ElseIf blnStarted = True And p_strExpressions(i) <> ";" Then
                    strAuxVariable = strAuxVariable & p_strExpressions(i)
                ElseIf p_strExpressions(i) = ";" And blnStarted = True Then
                    blnStarted = False
                    strAuxVariable = strAuxVariable & p_strExpressions(i)
                    If objReturn.Keys.Contains(strAuxVariable) = False Then
                        objReturn.Add(strAuxVariable, "0")
                    End If
                    strAuxVariable = ""
                End If

            Next

            If blnStarted = True Then
                Throw New Exception("Expression error, bad formated.")
            End If

            Return objReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function fnInterpreter(p_strExpression As String) As Double

        Dim dblReturn As Double
        Dim intPar As Integer
        Dim intStart As Integer
        Dim intEnd As Integer
        Dim strSubExpression As String
        Dim strExpression As String

        Try
            dblReturn = 0

            If p_strExpression.Contains("(") = True Or p_strExpression.Contains(")") Then

                intPar = 0
                intStart = -1
                intEnd = -1

                For i = 0 To p_strExpression.Length - 1

                    If p_strExpression(i) = "(" Then
                        intPar = intPar + 1
                        If intStart = -1 Then
                            intStart = i
                        End If
                    ElseIf p_strExpression(i) = ")" Then
                        intPar = intPar - 1
                    End If

                    If intStart <> -1 And intPar = 0 Then
                        intEnd = i
                        Exit For
                    End If

                Next

                If intPar = 0 Then

                    strSubExpression = p_strExpression.Substring(intStart + 1, intEnd - intStart - 1)

                    dblReturn = fnInterpreter(strSubExpression)

                    strExpression = p_strExpression.Replace("(" & strSubExpression & ")", dblReturn.ToString.Replace(",", "."))

                    dblReturn = fnInterpreter(strExpression)

                Else
                    Throw New Exception("Unclosed parentesis.")
                End If

            Else
                dblReturn = fnFinalizeCalculation(p_strExpression)
            End If

            Return dblReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function fnFinalizeCalculation(p_strExpression As String) As Double

        Dim dblReturn As Double
        Dim strExpression As String

        Try
            dblReturn = 0
            strExpression = p_strExpression

            'Properties
            strExpression = strExpression.Replace("--", "+")
            strExpression = strExpression.Replace("+-", "-")
            strExpression = strExpression.Replace("-+", "-")
            strExpression = strExpression.Replace("++", "+")

            'Square Root
            strExpression = fnReturnSqureRoot(strExpression)

            'Power and square root
            strExpression = fnReturnOperation(strExpression, "^", " ")

            'Module and Perc
            strExpression = fnReturnOperation(strExpression, "\", "%")

            'Multiply and Divide
            strExpression = fnReturnOperation(strExpression, "*", "/")

            'Sum and Subtraction
            strExpression = fnReturnOperation(strExpression, "+", "-")

            Try
                dblReturn = Double.Parse(strExpression)
            Catch ex As Exception
                Throw New Exception("Error.")
            End Try

            Return dblReturn

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function fnReturnSqureRoot(p_Expression As String) As String
        Dim dblAuxResult As String
        Dim strReturn As String
        Dim intIndex As Integer
        Dim intEnd As Integer
        Dim intStart As Integer
        Dim dblFactor As Double
        Dim strSubExpression As String

        Try

            dblAuxResult = 0
            strReturn = p_Expression.Trim

            Do While strReturn.Contains("¬")

                intIndex = strReturn.Trim.IndexOf("¬")
                intStart = intIndex + 1

                If intIndex <> -1 Then

                    intEnd = -1
                    For i = intStart + 1 To strReturn.Length - 1
                        If (IsNumeric(strReturn(i)) = False And strReturn(i) <> ".") Then
                            intEnd = i
                            If strReturn.Substring(intIndex + 1, intEnd - intIndex - 1) <> "" Then

                                If (strReturn.Substring(intIndex + 1, intEnd - intIndex - 1)).EndsWith("-") Then
                                    intEnd = intEnd - 1
                                End If
                                dblFactor = Double.Parse(strReturn.Substring(intIndex + 1, intEnd - intIndex - 1))
                                Exit For
                            Else
                                intEnd = -1
                            End If
                        End If
                    Next

                    If intEnd = -1 Then
                        intEnd = strReturn.Length
                        If strReturn.Substring(intIndex + 1, intEnd - intIndex - 1) <> "" Then
                            dblFactor = Double.Parse(strReturn.Substring(intIndex + 1, intEnd - intIndex - 1))
                        Else
                            Exit Do
                        End If
                    End If

                    strSubExpression = strReturn.Substring(intIndex, intEnd - intIndex)

                    dblAuxResult = Math.Sqrt(dblFactor)

                    strReturn = strReturn.Replace(strSubExpression, dblAuxResult.ToString.Replace(",", "."))

                Else
                    Exit Do
                End If

            Loop

            Return strReturn

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Shared Function fnReturnOperation(p_Expression As String, p_strOperation1 As String, p_strOperation2 As String) As String

        Dim dblAuxResult As String
        Dim strReturn As String
        Dim intIndex As Integer
        Dim intIndexOp1 As Integer
        Dim intIndexOp2 As Integer
        Dim intStart As Integer
        Dim intEnd As Integer
        Dim dblFactor1 As Double
        Dim dblFactor2 As Double
        Dim strSubExpression As String
        Dim blnNegativeFlag As Boolean

        Try

            dblAuxResult = 0
            strReturn = p_Expression

            Do While strReturn.Contains(p_strOperation1) Or strReturn.Contains(p_strOperation2)

                intIndexOp1 = strReturn.IndexOf(p_strOperation1, 1)
                intIndexOp2 = strReturn.IndexOf(p_strOperation2, 1)

                If intIndexOp1 = -1 Then
                    intIndex = intIndexOp2
                ElseIf intIndexOp2 = -1 Then
                    intIndex = intIndexOp1
                Else
                    If intIndexOp1 < intIndexOp2 Then
                        intIndex = intIndexOp1
                    Else
                        intIndex = intIndexOp2
                    End If
                End If

                If intIndex <> -1 Then

                    intStart = -1
                    blnNegativeFlag = False
                    For i = intIndex - 1 To 0 Step -1
                        If (IsNumeric(strReturn(i)) = False And strReturn(i) <> "." And strReturn(i) <> "-") Or blnNegativeFlag Then
                            intStart = i + 1
                            dblFactor1 = Double.Parse(strReturn.Substring(intStart, intIndex - intStart))
                            Exit For
                        ElseIf strReturn(i) = "-" Then
                            blnNegativeFlag = True
                        End If
                    Next
                    If intStart = -1 Then
                        intStart = 0
                        If strReturn.Substring(intStart, intIndex - intStart) <> "" Then
                            dblFactor1 = Double.Parse(strReturn.Substring(intStart, intIndex - intStart))
                        Else
                            Exit Do
                        End If
                    End If

                    intEnd = -1
                    For i = intIndex + 1 To strReturn.Length - 1
                        If (IsNumeric(strReturn(i)) = False And strReturn(i) <> ".") Then
                            intEnd = i
                            If strReturn.Substring(intIndex + 1, intEnd - intIndex - 1) <> "" Then

                                If (strReturn.Substring(intIndex + 1, intEnd - intIndex - 1)).EndsWith("-") Then
                                    intEnd = intEnd - 1
                                End If
                                dblFactor2 = Double.Parse(strReturn.Substring(intIndex + 1, intEnd - intIndex - 1))
                                Exit For
                            Else
                                intEnd = -1
                            End If
                        End If
                    Next


                    If intEnd = -1 Then
                            intEnd = strReturn.Length
                            If strReturn.Substring(intIndex + 1, intEnd - intIndex - 1) <> "" Then
                                dblFactor2 = Double.Parse(strReturn.Substring(intIndex + 1, intEnd - intIndex - 1))
                            Else
                                Exit Do
                            End If
                        End If


                    strSubExpression = strReturn.Substring(intStart, intEnd - intStart)

                    Select Case strReturn(intIndex)
                        Case "^"
                            dblAuxResult = dblFactor1 ^ dblFactor2
                        Case "*"
                            dblAuxResult = dblFactor1 * dblFactor2
                        Case "%"
                            If dblFactor2 <> 0 Then
                                dblAuxResult = (dblFactor1 / dblFactor2) * 100
                            Else
                                Throw New Exception("Division by 0.")
                            End If
                        Case "/"
                            If dblFactor2 <> 0 Then
                                dblAuxResult = dblFactor1 / dblFactor2
                            Else
                                Throw New Exception("Division by 0.")
                            End If
                        Case "\"
                            If dblFactor2 <> 0 Then
                                dblAuxResult = dblFactor1 Mod dblFactor2
                            Else
                                Throw New Exception("Division by 0.")
                            End If
                        Case "+"
                            dblAuxResult = dblFactor1 + dblFactor2
                        Case "-"
                            dblAuxResult = dblFactor1 - dblFactor2
                        Case Else
                            'dblAuxResultado = dblFator1 + dblFator2
                            Throw New Exception("Invalid operation: " & strReturn(intIndex))
                    End Select

                    strReturn = strReturn.Replace(strSubExpression, dblAuxResult.ToString.Replace(",", "."))

                Else
                    Exit Do
                End If

            Loop

            Return strReturn

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Properties"

#End Region

End Class
