using SQLite;

namespace UserTest.Model
{
    public class User
    {
        [PrimaryKey]
        public int Id { get; set; } = 1;
        public bool HasMotion { get; set; }
        public bool IsSynced { get; set; }
        public bool DarkTheme { get; set; } = true;
    }
}