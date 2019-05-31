﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedAndBreakfast.Models
{
    public class ConversationViewModel
    {
        public int ConversationID { get; set; }
        public string Title { get; set; }
        public DateTime DateStarted { get; set; }
        public bool ReadOnly { get; set; }
    }
}
