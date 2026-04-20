using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Entities
{
    public partial class GremitAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public long AccountId { get; set; }

        public string Country { get; set; } = null!;

        public string Guid { get; set; } = null!;

        public string DeliveryMethod { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
