using System.Collections.Generic;

namespace UserTest.Model
{
    public class UserData
    {
        public bool HasMotion { get; set; }
        public bool DarkTheme { get; set; }
        public bool IsSynced { get; set; }
        public List<Task> Tasks { get; set; }
        public int ClarityQuestion { get; set; }
        public int EnjoyabilityQuestion { get; set; }
        public bool FinishedAnswers { get; set; }
    }
}