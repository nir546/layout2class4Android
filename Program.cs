using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layout2Class
{
    class Program
    {
        static void Main(string[] args)
        {
            var layoutsPath = args[0];
            var outputPath = args[1];
            var package = args[2];

            var layoutsfolder = new DirectoryInfo(layoutsPath);
            if (!layoutsfolder.Exists)
            {
                Console.WriteLine("Folder " + layoutsPath + " doesn't exist");
                Console.ReadLine();
                return;
            }

            var outputFolder = new DirectoryInfo(outputPath);
            if (!outputFolder.Exists)
            {
                Console.WriteLine("Folder " + outputPath + " doesn't exist");
                Console.ReadLine();
                return;
            }

            ClassRenderer.CreateLayoutClasses(package, layoutsfolder, outputFolder);


        }

      
    }
}
