using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Entities
{
    public partial class GRemitBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string GenericCode { get; set; } = null!;
    }
}
