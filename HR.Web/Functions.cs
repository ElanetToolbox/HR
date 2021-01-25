using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web
{
    public static class Functions
    {
        public static string GetFileNameFromPath(string fName)
        {
            return Path.GetFileName(fName);
        }

        public static void GetSubordinates()
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

        public static void GetUnderlings()
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

        //public static Employee User;
        //public static List<Employee> Underlings;
        public static List<Employee> Subordinates;
        public static List<Employee> Emps;

        public static List<Position> Positions;
        public static List<Department> Departments;
    }
}