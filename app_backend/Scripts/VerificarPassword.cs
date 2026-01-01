using System;

class Program
{
    static void Main()
    {
        string password = "admin123";
        string hash = "$2a$11$8K1p/a0dL3LHR/nHkfuBiOCEZZ8QeKhQkrXfzIU4OqgnE0.jjKZ6e";
        
        bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
        
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Hash: {hash}");
        Console.WriteLine($"Is Valid: {isValid}");
        
        // Generar nuevo hash
        string newHash = BCrypt.Net.BCrypt.HashPassword(password, 11);
        Console.WriteLine($"New Hash: {newHash}");
    }
}

