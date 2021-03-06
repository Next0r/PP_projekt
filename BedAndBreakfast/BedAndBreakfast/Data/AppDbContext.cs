﻿using BedAndBreakfast.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using BedAndBreakfast.Settings;

namespace BedAndBreakfast.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<HelpPage> HelpPages { get; set; }
        public DbSet<HelpTag> HelpTags { get; set; }
        public DbSet<HelpPageHelpTag> HelpPageHelpTags { get; set; }
        public DbSet<NotificationsSetting> NotificationSettings { get; set; }
        public DbSet<PrivacySetting> PrivacySettings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AdditionalContact> AdditionalContacts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<AnnouncementToContact> AnnouncementToContacts { get; set; }
        public DbSet<AnnouncementToPayment> AnnouncementToPayments { get; set; }
        public DbSet<AnnouncementTag> AnnouncementTags { get; set; }
        public DbSet<AnnouncementToTag> AnnouncementToTags { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<AnnouncementToSchedule> AnnouncementToSchedules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ScheduleItemToConversation> ScheduleItemToConversations { get; set; }
        public DbSet<UserToConversation> UserToConversations { get; set; }
        public DbSet<HiddenConversationToUser> HiddenConversationToUsers { get; set; }
        public DbSet<Image> Images { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // This method must be called to map keys for user identity tables.
            base.OnModelCreating(modelBuilder);

            // ---------- Configure entities ----------
            modelBuilder.Entity<HelpTag>()
                .Property(ht => ht.Value).HasMaxLength(IoCContainer.DbSettings.Value.MaxTagLength);
            modelBuilder.Entity<HelpPage>()
                .Property(hp => hp.Content).HasMaxLength(IoCContainer.DbSettings.Value.MaxHelpPageSize);
            modelBuilder.Entity<HelpPage>()
                .Property(hp => hp.Title).HasMaxLength(IoCContainer.DbSettings.Value.MaxHelpPageTitleSize);
            modelBuilder.Entity<Announcement>()
                .Property(a => a.Description).HasMaxLength(IoCContainer.DbSettings.Value.MaxAnnouncementDescSize);
            modelBuilder.Entity<Review>()
                .Property(r => r.Content).HasMaxLength(IoCContainer.DbSettings.Value.MaxReviewContentLength);
            modelBuilder.Entity<Message>()
                .Property(m => m.Content).HasMaxLength(IoCContainer.DbSettings.Value.MaxConversationMessageLength);
            modelBuilder.Entity<Conversation>()
                .Property(c => c.Title).HasMaxLength(IoCContainer.DbSettings.Value.MaxConversationTitleLength);
            modelBuilder.Entity<ScheduleItemToConversation>()
                .HasKey(sm => new { sm.ConversationID, sm.ScheduleItemID });
            modelBuilder.Entity<UserToConversation>()
                .HasKey(uc => new { uc.ConversationID, uc.UserID });
            modelBuilder.Entity<HiddenConversationToUser>()
                .HasKey(uc => new { uc.ConversationID, uc.UserID });
            // ---------- Configure relations ----------

            // Single announcement may have multiple images.
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Announcement)
                .WithMany(a => a.Images)
                .HasForeignKey(i => i.AnnouncementID);

            // User may have multiple hidden conversations.
            // Conversation may have multiple users that wish to hide it.
            modelBuilder.Entity<HiddenConversationToUser>()
                .HasOne(cu => cu.Conversation)
                .WithMany(c => c.HiddenConversationToUsers)
                .HasForeignKey(cu => cu.ConversationID);
            modelBuilder.Entity<HiddenConversationToUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.HiddenConversationToUsers)
                .HasForeignKey(cu => cu.UserID);


            // One conversation may have multiple users and
            // one user may have multiple conversations.
            modelBuilder.Entity<UserToConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserToConversations)
                .HasForeignKey(uc => uc.ConversationID);
            modelBuilder.Entity<UserToConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserToConversations)
                .HasForeignKey(uc => uc.UserID);


            // Message belongs to single conversation,
            // always has single sender (user).
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationID);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SenderID);

            // Announcement may have multiple related conversations.
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Announcement)
                .WithMany(a => a.RelatedConversations)
                .HasForeignKey(c => c.AnnouncementID);

            // One conversation may have multiple schedule items related and
            // one schedule item may have multiple conversations related.
            modelBuilder.Entity<ScheduleItemToConversation>()
                .HasOne(sm => sm.Conversation)
                .WithMany(m => m.ScheduleItemToConversations)
                .HasForeignKey(sm => sm.ConversationID);
            modelBuilder.Entity<ScheduleItemToConversation>()
                .HasOne(sm => sm.ScheduleItem)
                .WithMany(si => si.ScheduleItemToConversations)
                .HasForeignKey(sm => sm.ScheduleItemID);


            // One review has one announcement and on user that both may have
            // multiple reviews.
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Announcement)
                .WithMany(a => a.Reviews);

            // Single reservation has one user, one announcement and one 
            // schedule item (or none if per day timetable selected).
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ScheduleItem)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.ScheduleItemID);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserID);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Announcement)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.AnnouncementID);


            // Many announcements has many schedule items.
            modelBuilder.Entity<AnnouncementToSchedule>()
                .HasKey(ats => new { ats.AnnouncementID, ats.ScheduleItemID });

            modelBuilder.Entity<AnnouncementToSchedule>()
                .HasOne(ats => ats.Announcement)
                .WithMany(a => a.AnnouncementToSchedules)
                .HasForeignKey(ats => ats.AnnouncementID);

            modelBuilder.Entity<AnnouncementToSchedule>()
                .HasOne(ats => ats.ScheduleItem)
                .WithMany(s => s.AnnouncementToSchedules)
                .HasForeignKey(ats => ats.ScheduleItemID);


            // Many announcements may have many tags.
            modelBuilder.Entity<AnnouncementToTag>()
                .HasKey(att => new { att.AnnouncementID, att.AnnouncementTagID });

            modelBuilder.Entity<AnnouncementToTag>()
                .HasOne(att => att.Announcement)
                .WithMany(a => a.AnnouncementToTags)
                .HasForeignKey(att => att.AnnouncementID);

            modelBuilder.Entity<AnnouncementToTag>()
                .HasOne(att => att.AnnouncementTag)
                .WithMany(at => at.AnnouncementToTags)
                .HasForeignKey(att => att.AnnouncementTagID);


            // One user has one profile.
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<User>(u => u.ProfileFK);


            // May help pages has many tags.
            modelBuilder.Entity<HelpPageHelpTag>()
                .HasKey(t => new { t.HelpPageID, t.HelpTagID });

            modelBuilder.Entity<HelpPageHelpTag>()
                .HasOne(hpht => hpht.HelpTag)
                .WithMany(ht => ht.HelpPageHelpTag)
                .HasForeignKey(hpht => hpht.HelpTagID);

            modelBuilder.Entity<HelpPageHelpTag>()
                .HasOne(hpht => hpht.HelpPage)
                .WithMany(hp => hp.HelpPageHelpTag)
                .HasForeignKey(hpht => hpht.HelpPageID);

            // One user has one message settings.
            modelBuilder.Entity<User>()
                .HasOne(u => u.NotificationsSetting)
                .WithOne(s => s.User)
                .HasForeignKey<NotificationsSetting>(s => s.UserFK);

            // One privacy setting has one user.
            modelBuilder.Entity<PrivacySetting>()
                .HasOne(s => s.User)
                .WithOne(u => u.PrivacySetting)
                .HasForeignKey<PrivacySetting>(s => s.UserFK);

            // Multiple profiles may be related to the same address.
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Address)
                .WithMany(a => a.Profiles)
                .HasForeignKey(p => p.AddressFK);

            // Each user has many announcements
            modelBuilder.Entity<User>()
                .HasMany(u => u.Announcements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserFK);

            // Multiple announcements may have multiple additional contacts
            modelBuilder.Entity<AnnouncementToContact>()
                .HasKey(ac => new { ac.AdditionalContactID, ac.AnnouncementID });


            modelBuilder.Entity<AnnouncementToContact>()
                .HasOne(ac => ac.Announcement)
                .WithMany(a => a.AnnouncementToContacts)
                .HasForeignKey(ac => ac.AnnouncementID);

            modelBuilder.Entity<AnnouncementToContact>()
                .HasOne(ac => ac.AdditionalContact)
                .WithMany(a => a.AnnouncementToContacts)
                .HasForeignKey(ac => ac.AdditionalContactID);

            // Multiple announcements may have multiple payment methods
            modelBuilder.Entity<AnnouncementToPayment>()
                .HasKey(ap => new { ap.AnnouncementID, ap.PaymentMethodID });

            modelBuilder.Entity<AnnouncementToPayment>()
                .HasOne(ap => ap.Announcement)
                .WithMany(a => a.AnnouncementToPayments)
                .HasForeignKey(ap => ap.AnnouncementID);

            modelBuilder.Entity<AnnouncementToPayment>()
                .HasOne(ap => ap.PaymentMethod)
                .WithMany(a => a.AnnouncementToPayments)
                .HasForeignKey(ap => ap.PaymentMethodID);

            // One address may be related to multiple announcements.
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Address)
                .WithMany(ad => ad.Announcements)
                .HasForeignKey(a => a.AddressFK);



        }

    }
}
