using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace News
{
    public class XmlValueParser
    {
        public static bool TryParseString(XElement node, out string result, string default_value = "")
        {
            if (node == null)
            {
                result = default_value;
                return false;
            }
            else
            {
                result = node.Value;
                return true;
            }
        }

        public static bool TryParseInteger(XElement node, out int result, int default_value = 0)
        {
            if (node == null)
            {
                result = default_value;
                return false;
            }
            else
            {
                int tmp_int = 0;
                bool parse_complete = int.TryParse(node.Value, out tmp_int);

                if (parse_complete)
                {
                    result = tmp_int;
                }
                else
                {
                    result = default_value;
                }

                return parse_complete;
            }
        }

        public static bool TryParseDouble(XElement node, out double result, double default_value = 0)
        {
            if (node == null)
            {
                result = default_value;
                return false;
            }
            else
            {
                double tmp_double = 0;
                bool parse_complete = double.TryParse(node.Value, out tmp_double);

                if (parse_complete)
                {
                    result = tmp_double;
                }
                else
                {
                    result = default_value;
                }

                return parse_complete;
            }
        }

        //----------

        public static string ParseString(XElement node, string default_value = "")
        {
            if (node == null)
            {
                return default_value;
            }
            else
            {
                return node.Value;
            }
        }

        public static int ParseInteger(XElement node, int default_value = 0)
        {
            if (node == null)
            {
                return default_value;
            }
            else
            {
                int tmp_int = 0;
                bool parse_complete = int.TryParse(node.Value, out tmp_int);
                if (parse_complete)
                {
                    return tmp_int;
                }
                else
                {
                    return default_value;
                }
            }
        }

        public static double ParseDouble(XElement node, double default_value = 0)
        {
            if (node == null)
            {
                return default_value;
            }
            else
            {
                double tmp_double = 0;
                bool parse_complete = double.TryParse(node.Value, out tmp_double);
                if (parse_complete)
                {
                    return tmp_double;
                }
                else
                {
                    return default_value;
                }
            }
        }

        public static bool ParseBool(XElement node, bool default_value = false)
        {
            if (node == null)
            {
                return default_value;
            }
            else
            {
                bool tmp_bool = false;
                bool parse_complete = bool.TryParse(node.Value, out tmp_bool);
                if (parse_complete)
                {
                    return tmp_bool;
                }
                else
                {
                    return default_value;
                }
            }
        }
    }
}
