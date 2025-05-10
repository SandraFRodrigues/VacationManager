using D00_Utility;
using System;
using System.Collections.Generic;
using VacationCore.Entities;
using VacationCore.Services;


public class AdminUser : User
{
    public AdminUser(string username, string password)
        : base(username, password)
    { }

    public override bool IsAdmin => true;  // Implementação de IsAdmin

    public override List<string> GetMenuOptions()
    {
        return new List<string>
        {
            "\n1. Register Vacation",
            "\n2. List Own Vacations",
            "\n3. Update Vacation",
            "\n4. Manage Users' Vacations",
            "\n5. View Operation History",
            "\n9. Logout",
            "\n0. Exit"
        };
    }
}