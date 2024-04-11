// See https://aka.ms/new-console-template for more information
using AccessLog;

Console.WriteLine("Журнал доступа");

var time_start = DateOnly.Parse("23.01.2024");
var time_end = new DateOnly(2024, 01, 24);
//var time_start = new DateTime(2024, 01, 25);
//var time_end = new DateTime(2024, 01, 25);
Console.WriteLine(time_start.ToString());
Console.WriteLine(time_end.ToString());
//string? address_start;
//string? address_mask;


var file1 = new TextFile("--file-log");
/*
file1.WriteingToFile("192.168.0.103:2024-01-23 08:10:00");
file1.WriteingToFile("192.168.0.107:2024-01-23 10:10:00");
file1.WriteingToFile("192.168.0.103:2024-01-24 10:15:00");
file1.WriteingToFile("192.168.0.107:2024-01-25 09:10:00");
file1.WriteingToFile("192.168.0.103:2024-01-25 09:40:00");
file1.WriteingToFile("192.168.0.106:2024-01-23 09:40:00");
file1.WriteingToFile("192.168.0.106:2024-01-24 10:40:00");
*/
Console.WriteLine(file1.ReadingFromFile());

var file2 = new TextFile("--file-output");

MyParser parser1 = new(time_start, time_end);
var requests = parser1.Parser(file1.ReadingFromFile());
var data = MyParser.ToDebugString(requests);
file2.WriteingToFile(data);
Console.WriteLine(file2.ReadingFromFile());