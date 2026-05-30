
using LogoCalculator.Core.Ldpa.Optimization;

namespace LdpaCalculator.test.Ldpa.Optimization
{
    public class LdpaOptimizerTests
    {
        [Fact]
        private void FindBest_ShouldReturnCandidateWithEstimatedGain()
        {
            //Arrange
            var optimizer = new LdpaOptimizer();
            var requirements = new LdpaDesignRequirements
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 6000,
                TargetApexAngleDeg = 30,
                TargetGainDbi = 7,
                VelocityFactor = 0.92
            };
            //Act
            LdpaDesignCandidate candidate = optimizer.FindBest(requirements);

            //Assert
            Assert.InRange(candidate.EstimateGainDbi, 4.0, 10.0);
        }
        [Fact]
        /// <summary>
        /// Тест №31 проверяет правельность заполнения и возврата полей подборщика
        /// </summary>
        public void FindBest_ShouldBeCandidateWithValidIputAndResult()
        {
            
            //Arrange
            var optimizer = new LdpaOptimizer();
            var requirements = new LdpaDesignRequirements
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 2000,
                TargetApexAngleDeg = 60,
                TargetGainDbi = 14,
                VelocityFactor = 0.92
            };
            //Act
            LdpaDesignCandidate candidate = optimizer.FindBest(requirements);
            //Assert
            Assert.NotNull(candidate);
            Assert.NotNull(candidate.Input);
            Assert.NotNull(candidate.Result);

            Assert.True(candidate.Input.ElementCount>0);
            Assert.True(candidate.Input.ElementCount<=30);

            Assert.NotEmpty(candidate.Result.Elements);
        }

        [Fact]
        public void FindBest_ShouldReturnCandidateThatCoversMaxFrequency()
        {
            //Arrange
            var optimizer = new LdpaOptimizer();

            var requirements = new LdpaDesignRequirements
            {
                MinFrequencyMHz = 1000,
                MaxFrequencyMHz = 3000,
                TargetApexAngleDeg = 30,
                TargetGainDbi = 7,
                VelocityFactor = 0.92
            };

            //Act
            LdpaDesignCandidate candidate = optimizer.FindBest(requirements);

            //Assert
            double highestFrequency = candidate.Result.Elements.Max(element => element.FrequencyMHz);
            Assert.True(highestFrequency >= requirements.MaxFrequencyMHz);
        }
    }
}
