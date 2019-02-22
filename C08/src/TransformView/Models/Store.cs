namespace TransformView.Models
{
    public class Store : BookComposite
    {
        public Store(string name) : base(name) { }

        public override string HeadingTagName => "h3";
    }
}
