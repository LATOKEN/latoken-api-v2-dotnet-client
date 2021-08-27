using System;

namespace Latoken_CSharp_Client_Library.Settings
{
    [Flags]
    public enum KeyPermission
    {
        Trade = 1,
        Data = 2,
        Transaction = 4
    }
}
