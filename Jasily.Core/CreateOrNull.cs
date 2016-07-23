namespace System
{
    public static class CreateOrNull
    {
        public static Uri CreateUri(string uriString, UriKind uriKind)
        {
            Uri uri;
            return Uri.TryCreate(uriString, uriKind, out uri) ? uri : null;
        }
    }
}