# ConsoleImage

ConsoleImage is a library for showing an image in the Windows Command Prompt. On one hand, this is a really bad idea because CMD supports 16 colors (Unlike the Linux Terminal UI, which might support 256, with transparency and other fanciness). Considering we're talking about down-converting your image to a 16-bit color palette, with a normal size around 80x50, expect the results to look like garbage.

But then again, if you can use some ASCII-graphic magic, maybe the results won't be complete and total garbage. If your image doesn't have a lot of fine detail, and if your use-case doesn't require it to look photo-perfect,  maybe this little library which turns images into garbage won't be so bad after all.
