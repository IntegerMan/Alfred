﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Notice: Use of the service proxies that accompany this notice is subject to
//            the terms and conditions of the license agreement located at
//            http://go.microsoft.com/fwlink/?LinkID=202740
//            If you do not agree to these terms you may not use this content.

using System;

namespace MattEland.Ani.Alfred.Search.Bing
{

    public partial class RelatedSearchResult
    {

        private Guid _ID;

        private string _Title;

        private string _BingUrl;

        public Guid ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        public string BingUrl
        {
            get
            {
                return _BingUrl;
            }
            set
            {
                _BingUrl = value;
            }
        }
    }
}