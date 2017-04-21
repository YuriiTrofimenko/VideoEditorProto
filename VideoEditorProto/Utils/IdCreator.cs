using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoEditorProto.Utils
{
    public class IdCreator
    {
        public static string createGuid(string _prefix)
        {
            return _prefix + Guid.NewGuid().ToString();
        }

        public static string createUserGuid()
        {
            return createGuid("u-");
        }

        public static string createProjectGuid()
        {
            return createGuid("p-");
        }

        public static string createLayerGuid()
        {
            return createGuid("l-");
        }

        public static string createRowGuid()
        {
            return createGuid("r-");
        }
    }
}