DROP TABLE OrderRows
DROP TABLE Orders
DROP TABLE Products
DROP TABLE Manufacturers
DROP TABLE Categories

CREATE TABLE Categories
(
	Id int primary key identity,
	CategoryName nvarchar(50) not null unique
)

CREATE TABLE Manufacturers
(
	Id int primary key identity,
	ManufacturerName nvarchar(50) not null unique
)

CREATE TABLE Products
(
	Id int primary key identity,
	Title nvarchar(100) not null,
	Description nvarchar(max) not null,
	Price money not null,
	CategoryId int not null references Categories(Id),
	ManufacturerId int not null references Manufacturers(Id)
)

CREATE TABLE Orders
(
	Id int primary key identity,
	UserId int not null
)

CREATE TABLE OrderRows
(
	OrderId int not null references Orders(Id),
	ProductId int not null references Products(Id),
	Amount int not null
	PRIMARY KEY (OrderId, ProductId)
)


-- Nedan följer testning av tabellerna som gjordes innan migration.

INSERT INTO Categories VALUES ('Electronics'), ('Cars')

INSERT INTO Manufacturers VALUES ('Nvidia'), ('BMW')

INSERT INTO Products VALUES ('Asus RTX 4090 Strix', 'Jävulskt Grafikkort', 10000.00, 1, 1) , ('F11 520D', 'Jävulsk Bil', 100000.00, 2, 2)

SELECT * FROM Products p
JOIN Categories c ON c.Id = p.CategoryId
JOIN Manufacturers m on m.Id = p.ManufacturerId

INSERT INTO Orders VALUES (1)

INSERT INTO OrderRows VALUES (1, 1, 3), (1, 2, 2)

SELECT 

	o.Id, o.UserId,
	p.Title, p.Price,
	r.Amount, r.ProductId,
	m.ManufacturerName,
	c.CategoryName

FROM OrderRows r
JOIN Orders o on o.Id = r.OrderId
JOIN Products p on p.Id = r.ProductId
JOIN Categories c ON c.Id = p.CategoryId
JOIN Manufacturers m on m.Id = p.ManufacturerId