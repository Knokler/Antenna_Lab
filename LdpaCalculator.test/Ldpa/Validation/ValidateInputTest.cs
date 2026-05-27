using LogoCalculator.Core.Ldpa;     
using LdpaCalc = LogoCalculator.Core.Ldpa.LdpaCalculator;

namespace LdpaCalculator.test.Ldpa.Validation
{
    public class ValidateInputTest
    {
        [Fact]
        public void Calculate_WithVelocityFactorGreaterThanOne_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 1.2
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
        }

        [Fact]
        public void Calculate_WithZeroVelocityFactor_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.90,
                Sigma = 0.07,
                VelocityFactor = 0
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
        }

        [Fact]
        public void Calculate_WidthMaxElementWidthLessThanMinElementWidth_ShouldThrowArgumentWidth_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new LdpaCalc();
            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.9,
                Sigma = 0.07,
                VelocityFactor = 0.92,
                MinElementWidthMm = 5.0,
                MaxElementWidthMm = 2.0 // Неправильное значение
            };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
        }
        [Fact]
        public void Calculate_WidthZeroMinElementWidth_ShouldThrowArgumentExcetion()
        {
            //Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.9,
                Sigma = 0.7,
                VelocityFactor = 0.92,
                MinElementWidthMm = 0.0, // Неправильное значение
                MaxElementWidthMm = 12.0
            };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
        }
        [Fact]
        public void Calculate_WithTauGreaterOrEqualOne_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 100.0,
                ElementCount = 5,
                Tau = 1.0, // Неправильное значение
                Sigma = 0.1,
                VelocityFactor = 0.95
            };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
        }
        [Fact]
        public void Calculate_WidthZeroSigma_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new LdpaCalc();

            var input = new LdpaInputParameters
            {
                MinFrequencyMHz = 1080,
                MaxFrequencyMHz = 6000,
                ElementCount = 16,
                Tau = 0.9,
                Sigma = 0.0, // Неправильное значение
                VelocityFactor = 0.92
            };

            // Act & Assert

            Assert.Throws<ArgumentException>(() => calculator.Calculate(input));

        }
    }
}
