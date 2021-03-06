﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedAndBreakfast.Models
{
    public class SaveAnnouncemenModel
    {
        public int? AnnouncementID { get; set; }
        public int Type { get; set; }
        public int Subtype { get; set; }
        public int? SharedPart { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Description { get; set; }
        public List<ContactPaymentItem> Contacts { get; set; }
        public List<ContactPaymentItem> Payments { get; set; }
        public int Timetable { get; set; }
        public int? PerDayReservations { get; set; }
        public List<ScheduleItemModel> ScheduleItems { get; set; }
        public List<ImageModel> Images { get; set; }
    }


}
