﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerData.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string Description { get; set; } 

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public int CreatedByUserId { get; set; }
        public Employee CreatedByUser { get; set; }


        public int TeamId { get; set; }

       
        public Team Team { get; set; } 
        public List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
