CREATE TABLE main.Clients
(
    Id         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    FullName   NVARCHAR(255)                     NOT NULL,
    BirthDate  DATE                              NOT NULL,
    SignInDate DATETIME                          NOT NULL
);

CREATE INDEX idx_clients_birthday
    ON Clients (
                strftime('%m-%d', BirthDate)
        );
CREATE TABLE main.Products
(
    Id       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name     NVARCHAR(50)                      NOT NULL,
    Category NVARCHAR(50)                      NOT NULL,
    Article  NVARCHAR(50)                      NOT NULL,
    Price    DECIMAL(15, 2)                    NOT NULL
);
CREATE TABLE main.Orders
(
    Id         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    ClientId   INT,
    PlaceDate  DATETIME                          NOT NULL,
    TotalValue DECIMAL(15, 2)                    NOT NULL,
    CONSTRAINT FK_Orders_Clients FOREIGN KEY (ClientId) REFERENCES Clients (Id)
);
CREATE TABLE main.Orders_Positions
(
    Id        INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    OrderId   INT                               NOT NULL,
    ProductId INT                               NOT NULL,
    Quantity  INT                               NOT NULL DEFAULT (1),
    CONSTRAINT FK_OrderPositions_Orders FOREIGN KEY (OrderId) REFERENCES Orders (Id),
    CONSTRAINT FK_OrderPositions_Products FOREIGN KEY (ProductId) REFERENCES Products (Id)
);