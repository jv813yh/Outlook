namespace Outlook.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependentViewAttribute : Attribute 
    {
        public string Region { get; set; }
        public Type Type { get; set; }

        public DependentViewAttribute(string region, Type type)
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new ArgumentNullException(nameof(Region));
            }

            Region = region;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
