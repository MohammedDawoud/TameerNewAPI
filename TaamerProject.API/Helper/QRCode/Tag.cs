using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace  TaamerProject.API.Helper
{
    public class Tag
    {
        int tag;
        string value;

        public Tag(int tag,string value)
        {
            this.tag = tag;
            this.value = value;
        }
        public int getTag()
        {
            return this.tag;
        }
        public string getValue()
        {
            return this.value;
        }
        public int getLength()
        {
            return ((this.ToHexString(this.value).Length)/2);
            //return this.value.Length;
        }
        //public string toHex(int value)
        //{
        //    string hex = string.Format("%02X", value);
        //    string input = hex.Length % 2 == 0 ? hex : hex + "0";
        //    StringBuilder output = new StringBuilder();
        //    for (int i = 0; i < input.Length; i += 2)
        //    {
        //        string str = input.Substring(i, i + 2);
        //        output.Append((char)Convert.ToInt32(str, 16));
        //    }
        //    return output.ToString();
        //}
        public string toHex_V(string value)
        {
            byte[] ba = Encoding.UTF8.GetBytes(value);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
        public string toHex_D(int value)
        {
            string hexValue = value.ToString("X2");
            return hexValue;
        }
        public string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.UTF8.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public string StrToHex(string input)
        {
            StringBuilder s = new StringBuilder();
            foreach (char chr in input)
            {
                s.Append(Convert.ToString(chr, 16).PadLeft(4,'-'));
            }
            string Value2 = s.ToString().Replace("-", "");
            int length = Value2.ToString().Length;
            if (length % 2 == 0)
            {}
            else
            {
                Value2 = Value2.Remove(Value2.Length - 1, 1);
            }
            return Value2;
        }
        public  string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public string StrToHexNEW(string value)
        {
            byte[] utfBytes = System.Text.Encoding.UTF8.GetBytes(value);
            string hex = BitConverter.ToString(utfBytes).Replace("-", "");
            return hex;
        }
        //reverse
        public  byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public string ToBinary(byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
        public byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public string strTLV()
        {
            byte[] ba_Tag = Encoding.UTF8.GetBytes((this.getTag()).ToString());
            string v_Tag = Encoding.UTF8.GetString(ba_Tag);

            byte[] ba_Length = Encoding.UTF8.GetBytes((this.getLength()).ToString());
            string v_Length = Encoding.UTF8.GetString(ba_Length);

            byte[] ba_Value = Encoding.UTF8.GetBytes((this.getValue()));
            string v_Value = Encoding.UTF8.GetString(ba_Value);

            //try
            //{
            //    var TestValue1 = StrToHexNEW(this.getValue());
            //    var TestValue2 = StrToHexNEW(v_Value);
            //    var TestValue3_4 = this.StrToHex(this.getValue());
            //    var TestValue3_5= this.StrToHex(v_Value);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            

            var hexavalue = this.toHex_D(Convert.ToInt32(v_Tag)) + this.toHex_D(Convert.ToInt32(v_Length)) + this.ToHexString(v_Value);
            //var aa = this.ToHexString(v_Value);
            return hexavalue;
        }

    }
}