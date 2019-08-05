using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;

namespace Data
{
    public static class PhaseKeywords
    {
        public static Dictionary<Phases, List<string>> ExactKeywords
        {
            get
            {
                return new Dictionary<Phases, List<string>>
                {
                    {Phases.Build_Test, new List<string>() },
                    {Phases.Deploy,  new List<string>() { "udeploy" } },
                    {Phases.Macro_Design, new List<string>() },
                    {Phases.Micro_Design, new List<string>() },
                    {Phases.Solution_Outline, new List<string>() },
                    {Phases.Start_Up, new List<string>() },
                    {Phases.Transition_Close, new List<string>() }
                };
            }
        }

        public static Dictionary<Phases, List<string>> FuzzyKeywords
        {
            get
            {
                return new Dictionary<Phases, List<string>>
                {
                    {Phases.Build_Test, new List<string>() { "build", "test", "testing" } },
                    {Phases.Deploy,  new List<string>() { "deployment" } },
                    {Phases.Macro_Design, new List<string>() {"macro", "high level", "design" } },
                    {Phases.Micro_Design, new List<string>() {"micro", "detail", "design" } },
                    {Phases.Solution_Outline, new List<string>() { "outline", "sketch"} },
                    {Phases.Start_Up, new List<string>() {"start up" } },
                    {Phases.Transition_Close, new List<string>() {"close", "transition" } }
                };
            }
        }

        public static Phases GuessPhase(string inputString)
        {
            inputString = inputString.ToLower();
            //__if there is a match for an exact key, then simply use that value
            foreach (var entry in ExactKeywords)
            {
                foreach (string searchTerm in entry.Value)
                {
                    if (inputString.Contains(searchTerm)) return entry.Key;
                }
            }

            Dictionary<Phases, int> phaseVotes = new Dictionary<Phases, int>();
            foreach (var fuzzyKeyword in FuzzyKeywords)
            {
                Phases currentPhase = fuzzyKeyword.Key;
                foreach (string searchTerm in fuzzyKeyword.Value)
                {
                    if (inputString.Contains(searchTerm))
                    {
                        if (phaseVotes.ContainsKey(currentPhase)) phaseVotes[currentPhase] = phaseVotes[currentPhase]+1;
                        else phaseVotes.Add(currentPhase, 1);
                    }
                }
            }

            if (phaseVotes.Count > 0)
            {

                var voteList = phaseVotes.ToList();
                voteList.Sort((p1, p2) => p1.Value.CompareTo(p2));
            }

            Phases resultingPhase = Phases.Not_Assigned;
            if (phaseVotes.Count > 0) resultingPhase = phaseVotes.First().Key;

            return resultingPhase;
        }

        public static void GuessPhase(ref ProjectUpdate projectUpdate)
        {
            string stringToSearch = "";
            stringToSearch += projectUpdate.Subject + " ";
            stringToSearch += projectUpdate.Body;
            projectUpdate.Phase = GuessPhase(stringToSearch).ToString();

        }

        public static Phases GuessPhase(List<StatusUpdate> updates)
        {
            string stringToSearch = "";
            foreach (StatusUpdate update in updates)
            {
                stringToSearch += update.UpdateKey + " ";
                stringToSearch += update.UpdateValue;
            }

            return GuessPhase(stringToSearch);
        }

    }
}
