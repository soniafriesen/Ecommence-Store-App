using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 Project: Case Study
 Purpose:Class for product
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DomainClasses
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [ForeignKey("BrandId")]    
        public int BrandId { get; set; }    
        public Brand Brand { get; set; }       
        [Required]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
        public string ProductName { get; set; }
        [Required]
        public string GraphicName { get; set; }
        [Required]
        [Column(TypeName ="money")]
        public float CostPrice { get; set; }
        [Required]
        [Column(TypeName ="money")]
        public float MSRP { get; set; }
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOnBackOrder { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 50)]        
        public string Description { get; set; }        
    }
}
