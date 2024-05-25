using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBUZhilishnikKuncevo.Models
{
    public partial class Address
    {
        public string fullAddress => $"г.{city} р.{area} ул.{street} д.{buildingNumber}/{buildingCorpse} под.{entranceNumber} э.{floorNumber} кв.{apartmentNumber}";
    }
}
