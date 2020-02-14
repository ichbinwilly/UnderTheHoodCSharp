appSetting
```csharp
<appSettings>
  <add key="ADFSBaseAddress" value="https://.../adfs" />
  <add key="ClientID" value="..." />
  <add key="RedirectUri" value="https://localhost" />
  <add key="ADFSMetadataAddress" value="https://.../federationmetadata/2007-06/federationmetadata.xml" />
</appSettings>
```

packages.config
```csharp
<packages>
  <package id="Microsoft.IdentityModel" version="7.0.0" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.Clients.ActiveDirectory" version="3.13.8" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.JsonWebTokens" version="5.6.0" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.Logging" version="5.6.0" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.Protocols" version="5.6.0" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.Protocols.OpenIdConnect" version="5.6.0" targetFramework="net452" />
  <package id="Microsoft.IdentityModel.Tokens" version="5.6.0" targetFramework="net452" />
  <package id="Newtonsoft.Json" version="10.0.1" targetFramework="net452" />
  <package id="System.IdentityModel.Tokens.Jwt" version="5.6.0" targetFramework="net452" />
</packages>
```
```csharp
static void Main(string[] args)
{
    //ValidateJWT validateJWT = new ValidateJWT();
    //return;
    string ADFSBaseAddress = ConfigurationManager.AppSettings["ADFSBaseAddress"];
    string ClientID = ConfigurationManager.AppSettings["ClientID"];
    string RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
    string ADFSMetadataAddress = ConfigurationManager.AppSettings["ADFSMetadataAddress"];

    try
    {
        //Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authcontext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(ADFSBaseAddress, false);

        //Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult authresult = authcontext.AcquireTokenAsync(ClientID, ClientID, new Uri(RedirectUri),
        //    new Microsoft.IdentityModel.Clients.ActiveDirectory.PlatformParameters(Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Auto)).Result;

        string IdToken = "...";
        //string AcToken = authresult.AccessToken;
        //string AcTokenType = authresult.AccessTokenType;

        if (!String.IsNullOrEmpty(IdToken))
        {
            List<Microsoft.IdentityModel.Tokens.X509SecurityKey> signingcerts = new List<Microsoft.IdentityModel.Tokens.X509SecurityKey>();
            signingcerts = GetSigningCertificates(ADFSMetadataAddress);
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();


            Microsoft.IdentityModel.Tokens.TokenValidationParameters parameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidAudience = ClientID,
                ValidIssuer = ADFSBaseAddress,
                IssuerSigningKeys = signingcerts
            };

            try
            {
                System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(IdToken);
                System.IdentityModel.Tokens.Jwt.JwtHeader header = jwt.Header;
                System.IdentityModel.Tokens.Jwt.JwtPayload payload = jwt.Payload;

                string jsonFormattedheader = Newtonsoft.Json.Linq.JValue.Parse(header.SerializeToJson()).ToString(Newtonsoft.Json.Formatting.Indented);
                string jsonFormattedpayload = Newtonsoft.Json.Linq.JValue.Parse(payload.SerializeToJson()).ToString(Newtonsoft.Json.Formatting.Indented);

                Microsoft.IdentityModel.Tokens.SecurityToken validated;
                var user = tokenHandler.ValidateToken(IdToken, parameters, out validated);
                Console.WriteLine($"User Id {user.Identity.Name}");
                Console.WriteLine($"User Id {user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
                Console.WriteLine($"validated {((System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validated).IssuedAt}");
                Console.WriteLine($"validated.ValidFrom {validated.ValidFrom}");
                Console.WriteLine($"validated.ValidTo {validated.ValidTo}");
                // Console.WriteLine("Valid IdToken for {0}: {1}\n\nDecoded Header:\n{2}\n\nDecoded Payload:\n{3}", authresult.UserInfo.DisplayableId, IdToken, jsonFormattedheader, jsonFormattedpayload);
                Console.WriteLine("Valid IdToken for {0}: {1}\n\nDecoded Header:\n{2}\n\nDecoded Payload:\n{3}", "Willy", IdToken, jsonFormattedheader, jsonFormattedpayload);


                //jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(AcToken);
                //header = jwt.Header;
                //payload = jwt.Payload;

                //jsonFormattedheader = Newtonsoft.Json.Linq.JValue.Parse(header.SerializeToJson()).ToString(Newtonsoft.Json.Formatting.Indented);
                //jsonFormattedpayload = Newtonsoft.Json.Linq.JValue.Parse(payload.SerializeToJson()).ToString(Newtonsoft.Json.Formatting.Indented);

                //tokenHandler.ValidateToken(IdToken, parameters, out validated);

                //Console.WriteLine("Valid AcToken for {0}: {1}\n\nDecoded Header:\n{2}\n\nDecoded Payload:\n{3}", authresult.UserInfo.DisplayableId, AcToken, jsonFormattedheader, jsonFormattedpayload);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message + "-" + ex.InnerException);
            }
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message + "-" + ex.InnerException);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
static List<Microsoft.IdentityModel.Tokens.X509SecurityKey> GetSigningCertificates(string metadataAddress)
{
    List<Microsoft.IdentityModel.Tokens.X509SecurityKey> tokens = new List<Microsoft.IdentityModel.Tokens.X509SecurityKey>();

    if (metadataAddress == null)
    {
        throw new ArgumentNullException(metadataAddress);
    }

    using (XmlReader metadataReader = XmlReader.Create(metadataAddress))
    {
        System.IdentityModel.Metadata.MetadataSerializer serializer = new System.IdentityModel.Metadata.MetadataSerializer()
        {
            RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck,
            CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust
        };

        System.IdentityModel.Metadata.EntityDescriptor metadata = serializer.ReadMetadata(metadataReader) as System.IdentityModel.Metadata.EntityDescriptor;

        if (metadata != null)
        {
            System.IdentityModel.Metadata.SecurityTokenServiceDescriptor stsd = metadata.RoleDescriptors.OfType<System.IdentityModel.Metadata.SecurityTokenServiceDescriptor>().First();

            if (stsd != null)
            {
                IEnumerable<System.IdentityModel.Tokens.X509RawDataKeyIdentifierClause> x509DataClauses = stsd.Keys.Where(key => key.KeyInfo != null && (key.Use == System.IdentityModel.Metadata.KeyType.Signing || key.Use == System.IdentityModel.Metadata.KeyType.Unspecified)).
                                                        Select(key => key.KeyInfo.OfType<System.IdentityModel.Tokens.X509RawDataKeyIdentifierClause>().First());

                tokens.AddRange(x509DataClauses.Select(token => new Microsoft.IdentityModel.Tokens.X509SecurityKey(new System.Security.Cryptography.X509Certificates.X509Certificate2(token.GetX509RawData()))));
            }
            else
            {
                throw new InvalidOperationException("There is no RoleDescriptor of type SecurityTokenServiceType in the metadata");
            }
        }
        else
        {
            throw new Exception("Invalid Federation Metadata document");
        }
    }
    return tokens;
}
```
