using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidator.UnitTests {
    internal class ParentObject {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }
        public ICollection<int> Ints { get; set; }
        public long NestedId { get; set; }
        public NestedObject Nested { get; set; }
        public NestedObject[] Nesteds {get; set; }

        public bool Test(Func<NestedObject, bool> predicate) {
            return Nesteds.All(predicate);
        }
    }
}