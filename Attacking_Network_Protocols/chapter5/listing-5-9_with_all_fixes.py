from struct import unpack
import sys
import os

# Read fixed number of bytes
def read_bytes(f, l):
    bytes = f.read(l)
    if len(bytes) != l:
        raise Exception("Not enough bytes in stream")
    return bytes

# Unpack a 4 byte network byte order integer
def read_int(f):
    return unpack("!i", read_bytes(f, 4))[0]

# Read a single byte
def read_byte(f):
    return ord(read_bytes(f, 1))

def calc_chksum(unk2, data):
    chksum = unk2
    for i in range(len(data)):
        chksum += ord(data[i:i+1])
    return chksum

filename = sys.argv[1]
file_size = os.path.getsize(filename)

f = open(filename, "rb")
if read_bytes(f, 4) != b'BINX': f.seek(0)

# Keep reading until we run out of file.
while f.tell() < file_size:
    length = read_int(f)
    chksum = read_int(f)
    unk2 = read_byte(f)
    data = read_bytes(f, length - 1)
    print("Len: %d, Chk: %d/%d, Unk2: %d, Data: %s" 
        % (length, chksum, calc_chksum(unk2, data), unk2, data))
