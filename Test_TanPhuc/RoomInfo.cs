using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_TanPhuc
{
    public class RoomInfo
    {
        public string RoomName { get; set; }
        public string Capacity { get; set; }
        public string Quantity { get; set; }
        public string Beds { get; set; }
        public string Price { get; set; }
        public string RoomType { get; set; }
        public string HotelName { get; set; }

        public override string ToString()
        {
            return $"Room: {RoomName}, Capacity: {Capacity}, Quantity: {Quantity}, Beds: {Beds}, " +
                   $"Price: {Price}, Type: {RoomType}, Hotel: {HotelName}";
        }
    }
}
