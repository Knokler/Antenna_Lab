
namespace LogoCalculator.Core.Ldpa.Optimization
{
    /// <summary>
    /// Подборщик параметров LDPA антенны под заданные параметры.
    /// </summary>
    public class LdpaOptimizer
    {
        private readonly LdpaCalculator _calculator=new();
        private static double CalculateScore(LdpaDesignRequirements requirements,LdpaInputParameters input,LdpaResult result,double estimatedGain)
        {
            double score = 1000;
            double highestFrequency = result.Elements.Max(element=> element.FrequencyMHz);

            if (highestFrequency < requirements.MaxFrequencyMHz)
            { 
                double missRatio = (requirements.MaxFrequencyMHz -highestFrequency) / requirements.MaxFrequencyMHz;
                score -= missRatio * 1000; 
            }
            return score;
        }

        /// <summary>
        /// Инженерная оценка усиления в дБи для заданных параметров и результата расчета. 
        /// Используется для ранжирования кандидатов до более точного расчета.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static double EstimateGainDbi(LdpaInputParameters input, LdpaResult result)
        {
            double gain = 5;
            gain += Math.Min(input.ElementCount, 24) * 0.8; // Увеличение усиления с количеством элементов, с затуханием после 24 элементов
            gain += (input.Tau - 0.86) * 10; // Увеличение усиления с увеличением Tau
            gain += (input.Sigma - 0.04) * 4; // Увеличение усиления с увеличением Sigma
            if (result.ApexAngleDeg > 50)
            {
                gain -= 0.7;
            }

            if (result.ApexAngleDeg < 12)
            {
                gain += 0.4;
            }

            return Math.Clamp(gain,4.0,10.0);
            
        }

        public LdpaDesignCandidate FindBest(LdpaDesignRequirements requirements)
        {
            LdpaDesignCandidate? bestCandidate = null;
            for (double tau = 0.86; tau <= 0.96; tau += 0.01)
            {
                for (double sigma = 0.04; sigma <= 0.12; sigma += 0.01)
                { 
                    for(int elementCount = 6; elementCount <=30; elementCount++)
                    {
                        var input = new LdpaInputParameters
                        {
                            MinFrequencyMHz = requirements.MinFrequencyMHz,
                            MaxFrequencyMHz = requirements.MaxFrequencyMHz,
                            ElementCount = elementCount,
                            Tau = tau,
                            Sigma = sigma,
                            VelocityFactor = requirements.VelocityFactor,
                            WidthMode = requirements.WidthMode,
                            ElementWidthRatio = requirements.ElementWidthRatio,
                            MinElementWidthMm = requirements.MinElementWidthMm,
                            MaxElementWidthMm = requirements.MaxElementWidthMm
                        };
                        LdpaResult result = _calculator.Calculate(input);

                        double highestFrequency = result.Elements.Max(element => element.FrequencyMHz);

                        if (highestFrequency < requirements.MaxFrequencyMHz)
                        {
                            continue;
                        }

                        double estimatedGain = EstimateGainDbi(input,result);
                        double score = CalculateScore(
                            requirements, 
                            input, 
                            result, 
                            estimatedGain);
                        var candidate = new LdpaDesignCandidate
                        {
                            Input = input,
                            Result = result,
                            EstimateGainDbi = estimatedGain,
                            Score = score
                        };
                        if(bestCandidate == null || candidate.Score > bestCandidate.Score)
                        {
                            bestCandidate = candidate;
                        }
                    }
                }
            }
            if (bestCandidate == null)
            {
                throw new InvalidOperationException("Неудалось подобрать вариант LDPA антенны");
            }
            return bestCandidate;

        }

    }
}
