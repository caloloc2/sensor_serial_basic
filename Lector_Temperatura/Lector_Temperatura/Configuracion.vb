Public Class Configuracion
    Private Sub Configuracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next

        TextBox1.Text = My.Settings.baudrate
        ComboBox1.Text = My.Settings.puerto_com
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.puerto_com = ComboBox1.Text
        My.Settings.baudrate = CInt(TextBox1.Text)
        My.Settings.Save()
        Me.Dispose()
        Me.Close()
    End Sub
End Class