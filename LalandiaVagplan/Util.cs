namespace LalandiaVagplan;

public static class Util
{
    public static int StationToTireLevel(WorkStation station)
    {
        return station switch
        {
            WorkStation.Highjump => 20,
            WorkStation.Skyrider => 15,
            WorkStation.SportAndFun => 10,
            WorkStation.HighjumpExtra => 16,
            WorkStation.SkyriderExtra => 14,
            WorkStation.SportAndFunExtra => 9,
        };
    }

    public static WorkStation GetStation(int totalWorkers, int selectedWorkers)
    {
        return Enum.GetValues<WorkStation>().Take(totalWorkers).OrderBy(StationToTireLevel).ToList()[selectedWorkers];
    }

    public static readonly IReadOnlyDictionary<string, int> StringToTimeInterval = new Dictionary<string, int>()
    {
        {"07:00", -5 },
        {"07:30", -4 },
        {"08:00", -3 },
        {"08:30", -2 },
        {"09:00", -1 },
        {"09:30", 0 },
        {"10:00", 1 },
        {"10:30", 2 },
        {"11:00", 3 },
        {"11:30", 4 },
        {"12:00", 5 },
        {"12:30", 6 },
        {"13:00", 7 },
        {"13:30", 8 },
        {"14:00", 9 },
        {"14:30", 10 },
        {"15:00", 11 },
        {"15:30", 12 },
        {"16:00", 13 },
        {"16:30", 14 },
        {"17:00", 15 },
        {"17:30", 16 },
        {"18:00", 17 },
        {"18:30", 18 },
        {"19:00", 19 },
        {"19:30", 20 },
        {"20:00", 21 },
        {"20:30", 22 },
        {"21:00", 23 },
        {"21:30", 24 },
        {"22:00", 25 },
        {"22:30", 26 },
    };

    public static readonly IReadOnlyDictionary<int, string> TimeIntervalToString = StringToTimeInterval.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static readonly HashSet<int> ShiftStartInterval = new HashSet<int>()
    {
        0, 5, 9, 13, 17
    };

    public static readonly HashSet<int> SkyriderIntervals = new HashSet<int>()
    {
        2, 5, 8, 11, 14, 17, 20
    };

    public static void WriteTableToConsole(TimeSlot[] tableData)
    {
        // Get the maximum width of each column
        Dictionary<WorkStation, int> columnWidths = new Dictionary<WorkStation, int>();
        foreach (var row in tableData)
        {
            foreach (var cell in row)
            {
                if (!columnWidths.ContainsKey(cell.Key))
                {
                    columnWidths.Add(cell.Key, 0);
                }
                if (cell.Value.Name.Length > columnWidths[cell.Key])
                {
                    columnWidths[cell.Key] = cell.Value.Name.Length;
                }
            }
        }

        // Print the table header
        Console.Write("| TimeSlot ");
        foreach (WorkStation key in columnWidths.Keys)
        {
            if (columnWidths[key] < key.ToString().Length)
            {
                columnWidths[key] = key.ToString().Length;
            }
            Console.Write("| ");
            Console.Write(key.ToString().PadRight(columnWidths[key]));
            Console.Write(" ");
        }
        Console.WriteLine("|");

        Console.Write("+----------");
        // Print the table separator
        foreach (WorkStation key in columnWidths.Keys)
        {
            Console.Write("+");
            Console.Write(new string('-', columnWidths[key] + 2));
        }
        Console.WriteLine("+");

        // Print the table rows
        for (int i = 0; i < tableData.Length; i++)
        {
            Console.Write("| ");
            Console.Write(Util.TimeIntervalToString[i]);
            if (SkyriderIntervals.Contains(i))
            {
                Console.Write(" SR ");
            }
            else
            {
                Console.Write("    ");
            }
            TimeSlot? row = tableData[i];
            foreach (WorkStation key in columnWidths.Keys)
            {
                Console.Write("| ");
                Console.Write((row.GetValueOrDefault(key)?.Name ?? string.Empty).PadRight(columnWidths[key]));
                Console.Write(" ");
            }
            Console.WriteLine("|");
        }

        // Print the table footer
        Console.Write("+----------");
        foreach (WorkStation key in columnWidths.Keys)
        {
            Console.Write("+");
            Console.Write(new string('-', columnWidths[key] + 2));
        }
        Console.WriteLine("+");
    }
}