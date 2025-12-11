namespace PetCare.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<string> ValidationErrors { get; }

        public BadRequestException(string message) : base(message)
        {
            ValidationErrors = new List<string>();
        }

        public BadRequestException(string message, List<string> errors) : base(message)
        {
            ValidationErrors = errors;
        }
    }
}
