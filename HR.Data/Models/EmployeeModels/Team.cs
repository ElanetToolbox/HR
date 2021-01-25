using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class Team
    {
        public float Position { get; set; }
        public string Name { get; set; }

        public void CreateTeam(JToken team)
        {
            Position = (float)team[0];
            Name = team[1].ToString();
        }
    }
}
