using System.Data.SqlClient;

class Program
{
    public static string connection_string = @"Data Source=DESKTOP-2S1UIGJ;Initial Catalog=FlashCards;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    static void Main(string[] args)
    {
        MainMenu();
    }
    static void MainMenu()
    {
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.Clear();
            Console.WriteLine("0 - Exit from program");
            Console.WriteLine("1 - See Both Tables");
            Console.WriteLine("2 - Insert both cards");
            Console.WriteLine("3 - Update");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("5 - See table by Name");
            int choose = GetNumber();
            switch (choose)
            {
                case 0:
                    closeApp = true;
                    break;
                case 1:
                    Get();
                    break;
                case 2:
                    Insert();
                    break;
                case 3:
                    Update();
                    break;
                case 4:
                    Delete(); //Delete Id decrementation ex. From 2 to 1 --> having 1-10 deleteing 5 and 1-9
                    break;
                case 5:
                    string sol = Console.ReadLine();
                    GetTableByName(sol);
                    System.Threading.Thread.Sleep(3000);
                    break;
                default:
                    Console.WriteLine("Choose from 0 to n");
                    break;
            }
        }
    }
    private static void Get()
    {
        SqlConnection conn = new SqlConnection(connection_string);
        conn.Open();
        SqlDataReader reader = null;

        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"SELECT FrontCards.Name AS FrontName, BackCards.Name AS BackName FROM FrontCards JOIN BackCards ON FrontCards.FrontId = BackCards.FrontId";
        reader = command.ExecuteReader();
        while (reader.Read())
        {//maybe select with both tables with Id and name? more readable
            Console.WriteLine(String.Format("{0} {1}", reader["FrontName"].ToString(), reader["BackName"].ToString()));
        }
        reader.Close();
        conn.Close();
        System.Threading.Thread.Sleep(2000);
    }
    private static void Insert()
    { 
        Console.WriteLine("Insert version of word you wanna add.");
        Console.WriteLine(" (Next, you will provide translation to back of the card)");
        string front_card_value = Console.ReadLine();
        SqlConnection conn = new SqlConnection(connection_string);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO FrontCards(Name) VALUES('{front_card_value}'); SELECT SCOPE_IDENTITY();"; //id incr, name front_card
        int front_id = Convert.ToInt32(cmd.ExecuteScalar()); // id of front_card
        Console.Clear();
        Console.WriteLine("Insert TRANSLATION of word that you added previously {0}", front_card_value);
        string back_card_value = Console.ReadLine();
        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"INSERT INTO BackCards(Name, FrontId) VALUES('{back_card_value}', '{front_id}')";
        command.ExecuteNonQuery();
        conn.Close();
    }
    private static void Update()
    {
        Console.Clear();
        //I can do here validation for exisitng table names and try catch it
        //SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'
        //profanation of programming here
        string table_name;
        while (true)
        {
            Console.WriteLine("Type what table You want to update: FrontCards or BackCards");
            table_name = Console.ReadLine();
            if (table_name == "FrontCards" || table_name == "BackCards")
            {
                break;
            } else
            {
                Console.Write("Type proper tablename.");
            }
        }
        GetTableByName(table_name);
        Console.WriteLine("Now choose Id to edit.");
        int table_id = GetNumber(); //validation if db have n rows etc. 
        Console.Clear();
        Console.WriteLine("Type new card name to replace");
        string value = Console.ReadLine();
        SqlConnection conn = new SqlConnection(connection_string);
        conn.Open();
        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"UPDATE {table_name} SET Name = '{value}' WHERE FrontId = {table_id}";
        command.ExecuteNonQuery();
        conn.Close();
    }
    private static void Delete()
    {
        Console.Clear();
        Console.WriteLine("Provide number to choose table");
        Console.WriteLine("1 - FrontCards");
        Console.WriteLine("2 - BackCards");
        int table_choose = GetNumber();
        string table_name="";
        if(table_choose != 1 || table_choose != 2)
        {
            bool flag = false;
            Console.WriteLine("Provide number 1 or 2");
            while(!flag)
            {
                table_choose = GetNumber();
                if(table_choose == 1) { table_name = "FrontCards"; flag = true; }
                else if(table_choose == 2) { table_name = "BackCards"; flag = true;}
                else Console.WriteLine("Provide number 1 or 2");
            }
        }
        GetTableByName(table_name);
        Console.WriteLine("Choose name that you wanna delete: ");
        string name_choose = Console.ReadLine();
        Console.Clear();
        SqlConnection conn = new SqlConnection(connection_string);
        conn.Open();
        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"DELETE FROM {table_name} WHERE Name = '{name_choose}'";
        command.ExecuteNonQuery();
        conn.Close();
    }
    static string GetString()
    {
        bool flag = false;
        string sol = "";
        while(!flag)
        {
            try
            {
                sol = Console.ReadLine();
                flag = true;
            }
            catch(FormatException) 
            {
                Console.WriteLine("Provide text.");
            }
        }
        return sol;   
    }
    static int GetNumber()
    {
        bool flag = false;
        int sol=0;
        while(!flag)
        {
            try
            {
                sol = Convert.ToInt32(Console.ReadLine());
                flag = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Provide number.");
            }
        }
        return sol;
    }
    static void GetTableByName(string table)
    {
        Console.Clear();
        SqlConnection conn = new SqlConnection(connection_string);
        SqlDataReader reader = null;
        conn.Open();
        SqlCommand id_command = conn.CreateCommand();
        id_command.CommandText = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}' AND COLUMN_NAME LIKE '%Id%'";
        string column_id = (string)id_command.ExecuteScalar(); //FrontId or BackId
        //========
        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"SELECT {column_id}, Name FROM {table}";
        reader = command.ExecuteReader();
        while(reader.Read())
        {
            Console.WriteLine(String.Format("Id: {0} Name: {1}", reader[column_id].ToString(), reader["Name"].ToString()));
        }
        reader.Close();
        conn.Close();
    }
}