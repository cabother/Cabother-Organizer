using System.Collections.Generic;

namespace Cabother.Organizer.Api.ViewModels
{
    public class ErrorResponseViewModel : BaseResponseViewModel
    {
        public List<ErrorViewModel> Errors { get; set; }
    }
}