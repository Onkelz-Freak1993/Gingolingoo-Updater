Imports System.ComponentModel
Imports System.IO
Imports System.Net

Public Class Form1
    Dim filename As String
    Dim WithEvents Client2 As WebClient = New WebClient()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Dim address As String = "https://www.gingolingoo.de/content/GameLauncherVersion"
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        UpdateVersion.Text = reader.ReadToEnd
        filename = "GGL_" & UpdateVersion.Text & "_Setup.exe"
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
            Return Nothing
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            UpdateBW.CancelAsync()
        Catch ex As Exception
        End Try
        Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Updateprogress.Visible = True
            Button2.Visible = False
            UpdateApp()
        Catch ex As Exception
            Dim res As Long
            res = MsgBox("Es ist ein Fehler aufgetreten. Möchtest du den Fehlercode sehen?", vbYesNo, "Fehler")
            Select Case res
                Case vbYes
                    MsgBox(ex.ToString)
                Case vbNo
                    'nix
            End Select
            Application.Exit()
        End Try
    End Sub
    Private Sub Client2_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
        Dim percentage As Double = bytesIn / totalBytes * 100
        Me.Text = "Gingolingoo Updater - " & Int32.Parse(Math.Truncate(percentage).ToString()) & "%"
        Updateprogress.Value = Int32.Parse(Math.Truncate(percentage).ToString())
    End Sub

    Public Function UpdateApp()
        Try
            AddHandler Client2.DownloadProgressChanged, AddressOf Client2_DownloadProgressChanged
            AddHandler Client2.DownloadFileCompleted, AddressOf Client2_DownloadFileCompleted
            Dim address As String = "https://www.gingolingoo.de/content/"
            Try
                Client2.DownloadFileAsync(New Uri(address & filename), Application.StartupPath & "\" & filename)
            Catch
            End Try
        Catch ex As Exception
            Dim res As Long
            res = MsgBox("Es ist ein Fehler aufgetreten. Möchtest du den Fehlercode sehen?", vbYesNo, "Fehler")
            Select Case res
                Case vbYes
                    MsgBox(ex.ToString)
                Case vbNo
                    'nix
            End Select
        End Try
        Return False
    End Function

    Private Sub Client2_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs)
        Updateprogress.Visible = False
        Button2.Visible = True
        Me.Text = "Gingolingoo Updater"
        Try
            Application.Exit()
            Process.Start(Application.StartupPath & "\" & filename)
        Catch exc As Exception
            Dim res As Long
            res = MsgBox("Es ist ein Fehler aufgetreten. Möchtest du den Fehlercode sehen?", vbYesNo, "Fehler")
            Select Case res
                Case vbYes
                    MsgBox(exc.ToString)
                Case vbNo
                    'nix
            End Select
        End Try
    End Sub
End Class