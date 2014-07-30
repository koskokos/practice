using System;
using System.Web.Mvc;

namespace Practice.Models.ODataTable
{
    public static class ODataTableExtension
    {
        const string template = 
@"<div id='{0}' class='odatatable'>
</div>
<script>
    $('#{0}').oDataTable({1});
</script>";
        public static MvcHtmlString ODataTable(this HtmlHelper html, Settings settings)
        {
            return new MvcHtmlString(string.Format(template, "odt" + new Random().Next(65536), settings.Serialize()));
        }
    }
}