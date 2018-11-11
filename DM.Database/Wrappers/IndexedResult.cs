using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DM.Models.Wrappers
{
    public class IndexedResult<T> where T: class
    {
        public T Result { get; set; }

        public bool IsLast { get; set; }

        public int Index { get; set; }
    }
}
