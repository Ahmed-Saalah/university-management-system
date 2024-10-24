﻿namespace university_management_system.Models
{
    public abstract class User 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
    }
}
