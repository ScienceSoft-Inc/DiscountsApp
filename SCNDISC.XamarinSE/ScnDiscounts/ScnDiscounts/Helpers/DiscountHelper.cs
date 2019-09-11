using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using System;

namespace ScnDiscounts.Helpers
{
    public static class DiscountHelper
    {
        public static string GetDiscountTypeName(this int? discountType)
        {
            return discountType == 1 ? "Br" : "%";
        }

        public static WebAddressTypeEnum GetWebAddressType(this int webAddressType)
        {
            WebAddressTypeEnum result;

            switch (webAddressType)
            {
                case 0:
                    result = WebAddressTypeEnum.Http;
                    break;
                case 1:
                    result = WebAddressTypeEnum.Vk;
                    break;
                case 2:
                    result = WebAddressTypeEnum.Facebook;
                    break;
                case 3:
                    result = WebAddressTypeEnum.Instagram;
                    break;
                case 4:
                    result = WebAddressTypeEnum.YouTube;
                    break;
                case 5:
                    result = WebAddressTypeEnum.GooglePlus;
                    break;
                case 6:
                    result = WebAddressTypeEnum.Odnoklassniki;
                    break;
                case 7:
                    result = WebAddressTypeEnum.Twitter;
                    break;
                default:
                    result = WebAddressTypeEnum.Http;
                    break;
            }

            return result;
        }

        public static string GetWebAddressIcon(this WebAddressTypeEnum webAddressType)
        {
            string result;

            switch (webAddressType)
            {
                case WebAddressTypeEnum.Http:
                    result = "web_http.png";
                    break;
                case WebAddressTypeEnum.Vk:
                    result = "web_vk.png";
                    break;
                case WebAddressTypeEnum.Facebook:
                    result = "web_facebook.png";
                    break;
                case WebAddressTypeEnum.Instagram:
                    result = "web_instagram.png";
                    break;
                case WebAddressTypeEnum.YouTube:
                    result = "web_youtube.png";
                    break;
                case WebAddressTypeEnum.GooglePlus:
                    result = "web_googleplus.png";
                    break;
                case WebAddressTypeEnum.Odnoklassniki:
                    result = "web_odnoklassniki.png";
                    break;
                case WebAddressTypeEnum.Twitter:
                    result = "web_twitter.png";
                    break;
                default:
                    result = "web_http.png";
                    break;
            }

            return result;
        }

        public static PhoneOperatorEnum GetPhoneOperatorType(this string codeOperator, char? firstDigit = null)
        {
            PhoneOperatorEnum result;

            int.TryParse(codeOperator, out var code);

            switch (code)
            {
                case 017:
                    result = PhoneOperatorEnum.Beltelecom;
                    break;
                case 025:
                    result = PhoneOperatorEnum.Life;
                    break;
                case 029:
                    switch (firstDigit)
                    {
                        case '2':
                        case '5':
                        case '7':
                        case '8':
                            result = PhoneOperatorEnum.Mts;
                            break;
                        case '1':
                        case '3':
                        case '6':
                        case '9':
                            result = PhoneOperatorEnum.Velcom;
                            break;
                        default:
                            result = PhoneOperatorEnum.Unknown;
                            break;
                    }
                    break;
                case 033:
                    result = PhoneOperatorEnum.Mts;
                    break;
                case 044:
                    result = PhoneOperatorEnum.Velcom;
                    break;
                default:
                    result = PhoneOperatorEnum.Unknown;
                    break;
            }

            return result;
        }

        public static string GetPhoneOperatorIcon(this PhoneOperatorEnum operatorType)
        {
            string result;

            switch (operatorType)
            {
                case PhoneOperatorEnum.Beltelecom:
                    result = "phone_beltelecom.png";
                    break;
                case PhoneOperatorEnum.Mts:
                    result = "phone_mts.png";
                    break;
                case PhoneOperatorEnum.Velcom:
                    result = "phone_velcom.png";
                    break;
                case PhoneOperatorEnum.Life:
                    result = "phone_life.png";
                    break;
                default:
                    result = "phone_unknown.png";
                    break;
            }

            return result;
        }

        public static string FormatPhoneNumber(this string phoneNumber, out PhoneOperatorEnum phoneOperator)
        {
            var result = phoneNumber.NormalizePhoneNumber();

            if (!string.IsNullOrEmpty(result))
            {
                //remove extension number +375297000000x123
                var xIndex = result.IndexOf("x", StringComparison.OrdinalIgnoreCase);
                var number = xIndex > -1 ? result.Remove(xIndex) : result;
                var postfix = xIndex > -1 ? result.Substring(xIndex) : null;

                //check for length +375297000000
                if (number.Length == 13 && number.StartsWith("+"))
                {
                    var prefix = number.Substring(0, 4);
                    var code = number.Substring(4, 2);
                    var part1 = number.Substring(6, 3);
                    var part2 = number.Substring(9, 2);
                    var part3 = number.Substring(11, 2);

                    result = $"{prefix} ({code}) {part1}-{part2}-{part3}{postfix}";
                    phoneOperator = prefix == "+375" ? code.GetPhoneOperatorType(part1[0]) : PhoneOperatorEnum.Unknown;
                }
                //check for length 375297000000
                else if (number.Length == 12 && number.StartsWith("375"))
                {
                    var prefix = number.Substring(0, 3);
                    var code = number.Substring(3, 2);
                    var part1 = number.Substring(5, 3);
                    var part2 = number.Substring(8, 2);
                    var part3 = number.Substring(10, 2);

                    result = $"+{prefix} ({code}) {part1}-{part2}-{part3}{postfix}";
                    phoneOperator = prefix == "375" ? code.GetPhoneOperatorType(part1[0]) : PhoneOperatorEnum.Unknown;
                }
                //check for length 80297000000
                else if (number.Length == 11 && number.StartsWith("8"))
                {
                    var prefix = number.Substring(0, 1);
                    var code = number.Substring(1, 3);
                    var part1 = number.Substring(4, 3);
                    var part2 = number.Substring(7, 2);
                    var part3 = number.Substring(9, 2);

                    result = $"{prefix} ({code}) {part1}-{part2}-{part3}{postfix}";
                    phoneOperator = code.GetPhoneOperatorType(part1[0]);
                }
                else 
                    phoneOperator = PhoneOperatorEnum.Unknown;
            }
            else
                phoneOperator = PhoneOperatorEnum.Unknown;

            return result;
        }

        public static string ToRatingString(this DiscountRatingData discountRating)
        {
            return discountRating != null
                ? $"{(discountRating.RatingCount > 0 ? (double) discountRating.RatingSum / discountRating.RatingCount : 0):F1} ({discountRating.RatingCount:N0})"
                : null;
        }
    }
}
