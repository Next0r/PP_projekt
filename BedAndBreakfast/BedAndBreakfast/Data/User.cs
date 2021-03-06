﻿using BedAndBreakfast.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BedAndBreakfast.Models
{
    public class User : IdentityUser
    {
        // Keys

        // Profile foreign key must allow null to create administrator account properly.
        public int? ProfileFK { get; set; }

        // Data fields

        public Profile Profile { get; set; }
        public NotificationsSetting NotificationsSetting { get; set; }
        public PrivacySetting PrivacySetting { get; set; }
        public bool IsLocked { get; set; } = false;
		public List<Announcement> Announcements { get; set; }
        public bool IsHost { get; set; } = false;
        public List<Reservation> Reservations { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Message> Messages { get; set; }
        public List<UserToConversation> UserToConversations { get; set; }
        public List<HiddenConversationToUser> HiddenConversationToUsers { get; set; }

    }
}
