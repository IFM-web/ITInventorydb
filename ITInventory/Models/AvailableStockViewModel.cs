using System;
using System.Collections.Generic;

namespace ContentManagementSystem.Models
{
    public class AvailableStockViewModel
    {
        public string SearchTerm { get; set; }
        public DateTime? ReceivedDateFrom { get; set; }
        public DateTime? ReceivedDateTo { get; set; }
        public List<MaterialItem> Materials { get; set; }
       
           public IEnumerable<MaterialItem> MaterialItems { get; set; }
        

    
}
} 