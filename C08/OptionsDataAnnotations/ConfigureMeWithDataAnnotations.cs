using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace OptionsDataAnnotations
{
    public class ConfigureMeWithDataAnnotations
    {
        [Fact]
        public void Test1()
        {

        }

        private class Options
        {
            [Required]
            public string Title { get; set; }
        }
    }
}
