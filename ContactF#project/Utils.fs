namespace ContactManagementSystem

module Utils =
    let formatContact contact =
        sprintf "Name: %s, Phone: %s, Email: %s" contact.Name contact.PhoneNumber contact.Email
