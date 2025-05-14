using System;
using UnityEngine;

namespace Shape
{
    [CreateAssetMenu(fileName = "ShapeData", menuName = "ScriptableObjects/ShapeData", order = 0)]
    [Serializable]
    public class ShapeData : ScriptableObject
    {
        [field: SerializeField] public int Columns { get; set; }
        [field: SerializeField] public int Rows { get; set; }

        [field: SerializeField] public Row[] Board { get; set; }

        public void Clear()
        {
            for (var i = 0; i < Rows; i++) Board[i].ClearRow();
        }

        public void CreateNewBoard()
        {
            Board = new Row[Rows];

            for (var i = 0; i < Rows; i++) Board[i] = new Row(Columns);
        }

        [Serializable]
        public class Row
        {
            [field: SerializeField] public bool[] Column { get; set; }
            private int _size;

            public Row(int size)
            {
                CreateRow(size);
            }

            public void CreateRow(int size)
            {
                Column = new bool[size];
                _size = size;
                ClearRow();
            }

            public void ClearRow()
            {
                for (var i = 0; i < _size; i++) Column[i] = false;
            }
        }
    }
}