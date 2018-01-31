#load "parser.csx"

using static System.Console;
using static CANAPE.Cli.ConsoleUtils;

var template = new FixedProxyTemplate();
// Local port of 4444, destination 127.0.0.1:12345
template.LocalPort = 4444;
template.Host = "127.0.0.1";
template.Port = 12345;
template.AddLayer<Parser>();

var service = template.Create();
service.Start();

WriteLine("Created {0}", service);
WriteLine("Press Enter to exit...");
ReadLine();
service.Stop();
WriteLine("Writing Outbound Packets to packets.bin");
service.Packets.WriteToFile("packets.bin");
WriteLine("Done");
