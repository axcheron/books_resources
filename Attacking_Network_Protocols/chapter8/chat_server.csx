using CANAPE.Nodes;
using CANAPE.DataAdapters;
using CANAPE.Net.Templates;

class ChatServerConfig {
    public LogPacketCollection Packets { get; private set; }
    public ChatServerConfig() {
        Packets = new LogPacketCollection();
    }
}

class ChatServer : BaseDataEndpoint<ChatServerConfig> {
    public override void Run(IDataAdapter adapter, ChatServerConfig config) {
        Console.WriteLine("New Connection");
        DataFrame frame = adapter.Read();
        if (frame != null) {
            foreach(var packet in config.Packets) {
                adapter.Write(packet.Frame);
            }
        }
        frame = adapter.Read();
    }
}
