using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AccessLog
{
    class MyParser
    {
        //Дата в журнале доступа записана в формате: yyyy-MM-dd HH:mm:ss  now:u
        //Даты в параметрах задаются в формате dd.MM.yyyy  now.ToString("d")
        public DateOnly time_start;
        public DateOnly time_end;
        public string? address_start;
        public string? address_mask;

        public MyParser(DateOnly time_start, DateOnly time_end) 
        {
            this.time_start = time_start;
            this.time_end = time_end;
        }

        public MyParser(DateOnly time_start, DateOnly time_end, string address_start) :
            this(time_start, time_end)
        {           
            this.address_start = address_start;
        }

        public MyParser(DateOnly time_start, DateOnly time_end, string address_start, string address_mask) :
            this (time_start, time_end, address_start)
        {            
            this.address_mask = address_mask;
        }


        public Dictionary<string, int> Parser (string input)
        {
            Dictionary<string, int> requests = new();

            string[] separator = { Environment.NewLine };
            string[] lines = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                int index1 = line.IndexOf(":");
                int index2 = line.IndexOf(" ")-index1-1;
                var address = line.Substring(0, index1);
                var log_datetime = line.Substring(index1 + 1, index2);
                DateOnly log_date;
                if (DateOnly.TryParse(log_datetime, out log_date))
                {
                    if (log_date >= time_start && log_date <= time_end)
                    {
                        if (requests.ContainsKey(address)) requests[address]++;
                        else requests.Add(address, 1);
                    }
                }    
            }
            return requests;
        }

        public static string ToDebugString(Dictionary<string, int> dictionary)
        {
            return string.Join("\n", dictionary.Select(kv => kv.Key + " - " + kv.Value).ToArray());
        }

    }
}
