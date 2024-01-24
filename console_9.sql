CREATE DATABASE Practice

GO

USE Practice

GO

CREATE TABLE Items
(
    [Id] INT PRIMARY KEY IDENTITY (1, 1),
    [Name] NVARCHAR(32) NOT NULL,
    [Quantity] INT NOT NULL,
    [Description] NVARCHAR(MAX),
    [Category] NVARCHAR(128),
    [Price] INT NOT NULL,
)


CREATE PROCEDURE AddItem(@Name NVARCHAR(32), @Quantity INT, @Description NVARCHAR(MAX), @Category NVARCHAR(128), @Price INT,
                         @ItemId INT OUTPUT)
AS
BEGIN
    INSERT INTO [Items](Name, Quantity, Description, Category, Price)
    VALUES(@Name, @Quantity, @Description, @Category, @Price)
    SET @ItemId = SCOPE_IDENTITY()
END


CREATE PROCEDURE DeleteItemById(@Id INT)
AS
BEGIN
    DELETE FROM Items
    WHERE Id = @Id
END

CREATE PROCEDURE GetItemById(@Id INT)
AS
BEGIN
    SELECT *
    FROM Items
    WHERE Id = @Id
END

CREATE PROCEDURE UpdateItemById(@Id INT, @Name NVARCHAR(32), @Quantity INT, @Description NVARCHAR(MAX), @Category NVARCHAR(128), @Price INT)
AS
BEGIN
    UPDATE Items
    SET [Name] = @Name,
        [Quantity] = @Quantity,
        [Description] = @Description,
        [Category] = @Category,
        [Price] = @Price
    WHERE [Id] = @Id
END

CREATE PROCEDURE GetAllItems
AS
BEGIN
    SELECT *
    FROM Items
END

