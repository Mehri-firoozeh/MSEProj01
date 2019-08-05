using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostcoSU.Models
{
    public class Project
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int VerticalID { get; set; }

    }

    public enum Phase
    {
        Startup,
        SolutionOutline,
        MacroDesign,
        MicroDesign,
        BuildTest,
        Deploy,
        TransitionClose
    }

    public enum Vertical
    {
        WarehouseSolutions,
        MerchandisingSolutions,
        MembershipSolutions,
        DistributionSolutions,
        InternationalSolutions,
        AncillarySolutions,
        eBusinessSolutions,
        CorporateSolutions
    }
}
