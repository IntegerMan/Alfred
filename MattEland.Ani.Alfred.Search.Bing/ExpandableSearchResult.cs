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


    public partial class ExpandableSearchResult
    {

        private Guid _ID;

        private long? _WebTotal;

        private long? _WebOffset;

        private long? _ImageTotal;

        private long? _ImageOffset;

        private long? _VideoTotal;

        private long? _VideoOffset;

        private long? _NewsTotal;

        private long? _NewsOffset;

        private long? _SpellingSuggestionsTotal;

        private string _AlteredQuery;

        private string _AlterationOverrideQuery;

        private System.Collections.ObjectModel.Collection<WebResult> _Web;

        private System.Collections.ObjectModel.Collection<ImageResult> _Image;

        private System.Collections.ObjectModel.Collection<VideoResult> _Video;

        private System.Collections.ObjectModel.Collection<NewsResult> _News;

        private System.Collections.ObjectModel.Collection<RelatedSearchResult> _RelatedSearch;

        private System.Collections.ObjectModel.Collection<SpellResult> _SpellingSuggestions;

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

        public long? WebTotal
        {
            get
            {
                return _WebTotal;
            }
            set
            {
                _WebTotal = value;
            }
        }

        public long? WebOffset
        {
            get
            {
                return _WebOffset;
            }
            set
            {
                _WebOffset = value;
            }
        }

        public long? ImageTotal
        {
            get
            {
                return _ImageTotal;
            }
            set
            {
                _ImageTotal = value;
            }
        }

        public long? ImageOffset
        {
            get
            {
                return _ImageOffset;
            }
            set
            {
                _ImageOffset = value;
            }
        }

        public long? VideoTotal
        {
            get
            {
                return _VideoTotal;
            }
            set
            {
                _VideoTotal = value;
            }
        }

        public long? VideoOffset
        {
            get
            {
                return _VideoOffset;
            }
            set
            {
                _VideoOffset = value;
            }
        }

        public long? NewsTotal
        {
            get
            {
                return _NewsTotal;
            }
            set
            {
                _NewsTotal = value;
            }
        }

        public long? NewsOffset
        {
            get
            {
                return _NewsOffset;
            }
            set
            {
                _NewsOffset = value;
            }
        }

        public long? SpellingSuggestionsTotal
        {
            get
            {
                return _SpellingSuggestionsTotal;
            }
            set
            {
                _SpellingSuggestionsTotal = value;
            }
        }

        public string AlteredQuery
        {
            get
            {
                return _AlteredQuery;
            }
            set
            {
                _AlteredQuery = value;
            }
        }

        public string AlterationOverrideQuery
        {
            get
            {
                return _AlterationOverrideQuery;
            }
            set
            {
                _AlterationOverrideQuery = value;
            }
        }

        public System.Collections.ObjectModel.Collection<WebResult> Web
        {
            get
            {
                return _Web;
            }
            set
            {
                _Web = value;
            }
        }

        public System.Collections.ObjectModel.Collection<ImageResult> Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
            }
        }

        public System.Collections.ObjectModel.Collection<VideoResult> Video
        {
            get
            {
                return _Video;
            }
            set
            {
                _Video = value;
            }
        }

        public System.Collections.ObjectModel.Collection<NewsResult> News
        {
            get
            {
                return _News;
            }
            set
            {
                _News = value;
            }
        }

        public System.Collections.ObjectModel.Collection<RelatedSearchResult> RelatedSearch
        {
            get
            {
                return _RelatedSearch;
            }
            set
            {
                _RelatedSearch = value;
            }
        }

        public System.Collections.ObjectModel.Collection<SpellResult> SpellingSuggestions
        {
            get
            {
                return _SpellingSuggestions;
            }
            set
            {
                _SpellingSuggestions = value;
            }
        }
    }
}
