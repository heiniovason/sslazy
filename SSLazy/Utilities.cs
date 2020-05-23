using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SSLazy
{
    public static class Utilities
    {
        public static X509Certificate2 GetCertificate(string thumbprint)
        {
            X509Certificate2 cert = null;
            foreach (StoreLocation storeLocation in (StoreLocation[])
                Enum.GetValues(typeof(StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])
                    Enum.GetValues(typeof(StoreName)))
                {
                    X509Store certStore = new X509Store(storeName, storeLocation);
                    try
                    {
                        certStore.Open(OpenFlags.ReadOnly);
                        X509Certificate2Collection certCollection = certStore.Certificates.Find(
                            X509FindType.FindByThumbprint,
                            thumbprint,
                            false
                            );
                        certStore.Close();

                        if (certCollection.Count > 0)
                        {
                            cert = certCollection[0];
                        }
                    }
                    catch (CryptographicException ex)
                    {
                        Console.WriteLine(ex);
                        // Log exception to ...?
                    }
                }
            }
            return cert;
        }

        public static HttpClient GetConfiguredHttpClient(X509Certificate2 certificate)
        {
            HttpClient httpClient = null;
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12
            };
            try
            {
                handler.ClientCertificates.Add(certificate);

                httpClient = new HttpClient(handler);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex);
                // Log exception to ...?
            }
            return httpClient;
        }
    }
}
