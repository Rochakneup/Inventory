using Inventory.Areas.Identity.Data;

namespace Inventory.Models
{
    public class UseActivity: AuthUser

    {

        public DateTime LoginDate { get; set; }
        public string Status { get; set; } = "Active";

    }
}
