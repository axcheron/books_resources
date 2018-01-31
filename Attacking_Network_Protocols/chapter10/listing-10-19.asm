BITS 64

%define SYS_execve 59
_start:
mov rax, SYS_execve
; Load the executable path
lea rdi, [rel _exec_path]
; Load the argument
lea rsi, [rel _argument]
; Build argument array on stack = { _exec_path, _argument, NULL }
push 0
push rsi
push rdi
mov rsi, rsp
; Build environment array on stack = { NULL }
push 0
mov rdx, rsp
syscall
; execve shouldn't return, but just in case
ret

_exec_path:
db "/bin/uname", 0
_argument:
db "-a", 0