using System;
using System.Collections.Generic;

namespace RolePlayOverlord.Utils
{
    public static class Debug
    {
        public static void Assert(bool expr)
        {
            if(!expr)
                throw new NullReferenceException();
        }
    }
}
