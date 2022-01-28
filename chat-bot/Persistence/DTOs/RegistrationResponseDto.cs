using System.Collections.Generic;

namespace Chat.Persistence.DTOs
{
    public sealed class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
