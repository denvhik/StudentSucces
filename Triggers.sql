
CREATE OR ALTER TRIGGER [CheckBorrowedBooksAmount]
ON [StudentBook]
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT [sb].[StudentID]
        FROM [inserted] [i]
        JOIN [StudentBook] [sb] ON i.[BookID] = [sb].[BookID]
        JOIN [Book] [b] ON [sb].[BookID] = [b].[BookID]
        GROUP BY [sb].[StudentID]
        HAVING SUM([b].[Price]) > 100
    )
    BEGIN
        RAISERROR ('Total borrowed books price exceeds 100 UAH for one or more students.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;

GO
CREATE OR ALTER TRIGGER [CheckRoomCapacity]
ON [StudentsDormitory]
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT [s].[DormitoryID]
        FROM [inserted] [i]
        JOIN [StudentsDormitory] [s] ON [i].[DormitoryID] = [s].[DormitoryID]
        GROUP BY [s].[DormitoryID]
        HAVING COUNT(*) > 3
    )
    BEGIN
        RAISERROR ('Room capacity exceeded. Maximum allowed number of students per room is three.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;

CREATE TRIGGER [tg_AddBookToBookCopies]
ON [dbo].[Book]
AFTER INSERT
AS
BEGIN
    DECLARE @NewBookID INT;
    SELECT @NewBookID = inserted.BookID FROM inserted;

   
    IF NOT EXISTS (SELECT * FROM [dbo].[BookCopies] WHERE [BookID] = @NewBookID)
    BEGIN
      
        INSERT INTO [dbo].[BookCopies] ([BookID]) VALUES (@NewBookID);
    END;
END;
ENABLE TRIGGER [tg_AddBookToBookCopies] ON [dbo].[Book];

INSERT INTO [dbo].[BookCopies] ([BookID], [NumberOfCopies])
SELECT [BookID], 20
FROM [dbo].[Book];

