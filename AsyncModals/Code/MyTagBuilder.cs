using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AsyncModals
{
    public class MyTagBuilder
    {
        private string TagName;
        public MyInnerHtml InnerHtml { get; set; } = new MyInnerHtml();
        private Dictionary<string, string> Attributes = new Dictionary<string, string>();
        private List<string> CssClasses = new List<string>();
        public MyTagBuilder(string tagName) => this.TagName = tagName;
        public class MyInnerHtml
        {
            private StringBuilder InnerHtml = new StringBuilder();
            public void Append(string entry) => this.InnerHtml.Append(entry);
            public override string ToString()
            {
                return this.InnerHtml.ToString();
            }
        }
        public void MergeAttribute(string key, string value)
        {
            if (!this.Attributes.ContainsKey(key))
                this.Attributes.Add(key, value);
            else
                this.Attributes[key] = value;
        }
        public void MergeAttributes(object htmlAttributes)
        {
            if (htmlAttributes == null) return;
            this.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        }
        public void MergeAttributes(RouteValueDictionary dictionary)
        {
            if (dictionary == null) return;
            foreach (string key in dictionary.Keys)
                if (!this.Attributes.ContainsKey(key))
                    this.Attributes.Add(key, dictionary.GetValueOrDefault(key).ToString());
                else
                    this.Attributes[key] = dictionary.GetValueOrDefault(key).ToString();
        }
        public void RemoveAttribute(string key)
        {
            if (this.Attributes.ContainsKey(key))
                this.Attributes.Remove(key);
        }
        public string GetAttribute(string key) => this.Attributes.ContainsKey(key) ? this.Attributes[key] : string.Empty;
        public void AddCssClass(string cssClass) => this.CssClasses.Add(cssClass);
        public HtmlString RenderBody()
        {
            string innerHtml = this.InnerHtml.ToString();
            if (!string.IsNullOrWhiteSpace(innerHtml) || this.TagName.ToLower().Equals("script"))
                return new HtmlString($"<{this.TagName} {SetAttributes()}{SetCssClass()}>{innerHtml}</{this.TagName}>");
            else
                return new HtmlString($"<{this.TagName} {SetAttributes()}{SetCssClass()}/>");
            string SetAttributes()
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in this.Attributes)
                    stringBuilder.Append($"{kvp.Key}='{kvp.Value}' ");
                return stringBuilder.ToString();
            }
            string SetCssClass()
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (this.CssClasses.Count > 0)
                {
                    stringBuilder.Append("class='");
                    foreach (string cssClass in this.CssClasses)
                        stringBuilder.Append($"{cssClass} ");
                    stringBuilder.Append("'");
                }
                return stringBuilder.ToString();
            }
        }
        public override string ToString()
        {
            return this.RenderBody().ToString();
        }
        public HtmlString MashUpBody(params MyTagBuilder[] myTagBuilders)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (MyTagBuilder myTagBuilder in myTagBuilders)
                stringBuilder.Append(myTagBuilder.ToString());
            return new HtmlString($"{this}{stringBuilder}");
        }
    }
}
