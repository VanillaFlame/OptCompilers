using System;
using System.Collections.Generic;
using System.Collections;
using SimpleLang.TAC;
namespace SimpleLang.CFG
{
    public class InOutVectorCreator
    {
        static List<TACInstruction> assignList;
        public InOutVectorCreator(ThreeAddressCode a)
        {
            changeCode(a);
        }

        public void changeCode(ThreeAddressCode a)
        {
            assignList = new List<TACInstruction>();
            foreach (var instr in a.Instructions)
                if (instr.Operation is "=")
                    assignList.Add(instr);
        }

        public InOutVector getVector(IEnumerable<TACInstruction> assigns)
        {
            return new InOutVector(assigns, assignList);
        }
    }

    public class InOutVector
    {
		BitArray data;
        public BitArray Data => data;

        public InOutVector(IEnumerable<TACInstruction> assigns, List<TACInstruction> assignList)
        {
            data = new BitArray(assignList.Count, false);
            foreach (var assign in assigns)
                data[assignList.IndexOf(assign)] = true;
        }

        InOutVector(BitArray a)
        {
            data = a;
        }

        public bool this[int key]
        {
            get => data[key];
            set => data[key] = value;
            //ошибка? да и пусть летит
        }

        public static InOutVector operator+(InOutVector a, InOutVector b)
        {
            var d = a.data.Count;
            if (d != b.data.Count)
                throw new Exception("Vectors have different size, how is that even possible?!");
            var c = new BitArray(d, false);
            for (int i = 0; i < d; i++)
                c[i] = a[i] || b[i];
            return new InOutVector(c);
        }

        public static InOutVector operator-(InOutVector a, InOutVector b)
        {
            var d = a.data.Count;
            if (d != b.data.Count)
                throw new Exception("Vectors have different size, how is that even possible?!");
            var c = new BitArray(d, false);
            for (int i = 0; i < d; i++)
                c[i] = a[i] != (a[i] && b[i]);
            return new InOutVector(c);
        }

        public static InOutVector operator-(InOutVector a)
        {
            var d = a.data.Count;
            var c = new BitArray(d, false);
            for (int i = 0; i < d; i++)
                c[i] = !a[i];
            return new InOutVector(c);
        }
    }
}
