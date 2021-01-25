using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web
{
    public class SessionData
    {
        public Employee User;
        public List<Employee> Underlings;
        public List<Employee> Subordinates;
        public List<Employee> Emps;

        public List<Position> Positions;
        public List<Department> Departments;

        public SessionData()
        {
            Emps = new ApiEmpData().GetAll().ToList();
        }

        public void GetSubordinates()
        {
            Subordinates = new List<Employee>();
            foreach (var team in User.Teams)
            {
                if (team.Position < 70)
                {
                    break;
                }
                if (team.Position >= 70 && team.Position < 90)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x => x.Teams.Where(y => y.Name == team.Name).Where(y => y.Position < team.Position).Any()).ToList();
                    Subordinates.AddRange(z);
                }
                else if (team.Position >=90 && team.Position<99)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x=>x.Directorate == User.Directorate).Where(x => x.Teams.Where(y=> y.Position >= 70).Any()).ToList();
                    Subordinates.AddRange(z);
                }
                else
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x => x.Teams.Where(y => y.Position >=90 && y.Position != 99).Any()).ToList();
                    Subordinates.AddRange(z);
                }
            }
            Subordinates = Subordinates.Distinct().ToList();
        }

        public void GetUnderlings()
        {
            Underlings = new List<Employee>();
            foreach (var team in User.Teams)
            {
                if (team.Position < 70)
                {
                    break;
                }
                if (team.Position >= 70 && team.Position < 90)
                {
                    Underlings = Subordinates;
                }
                else if (team.Position >=90 && team.Position<99)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x=>x.Directorate == User.Directorate).ToList();
                    Underlings.AddRange(z);
                }
                else
                {
                    Underlings = Emps.Where(x=>x.ID != User.ID).ToList();
                }
            }
            Subordinates = Subordinates.Distinct().ToList();
        }
    }
}