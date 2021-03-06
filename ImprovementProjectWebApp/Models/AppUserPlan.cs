﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class AppUserPlan
    {
        [Required]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        [Display(Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Display(Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public bool IfLock { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Price")]
        public double OrderTotal { get; set; }
        public string Status { get; set; }
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }

        [Display(Name = "PlanPackage")]
        public int? PlanPackageId { get; set; }
        [ForeignKey("PlanPackageId")]
        public virtual PlanPackage PlanPackage { get; set; }

        [Display(Name = "User")]
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<WeekPlan> WeekPlans { get; set; }
        [NotMapped]
        public byte[] QrImage { get; set; }
    }
}
