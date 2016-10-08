using System;
using System.Resources;
using System.Threading;
using BassinoLibrary.Resource;

namespace BassinoLibrary.Utils
{
    public static class EnumExtensions
    {
        public static string GetDescriptions(this Enum value)
        {
            if (value == null)
                return string.Empty;

            var resources = new ResourceManager(typeof (Resources));

            var resourceKey = String.Format("{0}{1}", value.GetType().Name, value);
            var localizedDescription = resources.GetString(resourceKey, Thread.CurrentThread.CurrentCulture);

            return localizedDescription ?? value.ToString();
        }
    }
}