using System.Linq;
using NUnit.Framework;

namespace FluentValidator.UnitTests {
    internal class RequireTests {
        private ValidationBuilder<ParentObject> _sut;

        [Test]
        public void Require_IdZero_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Id = 0};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Id)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Paths(messages.First().Paths, "/Id");
        }

        [Test]
        public void Require_IdNonZero_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Id = 1};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Id)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Require_StringNull_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Name = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Name)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Paths(messages.First().Paths, "/Name");
        }

        [Test]
        public void Require_StringEmpty_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Name = ""};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Name)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Paths(messages.First().Paths, "/Name");
        }

        [Test]
        public void Require_StringValid_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Name = "SomeName"};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Name)
                               .Build();

            //Assert
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages, Has.Count.EqualTo(0));
        }

        [Test]
        public void Require_NullableNull_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Amount = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Amount)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Paths(messages.First().Paths, "/Amount");
        }

        [Test]
        public void Require_NullableWithValue_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Amount = 5};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Amount)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Require_ObjectNull_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Nested = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Nested)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Paths(messages.First().Paths, "/Nested");
        }

        [Test]
        public void Require_ObjectNotNull_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Nested = new NestedObject()};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Require(o => o.Nested)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }
    }
}