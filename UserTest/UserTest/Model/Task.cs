using UserTest.Enums;

namespace UserTest.Model
{
    public class Task
    {
        public ETask TaskIdentifier { get; set; }
        public int FirstQuestion { get; set; }
        public int SecondQuestion { get; set; }
        public int SelectionCounter { get; set; }
        public float ActivityTime { get; set; }
        public bool Finished { get; set; }
    }
}
