
namespace LogoCalculator.Core.Ldpa.Optimization
{
    /// <summary>
    /// Один из возможных кандидатов.
    /// </summary>
    public class LdpaDesignCandidate
    {
        public LdpaInputParameters Input { get; set; } = new();
        public LdpaResult Result { get; set; } = new();

        /// <summary>
        /// Оценочный коэфицент усиления в dbi
        /// </summary>
        public double EstimateGainDbi { get; set; }
        /// <summary>
        /// Итоговая оценка кандидата . Чем выше, тем лучше.
        /// </summary>
        public double Score
        {
            get; set;
        }


    }
}
