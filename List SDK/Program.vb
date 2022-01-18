Imports System

Module Program
    Sub Main(args As String())
        Dim locSDKCoreFactory As New IntelliSoft.SDKCore.Factory
        locSDKCoreFactory.GetSDKs.ForEach(Sub(locItem)
                                              Console.WriteLine($"Installed SDK Version: {locItem.Version}, Path: {locItem.InstallationFolder}")
                                          End Sub)
        Console.ReadLine()
    End Sub
End Module
