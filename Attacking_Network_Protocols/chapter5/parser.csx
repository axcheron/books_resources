using CANAPE.Net.Layers;
using System.IO;

class Parser : DataParserNetworkLayer
{
    protected override bool NegotiateProtocol(Stream outboundStream, Stream inboundStream)
    {
        // Read magic.
        var reader = new DataReader(inboundStream);
        var writer = new DataWriter(outboundStream);

        uint magic = reader.ReadUInt32();
        Console.WriteLine("Magic: {0:X}", magic);
        writer.WriteUInt32(magic);

        return true;
    }

    int CalcChecksum(byte[] data) {
        int chksum = 0;
        foreach(byte b in data) {
            chksum += b;
        }
        return chksum;
    }

    DataFrame ReadData(DataReader reader) {
        int length = reader.ReadInt32();
        int chksum = reader.ReadInt32();
        return reader.ReadBytes(length).ToDataFrame();
    }

    void WriteData(DataFrame frame, DataWriter writer) {
        byte[] data = frame.ToArray();
        writer.WriteInt32(data.Length);
        writer.WriteInt32(CalcChecksum(data));
        writer.WriteBytes(data);
    }

    protected override DataFrame ReadInbound(DataReader reader)
    {
        return ReadData(reader);
    }

    protected override void WriteOutbound(DataFrame frame, DataWriter writer)
    {
        WriteData(frame, writer);
    }

    protected override DataFrame ReadOutbound(DataReader reader)
    {
        DataFrame frame = ReadData(reader);
        // Convert frame back to bytes.
        byte[] data = frame.ToArray();
        if (data[0] == 0) {
            Console.WriteLine("Disabling XOR Encryption");
            data[data.Length - 1] = 0;
            frame = data.ToDataFrame();
        }
        return frame;
    }

    protected override void WriteInbound(DataFrame frame, DataWriter writer)
    {
        WriteData(frame, writer);
    }
}
