using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MattEland.Ani.Alfred.Core
{
    public class SimpleCollectionProvider : ICollectionProvider
    {
        public ICollection<T> GenerateCollection<T>()
        {
            return new Collection<T>();
        }
    }
}
