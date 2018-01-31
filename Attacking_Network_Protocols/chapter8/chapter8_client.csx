#load "parser.csx"

using static System.Console;
using static CANAPE.Cli.ConsoleUtils;

if (args.Length < 1) {
    WriteLine("Please Specify a Capture File");
    return;
}

var template = new NetClientTemplate();
template.Port = 12345;
template.Host = "127.0.0.1";
template.InitialData = new byte[] { 0x42, 0x49, 0x4E, 0x58 };
template.AddLayer<Parser>();

var packets = LogPacketCollection.ReadFromFile(args[0]);

using(var adapter = template.Connect()) {
    WriteLine("Connected");
    // Write packets to adapter.
    foreach(var packet in packets.GetPacketsForTag("Out")) {
        adapter.Write(packet.Frame);
    }

    // Set a 1000ms timeout on read so we disconnect
    adapter.ReadTimeout = 1000;
    DataFrame frame = adapter.Read();
    while(frame != null) {
        WritePacket(frame);
        frame = adapter.Read();
    }
}
