#load "Server.csx"
#load "Parser.cs"

using static System.Console;

if (args.Length < 1) {
    WriteLine("Please Specify a Capture File");
    return;
}

var template = new NetServerTemplate<ChatServer, ChatServerConfig>();
template.LocalPort = 8888;
// Set to true to enable binding to all interfaces
template.AnyBind = false;
template.AddLayer<Parser>();
var packets = LogPacketCollection.ReadFromFile(args[0]).GetPacketsForTag("In");
template.ServerFactoryConfig.Packets.AddRange(packets);

var service = template.Create();
service.Start();
WriteLine("Created {0}", service);
WriteLine("Press Enter to exit...");
ReadLine();
service.Stop();
