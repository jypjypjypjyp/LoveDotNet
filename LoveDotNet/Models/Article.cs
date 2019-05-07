using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveDotNet.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }

        [ForeignKey("Anthor")]
        public int UserId { get; set; }
        public virtual User Anthor { get; set; }

        public virtual ICollection<Paragraph> Paragraphs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
