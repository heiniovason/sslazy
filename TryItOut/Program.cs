using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using SSLazy;

namespace TryItOut
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get and install PKCS12 from https://badssl.com/download/

            string thumbprint = args[0];

            // e.g. https://client.badssl.com/
            string url = args[1];

            X509Certificate2 certificate = Utilities.GetCertificate(thumbprint);

            if (certificate != null)
            {
                string output = string.Format("Certificate issued by: {0}", certificate.Issuer);
                Console.WriteLine(output);
            }

            Console.WriteLine("Configuring HttpClient ...");

            HttpClient client = Utilities.GetConfiguredHttpClient(certificate);

            Console.WriteLine("Calling server and waiting for response ...");

            var response = client.GetAsync(url);

            string result = response.Result.ToString();

            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
