# Kestre EPIPE Repro

Repro for https://github.com/aspnet/KestrelHttpServer/issues/1978

Uncommenting the jQuery submit callback prevents the "-32 EPIPE" on Linux and "-4081 ECANCELED" on windows.
