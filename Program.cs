using System;
using System.IO;
using System.Text;

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";
        lines = File.ReadAllLines(filePath);

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines);
                    break;
                case "3":
                    LevelUpCharacter(lines);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = ParseCsvLine(lines[i]);
            if (fields.Length >= 5)
            {
                string name = fields[0];
                string characterClass = fields[1];
                int level = int.Parse(fields[2]);
                int hitPoints = int.Parse(fields[3]);
                string[] equipment = fields[4].Split('|');

                Console.WriteLine($"Name: {name}, Class: {characterClass}, Level: {level}, HP: {hitPoints}, Equipment: {string.Join(", ", equipment)}");
            }
        }
    }

    static void AddCharacter(ref string[] lines)
    {
        Console.Write("Enter character name: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Enter character class: ");
        string? characterClass = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(characterClass))
        {
            Console.WriteLine("Class cannot be empty.");
            return;
        }

        Console.Write("Enter character level: ");
        if (!int.TryParse(Console.ReadLine(), out int level))
        {
            Console.WriteLine("Invalid level format.");
            return;
        }

        Console.Write("Enter character hit points: ");
        if (!int.TryParse(Console.ReadLine(), out int hitPoints))
        {
            Console.WriteLine("Invalid hit points format.");
            return;
        }

        Console.Write("Enter character equipment (separated by '|'): ");
        string? equipment = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(equipment))
        {
            Console.WriteLine("Equipment cannot be empty.");
            return;
        }

        string newLine = $"{(name.Contains(',') ? $"\"{name}\"" : name)},{characterClass},{level},{hitPoints},{equipment}";
        Array.Resize(ref lines, lines.Length + 1);
        lines[^1] = newLine;

        File.WriteAllLines("input.csv", lines);
        Console.WriteLine("Character added successfully.");
    }

    static void LevelUpCharacter(string[] lines)
    {
        Console.Write("Enter the name of the character to level up: ");
        string? nameToLevelUp = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nameToLevelUp))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        bool characterFound = false;
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = ParseCsvLine(lines[i]);
            if (fields[0].Equals(nameToLevelUp, StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(fields[2], out int level))
                {
                    Console.WriteLine("Invalid level format in file.");
                    return;
                }

                level++;
                Console.WriteLine($"Character {fields[0]} leveled up to level {level}!");

                fields[2] = level.ToString();
                lines[i] = string.Join(",", fields);

                File.WriteAllLines("input.csv", lines);
                characterFound = true;
                break;
            }
        }

        if (!characterFound)
        {
            Console.WriteLine("Character not found.");
        }
    }

    static string[] ParseCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        StringBuilder field = new StringBuilder();

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(field.ToString());
                field.Clear();
            }
            else
            {
                field.Append(c);
            }
        }
        result.Add(field.ToString());

        return result.ToArray();
    }
}
