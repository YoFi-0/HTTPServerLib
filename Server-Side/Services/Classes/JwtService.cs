using Domain.ReturnsModel;
using Domain.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server_Side.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server_Side.Services.Classes
{
    public class JwtService<T> : IJwtService<T> where T : class
    {
        private readonly IEncreptionService encreption;
        private readonly IHandlerService _handler;

        public JwtService(IEncreptionService encreption, IHandlerService handler)
        {
            this.encreption = encreption;
            _handler = handler;
        }
        public string Create(T data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                List<Claim> clams = new List<Claim>() {
                   new Claim(ClaimTypes.Name,json)
                 };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rashwer34534834yry43ehuhg8934y328yrehfhf9834yhtrfrtet34ed"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(claims: clams, expires: DateTime.UtcNow.AddDays(10), signingCredentials: creds);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                EncinreptionpoeRes<string> EncodedJWT = encreption.En_Code(jwt);
                return EncodedJWT.Enc_Value;
            } catch (Exception ex)
            {
                _handler.Err("حدث خطأ في حفظ بيانات المستخدم");
                return "";
            }
        }

        public ReturnJwtModel<T> Get(string EncodedToken)
        {
            var token = encreption.De_Code(EncodedToken);
            if (!token.IsOk)
            {
                Console.WriteLine(token.Msg);
                return new() { IsValid = false };
            }
            JwtSecurityTokenHandler handler = new();
            var isItToken = handler.CanReadToken(token.Enc_Value);
            if (!isItToken)
            {
                return new() { IsValid = false };
            }
            var decodedValue = handler.ReadJwtToken(token.Enc_Value);

            var time = decodedValue.ValidTo;
            var text = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.String).Value;
            DateTimeOffset expireDate = new DateTimeOffset(time);

            Console.WriteLine(expireDate.ToString());
            if (expireDate.UtcDateTime < DateTime.UtcNow)
            {
                return new() { IsValid = false };
            }
            T value = JsonConvert.DeserializeObject<T>(text)!;
            return new() { expireDate = expireDate.Date, Data = value, IsValid = true };
        }

    }
}
