Imports MySql.Data.MySqlClient



'Advisor form
Public Class Form2
    Dim con As New MySqlConnection(My.Settings.Connection)
    Dim reader As MySqlDataReader
    Dim cmd As MySqlCommand
    Dim query As String
    Dim str As String
    Dim c, t As String
    Dim y, s, b As Integer
    Dim temp1, temp2 As String
    Dim pre, nam, des, dept As String
    Dim current As DateTime = DateTime.Now
    Dim regno, classcode, year, semester, branch, status As String
    Dim collagecode As String



    'On load
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles Me.Load
        collagecode = "7317"
        classcode = collagecode + Form1.TextBox1.Text

        y = current.Year - (2000 + classcode.Substring(4, 2))
        b = classcode.Substring(6, 3)

        If current.Month < 7 Then
            s = y * 2
        Else
            y = y + 1
            s = y * 2 - 1
        End If

        Select Case y
            Case 1
                year = "I"
            Case 2
                year = "II"
            Case 3
                year = "III"
            Case 4
                year = "IV"
        End Select

        Select Case s
            Case 1
                semester = "I"
            Case 2
                'semester = "II"
                semester = "I"
            Case 3
                semester = "III"
            Case 4
                'semester = "IV"
                semester = "III"
            Case 5
                semester = "V"
            Case 6
                'semester = "VI"
                semester = "V"
            Case 7
                semester = "VII"
            Case 8
                'semester = "VIII"
                semester = "VII"
        End Select

        Select Case b
            Case 103
                branch = "CIVIL"
            Case 104
                branch = "CSE"
            Case 105
                branch = "EEE"
            Case 106
                branch = "ECE"
            Case 114
                branch = "MECH"
            Case 205
                branch = "IT"
            Case 631
                branch = "MBA"
            Case 621
                branch = "MCA"
        End Select

        Label6.Text = classcode
        Label7.Text = year
        Label8.Text = semester
        Label9.Text = branch

        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
            cmd = New MySqlCommand("select * from student where regno like '" & classcode + "%" & "'", con)
            reader = cmd.ExecuteReader()
            reader.Read()
            status = reader.Item(3).ToString
            reader.Close()
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As MySqlException
            If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                MsgBox("Connection lost", vbCritical, "No internet")
            ElseIf ex.Message = "Invalid attempt to access a field before calling Read()" Then
                status = 0
            Else
                MsgBox(ex.Message, vbCritical, "Error")
            End If
        Finally
            con.Dispose()
        End Try

        If status = 0 Then
            CheckBox1.CheckState = CheckState.Unchecked
        Else
            CheckBox1.CheckState = CheckState.Checked
        End If
    End Sub



    'Student Login checkbox click
    Private Sub CheckBox1_Click(sender As Object, e As EventArgs) Handles CheckBox1.Click
        Dim ans As String
        If CheckBox1.CheckState = CheckState.Checked Then
            ans = MsgBox("Confirm to enable online form", vbExclamation + vbYesNo, "Confirm")
            If ans = vbYes Then
                ProgressBar.Visible = True
                ProgressBar.Value = 25
                query = "update student set status='1' where regno like '" & classcode + "%" & "'"
                str = exe(query)
                ProgressBar.Value = 75
                If (str = "valid") Or (str = "invalid") Then
                    ProgressBar.Value = 100
                    MsgBox("Online form for this class student enabled on portal successfully", vbInformation, "Successful")
                    ProgressBar.Visible = False
                    Button9.PerformClick()
                ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                    MsgBox("Connection lost", vbCritical, "No internet")
                    CheckBox1.CheckState = CheckState.Unchecked
                    ProgressBar.Visible = False
                Else
                    MsgBox(str, vbCritical, "Error")
                    CheckBox1.CheckState = CheckState.Unchecked
                    ProgressBar.Visible = False
                End If
            Else
                CheckBox1.CheckState = CheckState.Unchecked
                ProgressBar.Visible = False
            End If
        Else
            ans = MsgBox("Confirm to disable online form", vbExclamation + vbYesNo, "Confirm")
            If ans = vbYes Then
                ProgressBar.Visible = True
                ProgressBar.Value = 25
                query = "update student set status='0' where regno like '" & classcode + "%" & "'"
                str = exe(query)
                ProgressBar.Value = 75
                If (str = "valid") Or (str = "invalid") Then
                    ProgressBar.Value = 100
                    MsgBox("Online form for this class student disabled on portal successfully", vbInformation, "Successful")
                    ProgressBar.Visible = False
                    Button9.PerformClick()
                ElseIf str = "Unable to connect to any of the specified MySQL hosts." Then
                    MsgBox("Connection lost", vbCritical, "No internet")
                    CheckBox1.CheckState = CheckState.Checked
                    ProgressBar.Visible = False
                Else
                    MsgBox(str, vbCritical, "Error")
                    CheckBox1.CheckState = CheckState.Checked
                    ProgressBar.Visible = False
                End If
            Else
                CheckBox1.CheckState = CheckState.Checked
                ProgressBar.Visible = False
            End If
        End If
    End Sub



    'Student menu click 
    Private Sub StudentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StudentToolStripMenuItem.Click
        GroupBox2.Visible = False
        GroupBox3.Visible = False

        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", "where regno like '" & classcode + "%" & "'")

        Label18.Text = classcode

        GroupBox1.Visible = True
        TextBox1.Focus()
    End Sub



    'Add button click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox1.Text = "" Then
            MsgBox("Register number is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf IsNumeric(TextBox1.Text) = False Then
            MsgBox("Register number should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox1.Text.Length <> 3 Then
            MsgBox("Register number should be a 3 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox2.Text = "" Then
            MsgBox("Name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox2.Focus()
        Else
            regno = classcode + TextBox1.Text
            If CheckBox1.CheckState = CheckState.Checked Then
                status = 1
            Else
                status = 0
            End If
            query = "insert into student values('" & regno & "', '" & DatePicker.Value.ToString("dd/MM/yyyy") & "', '" & TextBox2.Text & "','" & status & "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0')"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record added successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", "where regno like '" & classcode + "%" & "'")
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



    'Update button click
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox1.Text = "" Then
            MsgBox("Register number is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf IsNumeric(TextBox1.Text) = False Then
            MsgBox("Register number should be a number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox1.Text.Length <> 3 Then
            MsgBox("Register number should be a 3 digit number", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox1.Focus()
        ElseIf TextBox2.Text = "" Then
            MsgBox("Name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox2.Focus()
        Else
            regno = classcode + TextBox1.Text
            query = "update student set dob = '" & DatePicker.Value.ToString("dd/MM/yyyy") & "',name='" & TextBox2.Text & "' where regno= '" & regno & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record updated successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", "where regno like '" & classcode + "%" & "'")
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



    'Remove button click
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        Dim ans As String
        ans = MsgBox("Confirm to delete register number" + vbCrLf + classcode + TextBox1.Text, vbExclamation + vbYesNo, "Confirm")
        If ans = vbYes Then
            regno = classcode + TextBox1.Text
            query = "delete from student where regno='" & regno & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record removed successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", "where regno like '" & classcode + "%" & "'")
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



    'Search button click
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", " where regno='" & TextBox3.Text & "' and regno like '" & classcode + "%" & "'")
    End Sub



    'Move first button click
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



    'Move previous button click
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



    'Move next button click
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



    'Move last button click
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



    'Refresh button click
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("regno,dob,name,s1,s2,s3,s4,s5,s6,s7,s8,s9,s10", "student", "where regno like '" & classcode + "%" & "'")
    End Sub



    'Datagrid cell click
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim i As Integer
        With DataGridView1
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                If .Rows(i).Cells("regno").Value.ToString = "" Then
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                Else
                    Dim reg As String
                    reg = .Rows(i).Cells("regno").Value.ToString
                    TextBox1.Text = reg.Substring(9, 3)
                    TextBox2.Text = .Rows(i).Cells("name").Value.ToString
                End If
                If .Rows(i).Cells("dob").Value.ToString = "" Then
                    Dim dob As DateTime = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", Nothing)
                    DatePicker.Value = dob
                Else
                    Dim dob As DateTime = DateTime.ParseExact(.Rows(i).Cells("dob").Value.ToString, "dd/MM/yyyy", Nothing)
                    DatePicker.Value = dob
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




    'Subject menu click
    Private Sub SubjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubjectToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox3.Visible = False

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("CIVIL")
        ComboBox1.Items.Add("CSE")
        ComboBox1.Items.Add("ECE")
        ComboBox1.Items.Add("EEE")
        ComboBox1.Items.Add("IT")
        ComboBox1.Items.Add("MBA")
        ComboBox1.Items.Add("MCA")
        ComboBox1.Items.Add("MECH")

        ComboBox1.Items.Add("ENG")
        ComboBox1.Items.Add("CHEM")
        ComboBox1.Items.Add("MATH")
        ComboBox1.Items.Add("PHY")

        ProgressBar.Visible = True
        ProgressBar.Value = 25

        loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "'")

        GroupBox2.Visible = True
        TextBox5.Focus()
    End Sub



    'Add button click (subject)
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox5.Text = "" Then
            MsgBox("Subject name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf TextBox6.Text = "" Then
            MsgBox("Subject code is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf TextBox6.Text.Length <> 6 Then
            MsgBox("Subject code must be a 6 character", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf TextBox7.Text = "" Then
            MsgBox("Staff ID is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox7.Focus()
        Else
            Try
                query = "select * from subject where subject='" & TextBox5.Text & "' and classcode='" & classcode & "' and semester='" & semester & "'"
                cmd = New MySqlCommand(query, con)
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                reader = cmd.ExecuteReader

                If reader.HasRows = 0 Then
                    con.Close()

                    If con.State = ConnectionState.Closed Then
                        con.Open()
                    End If
                    cmd = New MySqlCommand("select * from staff where id='" & TextBox7.Text & "'", con)
                    reader = cmd.ExecuteReader()
                    reader.Read()
                    pre = reader.Item(1).ToString
                    nam = reader.Item(2).ToString
                    des = reader.Item(3).ToString
                    dept = reader.Item(4).ToString
                    nam = pre + "." + nam + " " + des
                    reader.Close()
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                    query = "insert into subject values('" & classcode & "', '" & semester & "', '" & branch & "', '" & TextBox5.Text & "', '" & TextBox6.Text & "', '" & TextBox7.Text & "','" & nam & "', '" & dept & "')"
                    str = exe(query)
                    If str = "valid" Then
                        MsgBox("Record added successfully", vbInformation, "Successful")
                        ProgressBar.Visible = False
                        loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "'")
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
                Else
                    con.Close()
                    MsgBox("Subject already found", vbInformation, "Information")
                    ProgressBar.Visible = False
                    clr()
                End If
            Catch ex As MySqlException
                If ex.Message = "Unable to connect to any of the specified MySQL hosts." Then
                    MsgBox("Connection lost", vbCritical, "No internet")
                    ProgressBar.Visible = False
                ElseIf ex.Message = "Invalid attempt to access a field before calling Read()" Then
                    MsgBox("Staff ID not found", vbCritical, "Failed")
                    clr()
                    ProgressBar.Visible = False
                Else
                    MsgBox(ex.Message, vbCritical, "Error")
                    ProgressBar.Visible = False
                End If
            Finally
                con.Dispose()
            End Try
        End If
    End Sub



    'Update button click (subject)
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        If TextBox5.Text = "" Then
            MsgBox("Subject name is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox5.Focus()
        ElseIf TextBox6.Text = "" Then
            MsgBox("Subject code is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf TextBox6.Text.Length <> 6 Then
            MsgBox("Subject code must be a 6 character", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox6.Focus()
        ElseIf TextBox7.Text = "" Then
            MsgBox("Staff ID is empty", vbInformation, "Information")
            ProgressBar.Visible = False
            TextBox7.Focus()
        Else
            Try
                con.Open()
                ProgressBar.Value = 25
                query = "select * from staff where id='" & TextBox7.Text & "'"
                ProgressBar.Value = 35
                cmd = New MySqlCommand(query, con)
                reader = cmd.ExecuteReader
                ProgressBar.Value = 50
                If reader.HasRows = 0 Then
                    con.Close()
                    MsgBox("Staff ID not found", vbCritical, "Failed")
                    clr()
                    ProgressBar.Visible = False
                Else
                    con.Close()
                    query = "update subject set code= '" & TextBox6.Text & "',id='" & TextBox7.Text & "' where subject= '" & TextBox5.Text & "' and classcode='" & classcode & "' and semester='" & semester & "'"
                    str = exe(query)
                    If str = "valid" Then
                        MsgBox("Record updated successfully", vbInformation, "Successful")
                        ProgressBar.Visible = False
                        loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "'")
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



    'Remove button click (subject)
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        Dim ans As String
        ans = MsgBox("Confirm to delete subject" + vbCrLf + TextBox5.Text, vbExclamation + vbYesNo, "Confirm")
        If ans = vbYes Then
            query = "delete from subject where subject='" & TextBox5.Text & "' and classcode='" & classcode & "' and semester='" & semester & "'"
            str = exe(query)
            If str = "valid" Then
                MsgBox("Record removed successfully", vbInformation, "Successful")
                ProgressBar.Visible = False
                loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "'")
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



    'Search button click (subject)
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "' and subject='" & TextBox8.Text & "'")
    End Sub



    'Move first button click (subject)
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        clr()
        With DataGridView2
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(0).Cells(0)
                .Rows(0).Selected = True
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                TextBox9.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move previous button click (subject)
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
                TextBox9.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move next button click (subject)
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
                TextBox9.Text = c + " of " + t
            End If
        End With
    End Sub



    'Move last button click (subject)
    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        With DataGridView2
            If .RowCount > 1 Then
                .ClearSelection()
                .CurrentCell = .Rows(.RowCount - 1).Cells(0)
                .Rows(.RowCount - 1).Selected = True
                TextBox9.Text = ""
            End If
        End With
    End Sub



    'Refresh button click (subject)
    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("subject,code,staffid,staffname,staffdept", "subject", "where classcode='" & classcode & "' and semester='" & semester & "'")
    End Sub



    'Datagrid cell click (subject)
    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Dim i As Integer
        With DataGridView2
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                If IsNothing(.Rows(i).Cells(0).Value.ToString) Then
                    clr()
                Else
                    TextBox5.Text = .Rows(i).Cells("subject").Value.ToString
                    TextBox6.Text = .Rows(i).Cells("code").Value.ToString
                    TextBox7.Text = .Rows(i).Cells("staffid").Value.ToString
                End If
                c = .CurrentRow.Index + 1
                t = .RowCount - 1
                If c <= t Then
                    TextBox9.Text = c + " of " + t
                Else
                    TextBox9.Text = ""
                End If
            End If
        End With
    End Sub



    'Add button click
    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ProgressBar.Visible = True
        temp1 = TextBox5.Text
        temp2 = TextBox6.Text
        ProgressBar.Value = 25

        TextBox12.Text = ""
        ComboBox1.SelectedIndex = -1
        Panel1.Visible = True
        loader("*", "staff", "")
        ProgressBar.Visible = False
    End Sub



    'Close button click
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        DataGridView3.DataSource = Nothing
        Panel1.Visible = False
    End Sub



    'Toggle Name/Department
    Private Sub Textbox12_Click(sender As Object, e As EventArgs) Handles TextBox12.Click
        ComboBox1.SelectedIndex = -1
    End Sub
    Private Sub ComboBox1_Click(sender As Object, e As EventArgs) Handles ComboBox1.Click
        TextBox12.Text = ""
    End Sub



    'Name text changed
    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        ProgressBar.Visible = True
        ComboBox1.SelectedIndex = -1
        ProgressBar.Value = 25
        loader("*", "staff", "where name LIKE '%" & TextBox12.Text & "%'")
        ProgressBar.Visible = False
    End Sub



    'Department text changed
    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        ProgressBar.Visible = True
        ProgressBar.Value = 25
        loader("*", "staff", "where dept='" & ComboBox1.Text & "'")
        ProgressBar.Visible = False
    End Sub



    'Datagrid cell click (staff)
    Private Sub DataGridView3_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellClick
        Dim i As Integer
        With DataGridView3
            If e.RowIndex >= 0 Then
                i = .CurrentRow.Index
                TextBox5.Text = temp1
                TextBox6.Text = temp2
                TextBox7.Text = .Rows(i).Cells("id").Value.ToString
                DataGridView3.DataSource = Nothing
                Panel1.Visible = False
            End If
        End With
    End Sub



    'Change password menu click
    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = True

        TextBox10.Focus()
    End Sub



    'Change password button click
    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        If TextBox10.Text = "" Then
            MsgBox("Old password is empty", vbInformation, "Information")
            TextBox10.Focus()
        ElseIf TextBox11.Text = "" Then
            MsgBox("New password is empty", vbInformation, "Information")
            TextBox11.Focus()
        Else
            Try
                ProgressBar.Visible = True
                con.Open()
                ProgressBar.Value = 25
                query = "select * from advisor where classcode='" & classcode & "' and password='" & TextBox10.Text & "' "
                ProgressBar.Value = 50
                cmd = New MySqlCommand(query, con)
                reader = cmd.ExecuteReader
                ProgressBar.Value = 75
                If reader.HasRows = 0 Then
                    con.Close()
                    MsgBox("Old password is wrong", vbCritical, "Try again")
                    TextBox10.Text = ""
                    TextBox11.Text = ""
                    TextBox10.Focus()
                    ProgressBar.Visible = False
                Else
                    ProgressBar.Value = 85
                    con.Close()
                    query = "update advisor set password = '" & TextBox11.Text & "' where classcode= '" & classcode & "'"
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
                    TextBox10.Text = ""
                    TextBox11.Text = ""
                    TextBox10.Focus()
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
        Form1.TextBox1.Text = ""
        Form1.TextBox2.Text = ""
        Form1.Show()
        Form1.TextBox1.Focus()
    End Sub



    'Help menu click
    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        Try
            Process.Start("Advisor.pdf")
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Error")
        End Try
    End Sub



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



    'Execute select query and load to datagrid function
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
            If tab = "student" Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = tab
                DataGridView1.Sort(DataGridView1.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                DataGridView1.Columns(0).HeaderText = "Register number"
                DataGridView1.Columns(0).Width = 150
                DataGridView1.Columns(1).HeaderText = "Date of birth"
                DataGridView1.Columns(1).Width = 120
                DataGridView1.Columns(2).HeaderText = "Name"
                DataGridView1.Columns(2).Width = 210
                DataGridView1.Columns(3).HeaderText = "S1"
                DataGridView1.Columns(3).Width = 34
                DataGridView1.Columns(4).HeaderText = "S2"
                DataGridView1.Columns(4).Width = 34
                DataGridView1.Columns(5).HeaderText = "S3"
                DataGridView1.Columns(5).Width = 34
                DataGridView1.Columns(6).HeaderText = "S4"
                DataGridView1.Columns(6).Width = 34
                DataGridView1.Columns(7).HeaderText = "S5"
                DataGridView1.Columns(7).Width = 34
                DataGridView1.Columns(8).HeaderText = "S6"
                DataGridView1.Columns(8).Width = 34
                DataGridView1.Columns(9).HeaderText = "S7"
                DataGridView1.Columns(9).Width = 34
                DataGridView1.Columns(10).HeaderText = "S8"
                DataGridView1.Columns(10).Width = 34
                DataGridView1.Columns(11).HeaderText = "S9"
                DataGridView1.Columns(11).Width = 34
                DataGridView1.Columns(12).HeaderText = "S10"
                DataGridView1.Columns(12).Width = 40
                ProgressBar.Visible = False
            ElseIf tab = "subject" Then
                DataGridView2.DataSource = ds
                DataGridView2.DataMember = tab
                DataGridView2.Columns(0).HeaderText = "Subject name"
                DataGridView2.Columns(0).Width = 122
                DataGridView2.Columns(1).HeaderText = "Code"
                DataGridView2.Columns(1).Width = 70
                DataGridView2.Columns(2).HeaderText = "Staff ID"
                DataGridView2.Columns(2).Width = 83
                DataGridView2.Columns(3).HeaderText = "Name"
                DataGridView2.Columns(3).Width = 125
                DataGridView2.Columns(4).HeaderText = "Dept"
                DataGridView2.Columns(4).Width = 55
                ProgressBar.Visible = False
            ElseIf tab = "staff" Then
                DataGridView3.DataSource = ds
                DataGridView3.DataMember = tab
                DataGridView3.Sort(DataGridView3.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                DataGridView3.Columns(0).HeaderText = "Staff ID"
                DataGridView3.Columns(0).Width = 85
                DataGridView3.Columns(1).HeaderText = "Prefix"
                DataGridView3.Columns(1).Width = 55
                DataGridView3.Columns(2).HeaderText = "Name"
                DataGridView3.Columns(2).Width = 140
                DataGridView3.Columns(3).HeaderText = "Designation"
                DataGridView3.Columns(3).Width = 90
                DataGridView3.Columns(4).HeaderText = "Department"
                DataGridView3.Columns(4).Width = 90
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



    'Clear function
    Sub clr()
        TextBox1.Text = ""
        DatePicker.Value = "01/01/2000"
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
    End Sub



    'Key press
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox1.Text = "" Then
                MsgBox("Register number is empty", vbInformation, "Information")
                TextBox1.Focus()
            ElseIf IsNumeric(TextBox1.Text) = False Then
                MsgBox("Register number should be a number", vbInformation, "Information")
                TextBox1.Focus()
            ElseIf TextBox1.Text.Length <> 3 Then
                MsgBox("Register number should be a 3 digit number", vbInformation, "Information")
                TextBox1.Focus()
            Else
                DatePicker.Focus()
            End If
        End If
    End Sub
    Private Sub DatePicker_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DatePicker.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox2.Focus()
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox2.Text = "" Then
                MsgBox("Name is empty", vbInformation, "Information")
                TextBox2.Focus()
            Else
                Button1.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox5.Text = "" Then
                MsgBox("Subject name is empty", vbInformation, "Information")
                TextBox5.Focus()
            Else
                TextBox6.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox6.Text = "" Then
                MsgBox("Subject code is empty", vbInformation, "Information")
                TextBox6.Focus()
            ElseIf TextBox6.Text.Length <> 6 Then
                MsgBox("Subject code must be a 6 character", vbInformation, "Information")
                TextBox6.Focus()
            Else
                TextBox7.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox7.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox7.Text = "" Then
                MsgBox("Staff ID is empty", vbInformation, "Information")
                TextBox7.Focus()
            Else
                Button10.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox10.Text = "" Then
                MsgBox("Old password is empty", vbInformation, "Information")
                TextBox10.Focus()
            Else
                TextBox11.Focus()
            End If
        End If
    End Sub
    Private Sub TextBox11_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox11.KeyPress
        If e.KeyChar = Chr(13) Then
            If TextBox11.Text = "" Then
                MsgBox("New password is empty", vbInformation, "Information")
                TextBox11.Focus()
            Else
                Button19.PerformClick()
            End If
        End If
    End Sub



End Class