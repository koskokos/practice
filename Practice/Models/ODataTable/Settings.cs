using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace Practice.Models.ODataTable
{

    public class Settings
    {
        public const string undef = "undefined";
        const string template = @"selector: {0}, table: {1}";

        public Selector Selector { get; set; }
        public Table Table { get; set; }
        public string Serialize()
        {
            return "{" + string.Format(template, Selector.Serialize(), Table.Serialize()) + "}";
        }
    }
    public class Selector
    {
        const string template = @"url: '{0}', valueField: '{1}', textField: '{2}', filterFieldName: '{3}'";
        public string Url { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string FilterFieldName { get; set; }
        public string Serialize()
        {
            return "{" + string.Format(template, Url, ValueField, TextField, FilterFieldName) + "}";
        }
    }

    public class Table
    {
        const string template = @"dataSource: '{0}', columnNames: {1}, columns: {2}, customReadScenarios: {3}";
        public string DataSource { get; set; }
        public string[] ColumnNames { get; set; }
        public string[] Columns { get; set; }
        public Dictionary<string, string> CustomReadScenarios { get; set; }
        public string Serialize()
        {
            return "{" + string.Format(template,
                DataSource,
                ColumnNames != null && ColumnNames.Length > 0 ? "['" + ColumnNames.ToDelimitedString("','") + "']" : Settings.undef,
                Columns != null && Columns.Length > 0 ? "['" + Columns.ToDelimitedString("','") + "']" : Settings.undef,
                CustomReadScenarios != null && CustomReadScenarios.Count > 0 ? CustomReadScenarios.Aggregate("{", (res, item) => res += item.Key + ":" + item.Value + ",").TrimEnd(',') + "}" : Settings.undef) + "}";
        }
    }
}