using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Script.Serialization;

namespace FinancialPlannerApplication.Extensions
{
    public static class MvcExtensions
    {
        public static IList<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> textSelector, Func<T, string> valueSelector, IEnumerable<T> selected, string defaultOption = "")
        {
            var items = enumerable.Select(f => new SelectListItem { Text = textSelector(f), Value = valueSelector(f), Selected = selected.Contains(f) }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "-1" });
            }

            return items;
        }

        public static IList<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> textSelector, Func<T, string> valueSelector, string defaultOption = "")
        {
            return ToSelectList(enumerable, textSelector, valueSelector, new List<T>(), defaultOption);
        }

        public static IList<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> textSelector, Func<T, int> valueSelector, IEnumerable<T> selected, string defaultOption = "")
        {
            var items = enumerable.Select(f => new SelectListItem { Text = textSelector(f), Value = valueSelector(f).ToString(CultureInfo.InvariantCulture), Selected = selected.Contains(f) }).ToList();

            if (!string.IsNullOrEmpty(defaultOption))
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "-1" });
            }

            return items;
        }

        public static MvcHtmlString TypeAheadFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IEnumerable<string> source, int items = 8)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (source == null)
                throw new ArgumentNullException("source");

            var jsonString = new JavaScriptSerializer().Serialize(source);

            return htmlHelper.TextBoxFor(expression, new { autocomplete = "off", data_provide = "typeahead", data_items = items, data_source = jsonString });
        }
    }
}