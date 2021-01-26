Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing



'Department form
Public Class Form3
    Dim con As New MySqlConnection(My.Settings.Connection)
    Dim adapter As New MySqlDataAdapter
    Dim newpage As Boolean = True
    Dim reader As MySqlDataReader
    Private bitmap As Bitmap
    Dim cmd As MySqlCommand
    Dim status As Integer = 0
    Dim y As Integer
    Dim query, str, r, c, t As String
    Dim current As DateTime = DateTime.Now
    Dim classcode, year, semester, branch, subject, code As String
    Dim staffid, staffname, staffdept, dept, deptcode, collagecode As String



    'On load
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dept = Form1.TextBox3.Text

        If dept = "ENG" Or dept = "CHEM" Or dept = "MATH" Or dept = "PHY" Then
            AdvisorToolStripMenuItem.Enabled = False
            ReportToolStripMenuItem.Enabled = False
        End If

        Select Case dept
            Case "CIVIL"
                deptcode = "103"
            Case "CSE"
                deptcode = "104"
            Case "EEE"
                deptcode = "105"
            Case "ECE"
                deptcode = "106"
            Case "MECH"
                deptcode = "114"
            Case "IT"
                deptcode = "205"
            Case "MCA"
                deptcode = "621"
            Case "MBA"
                deptcode = "631"
        End Select

        Label4.Text = dept
        Label5.Text = deptcode
        Label6.Text = "Department of " + dept

        Label12.Text = deptcode
        Label17.Text = deptcode
    End Sub



    'Advisor menu click
    Private Sub AdvisorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdvisorToolStripMenuItem.Click
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False

        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("*", "advisor", "where dept='" & dept & "'")

        GroupBox1.Visible = True
        TextBox1.Focus()
    End Sub



    'Add button click (advisor)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25

        y = current.Year - (2000 + TextBox1.Text.Substring(4, 2))
        If current.Month < 7 Then
            y = y
        Else
            y = y + 1
        End If

        If TextBox1.Text = "" Then
            MsgBox("Class code is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf IsNumeric(TextBox1.Text) = False Then
            MsgBox("Class code should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox1.Text.Length <> 6 Then
            MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox2.Text = "" Then
            MsgBox("Password is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox2.Focus()
        Else
            y = current.Year - (2000 + TextBox1.Text.Substring(4, 2))
            If current.Month < 7 Then
                y = y
            Else
                y += 1
            End If
            ProgressBar.Value = 35
            If y <> 1 And y <> 2 And y <> 3 And y <> 4 Then
                MsgBox("Invalid class code", vbExclamation, "Invalid")
                ProgressBar.Visible = False
                TextBox2.Text = ""
                TextBox1.Focus()
            Else
                query = "insert into advisor values('" & TextBox1.Text + deptcode & "', '" & TextBox2.Text & "', '" & dept & "')"
                str = exe(query)
                If str = "valid" Then
                    MsgBox("Record added successfully", vbInformation, "Successful")
                    ProgressBar.Visible = False
                    loader("*", "advisor", "where dept='" & dept & "'")
                ElseIf str = "invalid" Then
                    MsgBox("Record add failed", vbCritical, "Failed")
                    ProgressBar.Visible = False
                    clr()
                ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                    MsgBox("Connection lost", vbCritical, "No internet")
                    ProgressBar.Visible = False
                    clr()
                Else
                    MsgBox(str, vbCritical, "Error")
                    ProgressBar.Visible = False
                    clr()
                End If
            End If
        End If
    End Sub



    'Update button click (advisor)
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox1.Text = "" Then
            MsgBox("Class code is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf IsNumeric(TextBox1.Text) = False Then
            MsgBox("Class code should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox1.Text.Length <> 6 Then
            MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox2.Text = "" Then
            MsgBox("Password is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox2.Focus()
        Else
            query = "update advisor set password = '" & TextBox2.Text & "' where classcode= '" & TextBox1.Text + deptcode & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record updated successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("*", "advisor", "where dept='" & dept & "'")
            ElseIf str = "invalid" Then
                MsgBox("Record update failed", vbCritical, "Failed")
                ProgressBar.Visible = False
                clr()
            ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
                ProgressBar.Visible = False
                clr()
            Else
                MsgBox(str, vbCritical, "Error")
                ProgressBar.Visible = False
                clr()
            End If
        End If
    End Sub



    'Remove button click (advisor)
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        Dim ans As String
        ans = MsgBox("Confirm to delete class" + vbCrLf + TextBox1.Text + deptcode, vbExclamation + vbYesNo, "Confirm")
        If ans = vbYes Then
            query = "delete from advisor where classcode='" & TextBox1.Text + deptcode & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record removed successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("*", "advisor", "where dept='" & dept & "'")
            ElseIf str = "invalid" Then
                MsgBox("Record remove failed", vbCritical, "Failed")
                ProgressBar.Visible = False
                clr()
            ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
                ProgressBar.Visible = False
                clr()
            Else
                MsgBox(str, vbCritical, "Error")
                ProgressBar.Visible = False
                clr()
            End If
        Else
            ProgressBar.Visible = False
            clr()
        End If
    End Sub



    'Search button click (advisor)
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("*", "advisor", " where classcode='" & TextBox3.Text & "' and dept='" & dept & "'")
    End Sub



    'Move first button click (advisor)
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        clr()
        With DataGridView1
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(0).Cells(0)
                .Rows(0).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox4.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move previous button click (advisor)
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        clr()
        With DataGridView1
            If .RowCount > 1 Then
                Dim i As Integer = 0
                If .CurrentRow.Index <= .RowCount - 1 Then
                    If .CurrentRow.Index = 0 Then
                        i = .RowCount - 2
                    Else
                        i = .CurrentRow.Index - 1
                    End If
                End If
                .ClearSelection()
                .CurrentCell = .Rows(i).Cells(0)
                .Rows(i).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox4.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move next button click (advisor)
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        clr()
        With DataGridView1
            If .RowCount > 1 Then
                Dim i As Integer = 0
                If .CurrentRow.Index < .RowCount - 2 Then
                    i = .CurrentRow.Index + 1
                End If
                .ClearSelection()
                .CurrentCell = .Rows(i).Cells(0)
                .Rows(i).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox4.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move last button click (advisor)
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        clr()
        With DataGridView1
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(.RowCount - 1).Cells(0)
                .Rows(.RowCount - 1).Selected = True
                TextBox4.Text = ""
            End If
        End With
    End Sub



    'Refresh button click (advisor)
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("*", "advisor", "where dept='" & dept & "'")
    End Sub



    'Datagrid cell click (advisor)
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim i As Integer
        With DataGridView1
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                If .Rows(i).Cells("classcode").Value.ToString = "" Then
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                Else
                    Dim reg As String
                    reg = .Rows(i).Cells("classcode").Value.ToString
                    TextBox1.Text = reg.Substring(0, 6)
                    TextBox2.Text = .Rows(i).Cells("password").Value.ToString
                End If
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                If c <= t Then
                    TextBox4.Text = c + " of " + t
                Else
                    TextBox4.Text = ""
                End If
            End If
        End With
    End Sub



    'Staff menu click
    Private Sub StaffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StaffToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False

        ProgressBar.Visible = True
        ProgressBar.Value = 25

        loader("id,pre,name,des", "staff", "where dept='" & dept & "'")

        GroupBox2.Visible = True

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Mr")
        ComboBox1.Items.Add("Ms")
        ComboBox1.Items.Add("Mrs")
        ComboBox1.Items.Add("Dr")

        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("AP")
        ComboBox2.Items.Add("ASP")
        ComboBox2.Items.Add("PROF")

        TextBox5.Focus()
    End Sub



    'Add button click (staff)
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox5.Text = "" Then
            MsgBox("Staff ID is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf IsNumeric(TextBox5.Text) = False Then
            MsgBox("Staff ID should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf TextBox5.Text.Length <> 3 Then
            MsgBox("Staff ID should be a 3 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf ComboBox1.Text = "" Then
            MsgBox("Prefix is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox1.Focus()
        ElseIf (ComboBox1.Text = "Mr" = False) And (ComboBox1.Text = "Ms" = False) And (ComboBox1.Text = "Mrs" = False) And (ComboBox1.Text = "Dr" = False) Then
            MsgBox("Prefix is invalid", vbExclamation, "Invalid")
            ProgressBar.Visible = False
            ComboBox1.Focus()
        ElseIf TextBox6.Text = "" Then
            MsgBox("Name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf ComboBox2.Text = "" Then
            MsgBox("Designation is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox2.Focus()
        ElseIf (ComboBox2.Text = "AP" = False) And (ComboBox2.Text = "ASP" = False) And (ComboBox2.Text = "PROF" = False) Then
            MsgBox("Designation is invalid", vbExclamation, "Invalid")
            ProgressBar.Visible = False
            ComboBox2.Focus()
        Else
            query = "insert into staff values('" & dept + TextBox5.Text & "', '" & ComboBox1.Text & "', '" & TextBox6.Text & "', '" & ComboBox2.Text & "', '" & dept & "')"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record added successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("id,pre,name,des", "staff", "where dept='" & dept & "'")
            ElseIf str = "invalid" Then
                MsgBox("Record add failed", vbCritical, "Failed")
                ProgressBar.Visible = False
                clr()
            ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
                ProgressBar.Visible = False
                clr()
            Else
                MsgBox(str, vbCritical, "Error")
                ProgressBar.Visible = False
                clr()
            End If
        End If
    End Sub



    'Update button click (staff)
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox5.Text = "" Then
            MsgBox("Staff ID is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf IsNumeric(TextBox5.Text) = False Then
            MsgBox("Staff ID should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf TextBox5.Text.Length <> 3 Then
            MsgBox("Staff ID should be a 3 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf ComboBox1.Text = "" Then
            MsgBox("Prefix is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox1.Focus()
        ElseIf (ComboBox1.Text = "Mr" = False) And (ComboBox1.Text = "Ms" = False) And (ComboBox1.Text = "Mrs" = False) And (ComboBox1.Text = "Dr" = False) Then
            MsgBox("Prefix is invalid", vbExclamation, "Invalid")
            ProgressBar.Visible = False
            ComboBox1.Focus()
        ElseIf TextBox6.Text = "" Then
            MsgBox("Name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf ComboBox2.Text = "" Then
            MsgBox("Designation is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox2.Focus()
        ElseIf (ComboBox2.Text = "AP" = False) And (ComboBox2.Text = "ASP" = False) And (ComboBox2.Text = "PROF" = False) Then
            MsgBox("Designation is invalid", vbExclamation, "Invalid")
            ProgressBar.Visible = False
            ComboBox2.Focus()
        Else
            query = "update staff set pre = '" & ComboBox1.Text & "',name='" & TextBox6.Text & "',des='" & ComboBox2.Text & "' where id= '" & dept + TextBox5.Text & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record updated successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("id,pre,name,des", "staff", "where dept='" & dept & "'")
            ElseIf str = "invalid" Then
                MsgBox("Record update failed", vbCritical, "Failed")
                ProgressBar.Visible = False
                clr()
            ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
                ProgressBar.Visible = False
                clr()
            Else
                MsgBox(str, vbCritical, "Error")
                ProgressBar.Visible = False
                clr()
            End If
        End If
    End Sub



    'Remove button click (staff)
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        Dim ans As String
        ans = MsgBox("Confirm to delete staff ID" + vbCrLf + dept + TextBox5.Text, vbExclamation + vbYesNo, "Confirm")
        If ans = vbYes Then
            query = "delete from staff where id='" & dept + TextBox5.Text & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record removed successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("id,pre,name,des", "staff", "where dept='" & dept & "'")
            ElseIf str = "invalid" Then
                MsgBox("Record remove failed", vbCritical, "Failed")
                ProgressBar.Visible = False
                clr()
            ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
                ProgressBar.Visible = False
                clr()
            Else
                MsgBox(str, vbCritical, "Error")
                ProgressBar.Visible = False
                clr()
            End If
        Else
            ProgressBar.Visible = False
            clr()
        End If
    End Sub



    'Search button click (staff)
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("id,pre,name,des", "staff", " where id='" & TextBox7.Text & "'")
    End Sub



    'Move first button click (staff)
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        clr()
        With DataGridView2
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(0).Cells(0)
                .Rows(0).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox8.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move previous button click (staff)
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        With DataGridView2
            If .RowCount > 1 Then
                Dim i As Integer = 0
                If .CurrentRow.Index <= .RowCount - 1 Then
                    If .CurrentRow.Index = 0 Then
                        i = .RowCount - 2
                    Else
                        i = .CurrentRow.Index - 1
                    End If
                End If
                .ClearSelection()
                .CurrentCell = .Rows(i).Cells(0)
                .Rows(i).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox8.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move next button click (staff)
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        clr()
        With DataGridView2
            If .RowCount > 1 Then
                Dim i As Integer = 0
                If .CurrentRow.Index < .RowCount - 2 Then
                    i = .CurrentRow.Index + 1
                End If
                .ClearSelection()
                .CurrentCell = .Rows(i).Cells(0)
                .Rows(i).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox8.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move last button click (staff)
    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        With DataGridView2
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(.RowCount - 1).Cells(0)
                .Rows(.RowCount - 1).Selected = True
                TextBox8.Text = ""
            End If
        End With
    End Sub



    'Refresh button click (staff)
    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("id,pre,name,des", "staff", "where dept='" & dept & "'")
    End Sub



    'Datagrid cell click (staff)
    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Dim i As Integer
        With DataGridView2
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                If .Rows(i).Cells("id").Value.ToString = "" Then
                    TextBox5.Text = ""
                    TextBox6.Text = ""
                    ComboBox1.Text = ""
                    ComboBox2.Text = ""
                Else
                    Dim ss As String
                    ss = .Rows(i).Cells("id").Value.ToString
                    TextBox5.Text = ss.Substring(dept.Length, 3)
                    TextBox6.Text = .Rows(i).Cells("name").Value.ToString
                    ComboBox1.Text = .Rows(i).Cells("pre").Value.ToString
                    ComboBox2.Text = .Rows(i).Cells("des").Value.ToString
                End If
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                If c <= t Then
                    TextBox8.Text = c + " of " + t
                Else
                    TextBox8.Text = ""
                End If
            End If
        End With
    End Sub



    'Change password menu click
    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = True

        TextBox9.Focus()
    End Sub



    'Change password button click 
    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        If TextBox9.Text = "" Then
            MsgBox("Old password is empty", vbInformation, "Information")
            TextBox9.Focus()
        ElseIf TextBox10.Text = "" Then
            MsgBox("New password is empty", vbInformation, "Information")
            TextBox10.Focus()
        Else
            Try
                ProgressBar.Visible = True
                con.Open()
                ProgressBar.Value = 25
                query = "select * from account where deptname='" & dept & "' and password='" & TextBox9.Text & "' "
                ProgressBar.Value = 50
                cmd = New MySqlCommand(query, con)
                reader = cmd.ExecuteReader
                ProgressBar.Value = 75
                If reader.HasRows = 0 Then
                    con.Close()
                    MsgBox("Old password is wrong", vbCritical, "Try again")
                    TextBox9.Text = ""
                    TextBox10.Text = ""
                    TextBox9.Focus()
                    ProgressBar.Visible = False
                Else
                    ProgressBar.Value = 85
                    con.Close()
                    query = "update account set password = '" & TextBox10.Text & "' where deptname= '" & dept & "'"
                    str = exe(query)
                    If str = "valid" Then
                        MsgBox("Password changed successfully", vbInformation, "Successful")
                        ProgressBar.Visible = False
                    ElseIf str = "invalid" Then
                        MsgBox("Password change failed", vbCritical, "Failed")
                        ProgressBar.Visible = False
                    ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                        MsgBox("Connection lost", vbCritical, "No internet")
                        ProgressBar.Visible = False
                    Else
                        MsgBox(str, vbCritical, "Error")
                        ProgressBar.Visible = False
                    End If
                    TextBox9.Text = ""
                    TextBox10.Text = ""
                    TextBox9.Focus()
                End If
                con.Close()
            Catch ex As MySqlException
                If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                    MsgBox("Connection lost", vbCritical, "No internet")
                Else
                    MsgBox(ex.Message, vbCritical, "Error")
                End If
                ProgressBar.Visible = False
            Finally
                con.Dispose()
            End Try
        End If
    End Sub



    'Log out menu click
    Private Sub LogOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogOutToolStripMenuItem.Click
        Me.Close()
        Form1.TextBox3.Text = ""
        Form1.TextBox4.Text = ""
        Form1.Show()
        Form1.TextBox3.Focus()
    End Sub



    'Report menu click
    Private Sub FeedbackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = True
        GroupBox4.Visible = False

        TextBox11.Text = "7317"
        TextBox12.Text = "7317"

        ComboBox3.Items.Clear()
        ComboBox3.Items.Add("I")
        ComboBox3.Items.Add("II")
        ComboBox3.Items.Add("III")
        ComboBox3.Items.Add("IV")
        ComboBox3.Items.Add("V")
        ComboBox3.Items.Add("VI")
        ComboBox3.Items.Add("VII")
        ComboBox3.Items.Add("VIII")

        ComboBox4.Items.Clear()

        ComboBox5.Items.Clear()
        ComboBox5.Items.Add("I")
        ComboBox5.Items.Add("II")
        ComboBox5.Items.Add("III")
        ComboBox5.Items.Add("IV")
        ComboBox5.Items.Add("V")
        ComboBox5.Items.Add("VI")
        ComboBox5.Items.Add("VII")
        ComboBox5.Items.Add("VIII")

        DataGridView3.DataSource = Nothing

        TextBox11.Focus()
    End Sub


    'Tab control click
    Private Sub TabControl1_Click(sender As Object, e As EventArgs) Handles TabControl1.Click
        If TabPage1.Visible = True Then
            TextBox11.Focus()
        Else
            TextBox12.Focus()
        End If
    End Sub


    'Submit button click (classcode, semester, subject)
    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox11.Text = "" Then
            MsgBox("Please enter class code", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox11.Focus()
        ElseIf IsNumeric(TextBox11.Text) = False Then
            MsgBox("Class code should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox11.Focus()
        ElseIf TextBox11.Text.Length <> 6 Then
            MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox11.Focus()
        ElseIf ComboBox3.Text = "" Then
            MsgBox("Please select semester", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox3.Focus()
        ElseIf ComboBox4.Text = "" Then
            MsgBox("Please select subject", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox4.Focus()
        Else
            classcode = TextBox11.Text + deptcode
            semester = ComboBox3.Text
            subject = ComboBox4.Text
            loader("*", "feedback", "where branch='" & dept & "' and regno like '" & classcode & "%' and semester='" & semester & "' and subject='" & subject & "'")

            status = 1
            value()

            TabControl1.Visible = False
            If DataGridView3.RowCount > 1 Then
                t = DataGridView3.RowCount
                TextBox14.Text = " " + t + " records found"
            Else
                TextBox14.Text = " No record found"
            End If
        End If
    End Sub



    'Submit button click(classcode, semester)
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox12.Text = "" Then
            MsgBox("Please enter class code", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox12.Focus()
        ElseIf IsNumeric(TextBox12.Text) = False Then
            MsgBox("Class code should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox12.Focus()
        ElseIf TextBox12.Text.Length <> 6 Then
            MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox12.Focus()
        ElseIf ComboBox5.Text = "" Then
            MsgBox("Please select semester", vbInformation, "Information")
            ProgressBar.Visible = False
            ComboBox5.Focus()
        Else
            classcode = TextBox12.Text + deptcode
            semester = ComboBox5.Text
            loader("*", "college", "where branch='" & dept & "' and regno like '" & classcode & "%' and semester='" & semester & "'")

            status = 2

            TabControl1.Visible = False
            If DataGridView3.RowCount > 1 Then
                t = DataGridView3.RowCount
                TextBox14.Text = " " + t + " records found"
            Else
                TextBox14.Text = " No record found"
            End If
        End If

    End Sub



    'Print button click
    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        Printer.DefaultPageSettings.PaperSize = (From s As PaperSize In Printer.PrinterSettings.PaperSizes.Cast(Of PaperSize) Where s.RawKind = PaperKind.A4).FirstOrDefault
        Printer.DefaultPageSettings.Margins = New Margins(40, 40, 100, 60)
        Previewer.WindowState = FormWindowState.Maximized
        Previewer.ShowDialog()
    End Sub



    'Back button click
    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        DataGridView3.DataSource = Nothing
        DataGridView3.Refresh()

        TextBox11.Text = "7317"
        TextBox12.Text = "7317"

        ComboBox3.SelectedIndex = -1
        ComboBox4.SelectedIndex = -1
        ComboBox5.SelectedIndex = -1

        TabControl1.Visible = True

        If TabPage1.Visible = True Then
            TextBox11.Focus()
        Else
            TextBox12.Focus()
        End If
    End Sub



    'Begin print
    Private Sub Printer_BeginPrint(sender As Object, e As PrintEventArgs) Handles Printer.BeginPrint
        r = 0
        newpage = True
        Previewer.PrintPreviewControl.Zoom = 1.0
        Previewer.PrintPreviewControl.StartPage = 0
    End Sub



    'Print page 
    Private Sub Printer_PrintPage(sender As Object, e As PrintPageEventArgs) Handles Printer.PrintPage
        Try
            With DataGridView3
                Dim fmt As StringFormat = New StringFormat(StringFormatFlags.LineLimit)
                fmt.LineAlignment = StringAlignment.Center
                fmt.Trimming = StringTrimming.Word
                Dim font As Font = New Font("Arial", 14, GraphicsUnit.Pixel)
                Dim font16 As Font = New Font("Arial", 16, GraphicsUnit.Pixel)
                Dim y As Int32 = e.MarginBounds.Top
                Dim rc As Rectangle
                Dim x As Int32
                Dim h As Int32 = 0
                Dim row As DataGridViewRow

                Dim batch As String
                Dim byear As Integer

                Dim p As Double = 0
                Dim v1 As Double = 0
                Dim v2 As Double = 0
                Dim v3 As Double = 0
                Dim v4 As Double = 0
                Dim v5 As Double = 0
                Dim v6 As Double = 0
                Dim v7 As Double = 0
                Dim v8 As Double = 0
                Dim v9 As Double = 0
                Dim v10 As Double = 0
                Dim nv1 As Double = 0
                Dim nv2 As Double = 0
                Dim nv3 As Double = 0

                Dim nc1 As Integer = 0
                Dim nc2 As Integer = 0
                Dim nc3 As Integer = 0
                Dim count As Integer = 0

                Dim total As Long = 0

                e.Graphics.DrawString("MPNMJEC Feedback System Reports", font16, Brushes.Black, 40, 20, New StringFormat())
                e.Graphics.DrawString("Department of " + dept, font16, Brushes.Black, 640, 20, New StringFormat())

                byear = Convert.ToInt32(classcode.Substring(4, 2))
                batch = "20" & byear & "-" & byear + 4 & ""
                If semester = "I" Or semester = "II" Then
                    year = "I"
                ElseIf semester = "III" Or semester = "IV" Then
                    year = "II"
                ElseIf semester = "V" Or semester = "VI" Then
                    year = "III"
                ElseIf semester = "VII" Or semester = "VIII" Then
                    year = "IV"
                End If
                branch = dept


                If status = 1 Then
                    e.Graphics.DrawString("Batch: " + batch, font16, Brushes.Black, 40, 45, New StringFormat())
                    e.Graphics.DrawString("Year: " + year, font16, Brushes.Black, 340, 45, New StringFormat())
                    e.Graphics.DrawString("Semester: " + semester, font16, Brushes.Black, 440, 45, New StringFormat())
                    e.Graphics.DrawString("Branch: " + branch, font16, Brushes.Black, 685, 45, New StringFormat())

                    e.Graphics.DrawString("Faculty Name: " + staffname, font16, Brushes.Black, 40, 70, New StringFormat())
                    e.Graphics.DrawString("Subject: " + subject, font16, Brushes.Black, 535, 70, New StringFormat())
                    e.Graphics.DrawString("Code: " + code, font16, Brushes.Black, 675, 70, New StringFormat())
                ElseIf status = 2 Then
                    e.Graphics.DrawString("Batch: " + batch, font16, Brushes.Black, 40, 45, New StringFormat())
                    e.Graphics.DrawString("Year: " + year, font16, Brushes.Black, 340, 45, New StringFormat())
                    e.Graphics.DrawString("Semester: " + semester, font16, Brushes.Black, 440, 45, New StringFormat())
                    e.Graphics.DrawString("Branch: " + branch, font16, Brushes.Black, 685, 45, New StringFormat())

                    fmt.Alignment = StringAlignment.Center

                    rc = New Rectangle(40, 75, 110, 25)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)

                    rc = New Rectangle(150, 75, 410, 25)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" ABOUT THE COLLEGE", font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(560, 75, 220, 25)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" ABOUT THE DEPARTMENT", font, Brushes.Black, rc, fmt)
                End If

                If newpage Then
                    row = .Rows(r)
                    x = e.MarginBounds.Left
                    For Each cell As DataGridViewCell In row.Cells
                        If cell.Visible Then
                            rc = New Rectangle(x, y, cell.Size.Width, 75)
                            e.Graphics.FillRectangle(Brushes.LightGray, rc)
                            e.Graphics.DrawRectangle(Pens.Black, rc)
                            fmt.Alignment = StringAlignment.Center
                            e.Graphics.DrawString(.Columns(cell.ColumnIndex).HeaderText, font, Brushes.Black, rc, fmt)
                            x += rc.Width
                            h = Math.Max(h, rc.Height)
                        End If
                    Next
                    y += h
                End If


                newpage = False

                Dim ndx As Int32
                For ndx = r To .RowCount - 1
                    If .Rows(ndx).IsNewRow Then
                        Exit For
                    End If

                    row = .Rows(ndx)
                    x = e.MarginBounds.Left
                    h = 0

                    If status = 1 Then
                        count += 1
                        v1 += row.Cells(9).Value
                        v2 += row.Cells(10).Value
                        v3 += row.Cells(11).Value
                        v4 += row.Cells(12).Value
                        v5 += row.Cells(13).Value
                        v6 += row.Cells(14).Value
                        v7 += row.Cells(15).Value
                        v8 += row.Cells(16).Value
                        v9 += row.Cells(17).Value
                        v10 += row.Cells(18).Value
                        total += row.Cells(19).Value
                    ElseIf status = 2 Then
                        count += 1
                        v1 += row.Cells(4).Value
                        v2 += row.Cells(6).Value
                        v3 += row.Cells(9).Value
                        v4 += row.Cells(11).Value
                        v5 += row.Cells(12).Value
                        If row.Cells(7).Value.ToString <> "" Then
                            nc1 += 1
                            nv1 += row.Cells(7).Value
                        End If
                        If row.Cells(8).Value.ToString <> "" Then
                            nc2 += 1
                            nv2 += row.Cells(8).Value
                        End If
                        If row.Cells(10).Value.ToString <> "" Then
                            nc3 += 1
                            nv3 += row.Cells(10).Value
                        End If
                    End If

                    For Each cell As DataGridViewCell In row.Cells
                        If cell.Visible Then
                            rc = New Rectangle(x, y, cell.Size.Width, cell.Size.Height)
                            e.Graphics.DrawRectangle(Pens.Black, rc)
                            fmt.Alignment = StringAlignment.Center

                            If cell.FormattedValue.ToString() <> "" Then
                                e.Graphics.DrawString(cell.FormattedValue.ToString(), font, Brushes.Black, rc, fmt)
                            Else
                                e.Graphics.DrawString("-", font, Brushes.Black, rc, fmt)
                            End If

                            x += rc.Width
                            h = Math.Max(h, rc.Height)
                        End If
                    Next
                    y += h
                    r = ndx + 1

                    If y + h > e.MarginBounds.Bottom Then
                        e.HasMorePages = True
                        newpage = True
                        Return
                    End If
                Next

                If status = 1 Then
                    v1 /= count
                    v2 /= count
                    v3 /= count
                    v4 /= count
                    v5 /= count
                    v6 /= count
                    v7 /= count
                    v8 /= count
                    v9 /= count
                    v10 /= count
                    p = total / count

                    rc = New Rectangle(40, y, 110, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" Average", font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(150, y, 50, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v1, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(200, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v2, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(265, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v3, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(330, y, 60, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v4, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(390, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v5, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(455, y, 55, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v6, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(510, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v7, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(575, y, 40, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v8, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(615, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v9, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(680, y, 50, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v10, 2), font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(730, y, 50, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(p, 2), font, Brushes.Black, rc, fmt)

                    e.Graphics.DrawString("Percentage: " & Math.Round(p, 0) & "%", font, Brushes.Black, 670, y + 30, New StringFormat())

                ElseIf status = 2 Then
                    v1 /= count
                    v2 /= count
                    v3 /= count
                    v4 /= count
                    v5 /= count

                    nv1 /= nc1
                    nv2 /= nc2
                    nv3 /= nc3

                    Dim pen As New Pen(Color.LightGray, 2)
                    e.Graphics.DrawLine(pen, 41, 100, 149, 100)

                    rc = New Rectangle(40, y, 110, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" Average", .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(150, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v1, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(215, y, 75, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)

                    rc = New Rectangle(290, y, 50, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v2, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(340, y, 48, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(nv1, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(388, y, 48, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(nv2, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(436, y, 60, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v3, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(496, y, 64, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(nv3, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(560, y, 80, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v4, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(640, y, 65, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    e.Graphics.DrawString(" " & Math.Round(v5, 2), .Font, Brushes.Black, rc, fmt)

                    rc = New Rectangle(705, y, 75, 22)
                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)

                    e.Graphics.DrawString("1-Poor   2-Satisfactory   3-Good   4-Very Good   5-Excellent", font, Brushes.Black, 40, y + 30, New StringFormat())
                    e.Graphics.DrawString("SR-Some what Redressed   RW-Redressed Well", font, Brushes.Black, 465, y + 30, New StringFormat())
                End If
            End With
        Catch
            MsgBox("Error during preview/print the page", vbCritical, "Error")
            Previewer.Close()
        End Try
    End Sub



    'Help menu click
    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        Try
            Process.Start("Department.pdf")
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Error")
        End Try
    End Sub



    'Functions

    'Execute query functions
    Function exe(ByVal query As String) As String
        cmd = New MySqlCommand(query, con)
        ProgressBar.Value = 50
        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
            ProgressBar.Value = 75
            If cmd.ExecuteNonQuery() = 1 Then
                ProgressBar.Value = 100
                Return "valid"
            Else
                ProgressBar.Value = 75
                Return "invalid"
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

    'Execute select query function
    Sub loader(ByVal slt As String, tab As String, ext As String)
        ProgressBar.Value = 50
        query = "select " + slt + " from " + tab + " " + ext + ""
        Dim da As New MySqlDataAdapter(query, con)
        Dim ds As New DataSet()
        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
            ProgressBar.Value = 75
            da.Fill(ds, tab)
            ProgressBar.Value = 100
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            If tab = "advisor" Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = tab
                DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Descending)
                DataGridView1.Columns(0).HeaderText = "Class code"
                DataGridView1.Columns(0).Width = 150
                DataGridView1.Columns(1).HeaderText = "Password"
                DataGridView1.Columns(1).Width = 100
                DataGridView1.Columns(2).Visible = False
                ProgressBar.Visible = False
            ElseIf tab = "staff" Then
                DataGridView2.DataSource = ds
                DataGridView2.DataMember = tab
                DataGridView2.Sort(DataGridView2.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                DataGridView2.Columns(0).HeaderText = "Staff ID"
                DataGridView2.Columns(0).Width = 125
                DataGridView2.Columns(1).HeaderText = "Prefix"
                DataGridView2.Columns(1).Width = 75
                DataGridView2.Columns(2).HeaderText = "Name"
                DataGridView2.Columns(2).Width = 150
                DataGridView2.Columns(3).HeaderText = "Designation"
                DataGridView2.Columns(3).Width = 100
                ProgressBar.Visible = False
            ElseIf tab = "feedback" Then
                DataGridView3.DataSource = ds
                DataGridView3.DataMember = tab
                DataGridView3.Sort(DataGridView3.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
                DataGridView3.Columns(0).Visible = False    'Entry date
                DataGridView3.Columns(1).HeaderText = "Register number"
                DataGridView3.Columns(1).Width = 110
                DataGridView3.Columns(2).Visible = False    'Semester
                DataGridView3.Columns(3).Visible = False    'Branch
                DataGridView3.Columns(4).Visible = False    'Staff id
                DataGridView3.Columns(5).Visible = False    'Staff name
                DataGridView3.Columns(6).Visible = False    'dept
                DataGridView3.Columns(7).Visible = False    'subject
                DataGridView3.Columns(8).Visible = False    'code
                DataGridView3.Columns(9).HeaderText = "Punct uality"
                DataGridView3.Columns(9).Width = 50
                DataGridView3.Columns(10).HeaderText = "Lesson planning"
                DataGridView3.Columns(10).Width = 65
                DataGridView3.Columns(11).HeaderText = "Clarity of Explan ation"
                DataGridView3.Columns(11).Width = 65
                DataGridView3.Columns(12).HeaderText = "Subject Knowle dge"
                DataGridView3.Columns(12).Width = 60
                DataGridView3.Columns(13).HeaderText = "Syllabus Cover age"
                DataGridView3.Columns(13).Width = 65
                DataGridView3.Columns(14).HeaderText = "Clarify ing doubts"
                DataGridView3.Columns(14).Width = 55
                DataGridView3.Columns(15).HeaderText = "Motivat ing Students"
                DataGridView3.Columns(15).Width = 65
                DataGridView3.Columns(16).HeaderText = "Inte grity"
                DataGridView3.Columns(16).Width = 40
                DataGridView3.Columns(17).HeaderText = "Use of teaching aids"
                DataGridView3.Columns(17).Width = 65
                DataGridView3.Columns(18).HeaderText = "Amic ability"
                DataGridView3.Columns(18).Width = 50
                DataGridView3.Columns(19).HeaderText = "Total"
                DataGridView3.Columns(19).Width = 50
                ProgressBar.Visible = False
            ElseIf tab = "college" Then
                DataGridView3.DataSource = ds
                DataGridView3.DataMember = tab
                DataGridView3.Sort(DataGridView3.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
                DataGridView3.Columns(0).Visible = False    'Entry date
                DataGridView3.Columns(1).HeaderText = "Register number"
                DataGridView3.Columns(1).Width = 110
                DataGridView3.Columns(2).Visible = False    'Semester
                DataGridView3.Columns(3).Visible = False    'Department
                DataGridView3.Columns(4).HeaderText = "Function ing of the college"
                DataGridView3.Columns(4).Width = 65
                DataGridView3.Columns(5).HeaderText = "Convey your grievances"
                DataGridView3.Columns(5).Width = 75
                DataGridView3.Columns(6).HeaderText = "Library Facili ties"
                DataGridView3.Columns(6).Width = 50
                DataGridView3.Columns(7).HeaderText = "Hostel Facili ties"
                DataGridView3.Columns(7).Width = 48
                DataGridView3.Columns(8).HeaderText = "Hostel Food"
                DataGridView3.Columns(8).Width = 48
                DataGridView3.Columns(9).HeaderText = "Canteen Facili ties"
                DataGridView3.Columns(9).Width = 60
                DataGridView3.Columns(10).HeaderText = "Trans port Facilities"
                DataGridView3.Columns(10).Width = 64
                DataGridView3.Columns(11).HeaderText = "Function ing of the Department"
                DataGridView3.Columns(11).Width = 80
                DataGridView3.Columns(12).HeaderText = "Labora tory Facilities"
                DataGridView3.Columns(12).Width = 65
                DataGridView3.Columns(13).HeaderText = "Convey your grievances"
                DataGridView3.Columns(13).Width = 75
                ProgressBar.Visible = False
            Else
                MsgBox("Error during load the table", vbCritical, "Error")
            End If
            clr()
        Catch ex As MySqlException
            If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
            Else
                MsgBox(ex.Message, vbCritical, "Error")
            End If
            ProgressBar.Visible = False
        Finally
            con.Dispose()
        End Try
    End Sub

    'Function to find values
    Sub value()
        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If

            If status = 1 Then
                cmd = New MySqlCommand("select * from feedback where branch='" & dept & "' and regno like '" & classcode & "%' and semester='" & semester & "' and subject='" & subject & "'", con)
                reader = cmd.ExecuteReader()
                reader.Read()

                staffid = reader.Item(4).ToString
                staffname = reader.Item(5).ToString
                staffdept = reader.Item(6).ToString
                subject = reader.Item(7).ToString
                code = reader.Item(8).ToString

                reader.Close()
            End If

            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As Exception
            If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
            Else
                MsgBox(ex.Message, vbCritical, "Error")
            End If
        Finally
            con.Dispose()
        End Try
    End Sub



    'Function to find subjects
    Sub find()
        ComboBox4.Items.Clear()
        Try
            con.Open()
            Dim ds As New DataSet
            classcode = TextBox11.Text + deptcode
            semester = ComboBox3.Text

            adapter.SelectCommand = New MySqlCommand("select * from subject where classcode='" & classcode & "' and semester='" & semester & "'", con)
            adapter.Fill(ds)
            con.Close()

            For i = 0 To ds.Tables(0).Rows.Count - 1
                ComboBox4.Items.Add(ds.Tables(0).Rows(i).Item(3).ToString)
            Next
        Catch ex As Exception
            If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
            Else
                MsgBox(ex.Message, vbCritical, "Error")
            End If
        Finally
            con.Dispose()
        End Try
    End Sub

    'Clear function
    Sub clr()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""

        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub



    'Key press
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox1.Text = "" Then
                MsgBox("Class code is empty", vbInformation, "Information")
                TextBox1.Focus()
            ElseIf IsNumeric(TextBox1.Text) = False Then
                MsgBox("Class code should be a number", vbInformation, "Information")
                TextBox1.Focus()
            ElseIf TextBox1.Text.Length <> 6 Then
                MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
                TextBox1.Focus()
            Else
                TextBox2.Focus()
            End If
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox2.Text = "" Then
                MsgBox("Password is empty", vbInformation, "Information")
                TextBox2.Focus()
            Else
                Button1.Focus()
            End If
        End If
    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox5.Text = "" Then
                MsgBox("Staff ID is empty", vbInformation, "Information")
                TextBox5.Focus()
            ElseIf IsNumeric(TextBox5.Text) = False Then
                MsgBox("Staff ID should be a number", vbInformation, "Information")
                TextBox5.Focus()
            ElseIf TextBox5.Text.Length <> 3 Then
                MsgBox("Staff ID should be a 3 digit number", vbInformation, "Information")
                TextBox5.Focus()
            Else
                ComboBox1.Focus()
            End If
        End If
    End Sub

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            If ComboBox1.Text = "" Then
                MsgBox("Prefix is empty", vbInformation, "Information")
                ComboBox1.Focus()
            ElseIf (ComboBox1.Text = "Mr" = False) And (ComboBox1.Text = "Ms" = False) And (ComboBox1.Text = "Mrs" = False) And (ComboBox1.Text = "Dr" = False) Then
                MsgBox("Prefix is invalid", vbExclamation, "Invalid")
                ComboBox1.Focus()
            Else
                TextBox6.Focus()
            End If
        End If
    End Sub

    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox6.Text = "" Then
                MsgBox("Name is empty", vbInformation, "Information")
                TextBox6.Focus()
            Else
                ComboBox2.Focus()
            End If
        End If
    End Sub

    Private Sub ComboBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            If ComboBox2.Text = "" Then
                MsgBox("Designation is empty", vbInformation, "Information")
                ProgressBar.Visible = False
            ElseIf (ComboBox2.Text = "AP" = False) And (ComboBox2.Text = "ASP" = False) And (ComboBox2.Text = "PROF" = False) Then
                MsgBox("Designation is invalid", vbExclamation, "Invalid")
                ProgressBar.Visible = False
            Else
                Button10.Focus()
            End If
        End If
    End Sub

    Private Sub TextBox9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox9.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox9.Text = "" Then
                MsgBox("Old password is empty", vbInformation, "Information")
                TextBox9.Focus()
            Else
                TextBox10.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox10.Text = "" Then
                MsgBox("New password is empty", vbInformation, "Information")
                TextBox10.Focus()
            Else
                Button19.PerformClick()
            End If
        End If
    End Sub

    Private Sub TextBox11_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox11.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox11.Text = "" Then
                MsgBox("Class code is empty", vbInformation, "Information")
                TextBox11.Focus()
            ElseIf IsNumeric(TextBox11.Text) = False Then
                MsgBox("Class code should be a number", vbInformation, "Information")
                TextBox11.Focus()
            ElseIf TextBox11.Text.Length <> 6 Then
                MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
                TextBox11.Focus()
            Else
                ComboBox3.Focus()
            End If
        End If
    End Sub

    Private Sub ComboBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If ComboBox3.Text = "" Then
                MsgBox("Please select semester", vbInformation, "Information")
                ComboBox3.Focus()
            Else
                find()
                ComboBox4.Focus()
            End If
        End If
    End Sub

    Private Sub ComboBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            If ComboBox4.Text = "" Then
                MsgBox("Please select subject", vbInformation, "Information")
                ComboBox4.Focus()
            Else
                Button20.PerformClick()
            End If
        End If
    End Sub

    Private Sub ComboBox3_TextChanged(sender As Object, e As EventArgs) Handles ComboBox3.TextChanged
        find()
    End Sub

    Private Sub TextBox12_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            If TextBox12.Text = "" Then
                MsgBox("Class code is empty", vbInformation, "Information")
                TextBox12.Focus()
            ElseIf IsNumeric(TextBox12.Text) = False Then
                MsgBox("Class code should be a number", vbInformation, "Information")
                TextBox12.Focus()
            ElseIf TextBox12.Text.Length <> 6 Then
                MsgBox("Class code should be a 6 digit number", vbInformation, "Information")
                TextBox12.Focus()
            Else
                ComboBox5.Focus()
            End If
        End If
    End Sub

    Private Sub ComboBox5_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            If ComboBox5.Text = "" Then
                MsgBox("Please select semester", vbInformation, "Information")
                ComboBox5.Focus()
            Else
                Button21.PerformClick()
            End If
        End If
    End Sub



End Class