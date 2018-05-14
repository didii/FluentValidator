using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace FluentValidator.UnitTests
{
    public class ValidationBuilderTests {

        [Test]
        public void Test() {
            Expression<Func<ParentObject, bool>> expression1 = o => o.Nesteds.All(n => n.Name == "nope");
            Expression<Func<ParentObject, bool>> expression2 = o => o.Test(n => n.Name == "nope");

        }
    }
}
