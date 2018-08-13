using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagickNet.Sample.Models
{
    public class PosterModel
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        public string EventDate { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Explanation { get; set; }
        [EmailAddress][Required]
        public string RSVPEmail { get; set; }
        [DataType(DataType.MultilineText)][Required]
        public string AdditionalDetails { get; set; }
        public string ImageSrc { get; set; }
    }
}
