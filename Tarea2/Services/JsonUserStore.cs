using System.Collections.Generic;
using System.Linq;

namespace Tarea2.Services
{
    public class JsonUserStore : JsonStoreBase
    {
        private class UserRow
        {
            public string email { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }

        private string UsersPath => Map("~/App_Data/users.json");

        public class LoginResult
        {
            public bool Ok { get; set; }
            public string Role { get; set; }
        }

        public LoginResult Validate(string email, string password)
        {
            var users = Read(UsersPath, new List<UserRow>());
            var u = users.FirstOrDefault(x =>
                (x.email ?? "").ToLower() == (email ?? "").ToLower() &&
                (x.password ?? "") == (password ?? "")
            );

            return new LoginResult { Ok = (u != null), Role = (u != null) ? u.role : null };
        }

        public void EnsureSeed()
        {
            var users = Read(UsersPath, new List<UserRow>());
            if (users != null && users.Count > 0) return;

            users = new List<UserRow>
            {
                new UserRow{ email="admin@refugio.com", password="Admin123", role="Admin" },
                new UserRow{ email="cuidador@refugio.com", password="Cuidador123", role="Cuidador" }
            };

            Write(UsersPath, users);
        }
    }
}