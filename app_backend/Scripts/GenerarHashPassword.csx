#!/usr/bin/env dotnet-script
#r "nuget: BCrypt.Net-Next, 4.0.3"

using BCrypt.Net;

string password = "admin123";
string hash = BCrypt.HashPassword(password, 11);

Console.WriteLine($"Password: {password}");
Console.WriteLine($"Hash: {hash}");

// Verificar que el hash funciona
bool isValid = BCrypt.Verify(password, hash);
Console.WriteLine($"Verification: {isValid}");

