namespace SimpleLang.TACode.TacNodes
{
    public class TacGotoNode : TacNode
    {
        public string TargetLabel { get; set; }

        public override string ToString() => $"{base.ToString()}goto {TargetLabel}";
    }
}
