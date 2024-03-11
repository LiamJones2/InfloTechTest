using System;
using System.Collections.Generic;

namespace UserManagement.Data.TestData
{
    public class LogTestData
    {
        public static List<Log> Logs => new List<Log>
        {
            new Log { Id = 1, UserId = 1, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 36), Type = "Created User", Changes = "Changes" },
            new Log { Id = 2, UserId = 2, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 37), Type = "Created User", Changes = "Changes" },
            new Log { Id = 3, UserId = 3, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 38), Type = "Created User", Changes = "Changes" },
            new Log { Id = 4, UserId = 4, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 39), Type = "Created User", Changes = "Changes" },
            new Log { Id = 5, UserId = 5, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 40), Type = "Created User", Changes = "Changes" },
            new Log { Id = 6, UserId = 6, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 41), Type = "Created User", Changes = "Changes" },
            new Log { Id = 7, UserId = 7, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 42), Type = "Created User", Changes = "Changes" },
            new Log { Id = 8, UserId = 8, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 43), Type = "Created User", Changes = "Changes" },
            new Log { Id = 9, UserId = 9, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 44), Type = "Created User", Changes = "Changes" },
            new Log { Id = 10, UserId = 10, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 45), Type = "Created User", Changes = "Changes" },
            new Log { Id = 11, UserId = 11, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 46), Type = "Created User", Changes = "Changes" },
            new Log { Id = 12, UserId = 3, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 36), Type = "Updated User", Changes = "Changes for Update" },
            new Log { Id = 13, UserId = 4, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 36), Type = "Updated User", Changes = "Changes for Update" },
            new Log { Id = 14, UserId = 5, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 36), Type = "Deleted User", Changes = "Changes for Deletion" },
            new Log { Id = 15, UserId = 6, CreatedAt = new DateTime(2024, 3, 11, 13, 52, 36), Type = "Deleted User", Changes = "Changes for Deletion" }
        };

        public static Log[] GetLogArray() => Logs.ToArray();
    }
}
