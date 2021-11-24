//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeterKrausAP05LAP
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderLine
    {
        public OrderLine(Order order, Product product, decimal tax)
        {
            OrderId = order.Id;
            ProductId = product.Id;
            Amount = 1m;
            NetUnitPrice = product.NetUnitPrice;
            TaxRate = tax;
            IsActive = true;

        }
        public OrderLine()
        {
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> NetUnitPrice { get; set; }
        public Nullable<decimal> TaxRate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
