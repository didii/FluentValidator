using System;
using System.Collections;
using System.Linq.Expressions;

namespace FluentValidator {
    /// <summary>
    /// Allows for validation of a model using a fluent API.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IValidationBuilder<TObject> {
        /// <summary>
        /// Starts the validation process on the given <paramref name="objectToValidate" />. This method must be called before all other methods.
        /// </summary>
        /// <param name="objectToValidate">The object to validate</param>
        /// <returns></returns>
        IValidationBuilder<TObject> Start(TObject objectToValidate);

        /// <summary>
        /// Marks all following validation checks to be errors. The default when <see cref="Start"/> is called
        /// </summary>
        /// <returns></returns>
        IValidationBuilder<TObject> AsError();

        /// <summary>
        /// Marks all following validation checks to be warnings
        /// </summary>
        /// <returns></returns>
        IValidationBuilder<TObject> AsWarning();

        IValidationBuilder<TObject> Check(Expression<Func<TObject, bool>> check);

        /// <summary>
        /// A custom validation check where <paramref name="check"/> should evaluate to true to succeed the validation
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Check<TProp>(Expression<Func<TObject, TProp>> expression, Expression<Func<TProp, bool>> check);

        /// <summary>
        /// Requires the property to have a valid (non-null and non-zero) value
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Require<TProp>(Expression<Func<TObject, TProp>> expression);

        /// <summary>
        /// Requires any of the given properties to have a valid (non-null and non-zero) value
        /// </summary>
        /// <typeparam name="TProp1"></typeparam>
        /// <typeparam name="TProp2"></typeparam>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> RequireAny<TProp1, TProp2>(Expression<Func<TObject, TProp1>> expression1,
                                                               Expression<Func<TObject, TProp2>> expression2);

        /// <inheritdoc cref="RequireAny{TProp1,TProp2}" />
        IValidationBuilder<TObject> RequireAny<TProp1, TProp2, TProp3>(Expression<Func<TObject, TProp1>> expression1,
                                                                       Expression<Func<TObject, TProp2>> expression2,
                                                                       Expression<Func<TObject, TProp3>> expression3);

        /// <inheritdoc cref="RequireAny{TProp1,TProp2}" />
        IValidationBuilder<TObject> RequireAny<TProp1, TProp2, TProp3, TProp4>(Expression<Func<TObject, TProp1>> expression1,
                                                                               Expression<Func<TObject, TProp2>> expression2,
                                                                               Expression<Func<TObject, TProp3>> expression3,
                                                                               Expression<Func<TObject, TProp4>> expression4);

        /// <summary>
        /// Requires the <see cref="IEnumerable"/> property to have an exact length
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Length(Expression<Func<TObject, IEnumerable>> expression, int exact);

        /// <summary>
        /// Requires the <see cref="IEnumerable" /> property to have a minimum or maximum length.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Length(Expression<Func<TObject, IEnumerable>> expression, int? min = null, int? max = null);

        /// <summary>
        /// Checks both values for equality using <see cref="object.Equals(object,object)"/>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Equals<TProp>(Expression<Func<TObject, TProp>> expression, TProp expected);

        /// <summary>
        /// Checks both values for equality using the given <paramref name="comparer"/>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expected"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> Equals<TProp>(Expression<Func<TObject, TProp>> expression, TProp expected, Func<TProp, TProp, bool> comparer);

        /// <summary>
        /// Checks both values for inequality using <see cref="object.Equals(object,object)"/>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> NotEquals<TProp>(Expression<Func<TObject, TProp>> expression, TProp value);

        /// <summary>
        /// Checks both values for inequality using the given <paramref name="comparer"/>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        IValidationBuilder<TObject> NotEquals<TProp>(Expression<Func<TObject, TProp>> expression, TProp value, Func<TProp, TProp, bool> comparer);
        
        /// <summary>
        /// Returns all validation messages. Should be called after all validation methods are handled.
        /// </summary>
        /// <returns></returns>
        ValidationMessages Build();
    }
}