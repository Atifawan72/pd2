using System;
using System.IO;

class Program
{
    const int MaxCars = 100;
    const int MaxUsers = 100;
    const int MaxNewCars = 100;

    static void Main()
    {
        int userArrSize = MaxUsers;
        string[] users = new string[MaxUsers];
        string[] passwords = new string[MaxUsers];
        string[] roles = new string[MaxUsers];
        string adminBlog = "";
        const int numberOfCars = 3; // You can change this to the desired number of cars
        string[] makes = new string[MaxCars];
        string[] models = new string[MaxCars];
        string[] years = new string[MaxCars];
        int[] prices = new int[MaxCars];
        int numberOfCarsCount = 0;
        int usersCount = 0;
        int loginOption = 0;
        string newsAndEvents = "Welcome to our app!"; // Initialize with a default message

        LoadUserDataFromFile(users, passwords, roles, ref usersCount);
        LoadCarDataFromFile(makes, models, years, prices, ref numberOfCarsCount);

        while (loginOption != 3)
        {
            TopHeader();
            SubMenuBeforeMainMenu("\t\t\tLogin");
            loginOption = LoginMenu();

            if (loginOption == 1)
            {
                Console.Clear();
                string name, password, role;
                TopHeader();
                SubMenuBeforeMainMenu("SignIn");
                Console.Write("\t\t\t Enter your Name: ");
                name = Console.ReadLine();
                Console.Write("\t\t\t Enter your Password: ");
                password = Console.ReadLine();

                role = SignIn(name, password, users, passwords, roles, usersCount);

                if (role == "Admin")
                {
                    TopHeader();
                    Console.Clear();
                    AdminCon(makes, models, years, prices, ref numberOfCarsCount, ref adminBlog, ref newsAndEvents, users, passwords, roles, ref usersCount);
                    SaveUserDataToFile(users, passwords, roles, usersCount);
                    SaveCarDataToFile(makes, models, years, prices, numberOfCarsCount);
                }
                else if (role == "User")
                {
                    TopHeader();
                    Console.Clear();
                    UserInterface(makes, models, years, prices, numberOfCarsCount, adminBlog, newsAndEvents);
                    DisplayBlog(adminBlog); // User can view the blog.
                }
                else
                {
                    Console.WriteLine("\t\t\t You Entered wrong Credentials");
                }
            }
            else if (loginOption == 2)
            {
                Console.Clear();
                string name, password, role;
                TopHeader();
                SubMenuBeforeMainMenu("SignUp");
                Console.Write("\t\t\t Enter your Name: ");
                name = Console.ReadLine();
                Console.Write("\t\t\t Enter your Password: (Should be of 6 numbers)");
                password = Console.ReadLine();
                Console.Write("\t\t\t Enter your Role (Admin or User): ");
                role = Console.ReadLine();

                bool isValid = SignUp(name, password, role, users, passwords, roles, ref usersCount, userArrSize);

                if (isValid)
                {
                    Console.WriteLine("\t\t\t SignedUp Successfully");
                }
                else
                {
                    Console.WriteLine("\t\t\t Sign Up not Successful");
                }
                SaveUserDataToFile(users, passwords, roles, usersCount);
                SaveCarDataToFile(makes, models, years, prices, numberOfCarsCount);
            }

            Console.Clear();
        }

        // Save data to file before exiting
        SaveUserDataToFile(users, passwords, roles, usersCount);
        SaveCarDataToFile(makes, models, years, prices, numberOfCarsCount);
    }

    static int LoginMenu()
    {
        int option = 1; // Default option

        while (true)
        {
            Console.Clear();

            Console.WriteLine("\t\t\t 1. SignIn with your Credentials");
            Console.WriteLine("\t\t\t 2. SignUp to get your Credentials");
            Console.WriteLine("\t\t\t 3. Exit the Application");
            Console.WriteLine();

            // Highlight the current option
            switch (option)
            {
                case 1:
                    Console.WriteLine("\t\t\t [1] SignIn with your Credentials");
                    break;
                case 2:
                    Console.WriteLine("\t\t\t [2] SignUp to get your Credentials");
                    break;
                case 3:
                    Console.WriteLine("\t\t\t [3] Exit the Application");
                    break;
            }

            // Wait for a key press
            ConsoleKeyInfo key = Console.ReadKey();

            // Handle arrow key input
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    option = (option == 1) ? 3 : option - 1;
                    break;
                case ConsoleKey.DownArrow:
                    option = (option == 3) ? 1 : option + 1;
                    break;
                case ConsoleKey.Enter:
                    return option;
            }
        }
    }

    static string SignIn(string name, string password, string[] users, string[] passwords, string[] roles, int usersCount)
    {
        // Validate username
        if (string.IsNullOrEmpty(name) || !name.All(char.IsLetter))
        {
            return "Username should not be empty and should only contain alphabets.";
        }

        // Validate password
        if (password.Length != 6)
        {
            return "Password should be exactly 6 characters.";
        }

        for (int index = 0; index < usersCount; index++)
        {
            if (users[index] == name && passwords[index] == password)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return roles[index];
            }
        }

        return "You are not registered.";
    }

    static bool SignUp(string name, string password, string role, string[] users, string[] passwords, string[] roles, ref int usersCount, int userArrSize)
    {
        // Validate username
        if (string.IsNullOrEmpty(name) || !name.All(char.IsLetter))
        {
            Console.WriteLine("Username should not be empty and should only contain alphabets.");
            return false;
        }

        // Validate password
        if (password.Length != 6)
        {
            Console.WriteLine("Password should be exactly 6 characters.");
            return false;
        }

        for (int index = 0; index < usersCount; index++)
        {
            if (users[index] == name && passwords[index] == password)
            {
                Console.WriteLine("User already exists.");
                return false; // User already exists
            }
        }

        if (usersCount < userArrSize)
        {
            users[usersCount] = name;
            passwords[usersCount] = password;
            roles[usersCount] = role;
            usersCount++;
            return true; // Sign up successful
        }
        else
        {
            Console.WriteLine("Maximum users reached. Cannot sign up.");
            return false; // Maximum users reached
        }
    }

    static void TopHeader()
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("\t\t\t Welcome to My Business Application\n\n");

        Console.WriteLine(" $$$$$$\\                                             $$$$$$$$\\  $$$$$$\\  $$\\   $$\\ $$$$$$\\  ");
        Console.WriteLine("$$  __$$\\                                          $$  _____|$$  __$$\\ $$$\\  $$ |\\_$$  _| ");
        Console.WriteLine("$$ /  \\__| $$$$$$\\   $$$$$$\\   $$$$$$$\\        $$ |      $$ /  \\__|$$$$\\ $$ |  $$ |   ");
        Console.WriteLine("$$ |      $$  __$$\\ $$  __$$\\ $$  _____|        $$$$$\\    $$ |$$$$\\ $$$$\\$$ |  $$ |   ");
        Console.WriteLine("$$ |      $$ |  \\__|$$ /  $$ |\\$$$$$$\\         $$  __|   $$ |\\_$$ |$$  $$$$ |  $$ |   ");
        Console.WriteLine("$$ |  $$\\ $$ |      $$ |  $$ | \\____$$\\        $$ |      $$ |  $$ |$$  /\\$$ |  $$ |$$\\");
        Console.WriteLine("\\$$$$$$  |$$ |      \\$$$$$$  |$$$$$$$  |       $$$$$$$$\\ \\$$$$$$  |$$ /  $$ |  \\$$$$  |");
        Console.WriteLine(" \\______/ \\__|       \\______/ \\_______/        \\________| \\______/ \\__|  \\__|   \\____/ ");

        Console.ResetColor();
    }

    static void SubMenuBeforeMainMenu(string subMenu)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n\n\t\t\t\t{subMenu}\n\n");
        Console.ResetColor();
    }

    static void AdminCon(string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount, ref string adminBlog, ref string newsAndEvents, string[] users, string[] passwords, string[] roles, ref int usersCount)
    {
        int adminOption = 0;
        while (adminOption != 5)
        {
            TopHeader();
            SubMenuBeforeMainMenu("\t\t\t Admin Console");
            Console.WriteLine("\t\t\t 1. Add New Car");
            Console.WriteLine("\t\t\t 2. Remove Car");
            Console.WriteLine("\t\t\t 3. View All Cars");
            Console.WriteLine("\t\t\t 4. Update News and Events");
            Console.WriteLine("\t\t\t 5. Logout");
            Console.WriteLine();

            // Highlight the current option
            switch (adminOption)
            {
                case 1:
                    Console.WriteLine("\t\t\t [1] Add New Car");
                    break;
                case 2:
                    Console.WriteLine("\t\t\t [2] Remove Car");
                    break;
                case 3:
                    Console.WriteLine("\t\t\t [3] View All Cars");
                    break;
                case 4:
                    Console.WriteLine("\t\t\t [4] Update News and Events");
                    break;
                case 5:
                    Console.WriteLine("\t\t\t [5] Logout");
                    break;
            }

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    adminOption = (adminOption == 1) ? 5 : adminOption - 1;
                    break;
                case ConsoleKey.DownArrow:
                    adminOption = (adminOption == 5) ? 1 : adminOption + 1;
                    break;
                case ConsoleKey.Enter:
                    AdminMenu(adminOption, makes, models, years, prices, ref numberOfCarsCount, ref adminBlog, ref newsAndEvents, users, passwords, roles, ref usersCount);
                    break;
            }
        }
    }

    static void AdminMenu(int adminOption, string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount, ref string adminBlog, ref string newsAndEvents, string[] users, string[] passwords, string[] roles, ref int usersCount)
    {
        switch (adminOption)
        {
            case 1:
                Console.Clear();
                AdminAddNewCar(makes, models, years, prices, ref numberOfCarsCount);
                break;
            case 2:
                Console.Clear();
                AdminRemoveCar(makes, models, years, prices, ref numberOfCarsCount);
                break;
            case 3:
                Console.Clear();
                AdminViewAllCars(makes, models, years, prices, numberOfCarsCount);
                break;
            case 4:
                Console.Clear();
                AdminUpdateNewsAndEvents(ref adminBlog);
                break;
            case 5:
                Console.Clear();
                SaveUserDataToFile(users, passwords, roles, usersCount);
                SaveCarDataToFile(makes, models, years, prices, numberOfCarsCount);
                break;
        }
    }

    static void AdminAddNewCar(string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount)
    {
        if (numberOfCarsCount < MaxCars)
        {
            Console.WriteLine("\n\t\t\tEnter Make of Car: ");
            makes[numberOfCarsCount] = Console.ReadLine();
            Console.WriteLine("\n\t\t\tEnter Model of Car: ");
            models[numberOfCarsCount] = Console.ReadLine();
            Console.WriteLine("\n\t\t\tEnter Year of Car: ");
            years[numberOfCarsCount] = Console.ReadLine();
            Console.WriteLine("\n\t\t\tEnter Price of Car: ");
            prices[numberOfCarsCount] = int.Parse(Console.ReadLine());
            numberOfCarsCount++;
            Console.WriteLine("\n\t\t\tCar Added Successfully");
        }
        else
        {
            Console.WriteLine("\n\t\t\tMaximum Cars limit reached.");
        }
        Console.ReadKey();
    }

    static void AdminRemoveCar(string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount)
    {
        Console.WriteLine("\n\t\t\tEnter the Make of Car to Remove: ");
        string makeToRemove = Console.ReadLine();
        int index = Array.IndexOf(makes, makeToRemove);

        if (index != -1)
        {
            for (int i = index; i < numberOfCarsCount - 1; i++)
            {
                makes[i] = makes[i + 1];
                models[i] = models[i + 1];
                years[i] = years[i + 1];
                prices[i] = prices[i + 1];
            }
            numberOfCarsCount--;
            Console.WriteLine("\n\t\t\tCar Removed Successfully");
        }
        else
        {
            Console.WriteLine("\n\t\t\tCar not found.");
        }
        Console.ReadKey();
    }

    static void AdminViewAllCars(string[] makes, string[] models, string[] years, int[] prices, int numberOfCarsCount)
    {
        Console.WriteLine("\n\t\t\tAll Cars:\n");

        for (int i = 0; i < numberOfCarsCount; i++)
        {
            Console.WriteLine($"\t\t\t{i + 1}. {makes[i]} {models[i]}, {years[i]}, ${prices[i]}");
        }
        Console.ReadKey();
    }

    static void AdminUpdateNewsAndEvents(ref string adminBlog)
    {
        Console.WriteLine("\n\t\t\tEnter News and Events:");
        adminBlog = Console.ReadLine();
        Console.WriteLine("\n\t\t\tNews and Events Updated Successfully");
        Console.ReadKey();
    }

    static void UserInterface(string[] makes, string[] models, string[] years, int[] prices, int numberOfCarsCount, string adminBlog, string newsAndEvents)
    {
        int userOption = 0;

        while (userOption != 7)
        {
            TopHeader();
            SubMenu("User Menu");
            userOption = UserMenu();

            switch (userOption)
            {
                case 1:
                    Console.Clear();
                    TopHeader();
                    SubMenu("View Car Details");
                    DisplayCarDetails(makes, models, years, prices, numberOfCarsCount);
                    break;
                case 2:
                    Console.Clear();
                    TopHeader();
                    SubMenu("Purchase a Car");
                    if (PurchaseCar(makes, models, years, prices, ref numberOfCarsCount))
                    {
                        Console.WriteLine("Car purchased successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid car index. Purchase failed.");
                    }
                    break;
                case 3:
                    Console.Clear();
                    TopHeader();
                    SubMenu("Check Blog");
                    if (CheckBlog(adminBlog))
                    {
                        Console.WriteLine($"Blog:\n{adminBlog}");
                    }
                    else
                    {
                        Console.WriteLine("Blog is empty or not available.");
                    }
                    break;
                case 4:
                    Console.Clear();
                    TopHeader();
                    SubMenu("New Arrivals");
                    DisplayNewCarDetails(makes, models, years, prices, numberOfCarsCount);
                    break;
                case 5:
                    Console.Clear();
                    TopHeader();
                    SubMenu("Sort Cars by Price");
                    SortCarsByPrice(makes, models, years, prices, numberOfCarsCount);
                    break;
                case 6:
                    Console.Clear();
                    TopHeader();
                    SubMenu("Display News");
                    DisplayNews(newsAndEvents);
                    break;
                case 7:
                    Console.WriteLine("Exiting the user menu.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }

            ClearScreen();
        }
    }
    static void DisplayCarDetails(string[] makes, string[] models, string[] years, int[] prices, int numberOfCarsCount)
    {
        Console.WriteLine("\nCar Details:");

        for (int i = 0; i < numberOfCarsCount; ++i)
        {
            if (makes[i] != "-1")
            {
                Console.WriteLine($"Car {i + 1} - Make: {makes[i]}, Model: {models[i]}, Price: {prices[i]}, Year: {years[i]}");
            }
        }
    }
    static void SubMenu(string submenu)
    {
        string message = $"\t\t\t Main Menu > {submenu}";
        Console.WriteLine(message);
        Console.WriteLine("\t\t\t $$$$$$$$$$$$$$$$$$$");
    }

    static void ClearScreen()
    {
        Console.WriteLine("   Press Any Key to Continue..");
        Console.ReadKey();
        Console.Clear();
    }

    static void DisplayNews(string newsAndEvents)
    {
        Console.WriteLine("\nNews and Events:");
        Console.WriteLine(newsAndEvents);
    }
    static void DisplayNewCarDetails(string[] newMakes, string[] newModels, string[] newYears, int[] newPrices, int numberOfNewCarsCount)
    {
        Console.WriteLine("\nNew Arrival Car Details:");

        for (int i = 0; i < numberOfNewCarsCount; ++i)
        {
            if (newMakes[i] != "-1")
            {
                Console.WriteLine($"Car {i + 1} - Make: {newMakes[i]}, Model: {newModels[i]}, Price: {newPrices[i]}, Year: {newYears[i]}");
            }
        }
    }

    static int UserMenu()
    {
        Console.WriteLine("Select one of the following options:");
        Console.WriteLine("1. View Car Details");
        Console.WriteLine("2. Purchase a Car");
        Console.WriteLine("3. Check Blog");
        Console.WriteLine("4. New Arrivals"); // New option
        Console.WriteLine("5. Sort Cars by Price");
        Console.WriteLine("6. News and Events");
        Console.WriteLine("7. Exit");

        Console.Write("Your Option: ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            return option;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid option.");
            return UserMenu(); // Recursively call the method until valid input is provided
        }
    }
    static bool PurchaseCar(string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount)
    {
        Console.WriteLine("Enter Your price range:");
        Console.Write("From: ");
        int min;
        if (!int.TryParse(Console.ReadLine(), out min))
        {
            Console.WriteLine("Invalid input for minimum price.");
            return false;
        }

        Console.Write("To: ");
        int max;
        if (!int.TryParse(Console.ReadLine(), out max))
        {
            Console.WriteLine("Invalid input for maximum price.");
            return false;
        }

        if (DisplayCarDetailsInRange(makes, models, years, prices, min, max, numberOfCarsCount))
        {
            Console.Write("Enter the index of the car you want to purchase: ");
            if (int.TryParse(Console.ReadLine(), out int carIndex) && carIndex > 0 && carIndex <= numberOfCarsCount)
            {
                // Removing the car that was purchased.
                makes[carIndex - 1] = "-1";
                return true;
            }
            else
            {
                Console.WriteLine("Invalid car index. Purchase failed.");
                return false;
            }
        }

        return false;
    }
    static bool DisplayCarDetailsInRange(string[] makes, string[] models, string[] years, int[] prices, int min, int max, int numberOfCarsCount)
    {
        Console.WriteLine("\nCar Details:");

        bool flag = false;

        for (int i = 0; i < numberOfCarsCount; ++i)
        {
            if (prices[i] >= min && prices[i] <= max)
            {
                if (makes[i] != "-1")
                {
                    Console.WriteLine($"Car {i + 1} - Make: {makes[i]}, Model: {models[i]}, Price: {prices[i]}, Year: {years[i]}");
                }
                flag = true;
            }
        }

        if (!flag)
        {
            Console.WriteLine("No cars exist in the entered price range. Try changing the range.");
        }

        return flag;
    }
    static string Blog(string adminBlog)
    {
        return adminBlog; // Return Blog content for the user to view.
    }

    static void AdminWriteBlog(ref string adminBlog)
    {
        Console.WriteLine("Admin, Write your blog:");
        adminBlog = Console.ReadLine();
    }

    static void DisplayBlog(string adminBlog)
    {
        Console.WriteLine("\nBlog:");
        // Display additional content or formatting if needed
    }

    static bool CheckBlog(string adminBlog)
    {
        return !string.IsNullOrEmpty(adminBlog);
    }
    static void SortCarsByPrice(string[] makes, string[] models, string[] years, int[] prices, int numberOfCarsCount)
    {
        for (int i = 0; i < numberOfCarsCount - 1; ++i)
        {
            for (int j = 0; j < numberOfCarsCount - i - 1; ++j)
            {
                if (prices[j] > prices[j + 1])
                {
                    // Swap the details of cars if they are in the wrong order
                    SwapCars(ref makes[j], ref models[j], ref years[j], ref prices[j],
                             ref makes[j + 1], ref models[j + 1], ref years[j + 1], ref prices[j + 1]);
                }
            }
        }
    }

    static void SwapCars(ref string make1, ref string model1, ref string year1, ref int price1,
                         ref string make2, ref string model2, ref string year2, ref int price2)
    {
        // Swap values between two cars
        string tempMake = make1;
        string tempModel = model1;
        string tempYear = year1;
        int tempPrice = price1;

        make1 = make2;
        model1 = model2;
        year1 = year2;
        price1 = price2;

        make2 = tempMake;
        model2 = tempModel;
        year2 = tempYear;
        price2 = tempPrice;
    }
    static void SaveUserDataToFile(string[] users, string[] passwords, string[] roles, int usersCount)
    {
        string filePath = "user_data.txt";

        try
        {
            using (StreamWriter userFile = new StreamWriter(filePath))
            {
                for (int i = 0; i < usersCount; ++i)
                {
                    // Use comma as a separator
                    userFile.WriteLine($"{users[i]},{passwords[i]},{roles[i]}");
                }
            }

            Console.WriteLine("User data successfully saved to file.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error opening user data file for writing: {ex.Message}");
        }
    }
    static void LoadUserDataFromFile(string[] users, string[] passwords, string[] roles, ref int usersCount)
    {
        string filePath = "user_data.txt";

        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader userFile = new StreamReader(filePath))
                {
                    string line;
                    while ((line = userFile.ReadLine()) != null)
                    {
                        users[usersCount] = GetFieldData(line, 0);
                        passwords[usersCount] = GetFieldData(line, 1);
                        roles[usersCount] = GetFieldData(line, 2);
                        ++usersCount;
                    }
                }
            }
            else
            {
                Console.WriteLine("User data file not found. Creating a new file.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading user data file: {ex.Message}");
        }
    }

    static string GetFieldData(string line, int fieldIndex)
    {
        // Implement the logic to extract field data based on the field index
        string[] fields = line.Split(',');
        if (fieldIndex >= 0 && fieldIndex < fields.Length)
        {
            return fields[fieldIndex];
        }
        return string.Empty;
    }
    static void SaveCarDataToFile(string[] makes, string[] models, string[] years, int[] prices, int numberOfCarsCount)
    {
        string filePath = "car_data.txt";

        try
        {
            using (StreamWriter carFile = new StreamWriter(filePath))
            {
                for (int i = 0; i < numberOfCarsCount; ++i)
                {
                    // Use comma as a separator
                    carFile.WriteLine($"{makes[i]},{models[i]},{years[i]},{prices[i]}");
                }
            }

            Console.WriteLine("Car data successfully saved to file.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error opening car data file for writing: {ex.Message}");
        }
    }
    static void LoadCarDataFromFile(string[] makes, string[] models, string[] years, int[] prices, ref int numberOfCarsCount)
    {
        string filePath = "car_data.txt";

        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader carFile = new StreamReader(filePath))
                {
                    while (!carFile.EndOfStream)
                    {
                        string line = carFile.ReadLine();
                        string[] fields = line.Split(' ');

                        if (fields.Length >= 4)
                        {
                            makes[numberOfCarsCount] = fields[0];
                            models[numberOfCarsCount] = fields[1];
                            years[numberOfCarsCount] = fields[2];

                            if (int.TryParse(fields[3], out int price))
                            {
                                prices[numberOfCarsCount] = price;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid price format for car {numberOfCarsCount + 1}. Skipping.");
                                continue;
                            }

                            ++numberOfCarsCount;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid format for car data. Skipping line: {line}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Car data file not found. Creating a new file.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading car data file: {ex.Message}");
        }
    }
}