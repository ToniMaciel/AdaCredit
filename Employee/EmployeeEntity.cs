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

        public EmployeeEntity() { }

        public EmployeeEntity(string username, string name, string document, string hash, string salt)
        {
            Username = username;
            Name = name;
            Document = document;
            Hash = hash;
            Salt = salt;
            LastLogin = DateTime.Now;
            IsActive = true;
            IsFirstLogin = true;
        }

        internal void UpdateHash(string newHash)
        {
            this.Hash = newHash;
        }

        internal void Disable()
        {
            this.IsActive = false;
        }

        internal void UpdateLogin(DateTime now)
        {
            this.LastLogin = now;
        }
    }
}
