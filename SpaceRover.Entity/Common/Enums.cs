namespace SpaceRovers.Entity.Common
{
    public class Enums
    {
        public enum CompassEnum
        {
            /// <summary>
            /// North
            /// </summary>
            North = 1,
            /// <summary>
            /// East
            /// </summary>
            East = 2,
            /// <summary>
            /// South
            /// </summary>
            South = 3,
            /// <summary>
            /// West
            /// </summary>
            West = 4
        }

        public enum RoverStatusEnum
        {
            /// <summary>
            /// Rover hareketsiz. Henüz herhangi bir komut yürütmüyor.
            /// </summary>
            Idle = 1,
            /// <summary>
            /// Rover hareket haline. Komutu yürütüyor.
            /// </summary>
            InAction = 2,
            /// <summary>
            /// Rover harekete başlamak için başka bir rover'ın hareketini tamamlamasını bekliyor.
            /// </summary>
            WaitingToRun = 3
        }
    }
}
