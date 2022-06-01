//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Conta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Conta()
        {
            this.Transacoes = new HashSet<Transaco>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ClienteId { get; set; }
        [Required(ErrorMessage = "O campo Ag�ncia � obrigat�rio", AllowEmptyStrings = false)]
        public int Agencia { get; set; }
        [Required(ErrorMessage = "O campo N�mero � obrigat�rio", AllowEmptyStrings = false)]
        public int Numero { get; set; }
        [Required(ErrorMessage = "O campo Saldo � obrigat�rio", AllowEmptyStrings = false)]
        public double Saldo { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaco> Transacoes { get; set; }
    }
}
