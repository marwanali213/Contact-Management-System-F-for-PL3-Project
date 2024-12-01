open System
open System.Windows.Forms
open ContactManagementSystem

[<EntryPoint>]
let main _ =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    let form = UI.createForm()
    Application.Run(form)
    0

