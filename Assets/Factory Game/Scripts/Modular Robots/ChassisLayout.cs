namespace ModularRobot
{
    public static class ChassisLayout
    {
        public static AxlesGroup[] TwoAxles
        {
            get
            {
                AxlesGroup[] chassisLayout = new AxlesGroup[]{
                    new AxlesGroup(axlesNumber: 1, offset: 0.2f),
                    new AxlesGroup(axlesNumber: 1, offset: 0.8f)
                };
                return chassisLayout;
            }
        }

        public static AxlesGroup[] ThreeAxles
        {
            get
            {
                AxlesGroup[] chassisLayout = new AxlesGroup[]{
                    new AxlesGroup(axlesNumber: 2, offset: 0.3f),
                    new AxlesGroup(axlesNumber: 1, offset: 0.9f)
                };
                return chassisLayout;
            }
        }

        public static AxlesGroup[] FourAxles
        {
            get
            {
                AxlesGroup[] chassisLayout = new AxlesGroup[]{
                    new AxlesGroup(axlesNumber: 2, offset: 0.3f),
                    new AxlesGroup(axlesNumber: 2, offset: 0.7f)
                };
                return chassisLayout;
            }
        }
    }
}