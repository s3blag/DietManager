using System.Collections.Generic;

namespace DM.Models.Wrappers
{
    public class IndexedResult<T> where T: class
    {
        public IList<T> Result { get; set; }

        public bool IsLast { get; set; }
    }
}
