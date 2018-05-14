using System.Linq;
using NUnit.Framework;

namespace FluentValidator.UnitTests {
    internal class LengthTests {
        private ValidationBuilder<ParentObject> _sut;

        [Test]
        public void Length_NoBoundaries_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject();
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_Null_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1).Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Length_MinBoundaryWithSameObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, min: 2).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MinBoundaryWithLessObjects_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, min: 2).Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            Assert.That(messages.First().Path, Is.EqualTo("/Ints"));
        }

        [Test]
        public void Length_MinBoundaryWithMoreObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2, 3}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, min: 2).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MaxBoundaryWithSameObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2, 3}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, max: 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MaxBoundaryWithLessObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, max: 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MaxBoundaryWithMoreObjects_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2, 3, 4}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, max: 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Length_MinMaxBoundaryWithInBetween_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1, 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MinMaxBoundaryWithLessObjects_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new int[] { }};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1, 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Length_MinMaxBoundaryWithMinObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1, 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MinMaxBoundaryWithMaxObjects_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2, 3}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1, 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }

        [Test]
        public void Length_MinMaxBoundaryWithMoreObjects_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Ints = new[] {1, 2, 3, 4}};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Ints, 1, 3).Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/Ints");
        }

        [Test]
        public void Length_WithString_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {Name = "Hallo!"};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.Length(o => o.Name, 5, 10).Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }
    }
}