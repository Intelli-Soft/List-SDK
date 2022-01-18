Public Class Factory

    Private Const DotNetFileName As String = "dotnet"
    Private Const DotNetArguments = "--list-sdks"
    Private ReadOnly Property GetTempFolder As String = IO.Path.GetTempPath
    Private Property TempFileName As String = String.Empty
    Private mySDKInformations As List(Of SDKInformation)


    Public Sub GetSDKs()
        ReadSDKs()
    End Sub
    Public ReadOnly Property SDKInformations As List(Of SDKInformation)
        Get
            If mySDKInformations Is Nothing Then
                Throw New Exception("You need to run ReadSDKs method first")
            End If
            Return mySDKInformations
        End Get
    End Property

    Private Sub ReadSDKs()
        mySDKInformations = New List(Of SDKInformation)
        Dim locProcessStartInfo = New ProcessStartInfo With {.FileName = DotNetFileName,
                                                            .Arguments = $"{DotNetArguments}",
                                                            .UseShellExecute = False,
                                                            .CreateNoWindow = True, 
                                                            .RedirectStandardOutput= True}
        Try
            Using locProcess As New Process
                Dim locListOfStrings As New List(Of String)
                AddHandler locProcess.OutputDataReceived, Sub(sender As Object, args As DataReceivedEventArgs)
                                                              If args.Data IsNot Nothing Then

                                                                  mySDKInformations.Add(SplitGivenString(args.Data))

                                                              End If
                                                          End Sub
                locProcess.StartInfo = locProcessStartInfo
                locProcess.Start()
                locProcess.BeginOutputReadLine()
                locProcess.WaitForExit()
            End Using


        Catch locException As Exception
            Throw New Exception(locException.Message)
        End Try

    End Sub
    Private Function SplitGivenString(data As String) As SDKInformation
        Dim locReturnSDKInformation As New SDKInformation
        Dim locManipulateData = data
        Try
            Dim locFolderStartIndex = locManipulateData.IndexOf("[")
            Dim locFolderEndIndex = locManipulateData.IndexOf("]")
            locReturnSDKInformation.InstallationFolder = locManipulateData.Substring(locFolderStartIndex + 1, ((locFolderEndIndex - locFolderStartIndex) - 1))

            locManipulateData = locManipulateData.Substring(0, locFolderStartIndex).TrimEnd
            locReturnSDKInformation.Version = locManipulateData

        Catch locException As Exception
            Throw New Exception(locException.Message)
        End Try

        Return locReturnSDKInformation
    End Function

End Class
