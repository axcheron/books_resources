-- Declare our chat protocol for Dissection
chat_proto = Proto("chat","SuperFunkyChat Protocol")
-- Specify protocol fields
chat_proto.fields.chksum = ProtoField.uint32("chat.chksum", "Checksum", base.HEX)
chat_proto.fields.command = ProtoField.uint8("chat.command", "Command")
chat_proto.fields.data = ProtoField.bytes("chat.data", "Data")

-- buffer: A TVB containing packet data
-- start: The offset in the TVB to read the string from
-- returns The string and the total length used
function read_string(buffer, start)
    local len = buffer(start, 1):uint()
    local str = buffer(start + 1, len):string()
    return str, (1 + len)
end

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

    -- Get a TVB for the data component of the packet.
    local data = buffer(5):tvb()
    local datatree = subtree:add(chat_proto.fields.data, data())

    local MESSAGE_CMD = 3
    local command = buffer(4, 1):uint()
    if command == MESSAGE_CMD then
        local curr_ofs = 0
        local str, len = read_string(data, curr_ofs)
        datatree:add(chat_proto, data(curr_ofs, len), "Username: " .. str)
        curr_ofs = curr_ofs + len
        str, len = read_string(data, curr_ofs)
        datatree:add(chat_proto, data(curr_ofs, len), "Message: " .. str)
    end
end

-- Get UDP dissector table and add for port 12345
udp_table = DissectorTable.get("udp.port")
udp_table:add(12345, chat_proto)
