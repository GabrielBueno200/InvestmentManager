db = db.getSiblingDB('investment_db');

db.createCollection('products');
db.products.createIndex({ amount: 1, maturityDate: -1 });
db.products.insertMany([
  {
    _id: ObjectId().toString(),
    Name: "Premium Bond",
    Description: "A high-yield bond with a premium return rate.",
    MaturityDate: new Date("2034-12-31T00:00:00Z"),
    Price: 5000.00,
    Amount: 10,
    Type: 0, // Bond
    PriceHistory: [4800.00, 4900.00, 5000.00],
    CreatedAt: new Date("2024-01-15T09:00:00Z"),
    UpdatedAt: new Date("2024-06-30T09:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Tech Stocks Fund",
    Description: "A fund investing in top technology stocks.",
    MaturityDate: new Date("2030-06-30T00:00:00Z"),
    Price: 150.75,
    Amount: 1000,
    Type: 2, // Fund
    PriceHistory: [140.00, 145.50, 150.75],
    CreatedAt: new Date("2023-11-01T14:00:00Z"),
    UpdatedAt: new Date("2024-07-01T14:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Gold Coins Collection",
    Description: "A collection of rare and valuable gold coins.",
    MaturityDate: null,
    Price: 25000.00,
    Amount: 50,
    Type: 3, // Coins
    PriceHistory: [24000.00, 24500.00, 25000.00],
    CreatedAt: new Date("2024-05-20T10:30:00Z"),
    UpdatedAt: new Date("2024-05-20T10:30:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Blue Chip Stock",
    Description: "Investment in well-established companies with a history of reliability.",
    MaturityDate: null,
    Price: 1200.00,
    Amount: 200,
    Type: 1, // Stock
    PriceHistory: [1150.00, 1180.00, 1200.00],
    CreatedAt: new Date("2024-02-25T11:00:00Z"),
    UpdatedAt: new Date("2024-02-25T11:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Emerging Markets Fund",
    Description: "A fund investing in emerging market economies.",
    MaturityDate: new Date("2035-05-01T00:00:00Z"),
    Price: 75.00,
    Amount: 500,
    Type: 2, // Fund
    PriceHistory: [70.00, 72.50, 75.00],
    CreatedAt: new Date("2024-03-10T16:00:00Z"),
    UpdatedAt: new Date("2024-06-15T16:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Convertible Bond",
    Description: "A bond that can be converted into a predetermined amount of the company's equity.",
    MaturityDate: new Date("2028-11-30T00:00:00Z"),
    Price: 3000.00,
    Amount: 25,
    Type: 0, // Bond
    PriceHistory: [2900.00, 2950.00, 3000.00],
    CreatedAt: new Date("2024-04-05T12:00:00Z"),
    UpdatedAt: new Date("2024-04-05T12:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Vintage Coins Collection",
    Description: "Collection of vintage coins from different eras.",
    MaturityDate: null,
    Price: 12000.00,
    Amount: 30,
    Type: 3, // Coins
    PriceHistory: [11500.00, 11800.00, 12000.00],
    CreatedAt: new Date("2024-06-01T08:00:00Z"),
    UpdatedAt: new Date("2024-06-01T08:00:00Z")
  },
  {
    _id: ObjectId().toString(),
    Name: "Sustainable Tech Fund",
    Description: "Fund focused on investments in sustainable technology.",
    MaturityDate: new Date("2032-09-30T00:00:00Z"),
    Price: 100.00,
    Amount: 800,
    Type: 2, // Fund
    PriceHistory: [90.00, 95.00, 100.00],
    CreatedAt: new Date("2024-07-10T13:00:00Z"),
    UpdatedAt: new Date("2024-07-10T13:00:00Z")
  }
]);

db.createCollection("users");
db.users.insertMany([
  {
    "_id": ObjectId().toString(),
    "Username": "adminUser",
    "Email": "admin@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Admin",
    "CreatedAt": "2024-01-01T00:00:00Z",
    "UpdatedAt": "2024-07-01T00:00:00Z"
  },
  {
    "_id": ObjectId().toString(),
    "Username": "operationUser",
    "Email": "operation@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Operation",
    "CreatedAt": "2024-02-01T00:00:00Z",
    "UpdatedAt": "2024-07-01T00:00:00Z"
  },
  {
    "_id": ObjectId().toString(),
    "Username": "customerUser",
    "Email": "customer@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Customer",
    "CreatedAt": "2024-03-01T00:00:00Z",
    "UpdatedAt": null
  },
  {
    "_id": ObjectId().toString(),
    "Username": "anotherAdminUser",
    "Email": "anotheradmin@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Admin",
    "CreatedAt": "2024-04-01T00:00:00Z",
    "UpdatedAt": "2024-07-01T00:00:00Z"
  },
  {
    "_id": ObjectId().toString(),
    "Username": "anotherOperationUser",
    "Email": "anotheroperation@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Operation",
    "CreatedAt": "2024-05-01T00:00:00Z",
    "UpdatedAt": null
  },
  {
    "_id": ObjectId().toString(),
    "Username": "anotherCustomerUser",
    "Email": "anothercustomer@example.com",
    "PasswordHash": "wZJoKPAsTyfX3b9MRByuqQ==", //@Example2024
    "Role": "Customer",
    "CreatedAt": "2024-06-01T00:00:00Z",
    "UpdatedAt": "2024-07-01T00:00:00Z"
  }
]);