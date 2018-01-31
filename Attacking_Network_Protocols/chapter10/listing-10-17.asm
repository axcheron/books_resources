BITS 64
%define SYS_write 1
%define STDOUT 1

_start:
mov rax, SYS_write
; The first argument (rdi) is the STDOUT file descriptor
mov rdi, STDOUT
; The second argument (rsi) is a pointer to a string
lea rsi, [_greeting]
; The third argument (rdx) is the length of the string to write
mov rdx, _greeting_end - _greeting
; Execute the write system call
syscall
ret

_greeting:
db "Hello User!", 10
_greeting_end: