using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewApp.Models
{
    public class StylistServiceList
    {
        public List<serviceList> ListOfServices { get; set; }

        // Properties for single values
        public float rating { get; set; }
        public string backgroundImg { get; set; }
        public string FullName { get; set; }
        public string stylistName { get; set; }
        public int stylistID { get; set; }
    }

    public class serviceList
    {
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int ServiceID { get; set; }
        public string StatusName {  get; set; }
    }
}