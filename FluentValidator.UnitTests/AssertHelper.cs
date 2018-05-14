using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FluentValidator.UnitTests {
    internal static class AssertHelper {
        public static void MessageCount(ValidationMessages messages, int count) {
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages, Has.Count.EqualTo(count));
            foreach (var message in messages) {
                Assert.That(message.Title, Is.Not.Null.And.Not.Empty);
                Assert.That(message.Message, Is.Not.Null.And.Not.Empty);
                Assert.That(message.Paths.ToList(), Has.Count.GreaterThanOrEqualTo(1));
                foreach (var path in message.Paths)
                    Assert.That(path, Is.Not.Null.And.Not.Empty.And.Match(@"(/[a-zA-Z0-9_`]+)+"));
            }
        }

        public static void Paths(IEnumerable<string> paths, params string[] fullPaths) {
            paths = paths.ToList();
            var builder = new ConstraintBuilder();
            builder.Append(Has.Count.EqualTo(fullPaths.Length));
            foreach (var fullPath in fullPaths)
                builder.Append(Does.Contain(fullPath));
            Assert.That(paths, builder.Resolve());
        }
    }
}