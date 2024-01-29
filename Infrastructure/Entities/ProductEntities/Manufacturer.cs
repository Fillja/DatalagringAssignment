using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.ProductEntities;

[Index("ManufacturerName", Name = "UQ__Manufact__3B9CDE2E690C7502", IsUnique = true)]
public partial class Manufacturer
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ManufacturerName { get; set; } = null!;

    [InverseProperty("Manufacturer")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
