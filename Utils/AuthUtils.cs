using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

using LimsAuthenticationService.Models;
using System.Text;

namespace LimsAuthenticationService.Utils;
public class AuthUtils 
{
     // Utility to hash passwords
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
    }

    //verify password
    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var parts = hashedPassword.Split(':');
        byte[] salt = Convert.FromBase64String(parts[0]);
        string storedHash = parts[1];

        string enteredHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return storedHash == enteredHash;
    }

    //generate token
    public static string GenerateToken(Utilisateur user, List<string> roles)
    {
        //create the claims and add id, identifiant
        var claims = new List<Claim>
    {
        new Claim("id", user.IdUtilisateur.ToString()),
        new Claim("identifiant", user.Identifiant)
    };

    // Add roles as claims
    claims.AddRange(roles.Select(role => new Claim("roles", role)));

    // Think to store the key in the environment
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("L8jR5vH1aX3kZp9QoT2yW6e4UvYmNpA7T9fKdXrPoWyQvLXs"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddHours(12),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
    }

}