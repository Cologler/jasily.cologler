namespace System
{
    public static class Can
    {
        public static bool CreateUri(string uriString, UriKind uriKind)
        {
            Uri uri;
            return Uri.TryCreate(uriString, uriKind, out uri);
        }
    }
}