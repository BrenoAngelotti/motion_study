using System.Collections.Generic;

namespace UserTest.Model
{
    public class UserData
    {
        public int Id { get; set; } = 1;
        public bool HasMotion { get; set; }
        public bool DarkTheme { get; set; }
        public bool IsSynced { get; set; }
        public List<Task> Tasks { get; set; }
    }
}