using System.Text.Json;
using System.Text.Json.Serialization;

namespace LalandiaVagplan
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                Team team = GetTeam();
                TimeSlot[] plan = CreatePlan(team);
                Util.WriteTableToConsole(plan);
                
#if !DEBUG
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Contact FrederikJA with 'Workers.json' to learn more.");
            }
#endif
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static Team GetTeam()
        {
            string text = File.ReadAllText("Workers.json");
            Team? team = JsonSerializer.Deserialize<Team>(text);
            if (team == null)
            {
                throw new Exception("Failed parsing 'Workers.json'.");
            }
            return team;
        }

        private static TimeSlot[] CreatePlan(Team team)
        {
            int maxSlot = team.Workers.Select(w => w.ToSlot).Max()!;
            TimeSlot[] plan = new TimeSlot[maxSlot];

            TimeSlot? lastSlot = null;
            for (int i = 0; i < plan.Length; i++)
            {
                TimeSlot slot = new TimeSlot();

                if (!Util.ShiftStartInterval.Contains(i))
                {
                    plan[i] = plan[i - 1];
                    continue;
                }

                List<Worker> orderedWorkers = team.Workers
                    .Where(w => w.FromSlot <= i && i <= w.ToSlot)
                    .OrderByDescending(w => w.TiredLevel).ToList();
                int selectedWorkers = 0;
                int totalWorkers = orderedWorkers.Count;
                while (orderedWorkers.Any() && selectedWorkers < Enum.GetValues<WorkStation>().Length)
                {
                    for (int j = 0; j < orderedWorkers.Count && selectedWorkers < Enum.GetValues<WorkStation>().Length; j++)
                    {
                        Worker worker = orderedWorkers[j];
                        WorkStation station = Util.GetStation(totalWorkers, selectedWorkers);
                        if (i == 0 || plan[i - 1].GetValueOrDefault(station) != worker)
                        {
                            slot[station] = worker;
                            worker.TiredLevel += Util.StationToTireLevel(station) + Random.Shared.Next() % 3;
                            orderedWorkers.Remove(worker);
                            selectedWorkers++;
                        }
                    }
                }

                plan[i] = slot;
            }

            return plan;
        }
    }
}