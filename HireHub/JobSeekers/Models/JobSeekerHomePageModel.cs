﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireHub.JobSeekers.Models
{
    internal class JobSeekerHomePageModel
    {
       
        public IEnumerable<JobDetailModel>? jobDetails { get; set; }
        public string searchString { get; set; }
    }
}
