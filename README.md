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
