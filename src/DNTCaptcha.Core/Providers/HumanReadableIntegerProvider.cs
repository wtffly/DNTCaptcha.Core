using System.Collections.Generic;
using System.Linq;
using DNTCaptcha.Core.Contracts;
using System.Text.RegularExpressions;

namespace DNTCaptcha.Core.Providers
{
	/// <summary>
	/// Convert a number into words
	/// </summary>
	public class HumanReadableIntegerProvider : IHumanReadableIntegerProvider
	{
		private readonly IDictionary<Language, string> _and = new Dictionary<Language, string>
		{
			{ Language.English, " " },
			{ Language.Persian, " و " }
        };
		private readonly IList<NumberWord> _numberWords = new List<NumberWord>
		{
			new NumberWord { Group= DigitGroup.Ones, Language= Language.English, Names=
				new List<string> { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }},
			new NumberWord { Group= DigitGroup.Ones, Language= Language.Persian, Names=
				new List<string> { string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" }},

            new NumberWord { Group= DigitGroup.Teens, Language= Language.English, Names=
				new List<string> { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" }},
			new NumberWord { Group= DigitGroup.Teens, Language= Language.Persian, Names=
				new List<string> { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" }},

            new NumberWord { Group= DigitGroup.Tens, Language= Language.English, Names=
				new List<string> { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" }},
			new NumberWord { Group= DigitGroup.Tens, Language= Language.Persian, Names=
				new List<string> { "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" }},

            new NumberWord { Group= DigitGroup.Hundreds, Language= Language.English, Names=
				new List<string> {string.Empty, "One Hundred", "Two Hundred", "Three Hundred", "Four Hundred",
					"Five Hundred", "Six Hundred", "Seven Hundred", "Eight Hundred", "Nine Hundred" }},
			new NumberWord { Group= DigitGroup.Hundreds, Language= Language.Persian, Names=
				new List<string> {string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد" , "نهصد" }},

            new NumberWord { Group= DigitGroup.Thousands, Language= Language.English, Names=
			  new List<string> { string.Empty, " Thousand", " Million", " Billion"," Trillion", " Quadrillion", " Quintillion", " Sextillian",
			" Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
			" Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
			" Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
			" Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }},
			new NumberWord { Group= DigitGroup.Thousands, Language= Language.Persian, Names=
			  new List<string> { string.Empty, " هزار", " میلیون", " میلیارد"," تریلیون", " Quadrillion", " Quintillion", " Sextillian",
			" Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
			" Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
			" Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
			" Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }}
        };
		private readonly IDictionary<Language, string> _negative = new Dictionary<Language, string>
		{
			{ Language.English, "Negative " },
			{ Language.Persian, "منهای " }
        };
		private readonly IDictionary<Language, string> _zero = new Dictionary<Language, string>
		{
			{ Language.English, "Zero" },
			{ Language.Persian, "صفر" }
        };

		// Public Methods (5)

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(int number, Language language)
		{
			return NumberToText((long)number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(uint number, Language language)
		{
			return NumberToText((long)number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(byte number, Language language)
		{
			return NumberToText((long)number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(decimal number, Language language)
		{
			return NumberToText((long)number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(double number, Language language)
		{
			return NumberToText((long)number, language);
		}

		/// <summary>
		/// display a numeric value using the equivalent text
		/// </summary>
		/// <param name="number">input number</param>
		/// <param name="language">local language</param>
		/// <returns>the equivalent text</returns>
		public string NumberToText(long number, Language language)
		{
			if (number == 0)
			{
				return _zero[language];
			}

			if (number < 0)
			{
				return _negative[language] + NumberToText(-number, language);
			}

			return wordify(number, language, string.Empty, 0);
		}
		// Private Methods (2)

		private string getName(int idx, Language language, DigitGroup group)
		{
			return _numberWords.First(x => x.Group == group && x.Language == language).Names[idx];
		}

		private string wordify(long number, Language language, string leftDigitsText, int thousands)
		{
            if (language == Language.Chinese)
            {
                var needFixedZeroString = wordifyChinese(number, Language.Chinese, leftDigitsText, thousands);
                return Regex.Replace(needFixedZeroString, "零+", "零").Trim('零');
            }

            if (number == 0)
			{
				return leftDigitsText;
			}

			var wordValue = leftDigitsText;
			if (wordValue.Length > 0)
			{
				wordValue += _and[language];
			}

			if (number < 10)
			{
				wordValue += getName((int)number, language, DigitGroup.Ones);
			}
			else if (number < 20)
			{
				wordValue += getName((int)(number - 10), language, DigitGroup.Teens);
			}
			else if (number < 100)
			{
				wordValue += wordify(number % 10, language, getName((int)(number / 10 - 2), language, DigitGroup.Tens), 0);
			}
			else if (number < 1000)
			{
				wordValue += wordify(number % 100, language, getName((int)(number / 100), language, DigitGroup.Hundreds), 0);
			}
			else
			{
				wordValue += wordify(number % 1000, language, wordify(number / 1000, language, string.Empty, thousands + 1), 0);
			}

			if (number % 1000 == 0) return wordValue;
			return wordValue + getName(thousands, language, DigitGroup.Thousands);
		}

        private string wordifyChinese(long number, Language language, string leftDigitsText, int thousands)
        {
            if (number == 0)
            {
                return leftDigitsText;
            }

            var wordValue = leftDigitsText;

            if (number < 10000)
            {
                wordValue += getChineseName(number);
            }
            else
            {
                wordValue += wordifyChinese(number % 10000, language, wordifyChinese(number / 10000, language, string.Empty, thousands + 1), 0);
            }

            if (number % 10000 == 0) return wordValue;
            return wordValue + getChineseNameWan(thousands);
        }

        private string[] chineseOnes = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        /// <summary>
        /// X千X百X十X
        /// </summary>
        /// <param name="number">小于10000的数字</param>
        /// <returns></returns>
        private string getChineseName(long number)
        {
            if (number >= 10000) throw new System.ArgumentOutOfRangeException("数值不能超过1万。");
            var result = string.Empty;
            // 千
            var thousand = number / 1000;
            if(thousand > 0)
                result += chineseOnes[(int)thousand] + "千";
            else
                result += "零";
            // 百
            var hundred = number % 1000 / 100;
            if (hundred > 0)
                result += chineseOnes[(int)hundred] + "百";
            else
                result += "零";
            // 十
            var ten = number % 100 / 10;
            if (ten > 0)
                result += chineseOnes[(int)ten] + "十";
            else
                result += "零";
            // 个
            var one = number % 10;
            if (one > 0)
                result += chineseOnes[(int)one];

            return result;
        }

        /// <summary>
        /// 万、亿、万亿、亿亿
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string getChineseNameWan(int number)
        {
            if (number == 0) return "";
            // 首位是万，还是亿，
            // 除以2，余数是0，则是万，1则是亿
            var result = number % 2 == 0 ? "亿" : "万";

            // 后续的亿，除以2，商是几，就有几个亿字
            for(var i = 0; i < number / 2; i++)
            {
                result += "亿";
            }

            return result;
        }
    }
}