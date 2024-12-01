namespace ContactManagementSystem

open System.Collections.Generic

module ContactManager =
    let mutable contactList: Map<int, Contact> = Map.empty
    let mutable nextId = 1

    let addContact name phone email =
        let contact = { Id = nextId; Name = name; PhoneNumber = phone; Email = email }
        contactList <- contactList.Add(nextId, contact)
        nextId <- nextId + 1
        contact

    let searchContact predicate =
        contactList
        |> Map.toSeq
        |> Seq.map snd
        |> Seq.filter predicate
        |> Seq.toList

    let updateContact id updateFn =
        match contactList.TryFind(id) with
        | Some contact ->
            let updatedContact = updateFn contact
            contactList <- contactList.Add(id, updatedContact)
            Some updatedContact
        | None -> None

    let deleteContact id =
        contactList <- contactList.Remove(id)
        id
