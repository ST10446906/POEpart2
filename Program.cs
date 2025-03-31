using System;
using System.Collections.Generic; //dictionary for storing responses
using System.Media;
using System.Threading;

class CybersecurityAwarenessBot
{
    // Store responses for different cybersecurity topics //dictionary containing cybersecurity awareness responses
    private static readonly Dictionary<string, string> CybersecurityResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        //each key represents a cybersecurity topic, and the value is its response
        { "password safety", "Use strong, unique passwords. Avoid using personal information and consider a password manager." },
        { "phishing", "Never click on suspicious links. Verify the sender's email and look for red flags like urgent language or unexpected attachments." },
        { "safe browsing", "Use updated browsers, enable pop-up blockers, and be cautious when downloading files from unknown sources." },
        { "how are you", "I'm functioning well and ready to help you stay safe online!" },
        { "your purpose", "I'm a Cybersecurity Awareness Bot designed to educate users about online safety and best practices." }
    };

    // ASCII Art Logo for Cybersecurity Awareness-CyberBot
    //
    private static readonly string[] CybersecurityLogo = {
    @"   ██████╗██╗   ██╗██████╗ ███████╗██████╗  ██████╗  ██████╗ ████████╗",
    @"  ██╔════╝██║   ██║██╔══██╗██╔════╝██╔══██╗██╔═══██╗██╔═══██╗╚══██╔══╝",
    @"  ██║     ██║   ██║██████╔╝█████╗  ██████╔╝██║   ██║██║   ██║   ██║   ",
    @"  ██║     ██║   ██║██╔═══╝ ██╔══╝  ██╔═══╝ ██║   ██║██║   ██║   ██║   ",
    @"  ╚██████╗╚██████╔╝██║     ███████╗██║     ╚██████╔╝╚██████╔╝   ██║   ",
    @"   ╚═════╝ ╚═════╝ ╚═╝     ╚══════╝╚═╝      ╚═════╝  ╚═════╝    ╚═╝   ",
    @"      ⚡ CYBERBOT - YOUR CYBERSECURITY COMPANION ⚡"
};


    // Decorative border for console output
    private static string CreateBorder(int width)
    {
        return new string('=', width);
    }

    // Simulate typing effect
    //function to create a typing effect when displays text
    private static void TypeWriterEffect(string message, int delay = 20)
    {
        foreach (char c in message)//loop through each character
        {
            Console.Write(c);//print character without a new line
            Thread.Sleep(delay);
        }
        Console.WriteLine();//move to the next line after message is fully displayed
    }

    //function to Play welcome voice greeting when bot starts
    private static void PlayWelcomeAudio()
    {
        try
        {
            //tries to play a WAV file named welcome.wav
            using (SoundPlayer player = new SoundPlayer("welcome.wav"))
            {
                player.Play();
            }
        }
        catch (Exception)
        {
            //if the file is missing, display an error message but continue execution
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Audio file not found. Skipping audio...");
            Console.ResetColor();
        }
    }
   //print logo using loop
    // function to Display ASCII Logo
    private static void DisplayLogo()
    {
        //set text color to cyan
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (string line in CybersecurityLogo)
        {
            //print each line of the logo
            Console.WriteLine(line);
        }
        //reset console color to default
        Console.ResetColor();
    }

    // Get user input with validation
    private static string GetValidatedInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please try again.");
                Console.ResetColor();
            }
            else
            {
                break;
            }
        } while (true);

        return input;
    }

    // function to process and respond to user queries
    private static string ProcessUserQuery(string query)
    {

        //loop through all stored cybersecurity topics
        foreach (var topic in CybersecurityResponses.Keys)
        {

            //if user input contains a known topic, return the stored response
            if (query.IndexOf(topic, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return CybersecurityResponses[topic];
            }
        }
        //if no matching topic is found, provide a generic response
        return "I'm not sure about that. Could you rephrase or ask about password safety, phishing, or safe browsing?";
    }

    // Main bot interecation method oor funtion that runs in a loop until the user exists
    static void RunChatbot()
    {
        //clear the console screen for a clean interface
        Console.Clear();
        
        // Play welcome audio (simulated)
        PlayWelcomeAudio();

        // Display logo
        DisplayLogo();

        //display Border and welcome message
        string border = CreateBorder(50);
        Console.WriteLine(border);
        TypeWriterEffect("Welcome to the Cybersecurity Awareness Bot!", 30);
        Console.WriteLine(border);

        // Get user name
        string userName = GetValidatedInput("What's your name? ");

        // Personalized welcome
        Console.ForegroundColor = ConsoleColor.Green;
        TypeWriterEffect($"Hello, {userName}! I'm here to help you stay safe online.", 25);
        Console.ResetColor();

        // Main interaction loop
        while (true)
        {
            //prompt the user for a cybersecurity related question
            string userQuery = GetValidatedInput("\nWhat would you like to know about cybersecurity? (Type 'exit' to quit) ");

            //if user types "exit", end the chatbot session
            if (userQuery.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Goodbye, {userName}! Stay safe online!");
                break;
            }

            // Process and display response
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + ProcessUserQuery(userQuery));
            Console.ResetColor();
        }
    }

    // Entry point of the application
    static void Main()
    {
        try
        {
            RunChatbot();
        }
        catch (Exception ex)
        {
            //catch and display any unexpexted errors
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}
