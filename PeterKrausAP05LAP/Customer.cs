
namespace PeterKrausAP05LAP
{
    using System;
    using System.Collections.Generic;
    using PeterKrausAP05LAP.Models;
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer(
            string title, string firstName, string lastName, string eMail,
            string street, string zip, string city, string password)
        {

            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Email = eMail;
            Street = street;
            Zip = zip;
            City = city;

            var securefiles = SecureManager.GenerateHash(password);
            Salt = securefiles.salt;
            PWHash = securefiles.hash;

            this.Order = new HashSet<Order>();
        }
        public Customer()
        {
            this.Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string PWHash { get; set; }
        public string Salt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
