using System;
using Xunit;

namespace ConversionOperator
{
    public class Tests
    {
        [Fact]
        public void Value_should_be_set_implicitly()
        {
            var result = GetValue("Test");
            Assert.Equal("Test", result.Value);

            static SomeGenericClass<string> GetValue(string value)
            {
                return value;
            }
        }
    }

    public class SomeGenericClass<T>
    {
        public T Value { get; set; }

        public static implicit operator SomeGenericClass<T>(T value)
        {
            return new SomeGenericClass<T>
            {
                Value = value
            };
        }
    }
}
