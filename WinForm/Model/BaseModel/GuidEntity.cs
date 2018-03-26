using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BaseModel
{
    public class GuidEntity : Entity
    {
        /// <summary>
        /// Guid主键
        /// </summary>
        [Key]
         public new Guid Id { get; set; }
    }
}
