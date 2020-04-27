namespace SimpleLang.TACode
{      
   public class TmpNameManager
    {
        public static readonly TmpNameManager Instance = new TmpNameManager();	         
       private int _currentVariableCounter = 0;
        private int _currentLabelCounter = 0;                      
       public string GenerateTmpVariableName() => $"t{++_currentVariableCounter}";                      
       public string GenerateLabel() => $"L{++_currentLabelCounter}";              
       public void Drop() { _currentLabelCounter = 0; _currentVariableCounter = 0; }
    }
}
