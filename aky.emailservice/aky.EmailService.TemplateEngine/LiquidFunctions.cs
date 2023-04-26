namespace aky.EmailService.TemplateEngine
{
    using System;
    using System.Linq;
    using DotLiquid;

    public static class LiquidFunctions
    {
        public static string RenderViewModel(this Template template, object root)
        {
            return template.Render(Hash.FromAnonymousObject(root));
        }

        public static void RegisterViewModel(Type rootType)
        {
            rootType.
                Assembly.
                GetTypes().
                Where(t => t.Namespace == rootType.Namespace).
                ToList().
                ForEach(RegisterSafeTypeWithAllProperties);
        }

        private static void RegisterSafeTypeWithAllProperties(Type type)
        {
            Template.RegisterSafeType(
                type, 
                type.GetProperties().Select(p => p.Name).ToArray());
        }
    }

}
