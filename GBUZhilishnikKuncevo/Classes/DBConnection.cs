using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBUZhilishnikKuncevo.Classes
{
    /// <summary>
    /// Соединение с базой данных
    /// </summary>
    public static class DBConnection
    {
        //public static menshakova_publicUtilitiesEntities DBConnect { get; set; }
        public static menshakova_publicUtilitiesEntities DBConnect = new menshakova_publicUtilitiesEntities();
    }
}
