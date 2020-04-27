namespace SimpleLang.TACode.TacNodes
{

   public class TacAssignmentNode : TacNode
    {

       public string LeftPartIdentifier { get; set; }

       public string FirstOperand { get; set; }

       public string SecondOperand { get; set; } = null;

       public string Operation { get; set; } = null;

        public string RightPart =>
            (Operation == null) && (SecondOperand == null) ? $"{FirstOperand}"
                : $"{FirstOperand} {Operation} {SecondOperand}";

        public override string ToString()
        {
            return $"{base.ToString()}{LeftPartIdentifier} = {FirstOperand} {Operation} {SecondOperand}";
        }
    }
}
