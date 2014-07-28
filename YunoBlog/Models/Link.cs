using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace YunoBlog.Models
{
    [Table("links")]
    public class Link
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("url")]
        public string URL { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        public override bool Equals(object obj)
        {
            var data = obj as Link;
            if (data.ID == this.ID) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }
    }
}