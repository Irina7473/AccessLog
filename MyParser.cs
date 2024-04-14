using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccessLog
{    
    class MyParser
    {       
        public DateOnly time_start;
        public DateOnly time_end;
        public string? address_start;
        public string? address_mask;
        public string? address_end;

        public MyParser(DateOnly time_start, DateOnly time_end) 
        {
            this.time_start = time_start;
            this.time_end = time_end;
        }

        public MyParser(DateOnly time_start, DateOnly time_end, string? address_start) :
            this(time_start, time_end)
        {           
            this.address_start = address_start;
        }

        public MyParser(DateOnly time_start, DateOnly time_end, string? address_start, string? address_mask) :
            this (time_start, time_end, address_start)
        {            
            this.address_mask = address_mask;
            if (address_start!=null && address_mask!=null)
                this.address_end = AddressEnd(address_start, address_mask);            
        }

        private string AddressEnd(string address_start, string address_mask)
        {            
            UInt32 ipInt = AddressFromStringToInt(address_start);
            UInt32 subnetMask = UInt32.MaxValue << (32 - int.Parse(address_mask));
            UInt32 network = ipInt & subnetMask;
            UInt32 broadcast = network | ~subnetMask;
            UInt32 addressEnd = broadcast - 1;

            return ipFromIntToString(addressEnd);
        }

        UInt32 AddressFromStringToInt(string address)
        {
            UInt32 addressInt = 0;
            foreach (string part in address.Split('.'))
            {
                addressInt <<= 8;
                addressInt |= UInt32.Parse(part);
            }
            return addressInt;
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
                //Проверка адреса
                if (!ChekAddress(address)) continue;

                var log_datetime = line.Substring(index1 + 1, index2);
                DateOnly log_date;
                if (DateOnly.TryParse(log_datetime, out log_date))
                {
                    //Проверка даты доступа
                    if (log_date >= time_start && log_date <= time_end)
                    {
                        if (requests.ContainsKey(address)) requests[address]++;
                        else requests.Add(address, 1);
                    }
                }    
            }
            return requests;
        }

        public string ipFromIntToString(UInt32 intValue)
        {
            return ((intValue >> 24) & 255).ToString() + "." + ((intValue >> 16) & 255).ToString() +
                "." + ((intValue >> 8) & 255).ToString() + "." + (intValue & 255).ToString();
        }

        private bool ChekAddress(string address)
        {
            if (this.address_start == null) return true;
            else
            {
                if (System.String.Compare(address, this.address_start) < 0) return false;
                else {
                if (this.address_mask == null || System.String.Compare(address, this.address_end) <= 0) return true;
                else return false;
                }
            }            
        }

        public static string ToDebugString(Dictionary<string, int> dictionary)
        {
            return string.Join("\n", dictionary.Select(kv => kv.Key + " - " + kv.Value).ToArray());
        }

    }
}
