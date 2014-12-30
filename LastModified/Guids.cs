// Guids.cs
// MUST match guids.h
using System;

namespace bssthu.LastModified
{
    static class GuidList
    {
        public const string guidLastModifiedPkgString = "c8098ef2-6ea8-4c78-8150-9cca18bd888b";
        public const string guidLastModifiedCmdSetString = "15526a4a-b421-4b3b-bef4-aea531071bae";

        public static readonly Guid guidLastModifiedCmdSet = new Guid(guidLastModifiedCmdSetString);
    };
}