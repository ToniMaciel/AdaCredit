using System;

namespace AdaCredit.Employee
{
    public sealed class EmployeeEntity
    {
        public string Username { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string Hash { get; private set; }
        public string Salt { get; private set; }
        public DateTime LastLogin { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsFirstLogin { get; private set; }
        public string LastEmployeeToChange { get; private set; }


        public EmployeeEntity() { }

        public EmployeeEntity(string username, string name, string document, string hash, string salt, string usernameToChange)
        {
            Username = username;
            Name = name;
            Document = document;
            Hash = hash;
            Salt = salt;
            LastEmployeeToChange = usernameToChange;
            LastLogin = DateTime.Now;
            IsActive = true;
            IsFirstLogin = true;
        }

        internal void UpdateHash(string newHash, string userToChange)
        {
            this.Hash = newHash;
            this.LastEmployeeToChange = userToChange;
        }

        internal void Disable(string userToChange)
        {
            this.IsActive = false;
            this.LastEmployeeToChange = userToChange;
        }

        internal void UpdateLogin(DateTime now)
        {
            this.LastLogin = now;
            this.IsFirstLogin = false;
        }

        public override string ToString()
        {
            return $"{this.Username}({this.Name})";
        }
    }
}
