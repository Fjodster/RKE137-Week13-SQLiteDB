using System.Data;
using System.Data.SQLite;
using System.Globalization;

Console.WriteLine("Select database you want to connect 1 - Hero Database, 2 - Coffee database ");
int UserChoiceDB = Int32.Parse(Console.ReadLine());

if (UserChoiceDB == 1)
{
    ReadDataHeroes(CreateConnectionToHeroesDB());
    Console.WriteLine("Select function that you want to use :1 - Add new customer, 2 - Remove customer, 3 - Find Customer");
    int UserChoiceFunct = Int32.Parse(Console.ReadLine());
    if (UserChoiceFunct == 1)
    {
        insertcustomer(CreateConnectionToHeroesDB());
    }
    else if (UserChoiceFunct == 2)
    {
        RemoveCustomer(CreateConnectionToHeroesDB());
    }
    else if (UserChoiceFunct == 3)
    {
        FindCustomer(CreateConnectionToHeroesDB());
    }
    else
    {
        Console.WriteLine("Function not found ! Try again =(");
    }
}
else if (UserChoiceDB == 2)
{
    ReadDataCaffee(CreateConnectionToCafeDB());
    Console.WriteLine("Select function that you want to use :1 - Display products, 2 -  Display products with Category, 3 - Show Customer, 4 - Insert Customer, 5 - Delete Customer");
    int UserChoiceFunct2 = Int32.Parse(Console.ReadLine());
    if (UserChoiceFunct2 == 1)
    {
        DisplayProduct(CreateConnectionToCafeDB());
    }
    else if (UserChoiceFunct2 == 2)
    {
        DisplayProductWithCategory(CreateConnectionToCafeDB());
    }
    else if (UserChoiceFunct2 == 3)
    {
        ShowCustomer(CreateConnectionToCafeDB());
    }
    else if (UserChoiceFunct2 == 4)
    {
        InsertCustomer(CreateConnectionToCafeDB());
    }
    else if (UserChoiceFunct2 == 5)
    {
        DeleteCustomer(CreateConnectionToCafeDB());
    }
    else
    {
        Console.WriteLine("Function not found ! Try again =(");
    }

}
else
{
    Console.WriteLine("DataBase not Found Try again !");
}

static SQLiteConnection CreateConnectionToHeroesDB()
{

    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version =3; New = True; Compress = True;");

    try
    {
        connection.Open();
        Console.WriteLine("Heroes DB found.");
    }
    catch
    {
        Console.WriteLine("Heroes DB not found !");
    }
    return connection;

}

static SQLiteConnection CreateConnectionToCafeDB()
{

    SQLiteConnection connection = new SQLiteConnection("Data Source=bar.db; Version =3; New = True; Compress = True;");

    try
    {
        connection.Open();
        Console.WriteLine("Bar DB found.");
    }
    catch
    {
        Console.WriteLine("Bar DB not found !");
    }
    return connection;

}

static void ReadDataHeroes(SQLiteConnection myConnection)
{ 
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM Customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDob = reader.GetString(3);

        Console.WriteLine($"{readerRowID} - Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDob}");
    }
    myConnection.Close();
}

static void ReadDataCaffee(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM Product";

    myConnection.Close();
}

static void insertcustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    String fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name :");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyyy:");
    dob = Console.ReadLine();


    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstName, lastName, dateOfBirth) VALUES ('{fName}','{lName}', '{dob}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadDataHeroes(myConnection);
}

static void RemoveCustomer (SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter and id to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer.");

    ReadDataHeroes(myConnection);
}

static void FindCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");

    searchName = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText  = $"SELECT customer.rowid, customer.firstname, customer.lastname, status.statustype FROM customerStatus JOIN customer ON customer.rowid = customerStatus.customerid JOIN status on status.rowid = customerStatus.statusid WHERE firstname LIKE '{searchName}'";
    reader = command.ExecuteReader();

    while(reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);
        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");
    }

    myConnection.Close();
}
static void DisplayProduct(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, ProductName, Price FROM product";
    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerProductName = reader.GetString(1);
        int readerProductPrice = reader.GetInt32(2); //hinna tüüp andmebaasis on int, nii et siin loeme andmebaasis ka int-tüüpi andmeid
        Console.WriteLine($"{readerRowid}. {readerProductName}. Price: {readerProductPrice}");
    }
    myConnection.Close();
}
static void DisplayProductWithCategory(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT product.rowid, product.productName, ProductCategory.CategoryName FROM product JOIN ProductCategory ON ProductCategory.rowid = Product.CategoryId";
    reader = command.ExecuteReader();

    while (reader.Read()) 
    {
        string readerRowid = reader["rowid"].ToString() ;
        string readerProductName = reader.GetString(1);
        string readerProductCategory = reader.GetString(2);

        Console.WriteLine($"{readerRowid}. {readerProductName}. Category: {readerProductCategory}");
    }
    myConnection.Close();
}

static void ShowCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid,* FROM Customer";
    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        Console.WriteLine($"{readerRowID} - Full name: {readerStringFirstName} {readerStringLastName}");
    }
    myConnection.Close();
}

static void InsertCustomer (SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName;

    Console.WriteLine("First name:");
    fName = Console.ReadLine();

    Console.WriteLine("Last name:");
    lName = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstname, lastname) VALUES ('{fName}', '{lName}')";
    int rowsInserted = command.ExecuteNonQuery();

    Console.WriteLine($"{rowsInserted} new row has been inserted.");
    ShowCustomer(myConnection);
}
static void DeleteCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter an id to delete:");

    idToDelete = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM Customer WHERE rowid = {idToDelete}";
    int rowsDeleted = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsDeleted} has been deleted.");
    ShowCustomer(myConnection);
}