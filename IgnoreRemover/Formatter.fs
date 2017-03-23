module IgnoreRemover.Formatter

open System

type Message =
    | Info of string
    | Error of string

let readInput (title:string) = 
    Console.WriteLine   (title)
    Console.Write       (" > ")
    Console.ReadLine()

let writeMessage message = 

    let write (msg:string) color =
        if not <| String.IsNullOrEmpty(msg) then
            Console.ForegroundColor <- color 
            Console.WriteLine(" {0}", msg)
            Console.ResetColor()

    match message with
    | Info str -> write str ConsoleColor.Green
    | Error str ->  write str ConsoleColor.Red

let writeError = (Error >> writeMessage)
let writeInfo  = (Info  >> writeMessage)