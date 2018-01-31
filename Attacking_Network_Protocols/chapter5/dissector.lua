-- Declare our chat protocol for Dissection
chat_proto = Proto("chat","SuperFunkyChat Protocol")
-- Specify protocol fields
chat_proto.fields.chksum = ProtoField.uint32("chat.chksum", "Checksum", base.HEX)
chat_proto.fields.command = ProtoField.uint8("chat.command", "Command")
chat_proto.fields.data = ProtoField.bytes("chat.data", "Data")

-- Dissector function
-- buffer: The UDP packet data as a "Testy Virtual Buffer"
-- pinfo: Packet information 
-- tree: Root of the UI tree
function chat_proto.dissector(buffer, pinfo, tree)
    -- Set the name in the protocol column in the UI
    pinfo.cols.protocol = "CHAT"
    
    -- Create sub tree which represents the entire buffer.
    local subtree = tree:add(chat_proto,
                             buffer(),
                             "SuperFunkyChat Protocol Data")
    subtree:add(chat_proto.fields.chksum, buffer(0, 4))
    subtree:add(chat_proto.fields.command, buffer(4, 1))
    subtree:add(chat_proto.fields.data, buffer(5))
end

-- Get UDP dissector table and add for port 12345
udp_table = DissectorTable.get("udp.port")
udp_table:add(12345, chat_proto)
