using static SpaceRovers.Entity.Common.Enums;

namespace SpaceRovers.Entity.Rover
{
    public class SpaceRoverStatus
    {
        /// <summary>
        /// Rover'ın komut işleme/işleyebilirlik durumunu gösterir.
        /// </summary>
        public RoverStatusEnum Status { get; set; }

        /// <summary>
        /// Eğer rover hareket komutlarını işlemek için başka bir rover'ın kendi hereketlerini tamamlamasını bekliyorsa; beklediği rover'ın adı burada tutulur.
        /// </summary>
        public string WaitingFor { get; set; }
    }
}
