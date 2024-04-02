using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Author { get; set; } 

    public string Title { get; set; }

    public string Genre { get; set; } 
    public decimal? Price { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<StudentBook> StudentBooks { get; set; } = new List<StudentBook>();
}
