Public Class Form1
    Dim estado As Boolean = False
    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If My.Settings.puerto_com <> "" Then
            If estado Then
                If SerialPort1.IsOpen Then
                    SerialPort1.Close()
                End If
                sender.Text = "Iniciar Lectura"
                Timer1.Enabled = False
                Timer1.Stop()
                estado = False
            Else
                With SerialPort1
                    .PortName = My.Settings.puerto_com
                    .BaudRate = My.Settings.baudrate
                    .StopBits = IO.Ports.StopBits.None
                    .DataBits = 8
                    .Open()
                End With
                sender.Text = "Detener Lectura"
                Timer1.Enabled = True
                Timer1.Start()
                estado = True
            End If
        Else
            Configuracion.ShowDialog()
        End If
    End Sub

    Private Sub ConfiguraciónToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfiguraciónToolStripMenuItem.Click
        Configuracion.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim data As String = ""
        Try
            If SerialPort1.IsOpen Then
                data = SerialPort1.ReadLine
                If Trim(data) <> "" Then
                    TextBox2.Text = Trim(data)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
