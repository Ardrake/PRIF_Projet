//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGML
{
    using System;
    using System.Collections.Generic;
    
    public partial class CLIENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENT()
        {
            this.LOCATIONS = new HashSet<LOCATION>();
            this.PAIEMENTS = new HashSet<PAIEMENT>();
        }
    
        public int IDCLIENT { get; set; }
        public string PRENOM { get; set; }
        public string NOM { get; set; }
        public string ADDRESSE { get; set; }
        public string VILLE { get; set; }
        public string PROVINCE { get; set; }
        public string CODEPOSTAL { get; set; }
        public string TELEPHONE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOCATION> LOCATIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAIEMENT> PAIEMENTS { get; set; }
    }
}