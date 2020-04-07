using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;
namespace SimpleLang.Visitors
{
    class FindFalseVisitor: AutoVisitor
    {
        public override void Visit(BinExprNode BinOp)
        {
            if (BinOp.OpType==BinOpType.Greater && BinOp.Left is IdNode idl && BinOp.Right is IdNode idr 
                && idl.Name == idl.Name)
            {
                //ReplaceStat(ass, null); // Заменить на null.

                // Потом этот null надо специально проверять!!!

            }
            // Не обходить потомков
        }
    }

}
