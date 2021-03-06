using Dracoon.Sdk.Error;
using System;
using System.Collections.Generic;
using System.IO;

namespace Dracoon.Sdk.SdkInternal.Validator {
    internal static class ValidatorExtensions {

        private static readonly string[] INVALID_PATH_CHARS = { "<", ">", ":", "\"", "|", "?", "*" };

        internal static void MustNotNull<T>(this T param, string paramName) {
            if (param == null) {
                throw new ArgumentNullException(paramName);
            }
        }

        internal static void EnumerableMustNotNullOrEmpty<T>(this IEnumerable<T> param, string paramName) {
            param.MustNotNull(paramName);
            if (param is ICollection<T> colParam && colParam.Count == 0) {
                throw new ArgumentException(paramName + " cannot be empty.");
            }
        }

        internal static bool CheckEnumerableNullOrEmpty<T>(this IEnumerable<T> param) {
            if (param == null) {
                return true;
            }
            if (param is ICollection<T> colParam && colParam.Count == 0) {
                return true;
            }
            return false;
        }

        internal static void MustNotNullOrEmptyOrWhitespace(this string param, string paramName, bool nullAllowed = false) {
            if (!nullAllowed) {
                param.MustNotNull(paramName);
            } else if (param == null) {
                return;
            }

            if (param.Length == 0) {
                throw new ArgumentException(paramName + " cannot be empty.");
            }

            int whitespaces = 0;
            for (int i = 0; i < param.Length; i++) {
                if (char.IsWhiteSpace(param[i])) {
                    whitespaces++;
                }
            }
            if (param.Length == whitespaces) {
                throw new ArgumentException(paramName + " cannot be whitespace.");
            }
        }

        internal static void MustValidNodePath(this string param, string paramName) {
            param.MustNotNullOrEmptyOrWhitespace(paramName);
            if (param == "/") {
                throw new ArgumentException("You cannot request the root because it has no usable node object.");
            }
            if (!param.StartsWith("/")) {
                throw new ArgumentException("The node path must start with '/'.");
            }
            if (param.EndsWith("/")) {
                throw new ArgumentException("The node path cannot end with '/'.");
            }

            List<string> foundInvalidChars = new List<string>(INVALID_PATH_CHARS.Length);
            foreach (string current in INVALID_PATH_CHARS) {
                if (param.Contains(current)) {
                    foundInvalidChars.Add(current);
                }
            }
            if (foundInvalidChars.Count > 0) {
                throw new ArgumentException("The node path cannot contain " + string.Join(",", foundInvalidChars.ToArray()) + ".");
            }
        }

        internal static void CheckStreamCanRead(this Stream param, string paramName) {
            param.MustNotNull(paramName);
            if (!param.CanRead) {
                throw new DracoonFileIOException("Cannot read from the given stream.");
            }
        }

        internal static void CheckStreamCanWrite(this Stream param, string paramName) {
            param.MustNotNull(paramName);
            if (!param.CanWrite) {
                throw new DracoonFileIOException("Cannot write to the given stream.");
            }
        }

        internal static void MustBeValid(this Uri param, string paramName) {
            param.MustNotNull(paramName);
            if (String.IsNullOrWhiteSpace(param.Scheme) || !(param.Scheme == Uri.UriSchemeHttp || param.Scheme == Uri.UriSchemeHttps)) {
                throw new ArgumentException("Server URI can only have protocol http or https.");
            }
            if (!String.IsNullOrWhiteSpace(param.UserInfo)) {
                throw new ArgumentException("Server URI cannot have a user.");
            }
            if (!String.IsNullOrWhiteSpace(param.Query)) {
                throw new ArgumentException("Server URI cannot have a query.");
            }
        }

        #region Numeric checks

        internal static void MustPositive(this long param, string paramName) {
            param.MustNotNull(paramName);
            if (param <= 0) {
                throw new ArgumentException(paramName + " cannot be negative or 0.");
            }
        }

        internal static void MustPositive(this int param, string paramName) {
            param.MustNotNull(paramName);
            if (param <= 0) {
                throw new ArgumentException(paramName + " cannot be negative or 0.");
            }
        }

        internal static void MustNotNegative(this long param, string paramName) {
            param.MustNotNull(paramName);
            if (param < 0) {
                throw new ArgumentException(paramName + " cannot be negative.");
            }
        }

        internal static void MustNotNegative(this int param, string paramName) {
            param.MustNotNull(paramName);
            if (param < 0) {
                throw new ArgumentException(paramName + " cannot be negative.");
            }
        }

        internal static void MustPositive(this long? param, string paramName) {
            if (param.HasValue && param.Value <= 0) {
                throw new ArgumentException(paramName + " cannot be negative or 0.");
            }
        }

        internal static void MustPositive(this int? param, string paramName) {
            if (param.HasValue && param.Value <= 0) {
                throw new ArgumentException(paramName + " cannot be negative or 0.");
            }
        }

        internal static void MustNotNegative(this long? param, string paramName) {
            if (param.HasValue && param.Value < 0) {
                throw new ArgumentException(paramName + " cannot be negative.");
            }
        }

        internal static void MustNotNegative(this int? param, string paramName) {
            if (param.HasValue && param.Value < 0) {
                throw new ArgumentException(paramName + " cannot be negative.");
            }
        }

        #endregion
    }
}
