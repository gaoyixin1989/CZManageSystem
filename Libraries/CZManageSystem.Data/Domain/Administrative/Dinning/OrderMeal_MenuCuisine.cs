using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_MenuCuisine
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public Guid CuisineId { get; set; }
        public string CuisineName { get; set; }
        public Nullable<int> State { get; set; }


    }

}
