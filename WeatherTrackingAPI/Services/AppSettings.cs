public class AppSettings
{
    public FirebaseSettings Firebase { get; set; }
    public JwtSettings Jwt { get; set; }
}

public class FirebaseSettings
{
    public string ServiceAccountKeyPath { get; set; }
}

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}
