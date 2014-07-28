using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace YunoBlog.Models
{
    [Table("categories")]
    public class Category
    {
        [Column("id")]
        public int ID { get;set;}

        [Column("title")]
        public string Title { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        public virtual ICollection<Content> Contents { get; set; }

        public override bool Equals(object obj)
        {
            var data = obj as Category;
            if (data.ID == this.ID) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }
    }
}