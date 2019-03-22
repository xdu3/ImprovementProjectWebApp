﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "用户名至少要4位。", MinimumLength = 4)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码至少要6位。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

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
  
        [Display(Name = "微信二维码")]
        public byte[] WeChatQRCode { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
