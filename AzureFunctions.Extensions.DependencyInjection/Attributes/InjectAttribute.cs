using Microsoft.Azure.WebJobs.Description;
using System;

namespace AzureFunctions.Extensions.DependencyInjection
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public String Name { get; }

        public InjectAttribute(String name = null)
        {
            Name = name;
        }
    }
}
