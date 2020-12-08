using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentification.API.Routes
{
    public static class DefaultRoutes
    {
        public const string Root = "api";

        public const string Version = "v0";

        public const string Base = Root + "/" + Version;

        public static class Glossary
        {
            public const string Main = Base + "/glossary";

            public const string Term = Base + "/glossary/{term}";
        }
    }
}
