namespace Windows.System.Profile
{
    public static class AnalyticsVersionInfoExtensions
    {
        public static DeviceFamilyType GetDeviceFamilyType(this AnalyticsVersionInfo info)
        {
            switch (info.DeviceFamily)
            {
                case "Desktop":
                    return DeviceFamilyType.Desktop;
                case "Mobile":
                    return DeviceFamilyType.Mobile;
                default:
                    return DeviceFamilyType.Unknown;
            }
        }
    }
}