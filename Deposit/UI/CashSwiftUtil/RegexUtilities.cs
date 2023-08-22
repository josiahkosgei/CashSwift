﻿
// Type: CashSwiftUtil.RegexUtilities
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CashSwiftUtil
{
    public class RegexUtilities
    {
        private bool invalid;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;
            try
            {
                strIn = Regex.Replace(strIn, "(@)(.+)$", new MatchEvaluator(DomainMapper), RegexOptions.None, TimeSpan.FromMilliseconds(200.0));
            }
            catch (RegexMatchTimeoutException ex)
            {
                return false;
            }
            if (invalid)
                return false;
            try
            {
                return Regex.IsMatch(strIn, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250.0));
            }
            catch (RegexMatchTimeoutException ex)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            IdnMapping idnMapping = new IdnMapping();
            string ascii = match.Groups[2].Value;
            try
            {
                ascii = idnMapping.GetAscii(ascii);
            }
            catch (ArgumentException ex)
            {
                invalid = true;
            }
            return match.Groups[1].Value + ascii;
        }
    }
}
