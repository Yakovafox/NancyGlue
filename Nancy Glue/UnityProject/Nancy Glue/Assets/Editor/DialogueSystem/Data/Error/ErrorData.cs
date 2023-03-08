using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data.Error
{
    public class ErrorData
    {
        public Color color { get; set; }

        public ErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            color = new Color32(
                (byte) Random.Range(65, 256),
                (byte) Random.Range(50, 176),
                (byte) Random.Range(65, 176),
                255
            );
        }
    }
}
