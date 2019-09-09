using Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// رشته
    /// </summary>
    public class Field : BaseEntity
    {
        [DisplayName("عنوان انگلیسی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Name { get; set; }

        public virtual List<FieldBook> FieldBooks { get; set; }
        public virtual List<Student> Students { get; set; }
        public virtual List<Teacher> Teachers { get; set; }

    }
}