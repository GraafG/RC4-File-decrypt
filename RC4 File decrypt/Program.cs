using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Org.BouncyCastle.Crypto.Engines;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;

namespace RC4_File_decrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 3)
            {
                Console.WriteLine("Not all arguments are met use: key file-in file-out");
                Environment.Exit(0);
            }

            // To decrypt some files in RC4

            // Based on: http://stackoverflow.com/questions/31765588/encrypting-files-using-rc4-encryption-algorithm-in-c-sharp/31766216#31766216
            // All credits to: Nasreddine http://stackoverflow.com/users/162671/nasreddine

            // You encryption/decryption key as a bytes array
            // var key = Encoding.UTF8.GetBytes("");
            var key = Encoding.UTF8.GetBytes(args[0]);
            var cipher = new RC4Engine();
            var keyParam = new KeyParameter(key);

            // for decrypting the file just switch the first param here to false
            cipher.Init(true, keyParam);

            //using (var inputFile = new FileStream(@"C:\asdf\mr_robot_live-v2-AC_PROGRESSIVE-stereo-3840x2160-Q18_fs_enc.mp4", FileMode.Open, FileAccess.Read))
            //using (var outputFile = new FileStream(@"C:\asdf\mr_robot_live-v2-AC_PROGRESSIVE-stereo-3840x2160-Q18_fs_enc.mp4.decoded", FileMode.OpenOrCreate, FileAccess.Write))

            using (var inputFile = new FileStream(args[1], FileMode.Open, FileAccess.Read))
            using (var outputFile = new FileStream(args[2], FileMode.OpenOrCreate, FileAccess.Write))
            {
                // processing the file 4KB at a time.
                byte[] buffer = new byte[1024 * 4];
                long totalBytesRead = 0;
                long totalBytesToRead = inputFile.Length;
                while (totalBytesToRead > 0)
                {
                    // make sure that your method is marked as async
                    int read = inputFile.Read(buffer, 0, buffer.Length);

                    // break the loop if we didn't read anything (EOF)
                    if (read == 0)
                    {
                        break;
                    }

                    totalBytesRead += read;
                    totalBytesToRead -= read;

                    byte[] outBuffer = new byte[1024 * 4];
                    cipher.ProcessBytes(buffer, 0, read, outBuffer, 0);
                    outputFile.WriteAsync(outBuffer, 0, read);
                }
            }
        }
    }
}
