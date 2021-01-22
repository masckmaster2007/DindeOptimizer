Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Form2

    Public Function method(ByVal key As String) As TripleDES
        Dim m As MD5 = New MD5CryptoServiceProvider
        Dim d As TripleDES = New TripleDESCryptoServiceProvider
        d.Key = m.ComputeHash(Encoding.Unicode.GetBytes(key))
        d.IV = New Byte(((d.BlockSize / 8)) - 1) {}
        Return d
    End Function

    Public Function encrypt(ByVal value As String, ByVal key As String) As Byte()
        Dim d As TripleDES = method(key)
        Dim c As ICryptoTransform = d.CreateEncryptor
        Dim Input() As Byte = Encoding.Unicode.GetBytes(value)
        Return c.TransformFinalBlock(Input, 0, Input.Length)
    End Function

    Public Function devrypt(ByVal value As String, ByVal key As String) As String
        Dim b() As Byte = Convert.FromBase64String(value)
        Dim d As TripleDES = method(key)
        Dim c As ICryptoTransform = d.CreateEncryptor
        Dim output() As Byte = c.TransformFinalBlock(b, a, b.Length)
        Return Encoding.Unicode.GetString(output)
    End Function

    Public Function makekey() As Object
        Dim e, f, g, h, j As Object
        e = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()-+=_"
        f = Len(e)
        g = 4096 ' Key length is 4096 characters
        Randomize()
        h = ""
        For intstep = 1 To g
            j = Int(((f * Rnd()) + 1))
            h = h & Mid(e, j, 1)
        Next
        makekey = h
    End Function

    Dim a As String = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()-+=_"
    Dim b As String = makekey()

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\RegTest", "Tester", Nothing) Is Nothing Or
            File.Exists("C:\key.txt") = False Or My.Settings.key = Nothing Then
            Dim c As Byte() = encrypt(b, a)
            Dim d As String = Convert.ToBase64String(c)
            File.WriteAllText("C:\key.txt", d)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\RegTest", "Tester", d)
            My.Settings.key = b
            My.Settings.Save()
            My.Settings.Reload()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try ' Let's do some error checking and check if the registry key or file has been tampered with
            Dim c As String = File.ReadAllText("C:\key.txt")
            Dim d As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\RegTest", "Tester", My.Settings.key)
            If Not c = d Then
                MsgBox("An error has occurred. Please restart the program, as it will now close.", MsgBoxStyle.Critical, "Error")
                Try
                    If File.Exists("C:\key.txt") Then
                        File.Delete("C:\key.txt")
                    End If
                    My.Computer.Registry.CurrentUser.DeleteSubKey("RegTest")
                Catch ex As Exception
                    MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Error")
                End Try
                Form1.Close()
                Me.Close()
                Exit Sub
            End If
            Dim f As String = devrypt(c, a)
            Dim g As String = devrypt(d, a)
            If TextBox1.Text = f And TextBox1.Text = My.Settings.key Then
                MsgBox("You have entered the correct key. Thanks for Buying Dinde Optimizer !", MsgBoxStyle.Information, "Registered")
                My.Settings.registered = True
                My.Settings.Save()
                Form1.Button5.Enabled = True
                Me.Close()
                Exit Sub
            Else
                MsgBox("The key you entered in incorrect", MsgBoxStyle.Critical, "Registration Invalid")
            End If
        Catch ex As Exception
            MsgBox("An error has occurred. Please restart the program, as it will now close.", MsgBoxStyle.Critical, "Error")
            Try
                If File.Exists("C:\key.txt") Then
                    File.Delete("C:\key.txt")
                End If
                My.Computer.Registry.CurrentUser.DeleteSubKey("RegTest")
            Catch ex1 As Exception
                MsgBox("Error: " & ex1.Message, MsgBoxStyle.Critical, "Error")
            End Try
            Form1.Close()
            Me.Close()
        End Try
    End Sub
End Class