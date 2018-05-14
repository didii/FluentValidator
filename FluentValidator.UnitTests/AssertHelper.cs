using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace FluentValidator.UnitTests
{
    internal static class AssertHelper
    {
        public static void MessageCount(ValidationMessages messages, int count) {
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages, Has.Count.EqualTo(count));
            foreach (var message in messages) {
                Assert.That(message.Title, Is.Not.Null.And.Not.Empty);
                Assert.That(message.Message, Is.Not.Null.And.Not.Empty);
                Assert.That(message.Path, Is.Not.Null.And.Not.Empty);
            }
        }

        public static void Path(string path, params string[] fullPaths) {
            foreach (var fullPath in fullPaths)
                Assert.That(path, Does.Match($@"(^| |,){Regex.Escape(fullPath)}($| |,)"));
        }

    }
}
