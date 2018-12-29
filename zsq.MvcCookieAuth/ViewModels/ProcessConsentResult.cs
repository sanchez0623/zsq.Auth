namespace zsq.MvcCookieAuth.ViewModels
{
    public class ProcessConsentResult
    {
        public string ReturnUrl { get; set; }

        public bool IsRedirect => ReturnUrl != null;

        public ConsentViewModel ViewModel { get; set; }

        public string ValidationError { get; set; }
    }
}