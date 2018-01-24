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
            if (args.Length >= 2) {
                path = args[0];
            }

            var output = "codeplug.cfg";
            if (args.Length >= 1) {
                output = args[args.Length - 1];
            }

            var certificate = File.ReadAllBytes(Path.GetFullPath(Path.Combine(path, @"..\resources\mototrbocps")));

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
                using (var writer = new StreamWriter(output))
                {
                    writer.WriteLine("[codeplug]");
                    writer.WriteLine("key = {0}=", loadedStrings.Find(s => s.Length == 43));
                    writer.WriteLine("iv = {0}==", loadedStrings.Find(s => s.Length == 22));
                    writer.WriteLine("signing_password = {0}==", loadedStrings.FindAll(s => s.Length == 22)[loadedStrings.Count(s => s.Length == 43)]);
                    var certificateLines = Split(Convert.ToBase64String(certificate), 64);
                    writer.WriteLine("signing_key = {0}", String.Join(Environment.NewLine + "    ", certificateLines));
                }
                Console.WriteLine("Extracted keys to {0}", output);
                return 0;
            }

            Console.Error.WriteLine("Could not find keys.");
            return 1;
        }

        private static IEnumerable<string> Split(string text, int chunkSize) {
            for (int offset = 0; offset < text.Length; offset += chunkSize) {
                int size = Math.Min(chunkSize, text.Length - offset);
                yield return text.Substring(offset, size);
            }
        }
    }
}
