// See https://aka.ms/new-console-template for more information
using AccessLog;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

Console.WriteLine("Журнал доступа");

//Передача параметров через аргументы командной строки
/*
string[] theArgs = Environment.GetCommandLineArgs();
foreach (string arg in theArgs)
{
    Console.WriteLine(arg);
}

var nameInput = theArgs[0];
var nameOutput = theArgs[1];
if (string.IsNullOrEmpty(nameInput) || string.IsNullOrEmpty(nameOutput))
{
    Console.WriteLine("Не передано название файла (ов)");
    return;
}
var fileInput = new TextFile(nameInput);
var fileOutput = new TextFile(nameOutput);

DateOnly time_start;
DateOnly time_end;
if (!DateOnly.TryParse(theArgs[4], out time_start) || !DateOnly.TryParse(theArgs[5], out time_end))
{
    Console.WriteLine("Неправильно заданы границы временного интервала ");
    return;
}
if (time_start > time_end)
{
    Console.WriteLine("Нижняя граница временного интервала больше верхней");
    return;
}

string? address_start = theArgs[4];
string? address_mask = theArgs[5];
if (address_start != null)
    if (!ValidateAddress(address_start))
    {
        Console.WriteLine("Неправильно задана нижняя граница диапазона адресов");
        return;
    }
if (address_mask != null)
    if (!ValidateMask(address_mask))
    {
        Console.WriteLine("Неправильно задана маска подсети");
        return;
    }
if (address_start == null && address_mask != null)
{
    Console.WriteLine("Mаску подсети, задающую верхнюю границу диапазона, нельзя использовать, " +
        "если не задана нижняя граница диапазона адресов");
    return;
}
*/

//Передача параметров через файл конфигурации

// Создаю объект конфигурации, используя JSON.
IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile("parameters.json")
        .AddEnvironmentVariables()
        .Build();

var nameInput = config["input_file"];
var nameOutput = config["output_file"];
if (string.IsNullOrEmpty(nameInput) || string.IsNullOrEmpty(nameOutput))
{
    Console.WriteLine("Не передано название файла (ов)");
    return;
}

var fileInput = new TextFile(nameInput);
var fileOutput = new TextFile(nameOutput);

DateOnly time_start;
DateOnly time_end;
if (!DateOnly.TryParse(config["parser:time_start"], out time_start) || !DateOnly.TryParse(config["parser:time_end"], out time_end))
{
    Console.WriteLine("Неправильно заданы границы временного интервала ");
    return;
}
if (time_start > time_end)
{
    Console.WriteLine("Нижняя граница временного интервала больше верхней");
    return;
}

string? address_start = config["parser:address_start"];
string? address_mask = config["parser:address_mask"];
if (address_start != null)
    if (!ValidateAddress(address_start))
    {
        Console.WriteLine("Неправильно задана нижняя граница диапазона адресов");
        return;
    }
if (address_mask != null)
    if (!ValidateMask(address_mask))
    {
        Console.WriteLine("Неправильно задана маска подсети");
        return;
    }
if (address_start == null && address_mask != null)
{
    Console.WriteLine("Mаску подсети, задающую верхнюю границу диапазона, нельзя использовать, " +
            "если не задана нижняя граница диапазона адресов");
    return;
}
   

//Объект для работы с входными параметрами
MyParser parser = new(time_start, time_end, address_start, address_mask);
//Проверка запросов из входного файла
var requests = parser.Parser(fileInput.ReadingFromFile());
var data = MyParser.ToDebugString(requests);
data = "За период " + time_start + "-" + time_end + "\nзафиксировано количество обращений с IP-адресов :\n" + data;
fileOutput.WriteingToFile(data);
Console.WriteLine(fileOutput.ReadingFromFile());

//Валидация маски подсети
bool ValidateMask(string mask)
{
    int maskInt;
    if (!int.TryParse(mask, out maskInt) || maskInt < 0 || maskInt > 32) return false;
    return true;
}
//Валидация ip-адреса
bool ValidateAddress(string address)
{
    string[] ipParts = address.Split('.');
    if (ipParts.Length != 4) return false;
    foreach (string partString in ipParts)
    {
        int partInt;
        if (!int.TryParse(partString, out partInt) || partInt < 0 || partInt > 255) return false;
    }
    return true;
}