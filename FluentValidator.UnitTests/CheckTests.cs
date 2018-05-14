using System.Linq;
using NUnit.Framework;

namespace FluentValidator.UnitTests {
    internal class CheckTests {
        private ValidationBuilder<ParentObject> _sut;

        [Test]
        public void Check_BinaryExpression_ReturnsCorrectValidationMessage() {
            var obj = new ParentObject() {Name = "test"};
            _sut = new ValidationBuilder<ParentObject>(obj);

            var messages = _sut.Check(o => o.Name != "test")
                               .Build();

            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Name");
        }

        [Test]
        public void Check_MethodExpression_ReturnsCorrectValidationMessage() {
            var obj = new ParentObject() {Name = "test"};
            _sut = new ValidationBuilder<ParentObject>(obj);

            var messages = _sut.Check(o => o.Name.StartsWith("nope"))
                               .Build();

            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Name");
        }

        [Test]
        public void Check_LinqExpression_ReturnsCorrectValidationMessage() {
            var obj = new ParentObject() {Ints = new[] {0, 2, 4, 5, 8, 10}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            var messages = _sut.Check(o => o.Ints.All(i => i % 2 == 0))
                               .Build();

            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Check_CombinedLinqExpression_ReturnsCorrectValidationMessage() {
            var obj = new ParentObject() {Ints = new[] {0, 2, 4, 5, 8, 10}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            var messages = _sut.Check(o => o.Ints.Count(i => i % 2 == 0) > 10)
                               .Build();

            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Check_MultipleMembers_ReturnsCorrectValidationMessage() {
            var obj = new ParentObject() {Amount = 10, Ints = new[] {0, 1, 2, 3, 4, 5}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            var messages = _sut.Check(o => o.Ints.Count() == o.Amount)
                               .Build();

            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Amount", "/Ints");
        }

        [Test]
        public void Check_NestedMember_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Nested = new NestedObject() {Name = "test"}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Check(o => o.Nested.Name == "nope")
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Nested/Name");
        }

        [Test]
        public void Check_NestedLinq_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {
                Nesteds = new NestedObject[] {
                    new NestedObject() {Id = 1, Name = "One"},
                    new NestedObject() {Id = 2, Name = "Two"},
                    new NestedObject() {Id = 3, Name = "Three"},
                }
            };
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Check(o => o.Nesteds.All(n => n.Name == "nope"))
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Nesteds/Name");
        }
    }
}