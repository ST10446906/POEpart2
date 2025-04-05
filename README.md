using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;

class CybersecurityAwarenessBot
{
    // List of predefined chatbot responses based on keywords
    private static ArrayList CybersecurityResponses;

    // Initializes keyword-response pairs for chatbot to recognize and reply with
    private static void InitializeResponses()
    {
        CybersecurityResponses = new ArrayList
        {
            "password safety|Use strong, unique passwords. Avoid using personal information and consider a password manager.",
            "phishing|Never click on suspicious links. Verify the sender's email and look for red flags like urgent language or unexpected attachments.",
            "safe browsing|Use updated browsers, enable pop-up blockers, and be cautious when downloading files from unknown sources.",
            "how are you|I'm functioning well and ready to help you stay safe online!",
            "your purpose|I'm a Cybersecurity Awareness Bot designed to educate users about online safety and best practices."
        };
    }

    // Displays chatbot responses with a typing animation and color
    private static void TypeWriterEffect(string message, int delay = 20, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.Write("Chatbot: ");
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(delay); // Delay to simulate typing effect
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    // Plays an optional greeting sound when the bot starts
    private static void PlayGreetingAudio()
    {
        string filePath = "greeting.wav";
        try
        {
            SoundPlayer player = new SoundPlayer(filePath);
            player.PlaySync(); // Play audio synchronously
        }
        catch
        {
            // Display error if audio is missing
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Chatbot: Audio file not found. Skipping audio...");
            Console.ResetColor();
        }
    }

    // Prompts the user for input and displays prompt in a colored, labeled format
    private static string GetUserInput(string prompt, string userName)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Chatbot: {prompt}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{userName}: ");
        string input = Console.ReadLine()?.Trim();
        Console.ResetColor();

        return input;
    }

    // Matches user input against known keywords and returns the chatbot's reply
    private static string ProcessUserQuery(string query)
    {
        foreach (string response in CybersecurityResponses)
        {
            string[] parts = response.Split('|'); // Split keyword and response
            if (query.IndexOf(parts[0], StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return parts[1]; // Return the matching response
            }
        }

        // Default reply for unknown inputs
        return "I'm not sure about that. Could you rephrase or ask about password safety, phishing, or safe browsing?";
    }

    // Core logic for running the chatbot
    static void RunChatbot()
    {
        Console.Clear(); // Clear the console for a clean start
        new Logo(); // Display ASCII logo

        InitializeResponses(); // Load chatbot knowledge
        PlayGreetingAudio(); // Optional sound on start

        // Welcome banner
        Console.WriteLine("==================================================");
        TypeWriterEffect("Welcome to the Cybersecurity Awareness Bot!", 30, ConsoleColor.Green);
        Console.WriteLine("==================================================");

        // Ask for user's name
        string namePrompt = "What is your name?";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Chatbot: {namePrompt}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("User: ");
        string userName = Console.ReadLine()?.Trim(); // Read user name
        Console.ResetColor();

        // Greet the user
        TypeWriterEffect($"Nice to meet you, {userName}!", 25, ConsoleColor.Green);

        // Main conversation loop
        while (true)
        {
            // Prompt user for a cybersecurity question
            string query = GetUserInput("What would you like to know about cybersecurity? (password safety, phishing, or safe browsing) (Type 'exit' to quit)", userName);

            // Handle empty or invalid input
            if (string.IsNullOrWhiteSpace(query))
            {
                TypeWriterEffect("Please enter a valid question.", 25, ConsoleColor.Red);
                continue;
            }

            // Check for exit condition
            if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                TypeWriterEffect($"Goodbye, {userName}! Stay safe online!", 25, ConsoleColor.Magenta);
                break;
            }

            // Respond to valid user input
            string botResponse = ProcessUserQuery(query);
            TypeWriterEffect(botResponse, 25, ConsoleColor.Green);
        }
    }

    // Entry point for the program
    static void Main(string[] args)
    {
        try
        {
            RunChatbot(); // Start chatbot logic
        }
        catch (Exception ex)
        {
            // Display any unexpected errors
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Chatbot: An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}

// This class handles reading an image and converting it into ASCII art
public class Logo
{
    public Logo()
    {
        string path_project = AppDomain.CurrentDomain.BaseDirectory;
        string new_path_project = path_project.Replace("bin\\Debug\\", ""); // Adjust path if needed
        string full_path = Path.Combine(new_path_project, "cybersecuritylogo5.jpeg");

        if (!File.Exists(full_path))
        {
            Console.WriteLine("Chatbot: Logo image not found.");
            return;
        }

        Bitmap image = new Bitmap(full_path); // Load the image
        image = new Bitmap(image, new Size(100, 100)); // Resize image
        Console.ForegroundColor = ConsoleColor.Blue;

        // Loop through pixels to generate ASCII design
        for (int height = 0; height < image.Height; height++)
        {
            for (int width = 0; width < image.Width; width++)
            {
                Color pixelColor = image.GetPixel(width, height);
                int brightness = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                // Map brightness to ASCII characters
                char ascii_design = brightness > 200 ? '.' :
                                    brightness > 150 ? '*' :
                                    brightness > 100 ? 'O' :
                                    brightness > 50 ? '#' : '@';

                Console.Write(ascii_design);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

