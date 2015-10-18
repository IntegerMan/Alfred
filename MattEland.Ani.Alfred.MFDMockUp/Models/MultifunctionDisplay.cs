using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant.Fields;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    public sealed class MultifunctionDisplay
    {
        private readonly Observable<string> _name = new Observable<string>();

        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }
    }
}
