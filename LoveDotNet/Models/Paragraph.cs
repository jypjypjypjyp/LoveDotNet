using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveDotNet.Models
{
    public enum ParagraphType
    {
        Text,Image
    }

    public class Paragraph
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public ParagraphType Type { get; set; }
        public string Content { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}