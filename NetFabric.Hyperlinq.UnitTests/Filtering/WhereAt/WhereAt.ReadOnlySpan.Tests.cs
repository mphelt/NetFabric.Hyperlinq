using NetFabric.Assertive;
using System;
using System.Linq;
using Xunit;

namespace NetFabric.Hyperlinq.UnitTests.Filtering.WhereIndex
{
    public class ReadOnlySpanTests
    {
        [Fact]
        public void Where_With_NullPredicate_Must_Throw()
        {
            // Arrange
            var source = new int[0];
            var predicate = (PredicateAt<int>)null;

            // Act
            Action action = () => _ = ArrayExtensions
            .Where(source, predicate);

            // Assert
            _ = action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception => exception.ParamName == "predicate");
        }

        [Theory]
        [MemberData(nameof(TestData.PredicateAtEmpty), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtSingle), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.PredicateAtMultiple), MemberType = typeof(TestData))]
        public void Where_With_ValidData_Must_Succeed(int[] source, PredicateAt<int> predicate)
        {
            // Arrange
            var expected = Enumerable
                .Where(source, predicate.AsFunc());

            // Act
            var result = ArrayExtensions
                .Where((ReadOnlySpan<int>)source.AsSpan(), predicate);

            // Assert
            _ = result.SequenceEqual(expected).Must().BeTrue();
        }
    }
}