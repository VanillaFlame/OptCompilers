using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TAC;

namespace SimpleLang.TACOptimizers
{
    public class GoTotoGoTo_EmptyOptimizer : TACOptimizer
    {
        public GoTotoGoTo_EmptyOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        public struct GoToScan
        {
            public int index;
            public string labelto;
            public string labelfrom;

            public GoToScan(int index, string label1, string label2)
            {
                this.index = index;
                this.labelto = label1;
                this.labelfrom = label2;
            }
        }

        public override void Run()
        {
            var optRemoveEmptyNodes = RemoveEmptyNodes(Instructions);
            if (optRemoveEmptyNodes.Item1)
            {
                Instructions = optRemoveEmptyNodes.Item2;
            }
            var optGoTotoGoTo = ReplaceGoTotoGoTo(Instructions);
            if (optGoTotoGoTo.Item1)
            {
                Instructions = optGoTotoGoTo.Item2;
            }
            TAC = new ThreeAddressCode(Instructions);
        }
        

        public static Tuple<bool, List<TACInstruction>> ReplaceGoTotoGoTo(List<TACInstruction> commands)
        {
            bool changed = false;
            List<GoToScan> list = new List<GoToScan>();
            List<TACInstruction> tmpcommands = new List<TACInstruction>();
            for (int i = 0; i < commands.Count; i++)
            {
                tmpcommands.Add(commands[i]);
                if (commands[i].Operation == "goto")
                {
                    list.Add(new GoToScan(i, commands[i].Label, commands[i].Argument1));
                }

                if (commands[i].Operation == "ifgoto")
                {
                    list.Add(new GoToScan(i, commands[i].Label, commands[i].Argument2));
                }
            }

            for (int i = 0; i < tmpcommands.Count; i++)
            {
                if (tmpcommands[i].Operation == "goto")
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].labelto == tmpcommands[i].Argument1)
                        {
                            if (tmpcommands[i].Argument1.ToString() == list[j].labelfrom.ToString())
                            {
                                changed |= false;
                            }
                            else
                            {
                                changed |= true;
                                tmpcommands[i] = new TACInstruction(tmpcommands[i].Label, "goto", list[j].labelfrom.ToString(), "", "");
                            }

                        }
                    }
                }

                if (tmpcommands[i].Operation == "ifgoto")
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].labelto == tmpcommands[i].Argument2)
                        {

                            if (tmpcommands[i].Argument2.ToString() == list[j].labelfrom.ToString())
                            {
                                changed |= false;
                            }
                            else
                            {
                                tmpcommands[i] = new TACInstruction(tmpcommands[i].Label, "ifgoto", tmpcommands[i].Argument1, list[j].labelfrom.ToString(), "");
                                changed |= true;
                            }

                        }
                    }
                }
            }
            return Tuple.Create(changed, tmpcommands);
        }
        public static Tuple<bool, List<TACInstruction>> RemoveEmptyNodes(List<TACInstruction> commands)
        {
            if (commands.Count == 0) return new Tuple<bool, List<TACInstruction>>(false, commands);

            var result = new List<TACInstruction>();
            var changed = false;
            var toAddLast = true;

            // three cases:
            // a) Command = noop without label, in this case just remove it
            // b) Command = noop with label, next command - op without label.
            //    Then just combine current label and next op.
            // c) Command = noop with label, in this case,
            //    rename all GOTO current_label to GOTO next_label
            for (var i = 0; i < commands.Count - 1; i++)
            {
                var currentCommand = commands[i];
                if (currentCommand == null)
                    currentCommand = new TACInstruction("noop","","","","");
                if (currentCommand.Operation == "noop" && currentCommand.Label == "")
                {
                    changed = true;
                }
                // we have label here
                else if (currentCommand.Operation == "noop")
                {
                    // if next label is empty, we concat current label with next op
                    if (commands[i + 1].Label == "")
                    {
                        var nextCommand = commands[i + 1];
                        changed = true;
                        result.Add(
                            new TACInstruction(
                                currentCommand.Label,
                                nextCommand.Operation,
                                nextCommand.Argument1,
                                nextCommand.Argument2,
                                nextCommand.Result
                            )
                        );
                        i += 1;
                        if (i == commands.Count - 1)
                        {
                            toAddLast = false;
                        }
                    }
                    // if next label is not empty, instead of noop + next,
                    // rename GOTO current_label to GOTO next_label
                    else
                    {
                        var nextCommand = commands[i + 1];
                        changed = true;
                        var currentLabel = currentCommand.Label;
                        var nextLabel = nextCommand.Label;

                        result = result
                            .Select(com =>
                                com.Operation == "goto" && com.Argument1 == currentLabel
                                    ? new TACInstruction(com.Label, com.Operation, nextLabel, com.Argument2, com.Result)
                                    : com
                            ).ToList();

                        for (var j = i + 1; j < commands.Count; j++)
                        {
                            commands[j] = commands[j].Operation == "goto" && commands[j].Argument1 == currentLabel
                                ? new TACInstruction(
                                    commands[j].Label,
                                    commands[j].Operation,
                                    nextLabel,
                                    commands[j].Argument2,
                                    commands[j].Result
                                )
                                : commands[j];
                        }
                    }
                }
                else
                {
                    result.Add(commands[i]);
                }
            }

            if (toAddLast)
            {
                var lastCommand = commands[commands.Count - 1];
                var toSkip = lastCommand.Operation == "noop" && lastCommand.Label == "";
                if (toSkip)
                {
                    changed = true;
                }
                else
                {
                    result.Add(commands[commands.Count - 1]);
                }
            }
            return new Tuple<bool, List<TACInstruction>>(changed, result);
        }
    }
}