using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

public static class ActionResultAssertionExtensions
{
    public class ActionResultAssertions : ObjectAssertions
    {
        public new IActionResult Subject { get; }

        public ActionResultAssertions(IActionResult subject) : base(subject)
        {
            Subject = subject;
        }

        [CustomAssertion]
        public void BeRedirectAction(string actionName, string because = null, params object[] becauseArgs)
        {
            var redirectResult = AssertionExtensions.Should(Subject).BeOfType<RedirectToActionResult>().Which;

            var actual = redirectResult.ActionName;
            var expected = actionName;

            Execute.Assertion.ForCondition(actual == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to redirect to {0} Action but it is redirecting to {1}", expected, actual);
        }
    }

    public static ActionResultAssertions Should(this IActionResult subject)
    {
        return new ActionResultAssertions(subject);
    }
}