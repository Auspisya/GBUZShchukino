//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GBUZhilishnikKuncevo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Counter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Counter()
        {
            this.Accounting = new HashSet<Accounting>();
        }
    
        public int id { get; set; }
        public int typeOfCounterId { get; set; }
        public int apartmentId { get; set; }
        public string counterNumber { get; set; }
        public System.DateTime startOfOperation { get; set; }
        public System.DateTime endOfOperation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accounting> Accounting { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual TypeOfCounter TypeOfCounter { get; set; }
    }
}
