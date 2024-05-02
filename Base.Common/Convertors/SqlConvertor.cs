using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Base.Common.Convertors
{
    public static class SqlConvertor
    {
        public static string CommandToString(this SqlCommand sqlcommand)
        {
            string commandtext = string.Empty;
            switch (sqlcommand.CommandType)
            {
                case System.Data.CommandType.Text:
                    commandtext = sqlcommand.CommandText;
                    break;
                case System.Data.CommandType.StoredProcedure:
                    commandtext = sqlcommand.CommandText + "  ";
                    foreach (SqlParameter sqlParameter in sqlcommand.Parameters)
                    {
                        if (commandtext.Trim().Length > 0) commandtext += ",";
                        commandtext += sqlParameter.ParameterName + "=" + sqlParameter.Value.ToString();
                    }
                    break;
            };
            return commandtext;
        }
    }
}
