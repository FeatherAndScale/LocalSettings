using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Scale.Tests
{
    [TestClass]
    public class LocalSettingsTests
    {
        [TestMethod]
        public void LoadSettings_ReloadingSettingThatDidNotExistInOriginalConfig_AppSettingIsReset()
        {
            var settings1 = new NameValueCollection();
            settings1.Add("test1", "value1");

            LocalSettings.LoadSettings(settings1);

            var settings2 = new NameValueCollection();
            settings2.Add("test1", "value2");

            Assert.AreEqual("value2", System.Configuration.ConfigurationManager.AppSettings["test1"]);
        }
    }
}
