use StudentSucces;
INSERT INTO [dbo].[Dormitory] (DormitoryName, RoomNumber)
VALUES ('NULP11', 101),
       ('NULP11', 102),
       ('NULP11', 103),
       ('NULP12', 201),
       ('NULP12', 202),
       ('NULP12', 203),
       ('NULP13', 301),
       ('NULP13', 302),
       ('NULP13', 303);

INSERT INTO [dbo].[Student] (FirstName, LastName, TicketNumber, BirthYear, BirthPlace, Address, Gender)
VALUES ('John', 'Smith', 'VK123', 2003, 'Kyiv', 'Main St, 123', 'Male'),
       ('Mary', 'Johnson', 'VK456', 2004, 'Lviv', 'Forest Ave, 45', 'Female'),
       ('Oleg', 'Sidorov', 'VK789',2003, 'Kharkiv', 'Sun St, 78', 'Male'),
       ('Svetlana', 'Kovalenko', 'VK101', 2004, 'Odessa', 'Sea St, 10', 'Female'),
       ('Andrew', 'Kozak', 'VK202', 2004, 'Dnipro', 'River St, 20', 'Male'),
       ('Natalie', 'Bondar', 'VK303', 2003, 'Zaporizhzhia', 'Polar St, 30', 'Female'),
       ('Peter', 'Moroz', 'VK404', 2004, 'Ivano-Frankivsk', 'Winter St, 40', 'Male'),
       ('Helen', 'Shevchenko', 'VK505', 2004, 'Ternopil', 'Summer St, 50', 'Female'),
       ('Vasyl', 'Pavlyuk', 'VK606', 2003, 'Chernivtsi', 'Meadow St, 60', 'Male');


INSERT INTO dbo.Groups (GroupName)
VALUES ('KI-308'),
       ('KI-309'),
       ('KI-310');


INSERT INTO dbo.Subject (SubjectName, TeacherID)
VALUES ('Mathematics', 1),
       ('Physics', 2),
	   ('Computer Networks', 2),
	   ('Information Security', 2),
	   ('Programming', 2),
       ('History', 3);


INSERT INTO dbo.Teacher (TeacherName)
VALUES ('Vladimir Sergiyovich'),
       ('Denis Pavolochkov'),
	   ('Nikolai Vasiliovich'),
	   ('Petro Denisenko'),
       ('Michael Kovalenko');


INSERT INTO dbo.Hobbie (HobbyName)
VALUES ('Reading'),
       ('Drawing'),
	   ('Football'),
	   ('Programming'),
	   ('Chess'),
       ('Swimming');


INSERT INTO dbo.Book (Author, Title, Genre, Price)
VALUES 
('J.K. Rowling', 'Harry Potter and the Philosopher''s Stone', 'Fantasy', 14.99),
('Harper Lee', 'To Kill a Mockingbird', 'Fiction', 9.99),
('George Orwell', '1984', 'Dystopian', 11.49),
('Stephen King', 'The Shining', 'Horror', 12.99),
('Agatha Christie', 'Murder on the Orient Express', 'Mystery', 10.49);



INSERT INTO dbo.StudentGroup (StudentID, GroupID, Boss)
VALUES (1, 1, 1),
       (2, 2, 1),
       (3, 3, 0),
       (4, 1, 0),
       (5, 2, 0),
       (6, 3, 0),
       (7, 1, 0),
       (8, 2, 1),
       (9, 3, 0);


INSERT INTO dbo.TeacherSubject (TeacherID, SubjectID)
VALUES 
    (4, 4),
    (5, 5),
    (1, 6),
    (2, 6),
    (3, 6);


INSERT INTO dbo.StudentSubject (StudentID, SubjectID, Score, StartDate)
VALUES (1, 1, 90, '2023-09-01'),
       (2, 2, 85, '2023-09-01'),
       (3, 3, 95, '2023-09-01'),
       (4, 1, 88, '2023-09-01'),
       (5, 2, 92, '2023-09-01'),
       (6, 3, 87, '2023-09-01'),
       (7, 1, 90, '2023-09-01'),
       (8, 2, 85, '2023-09-01'),
       (9, 3, 95, '2023-09-01');


INSERT INTO dbo.StudentHobby (StudentID, HobbyID)
VALUES (1, 1),
       (2, 2),
       (3, 3),
       (4, 1),
       (5, 2),
       (6, 3),
       (7, 1),
       (8, 2),
       (9, 3);


INSERT INTO dbo.StudentBook (StudentID, BookID, PriceCheck, CheckStartDate)
VALUES 
    (1, 1, 50, '2023-09-01'),
    (2, 2, 30, '2023-09-01'),
    (3, 3, 40, '2023-09-01'),
    (4, 1, 20, '2023-09-01'),
    (5, 2, 60, '2023-09-01'),
    (6, 3, 30, '2023-09-01'),
    (7, 1, 10, '2023-09-01'),
    (8, 2, 40, '2023-09-01'),
    (9, 3, 50, '2023-09-01');


INSERT INTO dbo.GroupEnrollment (GroupID, StudentID, EnrollmentStartDate)
VALUES (1, 1, '2023-09-01'),
       (1, 2, '2023-09-01'),
       (1, 3, '2023-09-01'),
       (2, 4, '2023-09-01'),
       (2, 5, '2023-09-01'),
       (2, 6, '2023-09-01'),
       (3, 7, '2023-09-01'),
       (3, 8, '2023-09-01'),
       (3, 9, '2023-09-01');

WITH StudentDorms AS (
    SELECT 
        s.StudentID,
        d.DormitoryID,
        ROW_NUMBER() OVER(PARTITION BY d.DormitoryID ORDER BY NEWID()) AS RowNum
    FROM dbo.Student s
    LEFT JOIN dbo.Dormitory d ON s.StudentID % 3 = d.RoomNumber % 3
)
INSERT INTO dbo.StudentsDormitory (StudentID, DormitoryID, CheckStartDate)
SELECT sd.StudentID, sd.DormitoryID, GETUTCDATE()
FROM StudentDorms sd
LEFT JOIN dbo.StudentsDormitory existing ON sd.StudentID = existing.StudentID
WHERE (sd.RowNum <= 3 OR sd.DormitoryID IS NULL)
AND existing.StudentID IS NULL
AND NOT EXISTS (
    SELECT 1
    FROM dbo.StudentsDormitory
    WHERE StudentID = sd.StudentID AND DormitoryID = sd.DormitoryID
);


