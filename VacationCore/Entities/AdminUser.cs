using D00_Utility;
using System;
using System.Collections.Generic;
using VacationCore.Entities;

public class AdminUser : User
{
    public AdminUser(string username, string password)
            : base(username, password)
    { }

    public override bool IsAdmin => true;

    public override List<string> GetMenuOptions()
    {
        return new List<string>
            {
                "\n1. Register Vacation",
                "\n2. List Own Vacations",
                "\n3. Update Vacation",
                "\n4. Register Employees' Vacations",
                "\n5. List Employees' Vacations",
                "\n6. Update Employees' Vacations",
                "\n9. Logout",
                "\n0. Exit"
            };
    }
}

