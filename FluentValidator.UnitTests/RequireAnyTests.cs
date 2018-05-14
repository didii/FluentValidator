using System.Linq;
using NUnit.Framework;

namespace FluentValidator.UnitTests {
    class RequireAnyTests {
        private ValidationBuilder<ParentObject> _sut;

        [Test]
        public void RequireAny_NoneValid_ReturnsCorrectValidationMessage() {
            //Arrange
            var obj = new ParentObject() {NestedId = 0, Nested = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.RequireAny(o => o.NestedId, o => o.Nested)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 1);
            AssertHelper.Path(messages.First().Path, "/NestedId", "/Nested");
        }

        [Test]
        public void RequireAny_OneValid_ReturnsEmptyValidationMessage() {
            //Arrange
            var obj = new ParentObject() {NestedId = 5, Nested = null};
            _sut = new ValidationBuilder<ParentObject>(obj);

            //Act
            var messages = _sut.RequireAny(o => o.NestedId, o => o.Nested)
                               .Build();

            //Assert
            AssertHelper.MessageCount(messages, 0);
        }
    }
}