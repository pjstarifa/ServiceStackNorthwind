using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace ServiceStack.Test
{


    public class Column
    {
        public string Column_Name { get; set; }
        public int Ordinal_Position { get; set; }
        public string Data_Type { get; set; }
        public int Character_Maximum_Length { get; set; }
        // Form properties
        // Variablize
        public string Name { get; set; }
        // Humanize
        public string Label { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

    }

    public class Table
    {
        public string Table_Name { get; set; }
        public List<Column> Columns { get; set; }

    }

    [TestFixture]
    internal class FormGenerator
    {
        private OrmLiteConnectionFactory _factory;
        private List<string> _ignoreTables;

            [TestFixtureSetUp]
        public void Setup()
        {
             _factory = new OrmLiteConnectionFactory(
                "Data Source=KULTIS\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True"
                , SqlServerDialect.Provider);
                _ignoreTables = new List<string> {"zzOrderDetail", "sysdiagrams"};
        
        }

        [Test]
        public void GenerateForm()
        {
            var sb = new StringBuilder();
            var tables = GetTables();
            foreach (Table t in tables)
            {
                if (_ignoreTables.Contains( t.Table_Name))
                {
                    continue;
                }
                sb.AppendLine(string.Format("<!-- {0} -->", t.Table_Name));
                sb.AppendLine("<div class=\"alert span2\">");
                sb.AppendFormat("  <h3>{0}</h3>\n", t.Table_Name);
                sb.AppendFormat("  <form name={0}>", t.Table_Name);
                sb.AppendLine();
                foreach (Column col in t.Columns)
                {
                    //var type = GetType(col.Data_Type);
                    var model = string.Format("dto.Tables.{0}.{1}", t.Table_Name, col.Name);

                        sb.AppendFormat("    <label>{0}<br/><input type=\"text\" ng-model=\"{1}\" class=\"input-medium\"></label>", col.Label, model);

                     sb.AppendLine();
                }
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"update{0}()\">Save</a>\n", t.Table_Name);
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"cancel{0}()\">Cancel</a>\n", t.Table_Name);
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"delete{0}()\">Delete</a>\n", t.Table_Name);
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"new{0}()\">New</a>\n", t.Table_Name);

                sb.AppendLine("  </form>");
                sb.AppendLine("</div>");
               sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());

        }

        [Test]
        public void GeneratControllers()
        {
            var sb = new StringBuilder();
            var tables = GetTables();
            foreach (Table t in tables)
            {
                if (_ignoreTables.Contains(t.Table_Name))
                {
                    continue;
                }
                sb.AppendLine(string.Format("// {0}", t.Table_Name));
                sb.AppendLine("<div class=\"alert span2\">");
                sb.AppendFormat("  <h3>{0}</h3>\n", t.Table_Name);
                sb.AppendFormat("  <form name={0}>", t.Table_Name);
                sb.AppendLine();
                foreach (Column col in t.Columns)
                {
                    //var type = GetType(col.Data_Type);
                    var model = string.Format("dto.Tables.{0}.{1}", t.Table_Name, col.Name);

                    sb.AppendFormat("    <label>{0}<br/><input type=\"text\" ng-model=\"{1}\" class=\"input-medium\"></label>", col.Label, model);

                    sb.AppendLine();
                }
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"update{0}()\">Save</a>\n", t.Table_Name);
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"cancel{0}()\">Cancel</a>\n", t.Table_Name);
                sb.AppendFormat("    <a class=\"btn\" ng-click=\"delete{0}()\">Delete</a>\n", t.Table_Name);         

                sb.AppendLine("  </form>");
                sb.AppendLine("</div>");
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());

        }



        private IEnumerable<Table> GetTables()
        {
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' order by table_name";
                var tables=db.Select<Table>(sql);
                foreach (var t in tables)
                {
                    t.Columns = GetColumns(t);
                }
                return tables;
            }
        }

        private List<Column> GetColumns(Table table)
        {
            List<Column> cols;
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                var sql = string.Format("select *  from information_schema.columns where LOWER(table_name) =LOWER('{0}') order by ordinal_position", table.Table_Name);
                cols= db.Select<Column>(sql);
            }

            cols = PopulateColumnMetaData(cols);
            return cols;

        }

        private List<Column> PopulateColumnMetaData(List<Column> cols)
        {
            foreach (var c in cols)
            {
                c.Name = VariabliseString(c.Column_Name);
                c.Label = HumanizeString(c.Column_Name);                
            }
            return cols;
        }

        private string SetType(string sqlType)
        {
            switch (sqlType)
            {
                case "int":
                    return "number";
                case "DateTime":
                    return "date";
            }
          
            return string.Empty;
        }

        private string GenerateForm(Table table)
        {
            var sb = new StringBuilder();
            foreach (var c in table.Columns)
            {
                sb.AppendFormat("<input type={0}");
            }
            return sb.ToString();
        }


        #region Utilities

        private string VariabliseString(string source)
        {
            return source.Replace(" ", "");
        }

        private string HumanizeString(string source)
        {
            var sb = new StringBuilder();

            char last = char.MinValue;
            foreach (char c in source)
            {
                if (char.IsLower(last) &&
                char.IsUpper(c))
                { sb.Append(' '); }
                sb.Append(c);
                last = c;
            }
            return sb.ToString();
        } 
        #endregion

    }
}