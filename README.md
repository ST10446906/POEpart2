using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Linq;

class CybersecurityAwarenessBot
{
    private static Dictionary<string, List<string>> CybersecurityResponses;
    private static Dictionary<string, string> UserMemory;
    private static Random random = new Random();

    private static void InitializeResponses()
    {
        // Using a dictionary with lists to support multiple responses for each keyword
        CybersecurityResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "password", new List<string> {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                "Password managers are a great tool to keep track of complex passwords securely.",
                "Consider using passphrases instead of simple passwords for better security."
            }},
            { "phishing", new List<string> {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.",
                "Always verify the sender's email address and look for red flags like urgent language or unexpected attachments.",
                "Hover over links before clicking to see the actual URL destination."
            }},
            { "browsing", new List<string> {
                "Use updated browsers, enable pop-up blockers, and be cautious when downloading files from unknown sources.",
                "Consider using a VPN for additional security, especially when using public Wi-Fi networks.",
                "Keep your operating system and applications up-to-date with security patches."
            }},
            { "privacy", new List<string> {
                "Regularly review the privacy settings on your social media accounts and applications.",
                "Be mindful of what information you share online, as it can be difficult to remove once published.",
                "Consider using privacy-focused browsers and search engines."
            }},
            { "malware", new List<string> {
                "Install reliable antivirus software and keep it updated.",
                "Be careful when downloading software - only use trusted sources.",
                "Scan email attachments before opening them, even if they're from people you know."
            }},
            { "how are you", new List<string> {
                "I'm functioning well and ready to help you stay safe online!",
                "I'm operational and eager to discuss cybersecurity best practices with you!"
            }},
            { "your purpose", new List<string> {
                "I'm a Cybersecurity Awareness Bot designed to educate users about online safety and best practices.",
                "My purpose is to help you learn about protecting yourself in the digital world."
            }}
        };

        UserMemory = new Dictionary<string, string>();
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

    // Sentiment detection function
    private static string DetectSentiment(string input)
    {
        string inputLower = input.ToLower();

        // Simple sentiment detection based on keywords
        if (inputLower.Contains("worried") || inputLower.Contains("scared") ||
            inputLower.Contains("anxious") || inputLower.Contains("concerned"))
        {
            return "worried";
        }
        else if (inputLower.Contains("confused") || inputLower.Contains("don't understand") ||
                 inputLower.Contains("what is") || inputLower.Contains("how do") ||
                 inputLower.Contains("help me"))
        {
            return "confused";
        }
        else if (inputLower.Contains("thanks") || inputLower.Contains("thank you") ||
                 inputLower.Contains("helpful") || inputLower.Contains("great"))
        {
            return "pleased";
        }
        else if (inputLower.Contains("frustrated") || inputLower.Contains("angry") ||
                 inputLower.Contains("not working") || inputLower.Contains("stupid"))
        {
            return "frustrated";
        }

        return "neutral";
    }

    // Process user query with dynamic responses, sentiment awareness, and memory
    private static string ProcessUserQuery(string query, string userName)
    {
        string sentiment = DetectSentiment(query);
        string queryLower = query.ToLower();

        // Store topics the user shows interest in
        foreach (var topic in CybersecurityResponses.Keys)
        {
            if (queryLower.Contains(topic.ToLower()) && topic != "how are you" && topic != "your purpose")
            {
                UserMemory["favorite_topic"] = topic;
                break;
            }
        }

        // Check for specific keyword matches
        foreach (var kvp in CybersecurityResponses)
        {
            if (queryLower.Contains(kvp.Key.ToLower()))
            {
                // Randomly select a response from the list for variety
                int randomIndex = random.Next(kvp.Value.Count);
                string baseResponse = kvp.Value[randomIndex];

                // Enhance response based on sentiment
                switch (sentiment)
                {
                    case "worried":
                        return $"I understand your concern about {kvp.Key}. {baseResponse} Remember, staying informed is the first step to staying safe.";
                    case "confused":
                        return $"Let me clarify about {kvp.Key}. {baseResponse} Does that help explain it better?";
                    case "pleased":
                        return $"I'm glad I could help! About {kvp.Key}: {baseResponse} Is there anything else you'd like to know?";
                    case "frustrated":
                        return $"I'm sorry you're having trouble with {kvp.Key}. {baseResponse} Let's take it step by step.";
                    default:
                        // Reference previous interests if available
                        if (UserMemory.ContainsKey("favorite_topic") && !kvp.Key.Equals(UserMemory["favorite_topic"], StringComparison.OrdinalIgnoreCase))
                        {
                            return $"{baseResponse} Since you were interested in {UserMemory["favorite_topic"]} earlier, it's also good to know that these topics connect in cybersecurity.";
                        }
                        return baseResponse;
                }
            }
        }

        // If we're here, no direct keyword match was found
        // Check if the user is asking about something they previously mentioned
        if (queryLower.Contains("what did i ask") || queryLower.Contains("what was i interested"))
        {
            if (UserMemory.ContainsKey("favorite_topic"))
            {
                return $"You previously showed interest in {UserMemory["favorite_topic"]}. Would you like to know more about it?";
            }
        }

        // Default response with memory usage if available
        if (UserMemory.ContainsKey("favorite_topic"))
        {
            return $"I'm not sure about that specific query. Would you like to continue discussing {UserMemory["favorite_topic"]} or learn about other cybersecurity topics like password safety, phishing, or safe browsing?";
        }

        // Fallback response
        return "I'm not sure about that. Could you rephrase or ask about password safety, phishing, privacy, malware, or safe browsing?";
    }

    static void RunChatbot()
    {
        Console.Clear();
        new Logo();
        InitializeResponses();
        PlayGreetingAudio();

        Console.WriteLine("==================================================");
        TypeWriterEffect("Welcome to the Enhanced Cybersecurity Awareness Bot!", 30, ConsoleColor.Green);
        Console.WriteLine("==================================================");

        string namePrompt = "What is your name?";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Chatbot: {namePrompt}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("User: ");
        string userName = Console.ReadLine()?.Trim();
        Console.ResetColor();

        // Store the user's name in memory
        UserMemory["name"] = userName;

        TypeWriterEffect($"Nice to meet you, {userName}! I'll remember your name throughout our conversation.", 25, ConsoleColor.Green);
        TypeWriterEffect("I can help you with topics like password safety, phishing protection, safe browsing, privacy, and malware defense.", 25, ConsoleColor.Green);

        while (true)
        {
            string query = GetUserInput("What would you like to know about cybersecurity? (Type 'exit' to quit)", userName);

            if (string.IsNullOrWhiteSpace(query))
            {
                TypeWriterEffect("Please enter a valid question.", 25, ConsoleColor.Red);
                continue;
            }

            if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                string farewell = UserMemory.ContainsKey("favorite_topic")
                    ? $"Goodbye, {userName}! Remember to stay vigilant, especially regarding {UserMemory["favorite_topic"]}. Stay safe online!"
                    : $"Goodbye, {userName}! Stay safe online!";

                TypeWriterEffect(farewell, 25, ConsoleColor.Magenta);
                break;
            }

            string botResponse = ProcessUserQuery(query, userName);
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
