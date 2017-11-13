using System;
using Npgsql;

namespace TestProject.Models
{
    public abstract class Context
    {
        public String connectionString
        {
            get
            {
                return "Host=localhost;Username=postgres;Password=1234;Database=test_project_db";
            }
        }
    }
    
}