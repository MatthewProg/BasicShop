using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class ProductListModel
    {
        public string Name { get; set; }
        public decimal Price { get; set;}
        public string ImageUrl { get; set; }
        public List<SpecifitationModel> Specifitation { get; private set; }

        public void SetSpecification(List<SpecifitationModel> spec)
        {
            if (spec == null) throw new NullReferenceException("Specification is null");
            Specifitation = spec;
        }

        public void SetSpecification(string spec)
        {
            if (spec == null) throw new NullReferenceException("Specification is null");

            List<SpecifitationModel> buff = new List<SpecifitationModel>();

            spec = spec.Trim();
            spec = spec.Trim(';');
            foreach (var s in spec.Split(';'))
            {
                var tmp = s.Split(':');
                var model = new SpecifitationModel() { Element = tmp[0], Value = tmp[1] };
                buff.Add(model);
            }

            SetSpecification(buff);
        }
    }
}
