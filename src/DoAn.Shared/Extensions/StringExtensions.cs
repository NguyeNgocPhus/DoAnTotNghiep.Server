
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace DoAn.Shared.Extensions
{
  public static class StringExtensions
  {
    public static string ToTrimLower(this string input) => input.Trim().ToLower();

    public static string ToTrimUpper(this string input) => input.Trim().ToUpper();
    private static Random random = new Random();

    public static string RandomString(int length)
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public static string Md5Hash(this string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in MD5.Create().ComputeHash(new UTF8Encoding().GetBytes(input)))
        stringBuilder.Append(num.ToString("x2"));
      return stringBuilder.ToString();
    }

    public static string EncryptString(this string text, string keyString)
    {
      Debug.Assert(keyString.Length == 32);
      byte[] bytes = Encoding.ASCII.GetBytes(keyString);
      Debug.Assert(bytes.Length == 32);
      Debug.Assert(bytes.Length == keyString.Length);
      using (Aes aes = Aes.Create())
      {
        using (ICryptoTransform encryptor = aes.CreateEncryptor(bytes, aes.IV))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
            {
              using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
                streamWriter.Write(text);
            }
            byte[] iv = aes.IV;
            byte[] array = memoryStream.ToArray();
            byte[] numArray = new byte[iv.Length + array.Length];
            Buffer.BlockCopy((Array) iv, 0, (Array) numArray, 0, iv.Length);
            Buffer.BlockCopy((Array) array, 0, (Array) numArray, iv.Length, array.Length);
            return Convert.ToBase64String(numArray);
          }
        }
      }
    }

    public static string DecryptString(this string cipherText, string keyString)
    {
      Debug.Assert(keyString.Length == 32);
      byte[] src = Convert.FromBase64String(cipherText);
      byte[] numArray1 = new byte[16];
      byte[] numArray2 = new byte[src.Length - numArray1.Length];
      Buffer.BlockCopy((Array) src, 0, (Array) numArray1, 0, numArray1.Length);
      Buffer.BlockCopy((Array) src, numArray1.Length, (Array) numArray2, 0, src.Length - numArray1.Length);
      byte[] bytes = Encoding.ASCII.GetBytes(keyString);
      Debug.Assert(bytes.Length == 32);
      Debug.Assert(bytes.Length == keyString.Length);
      using (Aes aes = Aes.Create())
      {
        using (ICryptoTransform decryptor = aes.CreateDecryptor(bytes, numArray1))
        {
          string end;
          using (MemoryStream memoryStream = new MemoryStream(numArray2))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
            {
              using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
                end = streamReader.ReadToEnd();
            }
          }
          return end;
        }
      }
    }

    public static string ToSnakeCase(this string input)
    {
      return string.IsNullOrEmpty(input) ? input : Regex.Match(input, "^_+")?.ToString() + Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToSpaceSeparatedString(this IEnumerable<string> list)
    {
      if (!(list is string[] strArray))
        strArray = list.ToArray<string>();
      string[] source = strArray;
      if (!((IEnumerable<string>) source).Any<string>())
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(100);
      foreach (string str in source)
        stringBuilder.Append(str + " ");
      return stringBuilder.ToString().Trim();
    }

    public static IEnumerable<string> FromSpaceSeparatedString(this string input)
    {
      input = input.Trim();
      return (IEnumerable<string>) ((IEnumerable<string>) input.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
    }

    public static List<string> ParseScopesString(this string scopes)
    {
      if (scopes.IsMissing())
        return new List<string>();
      scopes = scopes.Trim();
      List<string> list = ((IEnumerable<string>) scopes.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries)).Distinct<string>().ToList<string>();
      if (list.Any<string>())
        list.Sort();
      return list;
    }

    public static bool IsMissing(this string? value) => string.IsNullOrWhiteSpace(value);

    public static string GetThumbnailImage(this string fileName)
    {
      return fileName.IsMissing() ? string.Empty : Path.GetFileNameWithoutExtension(fileName) + "_thumbnail.png";
    }

    public static bool IsMissingOrTooLong(this string? value, int maxLength)
    {
      return string.IsNullOrWhiteSpace(value) || value.Length > maxLength;
    }

    public static bool IsPresent(this string value) => !string.IsNullOrWhiteSpace(value);

    public static string EnsureLeadingSlash(this string url)
    {
      return !url.StartsWith("/") ? "/" + url : url;
    }

    public static string EnsureTrailingSlash(this string url)
    {
      return !url.EndsWith("/") ? url + "/" : url;
    }

    public static string RemoveLeadingSlash(this string url)
    {
      if (url.StartsWith("/"))
      {
        string str = url;
        url = str.Substring(1, str.Length - 1);
      }
      return url;
    }

    public static string RemoveTrailingSlash(this string url)
    {
      if (url.EndsWith("/"))
      {
        string str = url;
        url = str.Substring(0, str.Length - 1);
      }
      return url;
    }

    public static string CleanUrlPath(this string url)
    {
      if (url.IsMissing())
        url = "/";
      if (url != "/" && url.EndsWith("/"))
      {
        string str = url;
        url = str.Substring(0, str.Length - 1);
      }
      return url;
    }

    public static bool IsIp(this string ip)
    {
      return Regex.IsMatch(ip, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
    }

    public static bool IsLocalUrl(this string url)
    {
      if (string.IsNullOrEmpty(url))
        return false;
      return url[0] == '/' ? url.Length == 1 || url[1] != '/' && url[1] != '\\' : url[0] == '~' && url.Length > 1 && url[1] == '/' && (url.Length == 2 || url[2] != '/' && url[2] != '\\');
    }

    public static string AddQueryString(this string url, string query)
    {
      if (!url.Contains("?"))
        url += "?";
      else if (!url.EndsWith("&"))
        url += "&";
      return url + query;
    }

    public static string AddQueryString(this string url, string name, string value)
    {
      return url.AddQueryString(name + "=" + UrlEncoder.Default.Encode(value));
    }

    public static string AddHashFragment(this string url, string query)
    {
      if (!url.Contains("#"))
        url += "#";
      return url + query;
    }

    public static string GetOrigin(this string url)
    {
      if (!url.IsMissing())
      {
        Uri uri;
        try
        {
          uri = new Uri(url);
        }
        catch (Exception ex)
        {
          return string.Empty;
        }
        string scheme = uri.Scheme;
        if ((scheme == "http" ? 0 : (!(scheme == "https") ? 1 : 0)) == 0)
          return uri.Scheme + "://" + uri.Authority;
      }
      return string.Empty;
    }

    public static string Obfuscate(this string value)
    {
      string str1 = "****";
      if (value.IsPresent() && value.Length > 4)
      {
        string str2 = value;
        int length = str2.Length;
        int startIndex = length - 4;
        str1 = str2.Substring(startIndex, length - startIndex);
      }
      return "****" + str1;
    }

    public static string Masking(this string value, int len = 20)
    {
      string str1 = value;
      if (value.IsPresent() && value.Length > len)
      {
        string str2 = value;
        int num = len;
        int length = str2.Length;
        int startIndex = length - num;
        str1 = "**** " + str2.Substring(startIndex, length - startIndex);
      }
      return str1;
    }

    public static string Format(this string value, params object?[] args)
    {
      return string.Format(value, args);
    }

    public static bool IsAllowedFileExtension(
      this string fileName,
      IEnumerable<string> allowedExtensions)
    {
      return allowedExtensions.Any<string>((Func<string, bool>) (e => e.Equals(fileName.GetFileExtension())));
    }

    public static bool IsAllowedMimeType(this string mimeType, IEnumerable<string> allowedMimeTypes)
    {
      return allowedMimeTypes.Any<string>((Func<string, bool>) (m => m.Equals(mimeType)));
    }

    public static string GetFileExtension(this string fileName)
    {
      if (!fileName.Contains('.'))
        return fileName;
      string[] strArray1 = fileName.Split('.');
      string fileExtension;
      if (strArray1.Length <= 1)
      {
        fileExtension = fileName;
      }
      else
      {
        string[] strArray2 = strArray1;
        fileExtension = "." + strArray2[strArray2.Length - 1];
      }
      return fileExtension;
    }
  }
}
