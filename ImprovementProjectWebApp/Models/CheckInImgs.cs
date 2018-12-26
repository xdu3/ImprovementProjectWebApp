using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class CheckInImgs
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImgURL { get; set; }
        public int ImgPart { get; set; }



        [Required]
        [Display(Name = "CheckIn")]
        public int CheckInQAId { get; set; }
        [ForeignKey("CheckInQAId")]
        public virtual CheckInQA CheckInQA { get; set; }
    }
}
