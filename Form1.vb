Imports System.Threading
Public Class Form1
    Public t As Thread
    Public boxX As Integer
    Public boxY As Integer
    Public Structure Point
        Public X As Integer
        Public Y As Integer
    End Structure
    'Public head As New Point
    'Public tail As New Point
    Public body() As Point
    Private Delegate Sub VoidDelegate()
    Public bmp As Bitmap
    Public food As Point
    Public key As String = "None"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Visible = False
        t.Abort()
        t = New Thread(AddressOf GameLoad)
        t.Start()
        Timer1.Enabled = True
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        t = New Thread(AddressOf GameLoad)
        t.Start()
        Timer1.Interval = 200
        Timer1.Enabled = True
    End Sub

    Private Sub GameLoad()
        Initiate()

        Show()
        GenerateFood()

    End Sub
    'WriteOnly Property SetTail(ByVal tail As Point)
    '    Set(ByVal tail As Point)
    '        a = tail.X

    '    End Set
    'End Property


    Private Sub Initiate()
        boxX = PictureBox1.Width
        boxY = PictureBox1.Height
        bmp = New Bitmap(boxX, boxY, Imaging.PixelFormat.Format32bppArgb)
        ReDim body(4)

        For i = 0 To boxX / 2 - 1
            PrintPoints(i, 0, Color.White)
        Next i
        For i = 0 To boxX / 2 - 1
            PrintPoints(i, boxY / 2 - 1, Color.White)
        Next i
        For i = 0 To boxY / 2 - 1
            PrintPoints(0, i, Color.White)
        Next i
        For i = 0 To boxY / 2 - 1
            PrintPoints(boxX / 2 - 1, i, Color.White)
        Next i

        For i = 0 To body.Length - 1
            body(i).X = i
            body(i).Y = 0
        Next i
        'GenerateFood()
        'Timer1.Interval = 1000
        'Timer1.Enabled = True

    End Sub

    Private Sub Move(ByVal direction As String)




        '------------------------判断下一步会不会 犯规
        Dim nextPoint As Point
        Select Case direction
            Case "Up"
                nextPoint.X = body(body.Length - 1).X
                nextPoint.Y = body(body.Length - 1).Y - 1
            Case "Down"
                nextPoint.X = body(body.Length - 1).X
                nextPoint.Y = body(body.Length - 1).Y + 1
            Case "Left"
                nextPoint.X = body(body.Length - 1).X - 1
                nextPoint.Y = body(body.Length - 1).Y
            Case "Right"
                nextPoint.X = body(body.Length - 1).X + 1
                nextPoint.Y = body(body.Length - 1).Y
        End Select
        If nextPoint.X = body(body.Length - 2).X And nextPoint.Y = body(body.Length - 2).Y Then ' 不可能完全朝着相反方向走
            Exit Sub
        End If
        If IsCrash(nextPoint) Then '如果犯规
            Timer1.Enabled = False
            Button1.Visible = True
            Exit Sub
        End If
        '--------------------------------
        Dim DoesEat As Boolean = False
        Select Case direction
            Case "Up"
                If body(body.Length - 1).X = food.X And body(body.Length - 1).Y - 1 = food.Y Then
                    DoesEat = True
                End If
            Case "Down"
                If body(body.Length - 1).X = food.X And body(body.Length - 1).Y + 1 = food.Y Then
                    DoesEat = True
                End If
            Case "Left"
                If body(body.Length - 1).X - 1 = food.X And body(body.Length - 1).Y = food.Y Then
                    DoesEat = True
                End If
            Case "Right"
                If body(body.Length - 1).X + 1 = food.X And body(body.Length - 1).Y = food.Y Then
                    DoesEat = True
                End If
        End Select

        If DoesEat Then
            Dim tempX As Integer
            Dim tempY As Integer
            tempX = body(body.Length - 1).X
            tempY = body(body.Length - 1).Y
            ReDim Preserve body(body.Length + 1) '这玩意会把 原来 数组的最后一位给删掉
            body(body.Length - 2).X = tempX
            body(body.Length - 2).Y = tempY
            body(body.Length - 1).X = food.X
            body(body.Length - 1).Y = food.Y
            GenerateFood()

        Else


            '清屁股=====================
            'Dim x, y As Integer

            'x = body(0).X * 2
            'y = body(0).Y * 2
            'Me.Invoke(New VoidDelegate(Sub()
            '                               For xValue = x To x + 1
            '                                   For yValue = y To y + 1

            '                                       bmp.SetPixel(xValue, yValue, Color.FromArgb(255, 255, 255))
            '                                       PictureBox1.Image = bmp
            '                                   Next
            '                               Next
            '                           End Sub))
            PrintPoints(body(0).X, body(0).Y, Color.White)
            '清屁股完毕===================


            Dim i As Integer
            For i = 0 To body.Length - 2
                body(i).X = body(i + 1).X
                body(i).Y = body(i + 1).Y
                'Stop
            Next i 'i += 1



            Select Case direction
                Case "Up"
                    body(i).Y -= 1
                Case "Down"
                    body(i).Y += 1
                Case "Left"
                    body(i).X -= 1
                Case "Right"
                    body(i).X += 1
            End Select

        End If
        Show()
    End Sub

    Private Sub Show()
        'Me.Invoke(New VoidDelegate(Sub() '清屏
        '                               bmp.Dispose()
        '                               boxX = PictureBox1.Width
        '                               boxY = PictureBox1.Height
        '                               bmp = New Bitmap(boxX, boxY, Imaging.PixelFormat.Format32bppArgb)
        '                           End Sub))


        For i = 0 To (body.Length - 1)
            PrintPoints(body(i).X, body(i).Y, Color.Black)
        Next i

        'Dim x, y As Integer
        'For i = 0 To (body.Length - 1)
        '    x = body(i).X * 2
        '    y = body(i).Y * 2
        'Me.Invoke(New VoidDelegate(Sub()
        '                               For xValue = x To x + 1
        '                                   For yValue = y To y + 1

        '                                       bmp.SetPixel(xValue, yValue, Color.FromArgb(0, 0, 0))
        '                                       PictureBox1.Image = bmp
        '                                   Next
        '                               Next
        '                           End Sub))
        'Next
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        key = "Down"
        'Show()
    End Sub

    Private Sub BtnRight_Click(sender As Object, e As EventArgs) Handles BtnRight.Click
        key = "Right"
        'Show()
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        key = "Up"
        'Show()
    End Sub

    Private Sub BtnLeft_Click(sender As Object, e As EventArgs) Handles BtnLeft.Click
        key = "Left"
        'Show()
    End Sub

    Private Sub PrintPoints(ByVal x As Integer, ByVal y As Integer, ByVal color As Color)
        x *= 2
        y *= 2
        Me.Invoke(New VoidDelegate(Sub()
                                       For xValue = x To x + 1
                                           For yValue = y To y + 1

                                               bmp.SetPixel(xValue, yValue, color)
                                               PictureBox1.Image = bmp
                                           Next
                                       Next
                                   End Sub))
    End Sub

    Private Sub GenerateFood()
        Dim rnd As New Random
        Dim IsRight As Boolean = False
        While Not IsRight
            food.X = rnd.Next(boxX / 2 - 1)
            food.Y = rnd.Next(boxY / 2 - 1)
            IsRight = True
            For i = 0 To body.Length - 1
                If body(i).X = food.X And body(i).Y = food.Y Then
                    IsRight = False
                    Exit For
                End If
            Next i
        End While
        PrintPoints(food.X, food.Y, Color.Red)
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Down Then
            key = "Down"
        End If
        If e.KeyCode = Keys.Up Then
            key = "Up"
        End If
        If e.KeyCode = Keys.Left Then
            key = "Left"
        End If
        If e.KeyCode = Keys.Right Then
            key = "Right"
        End If
        'Show()
    End Sub

    Private Function IsCrash(ByVal nextPoint As Point) As Boolean
        Dim IsCrashValue As Boolean = False
        If nextPoint.X >= boxX / 2 - 1 Or nextPoint.X < 0 Or nextPoint.Y >= boxY / 2 - 1 Or nextPoint.Y < 0 Then
            IsCrashValue = True
        End If
        For i = 1 To body.Length - 3 ' 不可能完全朝着相反方向走，所以不考虑 body.Length - 2 (数组最后第二个)
            If nextPoint.X = body(i).X And nextPoint.Y = body(i).Y Then
                IsCrashValue = True
                Exit For
            End If
        Next i

        Return IsCrashValue
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If key = "None" Then
            If body(body.Length - 1).X > body(body.Length - 2).X Then
                key = "Right"
            ElseIf body(body.Length - 1).X < body(body.Length - 2).X Then
                key = "Left"
            ElseIf body(body.Length - 1).y > body(body.Length - 2).y Then
                key = "Down"
            ElseIf body(body.Length - 1).y < body(body.Length - 2).y Then
                key = "Up"
            End If
        End If

        Move(key)
        key = "None"
    End Sub
End Class
