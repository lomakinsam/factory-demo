using UnityEngine;

namespace ModularRobot
{
    public struct AxlesGroup
    {
        private int axlesNumber;
        private float offset;

        public int AxlesNumber
        {
            get { return axlesNumber; }
            set { axlesNumber = Mathf.Clamp(value, 1, 8); }
        }

        public float Offset
        {
            get { return offset; }
            set { offset = Mathf.Clamp(value, 0.0f, 1.0f); }
        }
        
        public AxlesGroup(int axlesNumber, float offset)
        {
            this.axlesNumber = 1;
            this.offset = 0;

            AxlesNumber = axlesNumber;
            Offset = offset;
        }
    }
}