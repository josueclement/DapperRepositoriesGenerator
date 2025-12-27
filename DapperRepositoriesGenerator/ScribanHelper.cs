using Scriban;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public static class ScribanHelper
{
    public static string RenderTemplate(string templateContent, ScriptObject scriptObject)
    {
        var template = Template.Parse(templateContent);
        var context = new TemplateContext();
        context.PushGlobal(scriptObject);
        return template.Render(context);
    }
}