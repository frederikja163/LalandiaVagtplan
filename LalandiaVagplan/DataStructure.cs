namespace LalandiaVagplan;


public record Team(Worker[] Workers);

public record Worker(string Name, string From, string To)
{
    public int FromSlot => Util.StringToTimeInterval[From];
    public int ToSlot => Util.StringToTimeInterval[To] - 1;

    public int TiredLevel { get; set; }

    public override string ToString()
    {
        return $"{Name} ({From}-{To})";
    }
}

public enum WorkStation
{
    Highjump = 0,
    Skyrider = 1,
    SportAndFun = 2,
    HighjumpExtra = 3,
    SkyriderExtra = 4,
    SportAndFunExtra = 5,
}

public class TimeSlot : Dictionary<WorkStation, Worker>
{

}
