using System.Text.RegularExpressions;

namespace TextReaderWPF;

public class RegexManager
{
    public List<string> CheckRegexInText(string text, SystemEnum systemID)
    {
        const string regexPatternForAptic = @"(?:\b[23][0])\d{7}|(?:([1])|([M])|([P])|([Z]))\d{8}";
        const string regexPatternForSolveIT = @"[1-2]\d{9}|[1-2]\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}|[4-9]\d{8,9}|[4-9]\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}|[4-9]\d{3}\.\d{1,3}\.\d{1,3}|[0][1-9]\d{8}";

        RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
        List<string> results = new();

        if (text is not null && text != String.Empty)
        {
            if (systemID == SystemEnum.Aptic)
            {
                MatchCollection matches = Regex.Matches(text, regexPatternForAptic, options);

                if (matches.Count > 0)
                {
                    foreach (string number in matches.Select(x => x.ToString()))
                    {
                        results.Add(number);
                    }
                }
            }
            else if (systemID == SystemEnum.SolveIT)
            {
                MatchCollection matches = Regex.Matches(text, regexPatternForSolveIT, options);

                if (matches.Count > 0)
                {
                    foreach (string number in matches.Select(x => x.ToString()))
                    {
                        string num = number;

                        if (num.Contains("."))
                        {
                            num = num.Replace(".", String.Empty);
                        }

                        results.Add(num);
                    }
                }
            }
        }

        return results;
    }
}
