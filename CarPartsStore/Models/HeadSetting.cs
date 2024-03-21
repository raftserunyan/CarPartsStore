using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsStore.Models
{
    public class HeadSetting
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FirstText { get; set; }
        public string SecondText { get; set; }
        public string ButtonText { get; set; }
        public string FirstTextColor { get; set; }
        public string SecondTextColor { get; set; }
    }
}
