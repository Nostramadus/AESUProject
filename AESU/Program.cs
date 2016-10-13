using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace AESU
{
    class Program
    {
        static void Main(string[] args)
        {
            string coding;
            string key;
            string iv = null;
            string mode;
            string inputPath;
            string outputPath;
            bool ivProvided;

            // help variables only needed for CTR
            string encrIV;
            string ciphertext = null;
            string plaintext;

            // checking for encryption and decryption
            switch (args[0])
            {
                case "-e":
                    coding = args[0];
                    Console.WriteLine("Encoding ok.");
                    break;
                case "-d":
                    coding = args[0];
                    Console.WriteLine("Decoding ok.");
                    break;
                default:
                    Console.WriteLine("ERROR: Please enter -e or -d for encryption or decryption nothing else (Check the Syntax in the README)");
                    return;
            }

            //checking if the correct keylength is provided
            if (args[2].Length != 32 && args[2].Length != 48 && args[2].Length != 64)
            {
                Console.WriteLine("key length not supported. (Check the Syntax in the README)");
                return;
            }
            else
            {
                key = args[2];
                Console.WriteLine("Keylength ok.");
            }

            //Checking if an IV-Key is provided and if the command is correct structured
            if (args[3] == "-i")
            {
                ivProvided = true;
                if (args[5] != "-m")
                {
                    Console.WriteLine("ERROR: Please provide a -m, for the mode (Check the Syntax in the README)");
                    return;
                }
                if (args[7] != "-in")
                {
                    Console.WriteLine("ERROR: Please provide -in for the inputfile  (Check the Syntax in the README)");
                    return;
                }
                if (args[9] != "-out")
                {
                    Console.WriteLine("ERROR: Please provide -out for the outputfile (Check the Syntax in the README)");
                    return;
                }
                iv = args[4];
                mode = args[6];
                inputPath = args[8];
                outputPath = args[10];
                Console.WriteLine("IV, mode, inputpath, outpath ok");
            }
            else if (args[3] == "-m")
            {
                ivProvided = false;
                if (args[5] != "-in")
                {
                    Console.WriteLine("ERROR: Please prive -in for the inputfile  (Check the Syntax in the README)");
                    return;
                }
                if (args[7] != "-out")
                {
                    Console.WriteLine("ERROR: Please provide -out for the outputfile (Check the Syntax in the README)");
                    return;
                }
                mode = args[4];
                inputPath = args[6];
                outputPath = args[8];
                Console.WriteLine("mode, inputpath, outpath ok");
            }
            else
            {
                Console.WriteLine("ERROR: Wrong input command. (Check the Syntax in the README)");
                return;
            }

            if (args[1] != "-k")
            {
                Console.WriteLine("ERROR: Please provide -k, because we need a key (Check the Syntax in the README)");
                return;
            }

            //Checking which mode is called
            switch (mode)
            {
                case "ecb":
                    Console.WriteLine("The mode is ok, it's ecb");
                    break;
                case "cbc":
                    Console.WriteLine("The mode is ok, it's cbc");
                    break;
                case "cfb":
                    Console.WriteLine("The mode is ok, it's cfb");
                    break;
                case "ofb":
                    Console.WriteLine("The mode is ok, it's ofb");
                    break;
                case "ctr":
                    Console.WriteLine("The mode is ok, it's ctr");
                    break;
                default:
                    Console.WriteLine("ERROR: Please enter a mode for encryption or decryption (Check the Syntax in the README)");
                    return;
            }

            if (mode != "ctr")
            {
                if (!ivProvided)
                {
                    // process to do the encryption through Openssl
                    Process process = new Process();
                    process.StartInfo.FileName = "openssl";
                    process.StartInfo.Arguments = " enc " + "-aes-" + (key.Length * 4) + "-" + mode + " " + coding + " " + " -K " + key + " -v -nosalt " + " " +
                        args[5] + " " + inputPath + " " + args[7] + " " + outputPath;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                }
                    //only if the initial Vector is provided
                else
                {
                    // process to do the encryption through Openssl
                    Process process = new Process();
                    process.StartInfo.FileName = "openssl";
                    process.StartInfo.Arguments = " enc " + "-aes-" + (key.Length * 4) + "-" + mode + " " + coding + " " + " -K " + key + " -iv "+ iv +" -v -nosalt " +
                        args[7] + " " + inputPath + " " + args[9] + " " + outputPath;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                }
            }
            // only when the mode is ctr
            else
            {

                // Creating a help file
                writing("a"+outputPath, iv);

                // reading the plaintext in a variable
                plaintext = reading(inputPath);

                // process to do the encryption through Openssl
                Process process = new Process();
                process.StartInfo.FileName = "openssl";
                process.StartInfo.Arguments = " enc " + "-aes-" + (key.Length * 4) + "-ecb -e -K " + key +" -v -nopad -nosalt " + " " +
                    args[7] + " " + "a"+outputPath + " " + args[9] + " " + outputPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                process.Close();

                // read the encrypted Initial Vector in the variable
                encrIV = reading(outputPath);

                // XOR the encrypted IV with the plaintext to get the ciphertext
                for (int i = 0; i < plaintext.Length; i++)
                {
                    ciphertext += (char)(encrIV[i % encrIV.Length] ^ plaintext[i]);
                }
                //writing the ciphertext to the outputphile
                writing(outputPath, ciphertext);

                //deleting the help file
                File.Delete("a" + outputPath);
            }


            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadLine();
        }



        // Reading the text from the textdocument provided, returning the conent as string
        public static string reading(string path)
        {
            string msg = "";
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(path);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    msg += line;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return msg;
        }


        // writing in the text document path provided
        public static void writing(string path, string message)
        {
            StreamWriter wr = null;
            try
            {
                wr = new StreamWriter(path);
                wr.WriteLine(message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                if (wr != null)
                {
                    wr.Close();
                }
            }
        }
    }
}