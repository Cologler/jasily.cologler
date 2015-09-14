#if WINDOWS_UWP

using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

#endif

namespace System
{
    public static class JasilyDeviceInfo
    {
        internal const string DefaultString = "Unknown";

        public static string Manufacturer { get; }

        public static string DeviceModel { get; }

        static JasilyDeviceInfo()
        {
#if WINDOWS_UWP

            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            Manufacturer = eas.SystemManufacturer;
            DeviceModel = eas.SystemProductName;

#else

            Manufacturer = DefaultString;
            DeviceModel = DefaultString;

#endif
        }

        public static class SystemInfo
        {
            public static Version OSVersion { get; }

            public static Version RuntimeVersion { get; }

#if WINDOWS_UWP

            public static string SystemFamilyName { get; } = AnalyticsInfo.VersionInfo.DeviceFamily;

#endif

            static SystemInfo()
            {
#if WINDOWS_UWP

                // os version
                var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                var v = ulong.Parse(sv);
                ulong v1 = (v & 0xFFFF000000000000L) >> 48;
                ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
                ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
                ulong v4 = (v & 0x000000000000FFFFL);
                string version = $"{v1}.{v2}.{v3}.{v4}";
                OSVersion = new Version((int) v1, (int) v2, (int) v3, (int) v4);

                // runtime version
                RuntimeVersion = new Version(0, 0, 0, 0);
#else
                OSVersion = Environment.OSVersion.Version;

                RuntimeVersion = Environment.Version;
#endif
            }

        }
    }
}