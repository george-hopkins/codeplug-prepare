namespace CodeplugPrepare {
    using System;
    using System.Xml;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;

    public class Program {

        public static int Main(string[] args) {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Motorola\MOTOTRBO CPS\cpservices.dll");
            if (args.Length > 0) {
                path = args[0];
            }

            var module = ModuleDefMD.Load(path);
            foreach (var type in module.Types) {
                var constructor = type.FindStaticConstructor();
                if(constructor == null) {
                    continue;
                }
                var loadedStrings = new List<string>();
                foreach (var i in constructor.Body.Instructions) {
                    if (i.OpCode != OpCodes.Ldstr) {
                        continue;
                    }
                    loadedStrings.Add((string) i.Operand);
                }
                if (!loadedStrings.Contains("mototrbocps")) {
                    continue;
                }
                Console.WriteLine("Prepare your environment:");
                Console.WriteLine("export CTB_KEY='{0}='", loadedStrings.Find(s => s.Length == 43));
                Console.WriteLine("export CTB_IV='{0}=='", loadedStrings.Find(s => s.Length == 22));
                Console.WriteLine();
                var encodedPassword = loadedStrings.FindAll(s => s.Length == 22)[loadedStrings.Count(s => s.Length == 43)] + "==";
                Console.WriteLine("Extract the private key (enter '{0}' when asked for the password):", Encoding.ASCII.GetString(Convert.FromBase64String(encodedPassword)));
                Console.WriteLine("openssl pkcs12 -in '{0}' -nocerts -nodes -out yourkey.pem", Path.GetFullPath(Path.Combine(path, @"..\resources\mototrbocps")));
                return 0;
            }

            Console.Error.WriteLine("Could not find keys.");
            return 1;
        }
    }
}
