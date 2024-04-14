// See https://aka.ms/new-console-template for more information
using AccessLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

Console.WriteLine("Журнал доступа");

var file1 = new TextFile("--file-log");
Console.WriteLine(file1.ReadingFromFile());
var file2 = new TextFile("--file-output");

var time_start = DateOnly.Parse("25.01.2024");
var time_end = DateOnly.Parse("25.01.2024");
string? address_start = "196.68.1.104";
string? address_mask = "27";

MyParser parser1 = new(time_start, time_end, address_start, address_mask);
var requests = parser1.Parser(file1.ReadingFromFile());
var data = MyParser.ToDebugString(requests);
data = "За период " + time_start + "-" + time_end + "\nзафиксировано количество обращений с IP-адресов :\n" + data;
file2.WriteingToFile(data);
Console.WriteLine(file2.ReadingFromFile());
