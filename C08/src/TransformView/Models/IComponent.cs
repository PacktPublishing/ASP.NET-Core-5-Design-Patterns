using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransformView.Models
{
    public interface IComponent
    {
        void Add(IComponent bookComponent);
        void Remove(IComponent bookComponent);
        int Count();
        string Type { get; }
    }

    public abstract class ComponentBase : IComponent
    {
        public abstract void Add(IComponent bookComponent);
        public abstract void Remove(IComponent bookComponent);
        public abstract int Count();
        public abstract string Type { get; }
    }
}
