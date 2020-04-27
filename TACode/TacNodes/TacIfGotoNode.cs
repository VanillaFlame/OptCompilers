namespace SimpleLang.TACode.TacNodes
{
    public class TacIfGotoNode : TacGotoNode
    {
        public string Condition { get; set; }

        public override string ToString() => (Label != null ? Label + ": " : "") + $"if {Condition} goto {TargetLabel}";
    }
}