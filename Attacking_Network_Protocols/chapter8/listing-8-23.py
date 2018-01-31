from ctypes import *

# On Linux
lib = cdll.LoadLibrary("./lib.so")
# On macOS
#lib = cdll.LoadLibrary("lib.dylib")
# On Windows
#lib = cdll.LoadLibrary("lib.dll")
# Or we can do the following on Windows
#lib = cdll.lib

lib.say_hello()

# Listing 8-24
print lib.add_numbers(1, 2)
lib.say_string("Hello from Python")

# Listing 8-25
print lib.get_hello()
lib.get_hello.restype = c_char_p
print lib.get_hello()

# Listing 8-26
lib.add_longs.restype = c_long
print lib.add_longs(0x100000000, 1)

lib.add_longs.argtypes = [c_long, c_long]
print lib.add_longs(0x100000000, 1)

# Listing 8-27
i = c_int()
lib.add_numbers_result(1, 2, byref(i))
print i.value

class SimpleStruct(Structure):
  _fields_ = [("str", c_char_p),
              ("num", c_int)]

s = SimpleStruct()
s.str = "Hello from Struct"
s.num = 100
lib.say_struct(byref(s))