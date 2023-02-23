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
            Console.WriteLine("0 - exit");
            Console.WriteLine("1 - Get ");
            Console.WriteLine("2 - Insert");
            Console.WriteLine("3 - Update");
            int choose = Convert.ToInt32(Console.ReadLine());
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
        //command.CommandText = $"SELECT FrontCards.Name AS FrontName, BackCards.Name AS BackName FROM FrontCards JOIN BackCards ON FrontCards.FrontId = BackCards.FrontId";
        command.CommandText = $"SELECT * FROM FrontCards";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            //Console.WriteLine(String.Format("{0} {1}", reader["FrontName"].ToString(), reader["BackName"].ToString()));
            Console.WriteLine(String.Format("{0} - {1}",reader["FrontId"].ToString(), reader["Name"].ToString()));
        }
        //Console.;
        reader.Close();
        conn.Close();
        System.Threading.Thread.Sleep(2000);
    }

    private static void Insert()
    {
        Console.WriteLine("Type where do you wanna insert your data FrontCards or BackCards");
        //1 or 2 to choose between them user will not have way to mistype smth
        string table_name = Console.ReadLine();
        //Get(column_names);
        Console.WriteLine("Type what you want to add");
        //Get(card_names);
        string card_value = Console.ReadLine();
        //Conn
        SqlConnection conn = new SqlConnection(connection_string);
        conn.Open();
        SqlCommand command = conn.CreateCommand();
        command.CommandText = $"INSERT INTO {table_name}(Name) VALUES('{card_value}')";
        command.ExecuteNonQuery();
        conn.Close();
    }

    private static void Update()
    {
        Console.Clear();
        Console.WriteLine("Type what table You want to update: FrontCards or BackCards");
        //GetTable()
        string table_name = Console.ReadLine();
        Get();// Get(table_name);
        Console.WriteLine("Choose Id to edit");
        int table_id = Convert.ToInt32(Console.ReadLine());
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
    static string GetColumnName()
    {
        Console.Clear();
        string sol = Console.ReadLine();
        //validation later
        return sol;   
    }
 }