using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoEditorProto.Utils
{
    public class IdCreator
    {
        public static string createUserGuid()
        {
            return "u-" + Guid.NewGuid().ToString();
        }

        public static string createProjectGuid()
        {
            return "p-" + Guid.NewGuid().ToString();
        }
    }
}