using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBUZhilishnikKuncevo.Models
{
    public partial class PersonalInfo
    {
        public string fullName => $"{surname} {name} {patronymic}";
    }
}
