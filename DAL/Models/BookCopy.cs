namespace DAL.Models;

public partial class BookCopy
{
    public int BookId { get; set; }

    public int NumberOfCopies { get; set; }

    public virtual Book Book { get; set; } = null!;
}
