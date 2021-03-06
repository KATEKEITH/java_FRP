﻿namespace NonNegative

open System.Windows
open FsXaml
open Sodium.Frp
open SWidgets

type MainWindowBase = XAML<"MainWindow.xaml">

type MainWindow = 
    inherit MainWindowBase
    
    new () as this =
        { inherit MainWindowBase () }
        then
            let flip f x y = f y x
            
            loopWithNoCapturesC (fun value ->
                let lblValue = new SLabel(value |> Cell.map string)
                let plus = new SButton(Content = "+", Width = 25.0, Margin = Thickness(5.0, 0.0, 0.0, 0.0))
                let minus = new SButton(Content = "-", Width = 25.0, Margin = Thickness(5.0, 0.0, 0.0, 0.0))
    
                this.Container.Children.Add(lblValue) |> ignore
                this.Container.Children.Add(plus) |> ignore
                this.Container.Children.Add(minus) |> ignore
    
                let sPlusDelta = plus.SClicked |> mapToS 1
                let sMinusDelta = minus.SClicked |> mapToS -1
                let sDelta = (sPlusDelta, sMinusDelta) |> orElseS
                let sUpdate = sDelta |> snapshotC value (+) |> filterS (flip (>=) 0)
                sUpdate |> Stream.hold 0
            ) |> ignore