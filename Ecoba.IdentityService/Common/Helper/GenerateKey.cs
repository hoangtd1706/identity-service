namespace Ecoba.IdentityService.Common.Helper;

public class GenerateKey
{
    private static Random random = new Random();
    public string Generate(int count)
    {
        string key = null;
        for (var i = 0; i < count; i++)
        {
            var shortKey = "";
            var length = random.Next(3, 6);
            shortKey = RandomString(length);
            key += (key == null) ? shortKey : "-" + shortKey;
        }
        return key;
    }
    public static string RandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}