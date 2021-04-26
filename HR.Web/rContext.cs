﻿using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace HR.Web
{
    public class rContext
    {
        public Employee User { get; set; }
        public List<Employee> Underlings { get; set; }
        public List<Employee> Subordinates { get; set; }
        public List<Employee> Emps { get; set; }

        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }

        public rContext()
        {

            Subordinates = new List<Employee>();
            Underlings = new List<Employee>();
            Emps = new List<Employee>();
            Positions = new List<Position>();
            Departments = new List<Department>();
        }

        public void GetSubordinates()
        {
            if (User.isHR)
            {
                return;
            }
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
                    z.RemoveAll(x => x.Teams.Where(y => y.Position < 70 && y.Name == "DRASEIS").Any());
                    //var e = z.Where(x => x.Teams.Where(y => y.Position < 70 && y.Name == "DRASEIS").Any()).ToList();
                    Subordinates.AddRange(z);
                }
                else
                {
                    var z = Emps.Where(x=>x.isDirector).ToList();
                    Subordinates.AddRange(z);
                }
            }
            Subordinates = Subordinates.Distinct().ToList();
            Subordinates.ForEach(x => x.GetEvalStatus(User.ID));
        }

        public void GetUnderlings()
        {
            Underlings = new List<Employee>();
            if (User.isHR || User.isEditor)
            {
                Underlings = Emps;
                return;
            }
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