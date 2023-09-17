using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TlpArchitectureCoreServer.Options;

public class AuthOptions
{
    public string Issuer
    {
        get; set;
    } = null!;

    public string Audience
    {
        get; set;
    } = null!;

    public string Key
    {
        get; set;
    } = null!;

    public SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(Key));
}
