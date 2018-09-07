Public Class Form1
    Dim estado As Boolean = False
    Dim valores As New ArrayList()

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
                TextBox1.ReadOnly = False
                Button1.Enabled = True
                Timer1.Enabled = False
                Timer1.Stop()
                estado = False
            Else
                Try
                    With SerialPort1
                        .PortName = My.Settings.puerto_com
                        .BaudRate = My.Settings.baudrate
                        .StopBits = IO.Ports.StopBits.One
                        .DataBits = 8
                        .Open()
                    End With
                    sender.Text = "Detener Lectura"
                    Chart1.Series(0).Points.Clear()
                    TextBox1.ReadOnly = True
                    Button1.Enabled = False
                    Timer1.Enabled = True
                    Timer1.Start()
                    estado = True
                Catch ex As Exception
                    MsgBox(ex.Message.ToString, MsgBoxStyle.Critical)
                    sender.Text = "Iniciar Lectura"
                    Timer1.Enabled = False
                    Timer1.Stop()
                    estado = False
                End Try
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
        Dim Fecha_Actual As String = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now)
        Try
            If SerialPort1.IsOpen Then
                data = SerialPort1.ReadLine
                If IsNumeric(data) Then
                    valores.Add(Fecha_Actual.ToString + " " + Trim(data))
                    Chart1.Series(0).Points.AddY(CInt(data))
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.Filter = "Archivos de Excel (*.xlsx*)|*.xlsx"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If valores.Count > 0 And Trim(TextBox1.Text) <> "" Then
                Dim oExcel As Object
                Dim oBook As Object
                Dim oSheet As Object

                oExcel = CreateObject("Excel.Application")
                oBook = oExcel.Workbooks.Add

                oSheet = oBook.Worksheets(1)
                oSheet.Range("A1").Value = "FECHA"
                oSheet.Range("B1").Value = "HORA"
                oSheet.Range("C1").Value = "MATERIAL"
                oSheet.Range("D1").Value = "VALOR"
                oSheet.Range("A1:D1").Font.Bold = True

                Dim numcelda As Integer = 2
                For Each linea In valores
                    Dim campos As String() = Split(linea, " ")
                    oSheet.Range("A" + CStr(numcelda)).Value = campos(0).ToString
                    oSheet.Range("B" + CStr(numcelda)).Value = campos(1).ToString
                    oSheet.Range("C" + CStr(numcelda)).Value = TextBox1.Text
                    oSheet.Range("D" + CStr(numcelda)).Value = CInt(campos(2))
                    numcelda += 1
                Next

                oBook.SaveAs(SaveFileDialog1.FileName)
                oExcel.Quit

                MsgBox("Archivo generado correctamente.", MsgBoxStyle.Information)
            Else
                MsgBox("No existen valores recopilados o no detalló el campo de material y diámetro para exportar.", MsgBoxStyle.Critical)
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If
        Application.Exit()
    End Sub
End Class
