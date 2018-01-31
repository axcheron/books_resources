import sys
import binascii
from socket import socket, AF_INET, SOCK_DGRAM
if len(sys.argv) < 3:
  print("Specify destination host and port")
  exit(1)

# Create a UDP socket with a 1sec receive timeout
sock = socket(AF_INET, SOCK_DGRAM)
sock.settimeout(1)
addr = (sys.argv[1], int(sys.argv[2]))

for line in sys.stdin:
  msg = binascii.a2b_hex(line.strip())
  sock.sendto(msg, addr)
  try:
    data, server = sock.recvfrom(1024)
    print(binascii.b2a_hex(data))
  except:
    pass