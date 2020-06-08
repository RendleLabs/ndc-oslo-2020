using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Certs
{
    public static class ClientCert
    {
        public static X509Certificate2 Get()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Certs.client.pfx");
            if (stream is null) return null;
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return new X509Certificate2(bytes, "secretsquirrel");
        }
    }
}
