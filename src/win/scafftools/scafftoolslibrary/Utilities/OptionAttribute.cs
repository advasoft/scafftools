
namespace scafftools.Utilities
{
    using System;

    /// <summary>
    /// Attribute contains common information about how to handle console argument
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute(string optionName)
        {
            Required = false;
            IsFlag = false;
            OptionName = optionName;
        }

        /// <summary>
        /// Option name from console argument
        /// </summary>
        public string OptionName { get; private set; }

        /// <summary>
        /// Mark that option is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Mark that argument without value as 'flag'
        /// </summary>
        public bool IsFlag { get; set; }

    }
}
