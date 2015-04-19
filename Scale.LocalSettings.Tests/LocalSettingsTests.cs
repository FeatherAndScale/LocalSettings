using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Scale.Tests
{
    [TestClass]
    public class LocalSettingsTests
    {
        [TestMethod]
        public void GetSettings()
        {
            var settings = LocalSettings.Settings;
            foreach (string setting in settings)
            {
                Trace.WriteLine(setting + " = " + settings[setting]);
            }
        }

        [TestMethod]
        public void Refresh()
        {
            LocalSettings.Refresh();
            var settings = LocalSettings.Settings;
            foreach (string setting in settings)
            {
                Trace.WriteLine(setting + " = " + settings[setting]);
            }
        }
    }
}
