WalletHash
==========

Extract the hash of the encryption password from a Bitcoin wallet file (`wallet.dat`). Useful for passing to a hash-cracking tool like hashcat, John the Ripper, etc.

C# .NET reimplementation of bitcoin2john Python code.

Use
---

 `WalletHash wallet.dat`

The output of the program will be the hash, formatted as a standard hash string (e.g. `$bitcoin$64$076b52991bb9adfc55d5e95cd65d4d157609de098256bbeb48332e057d9fac5a$16$893e8f978cbbb561$64756$2$00$2$00`).

Attribution
-----------

Based on [`bitcoin2john.py`](https://github.com/openwall/john/blob/bleeding-jumbo/run/bitcoin2john.py) Python implementation which uses code by Dhiru Kholia <dhiru at openwall.com>, Solar Designer, exploide, jackjack, Joric, and Gavin Andresen.

The original bitcoin2john.py attribution:

> This software is
>
> Copyright (c) 2012-2018 Dhiru Kholia <dhiru at openwall.com>
>
> Copyright (c) 2019 Solar Designer

> Copyright (c) 2019 exploide
>
> Redistribution and use in source and binary forms, with or without
> modification, are permitted.  (This is a heavily cut-down "BSD license".)
>
> While the above applies to the stated copyright holders' contributions,
> this software is also dual-licensed under the MIT License, to be certain
> of license compatibility with that of the components listed below.
>
> This script (bitcoin2john.py) might still contain portions of jackjack's
> [pywallet.py](https://github.com/jackjack-jj/pywallet) which is forked from Joric's pywallet.py whose licensing
> information follows,
>
> [PyWallet](http://github.com/joric/pywallet) 1.2.1 (Public Domain)
>
> PyWallet includes portions of free software, listed below.
>
> [BitcoinTools](https://github.com/gavinandresen/bitcointools) (wallet.dat handling code, MIT License)
>
> Copyright (c) 2010 Gavin Andresen
