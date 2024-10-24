using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.DAL
{
  

    /// <summary>
    /// Summary description for EncryDecry
    /// </summary>
    public class EncryDecry
    {
        public EncryDecry()
        {
            //
            // TODO: Add constructor logic here
            //

        }
        public string Encrypt(string args)
        {
            encry_decry objencry = new encry_decry();

            string passPhrase1 = "Pas5pr@setapwsjx";     // can be any string
            string saltValue2 = "s@1tValuetapwsjx";     // can be any string
            string hashAlgorithm3 = "SHA1";            // can be "MD5"
            int passwordIterations4 = 3;              // can be any number
            string initVector5 = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize6 = 256;                     // can be 192 or 128
            string enorderid = objencry.Encrypt(args, passPhrase1, saltValue2, hashAlgorithm3, passwordIterations4, initVector5, keySize6);

            return enorderid;
        }
        public string Decrypt(string oid)
        {
            encry_decry objencry = new encry_decry();

            oid = oid.Replace(" ", "+");
            string pText = oid;                          // Cipher plaintext
            string passPhrase1 = "Pas5pr@setapwsjx";     // can be any string
            string saltValue2 = "s@1tValuetapwsjx";     // can be any string
            string hashAlgorithm3 = "SHA1";            // can be "MD5"
            int passwordIterations4 = 3;              // can be any number
            string initVector5 = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize6 = 256;                     // can be 192 or 128
            string deorderid = objencry.Decrypt(oid, passPhrase1, saltValue2, hashAlgorithm3, passwordIterations4, initVector5, keySize6);
            return deorderid;
        }
    }
}