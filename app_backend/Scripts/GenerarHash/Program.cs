using System;

class Program
{
    static void Main()
    {
        string password = "admin123";
        
        // Generar hash con BCrypt
        string hash = BCrypt.Net.BCrypt.HashPassword(password, 11);
        
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Hash generado: {hash}");
        Console.WriteLine();
        
        // Verificar el hash actual de la DB
        string hashActual = "$2a$11$8K1p/a0dL3LHR/nHkfuBiOCEZZ8QeKhQkrXfzIU4OqgnE0.jjKZ6e";
        bool esValido = BCrypt.Net.BCrypt.Verify(password, hashActual);
        
        Console.WriteLine($"Hash actual en DB: {hashActual}");
        Console.WriteLine($"¿Es válido el hash actual?: {esValido}");
        Console.WriteLine();
        
        // Verificar el nuevo hash
        bool nuevoEsValido = BCrypt.Net.BCrypt.Verify(password, hash);
        Console.WriteLine($"¿Es válido el nuevo hash?: {nuevoEsValido}");
    }
}

