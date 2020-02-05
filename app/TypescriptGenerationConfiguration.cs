using System;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;

namespace scripting
{
    public class TypescriptGenerationConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(options =>
            {
                options.CamelCaseForProperties(true);
                options.UseModules(true);
            });

            builder.ExportAsInterface<Scripts.ExecuteScript>()
                .WithPublicProperties()
                .Substitute(typeof(Guid), new RtSimpleTypeName("string"))
                .OverrideNamespace("scripts")
                .AutoI(false);
        }
    }
}
