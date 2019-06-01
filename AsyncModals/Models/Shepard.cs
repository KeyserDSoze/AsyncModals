﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncModals.Models
{
    public class Shepard
    {
        public string Name { get; set; }
        public List<Policeman> Policemen { get; set; } = new List<Policeman>();
    }
}
