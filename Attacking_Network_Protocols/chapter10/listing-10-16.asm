BITS 64
; The syscall number of exit
mov rax, 60
; The exit code argument
mov rdi, 42
syscall
; exit should never return, but just in case.
ret