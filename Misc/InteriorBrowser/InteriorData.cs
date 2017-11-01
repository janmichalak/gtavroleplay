using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;



namespace Interiors
{
    class Interior
    {
        public String place { get; set; }
        public String interior_name { get; set; }
        public String ipl_name { get; set; }
        public Vector3 position { get; set; }

        public Interior(String place, String interior_name, String ipl_name, Vector3 position)
        {
            this.place = place;
            this.interior_name = interior_name;
            this.ipl_name = ipl_name;
            this.position = position;

        }
    }
}
