using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectTasksNodeVM
    {
        public IEnumerable<ProjectPhasesTasksVM>? nodeDataArray { get; set; }
        public IEnumerable<TasksDependencyVM>? linkDataArray { get; set; }
        public IEnumerable<ProjectPhasesTasksVM>? firstLevelNode { get; set; }
    }
}
