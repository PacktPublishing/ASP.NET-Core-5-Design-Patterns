namespace TransformView.Models
{
    public class Set : BookComposite
    {
        public Set(string name, params IComponent[] books)
            : base(name)
        {
            foreach (var book in books)
            {
                Add(book);
            }
        }

        public override string HeadingTagName => "h5";
    }
}
