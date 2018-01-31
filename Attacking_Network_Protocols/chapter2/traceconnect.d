/* traceconnect.d - A simple DTrace script to monitor a connect system call */
struct sockaddr_in {
	short sin_family;
	unsigned short sin_port;
	in_addr_t sin_addr;
	char sin_zero[8];
};

syscall::connect:entry
/arg2 == sizeof(struct sockaddr_in)/ {
	addr = (struct sockaddr_in*)copyin(arg1, arg2);
	printf("process:'%s' %s:%d", execname,
		inet_ntop(2, &addr->sin_addr), ntohs(addr->sin_port));
}
