﻿namespace WebChatApi.data
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInDays { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
