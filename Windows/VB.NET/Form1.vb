Imports MySql.Data.MySqlClient



'Home form
Public Class Form1
    Dim con As New MySqlConnection(My.Settings.Connection)
    Dim reader As MySqlDataReader
    Dim cmd As MySqlCommand
    Dim query As String
    Dim str As String
    Dim collagecode As String



    'On load
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        collagecode = "7317"
        check()
        Label3.Text = collagecode
    End Sub



    'Login menu click
    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Panel3.Visible = False
        Panel1.Visible = True
        Panel2.Visible = True
    End Sub



    'About menu click
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Panel1.Visible = False
        Panel2.Visible = False
        Panel3.Visible = True
    End Sub



    'Link clicked
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://www.ananthsoft.org")
    End Sub



    'Advisor login button click
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        query = "select * from advisor where classcode='" & collagecode + TextBox1.Text & "' and password='" & TextBox2.Text & "'"
        str = exe(query)
        If str = "valid" Then
            ProgressBar.Visible = False
            Form2.Show()
            Me.Hide()
        ElseIf str = "invalid" Then
            MsgBox("Invalid class code or password" + vbCrLf + "Or else contact your department", vbCritical, "Login failed")
            ProgressBar.Visible = False
            TextBox2.Text = ""
            TextBox1.Focus()
        ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
            Label2.Text = "Offline"
            Label2.ForeColor = Color.Red
            MsgBox("Unable to connect server", vbCritical, "No internet")
            ProgressBar.Visible = False
            TextBox1.Text = ""
            TextBox2.Text = ""
        Else
            MsgBox(str, vbCritical, "Error")
            ProgressBar.Visible = False
            TextBox1.Text = ""
            TextBox2.Text = ""
        End If
    End Sub



    'Department login button click
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        query = "select * from account where deptname='" & TextBox3.Text & "' and password='" & TextBox4.Text & "' "
            str = exe(query)
        If str = "valid" Then
            ProgressBar.Visible = False
            Form3.Show()
            Me.Hide()
        ElseIf str = "invalid" Then
            MsgBox("Invalid department name or password" + vbCrLf + "Or else contact database administrator", vbCritical, "Try again")
            ProgressBar.Visible = False
            TextBox4.Text = ""
            TextBox3.Focus()
        ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
            Label2.Text = "Offline"
            Label2.ForeColor = Color.Red
            MsgBox("Unable to connect server", vbCritical, "No internet")
            ProgressBar.Visible = False
            TextBox3.Text = ""
            TextBox4.Text = ""
        Else
            MsgBox(str, vbCritical, "Error")
            ProgressBar.Visible = False
            TextBox3.Text = ""
            TextBox4.Text = ""
        End If
    End Sub



    'Execute query function
    Function exe(ByVal query As String) As String
        cmd = New MySqlCommand(query, con)
        ProgressBar.Value = 50
        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
                Label2.Text = "Online"
                Label2.ForeColor = Color.LimeGreen
            End If
            ProgressBar.Value = 75
            reader = cmd.ExecuteReader
            If reader.HasRows = 0 Then
                ProgressBar.Value = 75
                Return "invalid"
            Else
                ProgressBar.Value = 100
                Return "valid"
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As MySqlException
            ProgressBar.Value = 50
            Return ex.Message
        Finally
            con.Dispose()
        End Try
    End Function



    'Check connection
    Sub check()
        Try
            con.Open()
            Label2.Text = "Online"
            Label2.ForeColor = Color.LimeGreen
            con.Close()
        Catch ex As Exception
            Label2.Text = "Offline"
            Label2.ForeColor = Color.Red
            MsgBox("Unable to connect server", vbCritical, "No internet")
        Finally
            con.Dispose()
        End Try
    End Sub



    'Key press
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox2.Focus()
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Button1.PerformClick()
        End If
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox4.Focus()
        End If
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            Button2.PerformClick()
        End If
    End Sub


End Class