using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Encrpytion_CLI_Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] MenuSelections = { "Encrypt", "Decrypt", "Information" };
            int selectionID = 0;
            int NewID = 0;
            bool selecting = true;
            while (selecting)
            {
                Menu(selectionID,MenuSelections);
                ConsoleKey key = Console.ReadKey().Key;
                NewID = IDCalculation(selectionID, key);
                //Console.WriteLine(selectionID);
                //Console.WriteLine(NewID);
                if(selectionID == NewID)
                {
                    if(key == ConsoleKey.Spacebar)
                    {
                        switch (NewID)
                        {
                            case 0:
                                //Console.WriteLine("Selected First Item");
                                Encrypt();
                                break;
                            case 1:
                                //Console.WriteLine("Selected Second Item");
                                Decrypt();
                                break;
                            case 2:
                                //Console.WriteLine("Selected Third Item");
                                Info();
                                break;
                        }
                        //Console.WriteLine("End Switch");
                        //Console.ReadLine();
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        Environment.Exit(9999999);
                    }
                }
                selectionID = NewID;
            }
        }

        static void Title()
        {
            var originColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("█████   ████ ████                       ██            ██████████                                                     █████                          █████   █████    ████        █████    ");
            Console.WriteLine("░░███   ███░ ░░███                      ███           ░░███░░░░░█                                                    ░░███                          ░░███   ░░███    ░░███      ███░░░███ ");
            Console.WriteLine(" ░███  ███    ░███  █████ ████  ██████ ░░░   █████     ░███  █ ░  ████████    ██████  ████████  █████ ████ ████████  ███████    ██████  ████████     ░███    ░███     ░███     ███   ░░███");
            Console.WriteLine(" ░███████     ░███ ░░███ ░███  ███░░███     ███░░      ░██████   ░░███░░███  ███░░███░░███░░███░░███ ░███ ░░███░░███░░░███░    ███░░███░░███░░███    ░███    ░███     ░███    ░███    ░███");
            Console.WriteLine(" ░███░░███    ░███  ░███ ░███ ░███████     ░░█████     ░███░░█    ░███ ░███ ░███ ░░░  ░███ ░░░  ░███ ░███  ░███ ░███  ░███    ░███ ░███ ░███ ░░░     ░░███   ███      ░███    ░███    ░███");
            Console.WriteLine(" ░███ ░░███   ░███  ░███ ░███ ░███░░░       ░░░░███    ░███ ░   █ ░███ ░███ ░███  ███ ░███      ░███ ░███  ░███ ░███  ░███ ███░███ ░███ ░███          ░░░█████░       ░███    ░░███   ███ ");
            Console.WriteLine(" █████ ░░████ █████ ░░███████ ░░██████      ██████     ██████████ ████ █████░░██████  █████     ░░███████  ░███████   ░░█████ ░░██████  █████           ░░███      ██ █████ ██ ░░░█████░  ");
            Console.WriteLine("░░░░░   ░░░░ ░░░░░   ░░░░░███  ░░░░░░      ░░░░░░     ░░░░░░░░░░ ░░░░ ░░░░░  ░░░░░░  ░░░░░       ░░░░░███  ░███░░░     ░░░░░   ░░░░░░  ░░░░░             ░░░      ░░ ░░░░░ ░░    ░░░░░░   ");
            Console.WriteLine("                     ███ ░███                                                                    ███ ░███  ░███                                                                           ");
            Console.WriteLine("                    ░░██████                                                                    ░░██████   █████                                                                          ");
            Console.WriteLine("                     ░░░░░░                                                                      ░░░░░░   ░░░░░                                                                           ");
            Console.ForegroundColor = originColor;
            Console.WriteLine("Tool made by Klye-Projects");
        }
        static void Menu(int ID,string[] MenuItems)
        {
            Console.Clear();
            Title();
            for (int i = 0; i < MenuItems.Length; i++)
            {
                if(i != ID)
                {
                    Console.WriteLine("  " + MenuItems[i]);
                }
                else
                {
                    Console.WriteLine("> " + MenuItems[i]);
                }
            }
        }

        static int IDCalculation(int CurrentID, ConsoleKey Key)
        {
            if(Key == ConsoleKey.UpArrow && CurrentID != 0)
            {
                CurrentID -= 1;
            }
            else if (Key == ConsoleKey.DownArrow && CurrentID != 2)
            {
                CurrentID += 1;
            }
            return CurrentID;
        }
        static void EndingCatch()
        {
            Console.WriteLine("Please Press Enter To Continue");
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.ReadLine();
        }

        static void Encrypt()
        {
            Console.WriteLine("Please Define File To Be Encrypted");
            string Directory = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please Defind A Encryption Key");
            bool GettingKey = true;
            string Key = "";
            while (GettingKey)
            {
                Key = Console.ReadLine();
                if (Key.Length == 8)
                {
                    GettingKey = false;
                }
                else
                {
                    Console.WriteLine("This is " + Key.Length + " bytes");
                }

            }
            try
            {


                byte[] plainContent = File.ReadAllBytes(Directory);
                using (var DES = new DESCryptoServiceProvider())
                {
                    DES.IV = Encoding.UTF8.GetBytes(Key);
                    DES.Key = Encoding.UTF8.GetBytes(Key);
                    DES.Mode = CipherMode.CBC;
                    DES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptostream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);

                        cryptostream.Write(plainContent, 0, plainContent.Length);
                        cryptostream.FlushFinalBlock();
                        File.WriteAllBytes(Directory, memStream.ToArray());
                        Console.WriteLine("Encryption Complete");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Uh-oh it broken");
                Environment.Exit(101);
                Console.WriteLine(e);
            }
            EndingCatch();

        }
        static void Decrypt()
        {
            Console.WriteLine("Please Define File To Be Encrypted");
            string Directory = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please Defind A Encryption Key");
            bool GettingKey = true;
            string Key = "";
            while (GettingKey)
            {
                Key = Console.ReadLine();
                if (Key.Length == 8)
                {
                    GettingKey = false;
                }
                else
                {
                    Console.WriteLine("This is " + Key.Length + " bytes");
                }

            }
            try
            {
                byte[] encrypted = File.ReadAllBytes(Directory);
                using (var DES = new DESCryptoServiceProvider())
                {
                    DES.IV = Encoding.UTF8.GetBytes(Key);
                    DES.Key = Encoding.UTF8.GetBytes(Key);
                    DES.Mode = CipherMode.CBC;
                    DES.Padding = PaddingMode.PKCS7;

                    using (var memStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(encrypted, 0, encrypted.Length);
                        cryptoStream.FlushFinalBlock();
                        File.WriteAllBytes(Directory, memStream.ToArray());
                        Console.WriteLine("Successfully Decrypted");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Uh Oh, Its broken");
                Environment.Exit(201);
                Console.WriteLine(e);
            }
            EndingCatch();
        }
        static void Info()
        {
            Console.WriteLine("Tool Made By Klye");
            Console.WriteLine("Security process powered by windows security");
            Console.WriteLine("");
            Console.WriteLine("This tool should be used for security use only");
            Console.WriteLine("any malicious actions with this tool is not tollerated");
            Console.WriteLine("This tool doesn't save keys or file locations");
            Console.WriteLine("so use this tool at your discretion");
            Console.WriteLine("Klye Projects is not liable to any lost files");
            Console.WriteLine("");
            EndingCatch();
        }
    }
}
