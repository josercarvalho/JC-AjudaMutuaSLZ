//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JC_AjudaMutuaSLZ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Arquivo
    {
        public int ID { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public Nullable<int> IdOrigem { get; set; }
        public string Nome { get; set; }
        public byte[] AnexoBytes { get; set; }
        public string AnexoExtensao { get; set; }
        public string AnexoTipo { get; set; }
        public Nullable<System.DateTime> DataEnvio { get; set; }
        public string Status { get; set; }
    
        public virtual Clientes Clientes { get; set; }
    }
}