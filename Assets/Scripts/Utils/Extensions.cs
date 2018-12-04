using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.Utils
{
    public static class Extensions
    {
        public static int OnePastLastSlash(this string str)
        {
            int result = str.LastIndexOf('/') + 1;
            return result;
        }
    }
}
