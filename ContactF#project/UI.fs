namespace ContactManagementSystem

open System
open System.Windows.Forms
open System.Drawing

module UI =
    let mutable contacts = []

    let refreshContacts (resultList: ListBox) =
        resultList.Items.Clear()
        contacts |> List.iter (fun (name, phone, email) ->
            resultList.Items.Add($"Name: {name}, Phone: {phone}, Email: {email}") |> ignore
        )

    let createForm () =
        let form = new Form(Text = "Contact Management System", Width = 800, Height = 600)
        
        form.Paint.Add(fun e ->
            let brush = new Drawing2D.LinearGradientBrush(form.ClientRectangle, Color.FromArgb(240, 240, 240), Color.FromArgb(200, 200, 200), Drawing2D.LinearGradientMode.Vertical)
            e.Graphics.FillRectangle(brush, form.ClientRectangle)
        )

        form.Font <- new Font("Segoe UI", 10.0F)
        
        let createButton text top left backColor =
            let button = new Button(Text = text, Top = top, Left = left, Width = 120, Height = 40)
            button.BackColor <- backColor
            button.Font <- new Font("Segoe UI", 10.0F, FontStyle.Bold)
            button.FlatStyle <- FlatStyle.Flat
            button.FlatAppearance.BorderSize <- 0
            button.ForeColor <- Color.White
            button.Padding <- new Padding(5)
            button.CausesValidation <- false
            button

        let addButton = createButton "Add Contact" 10 10 Color.LightBlue
        let editButton = createButton "Edit Contact" 10 140 Color.LightGreen
        let deleteButton = createButton "Delete Contact" 10 270 Color.LightCoral

        let searchBox = new TextBox(Top = 10, Left = 400, Width = 300, PlaceholderText = "Search by Name or Phone")
        searchBox.Font <- new Font("Segoe UI", 10.0F)
        let resultList = new ListBox(Top = 50, Left = 10, Width = 760, Height = 500)
        resultList.Font <- new Font("Segoe UI", 10.0F)

        addButton.Click.Add(fun _ -> 
            let addForm = new Form(Text = "Add New Contact", Width = 400, Height = 300)
            addForm.BackColor <- Color.White
            let nameBox = new TextBox(Top = 20, Left = 10, Width = 200, PlaceholderText = "Name")
            let phoneBox = new TextBox(Top = 60, Left = 10, Width = 200, PlaceholderText = "Phone Number")
            let emailBox = new TextBox(Top = 100, Left = 10, Width = 200, PlaceholderText = "Email")
            let saveButton = new Button(Text = "Save", Top = 140, Left = 10, Width = 100)
            saveButton.BackColor <- Color.LightBlue
            saveButton.Font <- new Font("Segoe UI", 10.0F)

            saveButton.Click.Add(fun _ ->
                if nameBox.Text <> "" && phoneBox.Text <> "" then
                    contacts <- (nameBox.Text, phoneBox.Text, emailBox.Text) :: contacts
                    addForm.Close()
                    MessageBox.Show("Contact added successfully!", "Info") |> ignore
                    refreshContacts resultList
                else
                    MessageBox.Show("Name and Phone are required!", "Error") |> ignore
            )

            addForm.Controls.AddRange([| nameBox; phoneBox; emailBox; saveButton |])
            addForm.ShowDialog() |> ignore
        )


        editButton.Click.Add(fun _ -> 
            if resultList.SelectedIndex >= 0 then
                let selectedContact = contacts.[resultList.SelectedIndex]
                let name, phone, email = selectedContact 
                let editForm = new Form(Text = "Edit Contact", Width = 400, Height = 300)
                editForm.BackColor <- Color.White

                let nameBox = new TextBox(Top = 20, Left = 10, Width = 200, Text = name)
                let phoneBox = new TextBox(Top = 60, Left = 10, Width = 200, Text = phone)
                let emailBox = new TextBox(Top = 100, Left = 10, Width = 200, Text = email)
                let saveButton = new Button(Text = "Save", Top = 140, Left = 10, Width = 100)
                saveButton.BackColor <- Color.LightBlue
                saveButton.Font <- new Font("Segoe UI", 10.0F)

                saveButton.Click.Add(fun _ ->
                    if nameBox.Text <> "" && phoneBox.Text <> "" then
                        contacts <- 
                            contacts
                            |> List.mapi (fun i (n, p, e) -> 
                                if i = resultList.SelectedIndex then (nameBox.Text, phoneBox.Text, emailBox.Text) 
                                else (n, p, e)
                            )
                        editForm.Close()
                        MessageBox.Show("Contact updated successfully!", "Info") |> ignore
                        refreshContacts resultList
                    else
                        MessageBox.Show("Name and Phone are required!", "Error") |> ignore
                )

                editForm.Controls.AddRange([| nameBox; phoneBox; emailBox; saveButton |])
                editForm.ShowDialog() |> ignore
            else
                MessageBox.Show("Please select a contact to edit.", "Error") |> ignore
        )

        deleteButton.Click.Add(fun _ -> 
            if resultList.SelectedIndex >= 0 then
                let confirm = MessageBox.Show("Are you sure you want to delete this contact?", "Confirm Delete", MessageBoxButtons.YesNo)
                if confirm = DialogResult.Yes then
                    contacts <- 
                        contacts
                        |> List.mapi (fun i c -> if i = resultList.SelectedIndex then None else Some c)
                        |> List.choose id
                    refreshContacts resultList
                    MessageBox.Show("Contact deleted successfully!", "Info") |> ignore
            else
                MessageBox.Show("Please select a contact to delete.", "Error") |> ignore
        )
        searchBox.TextChanged.Add(fun _ ->
            let query = searchBox.Text.ToLower()
            let filteredContacts =
                contacts
                |> List.filter (fun (name, phone, _) -> 
                    name.ToLower().Contains(query) || phone.Contains(query))
            
            resultList.Items.Clear()
            filteredContacts |> List.iter (fun (name, phone, email) -> 
                resultList.Items.Add($"Name: {name}, Phone: {phone}, Email: {email}") |> ignore
            )
        )

        form.Controls.AddRange([| addButton; editButton; deleteButton; searchBox; resultList |])

        form
