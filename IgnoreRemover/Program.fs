module IgnoreRemover.Program

open System
open System.IO
open System.Diagnostics
open IgnoreRemover.Executor
open IgnoreRemover.Formatter

type ValidPath = ValidPath of string
let f = sprintf

let findGitProjects root = 
    let dir = Directory.EnumerateDirectories(root, ".git", SearchOption.AllDirectories)
    dir |> Seq.map (fun x ->
            let dir = DirectoryInfo(x)
            dir.Parent.FullName |> ValidPath
        )

let remove dryRun = function
    | ValidPath path -> 
        Environment.CurrentDirectory <- path
        match dryRun with
        | false -> 
            f "Cleaning %s" path |> writeInfo
            executeCommand "git" "clean -Xdf"
        | true -> 
            f "Testing %s" path |> writeInfo
            executeCommand "git" "clean -Xdfn"

[<EntryPoint>]
let main argv = 
    let argv = argv |> Array.toList
    match argv with
    | [ path ] ->
        let fullPath = DirectoryInfo(path).FullName
        let dryRun dry =
            match dry with
            | true ->   f ">> Test target path %A" fullPath     |> writeInfo
            | false ->  f ">> Start removing"                   |> writeInfo
            let target = findGitProjects fullPath
            target |> Seq.iter (remove dry)

        dryRun true

        let rs = readInput <| f "[Confirm] Do you want to remove all above files/folders from %A (y/n)" fullPath
        match rs with
        | "y" -> dryRun false
        | _ -> ()
    | _ ->
        "Invalid command" |> writeError
    0