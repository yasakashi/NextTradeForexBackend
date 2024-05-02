using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Generator
{
    /// <summary>
    /// Generate Unique Token
    /// </summary>
    public class RequestTokenGenerator
    {
        public static string Generate(List<string> Items)
        {
            string contactitem = string.Empty;
            foreach (string item in Items)
            {
                if (contactitem.Length > 0) contactitem += ":";
                contactitem += item;
            }

            string autorizationcode = Convert.ToBase64String(Encoding.UTF8.GetBytes(contactitem));
            return autorizationcode;
        }

        public static List<string> Dedeserialize(string Item)
        {
            string contactitem = string.Empty;
            contactitem = Encoding.UTF8.GetString(Convert.FromBase64String(Item));
            List<string> ValueList = new List<string>();
            string[] Items = contactitem.Split(':');
            
            foreach (string item in Items)
            {
                ValueList.Add(item);
            }
            
            return ValueList;
        }
    }
}

