namespace SimpleLang.TACode.TacNodes
{
    public abstract class TacNode
    {
        public string Label { get; set; } = null;

        public bool IsUtility { get; set; } = false;
        
        public override string ToString() => Label != null ? Label + ": " : "";
    }
}