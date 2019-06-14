using Aran.Panda.RNAV.RNPAR.Properties;
using Aran.Panda.RNAV.RNPAR.UI.ViewModel;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class ApplicationContext
    {

        public MainViewModel MainViewModel { get; set; } 
        public int LangCode { get; protected set; }
        private AppEnvironment _environment;
        public ApplicationContext(AppEnvironment environment)
        {
            _environment = environment;
            LangCode = environment.Settings.Language;
            NativeMethods.SetThreadLocale(LangCode);
            Resources.Culture = new System.Globalization.CultureInfo(LangCode);

        }

    }
}
