using System.Web.Mvc;
using Kendo.Mvc.UI.Fluent;

namespace BassinoLibrary.Helpers
{
    public class KendoWidgetFactory<TModel> : WidgetFactory<TModel>
    {
        public KendoWidgetFactory(HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
        }

        public override TabStripBuilder TabStrip()
        {
            return new TabStripBuilder(base.TabStrip());
        }

        public override MultiSelectBuilder MultiSelect()
        {
            return new MultiSelectBuilder(base.MultiSelect());
        }
        public override Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownList()
        {
            return new DropDownListBuilder(base.DropDownList());
        }

        public GridBuilder<T, TC> Grid<T, TC>(GridDetailsMode gridDetailsMode = GridDetailsMode.Modal) where T : class
        {
            return new GridBuilder<T, TC>(base.Grid<T>(), gridDetailsMode);
        }
    }
}