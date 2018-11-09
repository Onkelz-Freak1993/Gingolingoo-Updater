Imports System.IO
Imports System.IO.Compression
Imports System.Net

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim address As String = "https://www.gingolingoo.de/content/GameLauncherVersion"
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        UpdateVersion.Text = reader.ReadToEnd
        Try
            AppVersion.Text = GetFileVersionInfo(Application.StartupPath & "\Gingolingoo Launcher.exe").ToString
            If UpdateVersion.Text <= AppVersion.Text Then
                MsgBox("Kein Update verfügbar.")
                Process.Start(Application.StartupPath & "\Gingolingoo Launcher.exe")
                Application.Exit()
            End If
        Catch
            MsgBox("Gingolingoo Game Launcher wurde nicht gefunden.")
            Application.Exit()
        End Try
    End Sub

    Private Function GetFileVersionInfo(ByVal filename As String) As Version
        Try
            Return Version.Parse(FileVersionInfo.GetVersionInfo(filename).FileVersion)
        Catch
        End Try
        Return Nothing
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim address As String = "https://www.gingolingoo.de/content/"
            Dim client As WebClient = New WebClient()
            Dim filename As String = "GGL_" & UpdateVersion.Text & ".zip"
            client.DownloadFile(New Uri(address & filename), Application.StartupPath & "\" & filename)
            Button2.Enabled = False
            Button2.Text = "Arbeitet..."
            My.Computer.FileSystem.RenameFile(Application.StartupPath & "\Updater.exe", "Updater_old.exe")
            Threading.Thread.Sleep(2)
            UnZip()
            Process.Start(Application.StartupPath & "\Gingolingoo Launcher.exe")
            Application.Exit()
        Catch ex As Exception
            MsgBox(ex.ToString)
            My.Computer.FileSystem.RenameFile(Application.StartupPath & "\Updater_old.exe", "Updater.exe")
            Application.Exit()
        End Try
    End Sub
End Class

Module Module1
    Sub UnZip()
        Dim filename As String = "GGL_" & Form1.UpdateVersion.Text & ".zip"
        Dim sc As New Shell32.Shell()
        Dim output As Shell32.Folder = sc.NameSpace(Application.StartupPath & "\")
        Dim input As Shell32.Folder = sc.NameSpace(Application.StartupPath & "\" & filename)
        output.CopyHere(input.Items, 4)
        File.Delete(Application.StartupPath & "\" & filename)
    End Sub
End Module