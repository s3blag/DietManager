using System;
using System.Collections;

namespace DM.Models.Wrappers
{
    public class IndexedResult<T> where T: class
    {
        public T Result { get; set; }

        public bool IsLast { get; set; }

        public int Index { get; set; }

    }
}
