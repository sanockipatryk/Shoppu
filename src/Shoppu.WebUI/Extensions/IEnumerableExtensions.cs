using Microsoft.AspNetCore.Mvc.Rendering;
using Shoppu.Domain.Entities;

namespace Shoppu.WebUI.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue, string? firstItemText)
        {
            var selectListItems = from item in items
                                  select new SelectListItem
                                  {
                                      Text = item.GetPropertyValue("Name"),
                                      Value = item.GetPropertyValue("Id"),
                                      Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                                  };
            if (firstItemText != null)
            {
                selectListItems = selectListItems.Prepend(new SelectListItem
                {
                    Text = firstItemText,
                    Value = (-1).ToString(),
                    Selected = true
                });
            }

            return selectListItems;
        }

        public static IEnumerable<SelectListItem> ToSelectListI(this List<ProductCategory> items, int selectedValue, string? firstItemText)
        {
            var selectListGroups = new List<SelectListGroup>();
            foreach (var item in items)
            {
                if(item.ParentCategoryId == null && selectListGroups.FirstOrDefault(g => g.Name == item.ParentCategory?.Name) == null)
                {
                    selectListGroups.Add(new SelectListGroup()
                    {
                        Name = item.Name
                    });
                }
            }

            //var selectListGroups = from item in items
            //                       where item.ParentCategoryId == null
            //                       select new SelectListGroup
            //                       {
            //                           Name = item.Name,
            //                       };
            var selectListItems = from item in items
                                  select new SelectListItem
                                  {
                                      Text = item.GetPropertyValue("Name"),
                                      Value = item.GetPropertyValue("Id"),
                                      Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString()),
                                      Group = (item.ParentCategory?.Name != null && selectListGroups.FirstOrDefault(g => g.Name == item.ParentCategory?.Name) != null) ? selectListGroups.FirstOrDefault(g => g.Name == item.ParentCategory.Name) : null
                                  };
            if (firstItemText != null)
            {
                selectListItems = selectListItems.Prepend(new SelectListItem
                {
                    Text = firstItemText,
                    Value = (-1).ToString(),
                    Selected = true
                });
            }

            return selectListItems;
        }
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
    }
}
