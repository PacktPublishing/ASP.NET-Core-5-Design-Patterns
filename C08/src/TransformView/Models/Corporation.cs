namespace TransformView.Models
{
    public class Corporation : BookComposite
    {
        public Corporation(string name) : base(name) { }

        public override string HeadingTagName => "h2";
    }
}
