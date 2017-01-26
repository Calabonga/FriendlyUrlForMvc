using System;
using System.Text;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {

    /// <summary>
    /// Exception Inner message helper
    /// </summary>
    public static class ExceptionHelper {
        public static string GetMessages(Exception exception) {
            return GetErrorMessage(exception);
        }

        private static string GetErrorMessage(Exception exception) {
            var sb = new StringBuilder();
            sb.AppendFormat(exception.Message);
            if (exception.InnerException != null) {
                sb.AppendLine(GetErrorMessage(exception.InnerException));
            }
            return sb.ToString();
        }
    }
}
