using System;
using System.IO;

namespace Vault
{
    class Program
    {
        static string curDir = Directory.GetCurrentDirectory();
        static string vaultDir = curDir + "\\Vault";
        static string dumpDir = curDir + "\\data";
        static string pass = vaultDir + "\\pass.txt";
        static void Main(string[] args)
        {
            //Check if the vault exists
            if (!Directory.Exists(vaultDir))
            {
                DirectoryInfo di = Directory.CreateDirectory(vaultDir);
                di.Attributes = FileAttributes.Hidden;
            }
            //Check if the vault dump exists
            if (!Directory.Exists(dumpDir))
            {
                DirectoryInfo di2 = Directory.CreateDirectory(dumpDir);
            }
            //Check to see if the user has a password
            if (!File.Exists(pass))
            {
                Console.Write("Please create a password: ");
                string password = Console.ReadLine();
                File.WriteAllText(pass, password);
                DirectoryInfo di = new DirectoryInfo(pass);
                di.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly;
            }
            else
            {
                Verify();
            }
            //Procced to the main menu
            Menu();
        }

        static void Menu()
        {
            //Selection Integer
            int select;
            //Greeting
            Console.Clear();
            Console.WriteLine("Welcome to the Vault manager!\n\nWhat would you like to do today?");
            Console.WriteLine("1 - Import files to the Vault\n2 - Show the vault\n3 - list the items in the vault\n4 - Nuke the Vault");
            select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    Import();
                    break;
                case 2:
                    Show();
                    break;
                case 3:
                    List();
                    break;
                case 4:
                    Nuke();
                    break;
            }
        }
        //Verify user password
        static void Verify()
        {
            Console.Write("Please enter your password: ");
            if (Console.ReadLine() != File.ReadAllText(pass))
            {
                Console.WriteLine("Incorrect password, please try again.");
                Verify();
            }
        }
        //Import items from data to the Vault
        static void Import()
        {
            Console.Clear();
            Console.WriteLine("Make sure you have all the files you wish to import placed in the data folder.\nPress Enter to continue...");
            Console.ReadLine();
            string[] files = Directory.GetFiles(dumpDir);
            foreach (string s in files)
            {
                string fileName = Path.GetFileName(s);
                string destFile = Path.Combine(vaultDir, fileName);
                File.Move(s, destFile);
            }
            Console.WriteLine("Finished!\nPress Enter to continue...");
            Console.ReadLine();
            Menu();
        }
        //Show the Vault to the user
        static void Show()
        {
            Console.Clear();
            Console.WriteLine("You are about to reveal the Vault.\nDo you wish to continue? (y/n)");
            string confirm = Console.ReadLine();
            if (confirm != "y")
            {
                Console.WriteLine("Task aborted.\nPress Enter to continue...");
                Console.ReadLine();
                Menu();
            } else
            {
                DirectoryInfo di = new DirectoryInfo(vaultDir);
                di.Attributes = FileAttributes.Normal;
                Console.Clear();
                Console.WriteLine("The Vault has been revealed.\nPress Enter to hide the vault again and return to the menu.");
                Console.ReadLine();
                di.Attributes = FileAttributes.Hidden;
                Menu();
            }
        }
        //List the items in the vault
        static void List()
        {
            Console.Clear();
            string[] files = Directory.GetFiles(vaultDir);
            Console.WriteLine("There are " + (files.Length - 1) + " files in the Vault.");
            foreach (string s in files)
            {
                string fileName = Path.GetFileName(s);
                if (fileName != "pass.txt")
                {
                    Console.WriteLine(fileName);
                }
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Menu();
        }
        //Nuke the vault
        static void Nuke()
        {
            Console.Clear();
            Console.WriteLine("You are about to wipe the vault.\nThe password for the vault will remain.\nAre you sure you want to continue?");
            if (Console.ReadLine() != "y")
            {
                Console.WriteLine("Task aborted.\nPress Enter to continue...");
                Console.ReadLine();
                Menu();
            } else
            {
                string[] files = Directory.GetFiles(vaultDir);
                foreach (string s in files)
                {
                    try
                    {
                        File.Delete(s);
                    }
                    catch
                    {

                    }
                }
                Console.Clear();
                Console.WriteLine("The Vault has been nuked.\nThe application will now close.\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
