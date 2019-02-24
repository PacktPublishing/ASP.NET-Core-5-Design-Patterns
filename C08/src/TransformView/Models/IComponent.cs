using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransformView.Models
{
    public interface IComponent
    {
        void Add(IComponent bookComponent);
        void Remove(IComponent bookComponent);
        int Count();
    }
}
