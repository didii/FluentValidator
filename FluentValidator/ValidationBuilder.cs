using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentValidator {
    internal class ValidationBuilder<TObject> : IValidationBuilder<TObject> {

        private const string RequireTitle = "A required field is missing";
        private const string RequireAnyMessage = "One of these properties must have a non-null or non-zero value";
        private TObject _obj;
        private ValidationMessages _result;
        private EValidationSeverity _severity = EValidationSeverity.Error;

        public ValidationBuilder() { }

        public ValidationBuilder(TObject obj) {
            Start(obj);
        }

        public IValidationBuilder<TObject> Start(TObject objectToValidate) {
            _obj = objectToValidate;
            _result = new ValidationMessages();
            _severity = EValidationSeverity.Error;
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> AsError() {
            _severity = EValidationSeverity.Error;
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> AsWarning() {
            _severity = EValidationSeverity.Warning;
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Check(Expression<Func<TObject, bool>> check) {
            var members = FindMemberExpressions(check).ToArray();
            if (members == null || !members.Any())
                throw new Exception("At least 1 property was expected to exist in the expression");

            var valid = check.Compile()(_obj);
            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = "A custom check failed",
                    Message = $"The check {check.Body} evaluated to false",
                    Paths = GetPropertyPaths(members)
                });
            }
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Check<TProp>(Expression<Func<TObject, TProp>> expression, Expression<Func<TProp, bool>> check) {
            var value = expression.Compile()(_obj);
            var valid = check.Compile()(value);
            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = "A custom check failed",
                    Message = $"The expression {check.Body} evaluated to false",
                    Paths = GetPropertyPath(expression)
                });
            }
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Require<TProperty>(Expression<Func<TObject, TProperty>> expression) {
            var value = expression.Compile()(_obj);

            if (!IsValid(value)) {
                AddMessage(new ValidationMessage() {
                    Title = RequireTitle,
                    Message = "This property must have a non-null or non-zero value",
                    Paths = GetPropertyPath(expression)
                });
            }
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> RequireAny<TProp1, TProp2>(Expression<Func<TObject, TProp1>> expression1,
                                                                      Expression<Func<TObject, TProp2>> expression2) {
            var value1 = expression1.Compile()(_obj);
            var value2 = expression2.Compile()(_obj);
            var valid = IsValid(value1) || IsValid(value2);

            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = RequireTitle,
                    Message = RequireAnyMessage,
                    Paths = GetPropertyPaths(new[] {
                        expression1.Body as MemberExpression,
                        expression2.Body as MemberExpression
                    })
                });
            }
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> RequireAny<TProp1, TProp2, TProp3>(Expression<Func<TObject, TProp1>> expression1,
                                                                              Expression<Func<TObject, TProp2>> expression2,
                                                                              Expression<Func<TObject, TProp3>> expression3) {
            var value1 = expression1.Compile()(_obj);
            var value2 = expression2.Compile()(_obj);
            var value3 = expression3.Compile()(_obj);
            var valid = IsValid(value1) || IsValid(value2) || IsValid(value3);
            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = RequireTitle,
                    Message = RequireAnyMessage,
                    Paths = GetPropertyPaths(new[] {
                        expression1.Body as MemberExpression,
                        expression2.Body as MemberExpression,
                        expression3.Body as MemberExpression
                    })
                });
            }
            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> RequireAny<TProp1, TProp2, TProp3, TProp4>(Expression<Func<TObject, TProp1>> expression1,
                                                                                      Expression<Func<TObject, TProp2>> expression2,
                                                                                      Expression<Func<TObject, TProp3>> expression3,
                                                                                      Expression<Func<TObject, TProp4>> expression4) {
            var value1 = expression1.Compile()(_obj);
            var value2 = expression2.Compile()(_obj);
            var value3 = expression3.Compile()(_obj);
            var value4 = expression4.Compile()(_obj);
            var valid = IsValid(value1) || IsValid(value2) || IsValid(value3) || IsValid(value4);
            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = RequireTitle,
                    Message = RequireAnyMessage,
                    Paths = GetPropertyPaths(new[] {
                        expression1.Body as MemberExpression,
                        expression2.Body as MemberExpression,
                        expression3.Body as MemberExpression,
                        expression4.Body as MemberExpression, 
                    })
                });
            }

            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Length(Expression<Func<TObject, IEnumerable>> expression, int exact) {
            var value = Count(expression.Compile()(_obj));
            var valid = value == exact;

            if (!valid) {
                AddMessage(new ValidationMessage() {
                    Title = "A field does not match the required length constraint",
                    Message = $"This property must have a length equal to {exact} (actual: {value})",
                    Paths = GetPropertyPath(expression)
                });
            }

            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Length(Expression<Func<TObject, IEnumerable>> expression, int? min = null, int? max = null) {
            var value = Count(expression.Compile()(_obj));
            var valid = (!min.HasValue || value >= min.Value) && (!max.HasValue || value <= max.Value);

            if (!valid) {
                var message = "This property must have a length ";
                if (min.HasValue && max.HasValue)
                    message += $"between {min} and {max}";
                else if (min.HasValue)
                    message += $"greater than {min}";
                else
                    message += $"smaller than {max}";
                message += $" (actual: {value})";
                AddMessage(new ValidationMessage() {
                    Title = "A field does not match the required length constraint",
                    Message = message,
                    Paths = GetPropertyPath(expression)
                });
            }

            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Equals<TProp>(Expression<Func<TObject, TProp>> expression, TProp expected) {
            return Equals(expression, expected, (lhs, rhs) => Equals(lhs, rhs));
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> Equals<TProp>(Expression<Func<TObject, TProp>> expression, TProp expected,
                                                         Func<TProp, TProp, bool> comparer) {
            var value = expression.Compile()(_obj);

            if (!comparer(value, expected)) {
                AddMessage(new ValidationMessage() {
                    Title = "A property has a wrong value",
                    Message = $"Value {expected} was expected but got {value}",
                    Paths = GetPropertyPath(expression),
                    Data = {{"Expected", expected}, {"Actual", value}}
                });
            }

            return this;
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> NotEquals<TProp>(Expression<Func<TObject, TProp>> expression, TProp value) {
            return NotEquals(expression, value, (lhs, rhs) => Equals(lhs, rhs));
        }

        /// <inheritdoc />
        public IValidationBuilder<TObject> NotEquals<TProp>(Expression<Func<TObject, TProp>> expression, TProp value,
                                                            Func<TProp, TProp, bool> comparer) {
            var orig = expression.Compile()(_obj);

            if (comparer(orig, value)) {
                AddMessage(new ValidationMessage() {
                    Title = "A property has a forbidden value",
                    Message = $"Value {value} is forbidden",
                    Paths = GetPropertyPath(expression)
                });
            }

            return this;
        }


        /// <inheritdoc />
        public ValidationMessages Build() {
            return _result;
        }

#region Helpers

        private void AddMessage(ValidationMessage message) {
            message.ValidationSeverity = _severity;
            _result.Add(message);
        }

        private IEnumerable<string> GetPropertyNames(MemberExpression expression) {
            var result = new Stack<string>();
            while (expression != null) {
                result.Push(expression.Member.Name);
                expression = FindMemberExpressions(expression.Expression).FirstOrDefault();
            }
            return result;
        }



        private IEnumerable<string> GetPropertyPath<TProp>(Expression<Func<TObject, TProp>> expression) {
            return GetPropertyPath(expression.Body as MemberExpression);
        }

        private IEnumerable<string> GetPropertyPath(MemberExpression expression) {
            return new []{"/" + GetPropertyNames(expression).Aggregate((a, b) => $"{a}/{b}")};
        }

        private IEnumerable<string> GetPropertyPaths(IEnumerable<MemberExpression> expression) {
            return expression.Select(e => "/" + GetPropertyNames(e).Aggregate((a, b) => $"{a}/{b}"));
        }

        private string Combine(string separator, params string[] strings) {
            return strings.Where(string.IsNullOrWhiteSpace).Aggregate((a, b) => a + separator + b);
        }

        private int Count(IEnumerable source) {
            if (source == null)
                return 0;
            var count = 0;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext()) {
                count++;
            }
            return count;
        }

        private bool IsValid<T>(T value) {
            bool valid = value != null;
            if (valid && value is long) {
                var id = (value as long?).Value;
                if (id == 0)
                    valid = false;
            }
            if (valid && value is string) {
                var str = value as string;
                if (string.IsNullOrEmpty(str))
                    valid = false;
            }
            return valid;
        }

        private IEnumerable<MemberExpression> FindMemberExpressions(Expression expression) {
            var result = new List<MemberExpression>();
            FindMemberExpressions(expression, result);
            return result;
        }

        private void FindMemberExpressions(Expression expression, ICollection<MemberExpression> current) {
            if (expression == null)
                return;
            switch (expression) {
                case BinaryExpression binaryExpression:
                    FindMemberExpressions(binaryExpression.Left, current);
                    FindMemberExpressions(binaryExpression.Right, current);
                    break;
                case BlockExpression blockExpression:
                    foreach (var e in blockExpression.Expressions)
                        FindMemberExpressions(e, current);
                    break;
                case ConditionalExpression conditionalExpression:
                    FindMemberExpressions(conditionalExpression.IfTrue, current);
                    FindMemberExpressions(conditionalExpression.IfFalse, current);
                    break;
                case IndexExpression indexExpression:
                    foreach (var argument in indexExpression.Arguments)
                        FindMemberExpressions(argument, current);
                    break;
                case InvocationExpression invocationExpression:
                    FindMemberExpressions(invocationExpression.Expression);
                    foreach (var argument in invocationExpression.Arguments)
                        FindMemberExpressions(argument, current);
                    break;
                case LabelExpression labelExpression:
                    FindMemberExpressions(labelExpression.DefaultValue, current);
                    break;
                case LambdaExpression lambdaExpression:
                    FindMemberExpressions(lambdaExpression.Body, current);
                    break;
                case LoopExpression loopExpression:
                    FindMemberExpressions(loopExpression.Body, current);
                    break;
                case MemberExpression memberExpression:
                    current.Add(memberExpression);
                    break;
                case MethodCallExpression methodCallExpression:
                    FindMemberExpressions(methodCallExpression.Object, current);
                    foreach (var argument in methodCallExpression.Arguments)
                        FindMemberExpressions(argument, current);
                    break;
                case NewArrayExpression newArrayExpression:
                    foreach (var e in newArrayExpression.Expressions)
                        FindMemberExpressions(e, current);
                    break;
                case NewExpression newExpression:
                    foreach (var argument in newExpression.Arguments)
                        FindMemberExpressions(argument, current);
                    break;
                case RuntimeVariablesExpression runtimeVariablesExpression:
                    foreach (var var in runtimeVariablesExpression.Variables)
                        FindMemberExpressions(var, current);
                    break;
                case SwitchExpression switchExpression:
                    FindMemberExpressions(switchExpression.SwitchValue, current);
                    foreach (var cases in switchExpression.Cases) {
                        FindMemberExpressions(cases.Body, current);
                        foreach (var test in cases.TestValues)
                            FindMemberExpressions(test, current);
                    }
                    FindMemberExpressions(switchExpression.DefaultBody);
                    break;
                case TypeBinaryExpression typeBinaryExpression:
                    FindMemberExpressions(typeBinaryExpression.Expression, current);
                    break;
                case UnaryExpression unaryExpression:
                    FindMemberExpressions(unaryExpression.Operand, current);
                    break;
                case ConstantExpression constantExpression:
                case DebugInfoExpression debugInfoExpression:
                case DefaultExpression defaultExpression:
                case ListInitExpression listInitExpression:
                case ParameterExpression parameterExpression:
                    break;
                case DynamicExpression dynamicExpression:
                case GotoExpression gotoExpression:
                case MemberInitExpression memberInitExpression:
                case TryExpression tryExpression:
                default:
                    throw new NotSupportedException();
            }

        }
#endregion
    }
}