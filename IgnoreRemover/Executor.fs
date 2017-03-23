module IgnoreRemover.Executor

open System.Diagnostics
open System
open System.Text
open IgnoreRemover.Formatter


let executeCommand cmd args=
    let builder = StringBuilder()
    let info = ProcessStartInfo()
    info.FileName <- cmd
    info.Arguments <- args
    info.RedirectStandardOutput <- true
    info.UseShellExecute <- false
    info.RedirectStandardError <- true

    let outputHandler (s:DataReceivedEventArgs) = 
        let line = s.Data
        //builder.AppendLine(line)  |> ignore
        s.Data |> writeInfo

    let errorHandler (s: DataReceivedEventArgs) =
        s.Data |> writeError 

    let ps = new Process()
    ps.StartInfo <- info
    ps.Start() |> ignore
    
    ps.OutputDataReceived.Add(outputHandler)
    ps.BeginOutputReadLine()

    ps.ErrorDataReceived.Add(errorHandler)
    ps.BeginErrorReadLine()
    ps.WaitForExit() |> ignore

    //builder.ToString().Split('\n')