using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Certificate;

namespace Certs
{
    public static class DevelopmentModeCertificateHelper
    {
        public static readonly X509Certificate2 Certificate = ServerCert.Get();

        public static Task Validate(CertificateValidatedContext context)
        {
            if (context.ClientCertificate.Issuer == Certificate.Issuer)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Subject, ClaimValueTypes.String,
                        context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.ClientCertificate.Subject, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                };

                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
            }
            else
            {
                context.Fail("Invalid certificate.");
            }

            return Task.CompletedTask; 
        }

        public static HttpMessageHandler CreateClientHandler()
        {
            var clientCert = ClientCert.Get();
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(clientCert);
            handler.ServerCertificateCustomValidationCallback = (message, serverCert, arg3, arg4) => serverCert.Issuer == clientCert.Issuer;
            return handler;
        }
    }
}