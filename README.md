# altCrypt
A highly flexible symmetric file encryption/decryption library for .NET. No messing around with streams - this library does that for you! (but you're welcome to continue using them too).

The library can be used in your own applications.
Or you can use the command line utility to encrypt your files and directories manually.

It currently only supports the full version of .NET (4.6) but a WinRT / Universal solution is in the pipeline.

## Sample Usage
```c#
    IFile file = new LocalFile(@"C:\temp\MyFile.txt");         //1. Select a file to be encrypted
    IKey key = new Key("Pass@w0rd1");                          //2. Create a key
    IIV iv = new RandomIV();                                   //3. Choose how you want an IV / nonce to be generated per file
    using(SymmetricAlgorithm algorithm = Aes.Create()){        //4. Choose the algorithm you would like to use
        var encryptor = new FileEncryptor(key, iv, algorithm); //5. Create the encryptor
        await encryptor.EncryptAsync(file);                    //6. Encrypt the file
        await encryptor.DecryptAsync(file);                    //7. Decrypt the file
    }
```
