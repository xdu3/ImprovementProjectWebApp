using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class CustomerProfile
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "性别")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "微信号")]
        public string WeChatNumber { get; set; }
        [Required]
        [Display(Name = "微信二维码")]
        public byte[] WeChatQRCode { get; set; }
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
