using System;
using System.Text;
using System.Security.Cryptography;

namespace Biblioteca.Models
{
    public static class Criptografo
    {
        public static string TextoCriptografado(string textoClaro)
        {
            MD5 MD5Hasher = MD5.Create();

            byte[] by = Encoding.Default.GetBytes(textoClaro);
            byte[] byteCriptografado = MD5Hasher.ComputeHash(by);

            StringBuilder SB = new StringBuilder();

            foreach (byte b in byteCriptografado)
            {
             string DebugB = b.ToString("x2");
             SB.Append(DebugB);   
            }            

            return SB.ToString();
        }
        
    }
}