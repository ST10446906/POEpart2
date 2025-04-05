using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;

class CybersecurityAwarenessBot
{
    private static ArrayList CybersecurityResponses;

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

    private static void TypeWriterEffect(string message, int delay = 20, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.Write("Chatbot: ");
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    private static void PlayGreetingAudio()
    {
        string filePath = "greeting.wav";
        try
        {
            SoundPlayer player = new SoundPlayer(filePath);
            player.PlaySync();
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Chatbot: Audio file not found. Skipping audio...");
            Console.ResetColor();
        }
    }

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

    private static string ProcessUserQuery(string query)
    {
        foreach (string response in CybersecurityResponses)
        {
            string[] parts = response.Split('|');
            if (query.IndexOf(parts[0], StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return parts[1];
            }
        }

        return "I'm not sure about that. Could you rephrase or ask about password safety, phishing, or safe browsing?";
    }

    static void RunChatbot()
    {
        Console.Clear();
        new Logo();
        InitializeResponses();
        PlayGreetingAudio();

        Console.WriteLine("==================================================");
        TypeWriterEffect("Welcome to the Cybersecurity Awareness Bot!", 30, ConsoleColor.Green);
        Console.WriteLine("==================================================");

        string namePrompt = "What is your name?";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Chatbot: {namePrompt}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("User: ");
        string userName = Console.ReadLine()?.Trim();
        Console.ResetColor();

        TypeWriterEffect($"Nice to meet you, {userName}!", 25, ConsoleColor.Green);

        while (true)
        {
            string query = GetUserInput("What would you like to know about cybersecurity? (password safety, phishing, or safe browsing) (Type 'exit' to quit)", userName);

            if (string.IsNullOrWhiteSpace(query))
            {
                TypeWriterEffect("Please enter a valid question.", 25, ConsoleColor.Red);
                continue;
            }

            if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                TypeWriterEffect($"Goodbye, {userName}! Stay safe online!", 25, ConsoleColor.Magenta);
                break;
            }

            string botResponse = ProcessUserQuery(query);
            TypeWriterEffect(botResponse, 25, ConsoleColor.Green);
        }
    }

    static void Main(string[] args)
    {
        try
        {
            RunChatbot();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Chatbot: An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}

public class Logo
{
    public Logo()
    {
        string path_project = AppDomain.CurrentDomain.BaseDirectory;
        string new_path_project = path_project.Replace("bin\\Debug\\", "");
        string full_path = Path.Combine(new_path_project, "cybersecuritylogo5.jpeg");

        if (!File.Exists(full_path))
        {
            Console.WriteLine("Chatbot: Logo image not found.");
            return;
        }

        Bitmap image = new Bitmap(full_path);
        image = new Bitmap(image, new Size(100, 100));
        Console.ForegroundColor = ConsoleColor.Blue;

        for (int height = 0; height < image.Height; height++)
        {
            for (int width = 0; width < image.Width; width++)
            {
                Color pixelColor = image.GetPixel(width, height);
                int color = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                char ascii_design = color > 200 ? '.' : color > 150 ? '*' : color > 100 ? 'O' : color > 50 ? '#' : '@';
                Console.Write(ascii_design);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

