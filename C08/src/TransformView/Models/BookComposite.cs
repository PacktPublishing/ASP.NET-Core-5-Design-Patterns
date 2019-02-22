using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TransformView.Models
{
    public abstract class BookComposite : IComponent
    {
        protected readonly List<IComponent> children;
        public BookComposite(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            children = new List<IComponent>();
        }

        public virtual ReadOnlyCollection<IComponent> Components => new ReadOnlyCollection<IComponent>(children);

        public string Name { get; }

        public virtual string Type => GetType().Name;

        public virtual void Add(IComponent bookComponent)
        {
            children.Add(bookComponent);
        }

        public virtual int Count()
        {
            return children.Sum(child => child.Count());
        }

        public virtual void Remove(IComponent bookComponent)
        {
            children.Remove(bookComponent);
        }
    }
}
