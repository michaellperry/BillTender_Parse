using System;
using Parse;

namespace BillTender
{
    public partial class ParseConfig
    {
        public static void Initialize()
        {
            ParseObject.RegisterSubclass<Budget.Models.Bill>();

            var config = new ParseConfig();
            config.InitializeParse();
        }

        partial void InitializeParse();
    }
}

/*
Create a file called ParseConfig_Keys.cs that contains your Parse API keys.
This file is in .gitignore so that your keys will not be checked in to source control.

using Parse;

namespace BillTender
{
    public partial class ParseConfig
    {
        partial void InitializeParse()
        {
            ParseClient.Initialize("Super", "Secret");
        }
    }
}

*/