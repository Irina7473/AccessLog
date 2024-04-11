// See https://aka.ms/new-console-template for more information
using AccessLog;

Console.WriteLine("Журнал доступа");

var file1 = new TextFile("--file-log");
/*
file1.WriteingToFile("192.168.0.104:2024-01-25 08:10:00");
file1.WriteingToFile("192.168.0.104:2024-01-25 10:10:00");
file1.WriteingToFile("192.168.0.104:2024-01-25 10:15:00");
file1.WriteingToFile("192.168.0.105:2024-01-25 09:10:00");
file1.WriteingToFile("192.168.0.104:2024-01-25 09:40:00");
file1.WriteingToFile("192.168.0.104:2024-01-25 09:40:00");
file1.WriteingToFile("192.168.0.104:2024-01-25 10:40:00");
*/
Console.WriteLine(file1.ReadingFromFile());

var file2 = new TextFile("--file-output");
file2.WriteingToFile("пусто");
Console.WriteLine(file2.ReadingFromFile());
