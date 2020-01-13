namespace ZipPay.User.Domain.Models
{
    public class BusinessRulesValidation
    {
        public bool IsValid { get; set; }

        public string ErrorCode { get; set; }

        public string Error { get; set; }

        public BusinessRulesValidation()
        { }

        public BusinessRulesValidation(bool isValid)
        {
            IsValid = isValid;
        }

        public BusinessRulesValidation(bool isValid, string errorCode, string error)
        {
            IsValid = isValid;
            ErrorCode = errorCode;
            Error = error;
        }
    }
}