using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace YunoBlog.Models
{
    [Table("contents")]
    public class Content
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("category_id")]
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }

        [ScriptIgnore]
        public virtual Category Category { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("type")]
        public int TypeAsInt { get; set; }

        [NotMapped]
        public ContentType Type
        {
            get { return (ContentType)TypeAsInt; }
            set { TypeAsInt = (int)value; }
        }

        public override bool Equals(object obj)
        {
            var data = obj as Content;
            if (data.ID == this.ID) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }
    }
    public enum ContentType { Article, Page };
}