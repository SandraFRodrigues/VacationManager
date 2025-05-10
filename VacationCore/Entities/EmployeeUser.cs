using System;
using System.Collections.Generic;
using VacationCore.Entities;

public class EmployeeUser : User
{
    public EmployeeUser(string username, string password)
        : base(username, password)
    { }

    public override bool IsAdmin => false;  // Implementação de IsAdmin

    public override List<string> GetMenuOptions()
    {
        return new List<string>
        {
            "\n1. Register Vacation",
            "\n2. List Own Vacations",
            "\n3. Update Vacation",
            "\n9. Logout",
            "\n0. Exit"
        };
    }
}