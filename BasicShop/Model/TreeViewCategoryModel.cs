using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;

namespace BasicShop.Model
{
    public class TreeViewCategoryModel
    {
        public List<TreeViewCategoryModel> Subcategories { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public IList Items
        {
            get
            {
                return new CompositeCollection()
                {
                    new CollectionContainer() { Collection = Subcategories }
                };
            }
        }

        public TreeViewCategoryModel()
        {
            Subcategories = new List<TreeViewCategoryModel>();
        }
    }
}
