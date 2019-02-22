namespace TransformView.Models
{
    public class Section : BookComposite
    {
        public Section(string name) : base(name) { }

        public override string HeadingTagName => "h4";
    }
}
