﻿module IgnoreRemover.Program

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

let remove = function
    | ValidPath path -> 
        Environment.CurrentDirectory <- path
        f "Cleaning %s" path |> writeInfo
        executeCommand "git" "clean -Xdf"
    
let verifiyGitDir root =
    let gitDir = Path.Combine(root, ".git")
    match Directory.Exists gitDir with
    | true -> Result.Ok root
    | false -> Result.Error "Invalid git path"

[<EntryPoint>]
let main argv = 
    let argv = argv |> Array.toList
    match argv with
    | [path] ->
        let path = DirectoryInfo(path).FullName
        let target = findGitProjects path
        let rs = readInput <| f "Dot you want to remove all untracted files/folder from %A (y/n)" path
        match rs with
        | "y" ->
            target |> Seq.iter remove
        | _ -> ()
    | _ ->
        "Invalid command" |> writeError
    0