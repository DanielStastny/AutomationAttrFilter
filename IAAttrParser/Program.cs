using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace IAAttrParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IA XML Reparser -- filters out nonlisted attributes");
            Console.WriteLine("2015 | daniel@dstastny.com");
            Console.WriteLine("dictionary.txt and IA .xml should be enclosed in the same dir with this app");
            Console.WriteLine();
            Console.Write("Write whole name of the xml file to reparse: ");

            string currentDir = Directory.GetCurrentDirectory();
            string iaFilename = Console.ReadLine();
            string iaFilePath = currentDir + "\\" + iaFilename;

            string dictionaryPath = currentDir + "\\dictionary.txt";

            if (File.Exists(iaFilePath))
            {
                // read dictionary
                string[] attributes = File.ReadAllLines(dictionaryPath);

                // read ia xml
                string fileContent = "";
                string[] lines = File.ReadAllLines(iaFilePath);

                bool isAttributeLine = false;
                string currentAttrName = "";

                int index = 0;
                foreach (string l in lines)
                {
                    Console.WriteLine(index.ToString() + "|" + lines.Length.ToString());
                    
                    if (l.Contains("attrType name="))
                    {
                        isAttributeLine = true;
                        Match lineRegexp = Regex.Match(l, @".*attrType name=(.*) type=.*");
                        if (lineRegexp.Success)
                        {
                            string content = lineRegexp.Groups[1].Value;
                            currentAttrName = content.Substring(1, content.Length - 2);
                        } else {
                            throw new Exception("Error when reading the file!");
                        }
                    }

                    if ((isAttributeLine == true && attributes.Contains(currentAttrName)) || isAttributeLine == false)
                    {
                        fileContent += l + Environment.NewLine;
                    }

                    if (l.Contains("&lt;/attrType&gt;"))
                    {
                        isAttributeLine = false;
                        currentAttrName = "";
                    }
                    index++;
                }
                string outputPath = currentDir + "\\Filtered_" + iaFilename;
                File.WriteAllText(outputPath, fileContent);
                Console.WriteLine("Output file: " + outputPath);
            }
            else {
                Console.WriteLine("File does not exist!");
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
