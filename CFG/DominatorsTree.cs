using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG
{
    public class DominatorsTree
    {
        public Dictionary<BasicBlock, List<BasicBlock>> dominators;
        public ControlFlowGraph controlFlow;

        public DominatorsTree(ControlFlowGraph cfg)
        {
            dominators = new Dictionary<BasicBlock, List<BasicBlock>>();
            controlFlow = cfg;
        }

        public Dictionary<BasicBlock, List<BasicBlock>> GenDominatorsTree()
        {
            // dominator of the start node is the start itself
            dominators.Add(controlFlow.blocks[0], new List<BasicBlock> { controlFlow.blocks[0] });

            int changedCount = 0;
            List<BasicBlock> temp = new List<BasicBlock>();
            List<BasicBlock> previos;
            // считаем все блоки доминирует над всеми
            for (int i = 1; i < controlFlow.BlockCount; i++)
                dominators.Add(controlFlow.blocks[i], controlFlow.blocks.ToList());
            
            // iteratively eliminate nodes that are not dominators

            while (changedCount < controlFlow.BlockCount) { 
                for (int i = 1; i < controlFlow.BlockCount; i++){
                    previos = dominators[controlFlow.blocks[i]];
                    //пересечение блоков доминаторов предшествеников
                    if (controlFlow.blocks[i].In.Count == 1)
                        temp = dominators[controlFlow.blocks[i].In[0]].ToList();
                    else
                        for (int j = 0; j < controlFlow.blocks[i].In.Count - 1; j++)
                        {
                            var intersect = dominators[controlFlow.blocks[i].In[j]]
                                .Intersect(dominators[controlFlow.blocks[i].In[j + 1]]).ToList();
                            if (temp.Count != 0)
                            {
                                temp = temp.Intersect(intersect).ToList();
                            } else
                            {
                                temp = intersect;
                            }
                        }
                    //сам блок
                    temp.Add(controlFlow.blocks[i]);
                    dominators[controlFlow.blocks[i]] = temp.ToList();
                    if (dominators[controlFlow.blocks[i]].SequenceEqual(previos))
                        changedCount++;
                }
            } 
            return this.dominators;
        }
    }
}
