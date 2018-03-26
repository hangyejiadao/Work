using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.BaseModel
{
    public class IntEntity:Entity
    {
        /// <summary>
        /// int主键
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
