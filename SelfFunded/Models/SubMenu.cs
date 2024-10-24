using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class SubMenu
    {
        public int subMenuId { get; set; }
        public int menuId { get; set; }
        public string subMenuName { get; set; }
        public string routerLink { get; set; }
    }
}